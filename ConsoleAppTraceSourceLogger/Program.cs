using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ClassLibrary1;
using Fonlow.Diagnostics;
using System.Diagnostics;

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

			using (var listener = new TextWriterTraceListener("c:\\temp\\mylog.txt"))
			using (var consoleListener = new ConsoleTraceListener())
			{
				using (var serviceProvider = new ServiceCollection()
					.AddSingleton<IFooService, FooService>()
					.AddLogging(cfg =>
					{
						cfg.AddConfiguration(configuration.GetSection("Logging"));
						cfg.AddTraceSource(new System.Diagnostics.SourceSwitch("HouseKeeping") { Level = System.Diagnostics.SourceLevels.All }, consoleListener);
					//	cfg.AddTraceSource(new System.Diagnostics.SourceSwitch("HouseKeeping") { Level = System.Diagnostics.SourceLevels.All }, listener);
					})
					.BuildServiceProvider())
				{
					logger = serviceProvider.GetService<ILogger<Program>>();
					fooService = serviceProvider.GetService<IFooService>();
				}

				MyAppTraceSources.HouseKeeping.TraceInformation("OK");

				TraceLover.DoSomething();
				TraceSourceLover.DoSomething();

				logger.LogInformation("logger information");
				logger.LogWarning("logger warning");


				var ts = new TraceSource("HouseKeeping", SourceLevels.All);
				ts.Listeners.Add(consoleListener);
				ts.Listeners.Add(listener);
				ts.TraceError("aaaaaaaaaaaaaaaaaaaaa");

				fooService.DoWork();
			}
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
