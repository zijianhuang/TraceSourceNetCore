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


			using (var textWriterTraceListener = new TextWriterTraceListener(@"C:\temp\ConsoleAppTraceSourceLogger2.log"))
			using (var consoleTraceListener = new ConsoleTraceListener())
			{
				using var loggerFactory = LoggerFactory.Create(builder =>
				{
					builder
						.AddConfiguration(configuration.GetSection("Logging"))
						.AddTraceSource(new SourceSwitch("Something") { Level = SourceLevels.All }, consoleTraceListener)
						.AddTraceSource(new SourceSwitch("HouseKeeping") { Level = SourceLevels.All }, textWriterTraceListener);
					// writer: Console.Out));
				});
				var logger = loggerFactory.CreateLogger<Program>();
				logger.LogInformation("LogInformation information");
				logger.LogWarning("LogWarning warning");

				MyAppTraceSources.HouseKeeping.TraceInformation("OK");

				TraceLover.DoSomething();
				TraceSourceLover.DoSomething();

				//var ts = new TraceSource("HouseKeeping", SourceLevels.All);
				//ts.Listeners.Add(consoleTraceListener);
				//ts.Listeners.Add(textWriterTraceListener);
				//ts.TraceInformation("HouseKeeping info");
				//ts.TraceEvent(TraceEventType.Error, 0, "trace error");
				//ts.TraceError("aaaaaaaaaaaaaaaaaaaaa");
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
