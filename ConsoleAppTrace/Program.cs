using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
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

			var listener = new LoggerTraceListener(logger);
			System.Diagnostics.Trace.Listeners.Add(listener);

			TraceSources.Instance.InitLoggerTraceListener(listener);// thanks to https://stackoverflow.com/questions/54537694/how-i-can-transfer-all-messages-from-system-diagnostics-trace-to-ilogger and

			TraceLover.DoSomething();

			TraceSourceLover.DoSomething();
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
