using System;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace CleanTemplate.Application.CrossCuttingConcerns.Mapping
{
    public class MappingProfile : Profile
    {
        // Registers all the mapping profiles of classes that implement IMapFrom
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);

                var methodInfo = type.GetMethod("Mapping")
                                 ?? type.GetInterface("IMapFrom`1").GetMethod("Mapping");

                methodInfo?.Invoke(instance, new object[] {this});
            }
        }
    }
}
