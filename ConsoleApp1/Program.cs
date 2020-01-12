using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace ConsoleApp1
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World! from console");

			var configuration = new ConfigurationBuilder()
								.SetBasePath(Directory.GetCurrentDirectory())
								.AddJsonFile("appsettings.json", false, true)
								.AddEnvironmentVariables()
								.Build();


			ILogger logger;
			IFooService fooService;

			using (var serviceProvider = new ServiceCollection()// thanks to https://thecodebuzz.com/logging-in-net-core-console-application/
				.AddSingleton<IFooService, FooService>()
				.AddLogging(cfg => 
				{
					cfg.AddConfiguration(configuration.GetSection("Logging"));
					cfg.AddConsole(); 
				})
				.BuildServiceProvider())
			{

				logger = serviceProvider.GetService<ILogger<Program>>();
				//logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>(); // Factory first. This works too.

				fooService = serviceProvider.GetService<IFooService>();
			}

			logger.LogInformation("logger information");
			logger.LogWarning("logger warning");

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
			logger.LogInformation("Doing work.");
			logger.LogWarning("Something warning");
			logger.LogCritical("Something critical");
		}
	}
}
