using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Fonlow.Diagnostics;

namespace ClassLibrary1
{
	public static class TraceSourceLover
	{
		public static void DoSomething()
		{
			MyAppTraceSources.HouseKeeping.TraceInformation("HouseKeeping traceinfo");
			MyAppTraceSources.HouseKeeping.TraceWarning("HouseKeeping tracewarning");
		}
	}
}
