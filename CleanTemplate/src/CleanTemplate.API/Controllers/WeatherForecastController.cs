using CleanTemplate.Domain.Todos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanTemplate.Application.CrossCuttingConcerns;

namespace CleanTemplate.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		public IApplicationDbContext DbContext { get; }

		private static readonly string[] Summaries = new[]
		{
						"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
				};

		private readonly ILogger<WeatherForecastController> _logger;

		public WeatherForecastController(ILogger<WeatherForecastController> logger, IApplicationDbContext dbContext)
		{
			DbContext = dbContext;
			_logger = logger;
		}

		[HttpGet]
		public IEnumerable<WeatherForecast> Get()
		{
			var rng = new Random();
			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateTime.Now.AddDays(index),
				TemperatureC = rng.Next(-20, 55),
				Summary = Summaries[rng.Next(Summaries.Length)]
			})
			.ToArray();
		}

		[HttpGet("todo")]
		public IEnumerable<TodoItem> GetTodos()
		{
			return DbContext.TodoItems.ToList();
		}

		[HttpPost("todo")]
		public async Task<TodoItem> CreateTodo(TodoItem item)
		{
			DbContext.TodoItems.Add(item);
			await DbContext.SaveChangesAsync(new CancellationToken());
			return item;
		}
	}
}
