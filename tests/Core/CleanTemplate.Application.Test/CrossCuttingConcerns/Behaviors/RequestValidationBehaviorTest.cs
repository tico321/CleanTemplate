using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.CrossCuttingConcerns.Behaviors;
using CleanTemplate.Application.CrossCuttingConcerns.Exceptions;
using FluentValidation;
using MediatR;
using Xunit;

namespace CleanTemplate.Application.Test.CrossCuttingConcerns.Behaviors
{
    public class RequestValidationBehaviorTest
    {
        private class Model : IRequest<Model>
        {
            public string Data { get; set; }
        }

        private class ModelValidator : AbstractValidator<Model>
        {
            public ModelValidator()
            {
                RuleFor(m => m.Data)
                    .NotNull()
                    .WithMessage("Data cannot be null")
                    .MinimumLength(5)
                    .WithMessage("5 is the minimum length");
            }
        }

        [Fact]
        public async Task Handle_WithInvalidData()
        {
            var sut = new RequestValidationBehavior<Model, Model>(new List<IValidator<Model>> { new ModelValidator() });
            var nullData = new Model();

            try
            {
                await sut.Handle(nullData, CancellationToken.None, () => Task.FromResult(null as Model));
                Assert.False(true, "Should throw bad request");
            }
            catch (BadRequestException e)
            {
                Assert.True(e.Failures.ContainsKey(nameof(Model.Data)));
                Assert.Contains("Data cannot be null", e.Failures[nameof(Model.Data)]);
            }

            var invalidLength = new Model { Data = string.Empty };
            try
            {
                await sut.Handle(invalidLength, CancellationToken.None, () => Task.FromResult(null as Model));
                Assert.False(true, "Should throw bad request");
            }
            catch (BadRequestException e)
            {
                Assert.True(e.Failures.ContainsKey(nameof(Model.Data)));
                Assert.Contains("5 is the minimum length", e.Failures[nameof(Model.Data)]);
            }
        }

        [Fact]
        public async Task Handle_WithValidData()
        {
            var sut = new RequestValidationBehavior<Model, Model>(new List<IValidator<Model>> { new ModelValidator() });
            var model = new Model { Data = nameof(Model) };

            await sut.Handle(model, CancellationToken.None, () => Task.FromResult(null as Model));

            Assert.False(false, "Does not throw when the data is valid");
        }
    }
}
