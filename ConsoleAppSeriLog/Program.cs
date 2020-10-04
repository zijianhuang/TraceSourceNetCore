using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ClassLibrary1;
using Fonlow.Diagnostics;
using Serilog;

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

			Serilog.Log.Logger = new Serilog.LoggerConfiguration()
								  .Enrich.FromLogContext()
								  //.WriteTo.Console() I prefer plugging through the config file
								  .ReadFrom.Configuration(configuration)
								  .CreateLogger();

			using var serviceProvider = new ServiceCollection()
				.AddLogging(builder => builder.AddSerilog())
				.AddSingleton<IFooService, FooService>()
				.BuildServiceProvider();

			var	logger = serviceProvider.GetService<ILogger<Program>>();
			var	fooService = serviceProvider.GetService<IFooService>();

			try
			{
				Log.Information("Starting up");
				logger.LogInformation("1111logger information");
				logger.LogWarning("2222logger warning");

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
