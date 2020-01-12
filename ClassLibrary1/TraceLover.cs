using System;
using System.Diagnostics;

namespace ClassLibrary1
{
	public static class TraceLover
	{
		public static void DoSomething()
		{
			Trace.TraceInformation("Trace info");
			Trace.TraceWarning("Trace warning");
			Trace.WriteLine("Trace Writeline");
		}
	}
}
