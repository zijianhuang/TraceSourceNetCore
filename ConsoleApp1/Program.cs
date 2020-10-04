using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleAppLoggerDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World! from console");

			var configuration = new ConfigurationBuilder()
								.AddJsonFile("appsettings.json", false, true)
								.Build();


			using var serviceProvider = new ServiceCollection()
				.AddSingleton<IFooService, FooService>()
				.AddLogging(builder =>
				{
					builder.AddConfiguration(configuration.GetSection("Logging"));
					builder.AddConsole();
				})
				.BuildServiceProvider();


			ILogger<Program> logger = serviceProvider.GetService<ILogger<Program>>();
			//logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>(); // Factory first. This works too.

			IFooService fooService = serviceProvider.GetService<IFooService>();


			logger.LogInformation("1111logger information");
			logger.LogWarning("2222logger warning");

			fooService.DoWork();
		}


	}

	public interface IFooService
	{
		void DoWork();
	}

	public class FooService : IFooService
	{
		private readonly ILogger logger;

		public FooService(ILogger<FooService> logger)
		{
			this.logger = logger;
		}

		public void DoWork()
		{
			logger.LogInformation("3333Doing work.");
			logger.LogWarning("4444Something warning");
			logger.LogCritical("5555Something critical");
		}
	}
}
