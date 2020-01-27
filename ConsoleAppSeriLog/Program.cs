using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ClassLibrary1;
using Fonlow.Diagnostics;
using Serilog;

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

			Serilog.Log.Logger = new Serilog.LoggerConfiguration()
								  .Enrich.FromLogContext()
								  //.WriteTo.Console() I prefer plugging through the config file
								  .ReadFrom.Configuration(configuration)
								  .CreateLogger();

			Microsoft.Extensions.Logging.ILogger logger;
			IFooService fooService;

			var services = new ServiceCollection();
			services.AddLogging(configure => configure.AddSerilog());

			using (var serviceProvider = services
				.AddSingleton<IFooService, FooService>()
				.BuildServiceProvider())
			{
				logger = serviceProvider.GetService<ILogger<Program>>();
				fooService = serviceProvider.GetService<IFooService>();
			}

			try
			{
				Log.Information("Starting up");
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
			catch (Exception ex)
			{
				Log.Fatal(ex, "Application start-up failed");
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

	}

	public interface IFooService
	{
		void DoWork();
	}

	public class FooService : IFooService
	{
		private readonly Microsoft.Extensions.Logging.ILogger logger;

		public FooService(Microsoft.Extensions.Logging.ILogger<FooService> logger)
		{
			this.logger = logger;
		}

		public void DoWork()
		{
			logger.LogInformation("FooService Doing work.");
			logger.LogWarning("FooService warning");
			logger.LogCritical("FooService critical");
		}
	}
}
