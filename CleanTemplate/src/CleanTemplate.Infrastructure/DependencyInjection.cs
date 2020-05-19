using CleanTemplate.Application.Infrastructure;
using CleanTemplate.Infrastructure.Common;
using CleanTemplate.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTemplate.Infrastructure
{
	public static class DependencyInjection
	{
		public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddPersistence(configuration);
			services.AddTransient<IDateTime, DateTimeService>();
		}
	}
}
