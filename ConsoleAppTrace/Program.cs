using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ClassLibrary1;
using Fonlow.Diagnostics;

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
			IFooService fooService = serviceProvider.GetService<IFooService>();


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
