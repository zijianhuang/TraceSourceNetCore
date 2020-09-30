using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace ConsoleApp0
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World! from console");

			ILogger logger;
			IFooService fooService;

			using (var serviceProvider = new ServiceCollection()
				.AddSingleton<IFooService, FooService>()
				.AddLogging(cfg =>
				{
					cfg.AddConsole(options => options.DisableColors = true);
				})
				.BuildServiceProvider())
			{

				logger = serviceProvider.GetService<ILogger<Program>>();

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
