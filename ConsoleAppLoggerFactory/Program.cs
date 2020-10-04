using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ConsoleAppLoggerDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			var configuration = new ConfigurationBuilder()
								.AddJsonFile("appsettings.json", false, true)
								.Build();

			using var loggerFactory = LoggerFactory.Create(
				builder =>
					{
						builder.AddConfiguration(configuration.GetSection("Logging"));
						builder.AddConsole();
					}
			);

			var logger = loggerFactory.CreateLogger<Program>();
			logger.LogInformation("1111logger information"); //settings in appsettings.json filters this out
			logger.LogWarning("2222logger warning");

			var fooLogger = loggerFactory.CreateLogger<FooService>();
			IFooService fooService = new FooService(fooLogger);
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
