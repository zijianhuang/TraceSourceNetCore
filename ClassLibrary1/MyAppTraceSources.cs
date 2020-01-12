using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using Fonlow.Diagnostics;

namespace ClassLibrary1
{

	public static class MyAppTraceSources
	{
		public static TraceSource Source(string name)
		{
			return TraceSources.Instance.GetTraceSource(name);
		}

		/// <summary>
		/// To log important DB operations, some of which may be related to BAM or Audit Trail
		/// </summary>
		public static TraceSource DbAudit
		{
			get
			{
				return TraceSources.Instance.GetTraceSource("DbAudit");
			}
		}

		/// <summary>
		/// To log basic house keeping operation of ASP.NET
		/// </summary>
		public static TraceSource HouseKeeping
		{
			get
			{
				return TraceSources.Instance.GetTraceSource("HouseKeeping");
			}
		}

		/// <summary>
		/// This one should generally be hooked to an Email trace listener which will send Email to customer.
		/// </summary>
		public static TraceSource CustomerAware
		{
			get
			{
				return TraceSources.Instance.GetTraceSource("CustomerAware");
			}
		}
	}


}

