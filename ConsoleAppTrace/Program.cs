using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ClassLibrary1;
using Fonlow.Diagnostics;

namespace ConsoleApp1
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World! from console");

			var configuration = new ConfigurationBuilder()
								.AddJsonFile("appsettings.json", false, true)
								.Build();


			ILogger logger;
			IFooService fooService;

			using (var serviceProvider = new ServiceCollection()
				.AddSingleton<IFooService, FooService>()
				.AddLogging(cfg =>
				{
					cfg.AddConfiguration(configuration.GetSection("Logging"));
					cfg.AddConsole();
				})
				.BuildServiceProvider())
			{
				logger = serviceProvider.GetService<ILogger<Program>>();
				fooService = serviceProvider.GetService<IFooService>();
			}

			logger.LogInformation("logger information");
			logger.LogWarning("logger warning");

			fooService.DoWork();

			using (var listener = new LoggerTraceListener(logger))
			{
				System.Diagnostics.Trace.Listeners.Add(listener);
				TraceSources.Instance.InitLoggerTraceListener(listener);

				TraceLover.DoSomething();
				TraceSourceLover.DoSomething();
			}
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
