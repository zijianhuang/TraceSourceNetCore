using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ClassLibrary1;
using Fonlow.Diagnostics;
using System.Diagnostics;

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


			ILogger logger;
			IFooService fooService;

			using (var writerTraceListener = new TextWriterTraceListener("c:\\temp\\ConsoleAppTraceSourceLogger.txt"))
			using (var consoleListener = new ConsoleTraceListener())
			{
				using var serviceProvider = new ServiceCollection()
					.AddSingleton<IFooService, FooService>()
					.AddLogging(builder =>
					{
						builder.AddConfiguration(configuration.GetSection("Logging"));
						builder.AddTraceSource(new SourceSwitch("HouseKeeping", "All"), consoleListener);
						builder.AddTraceSource(new SourceSwitch("Something", "All"), writerTraceListener);
					})
					.BuildServiceProvider();

				logger = serviceProvider.GetService<ILogger<Program>>();
				fooService = serviceProvider.GetService<IFooService>();

				logger.LogInformation("1111logger information");
				logger.LogWarning("2222logger warning");
				//logger can actually then use consoleListener and writerTraceListener, while ConsoleProvider through AddConsole is not needed.
				//https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-2.1 mentiones TraceSourceProvider has to run on .NET Framework, but this somethis works in .NET Core console
				//.NET Core 3.1 doc does not mention this.
				//So far no info could be for how to use appsetting.json for TraceSourceProvider.

				var ts = new TraceSource("HouseKeeping", SourceLevels.All);
				ts.Listeners.Add(consoleListener);
				ts.Listeners.Add(writerTraceListener);
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
		logger.LogInformation("3333Doing work.");
		logger.LogWarning("4444Something warning");
		logger.LogCritical("5555Something critical");
	}
}
