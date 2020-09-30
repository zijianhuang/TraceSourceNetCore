using ClassLibrary1;
using Fonlow.Diagnostics;
using System;
using System.Diagnostics;

namespace ConsoleApp1
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World! from console");

			using (var listener = new TextWriterTraceListener("c:\\temp\\ConsoleAppTraceListener.txt"))
			using (var consoleListener = new ConsoleTraceListener())
			{
				Trace.Listeners.Add(listener);
				Trace.Listeners.Add(consoleListener);
				TraceSources.Instance.InitLoggerTraceListener(listener);
				TraceSources.Instance.InitLoggerTraceListener(consoleListener);

				TraceLover.DoSomething();
				TraceSourceLover.DoSomething();
			}
		}


	}

}
