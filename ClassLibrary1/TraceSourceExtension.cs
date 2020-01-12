using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using Fonlow.Diagnostics;

namespace Fonlow.Diagnostics
{
	public static class TraceSourceExtension
	{

		public static void TraceWarning(this TraceSource traceSource, string message)
		{
			traceSource.TraceEvent(TraceEventType.Warning, 0, message);
		}

		public static void TraceWarning(this TraceSource traceSource, string format, params object[] args)
		{
			traceSource.TraceEvent(TraceEventType.Warning, 0, format, args);
		}


		public static void TraceError(this TraceSource traceSource, string message)
		{
			traceSource.TraceEvent(TraceEventType.Error, 0, message);
		}

		public static void TraceError(this TraceSource traceSource, string format, params object[] args)
		{
			traceSource.TraceEvent(TraceEventType.Error, 0, format, args);
		}

		public static void TraceInformation(this TraceSource traceSource,
											string format, params object[] args)
		{
			traceSource.TraceInformation(format, args);
		}

		public static void TraceInformation(this TraceSource traceSource, string message)
		{
			traceSource.TraceInformation(message);
		}

		public static void WriteLine(this TraceSource traceSource, string message)
		{
			traceSource.TraceEvent(TraceEventType.Verbose, 0, message);
		}

		public static void WriteLine(this TraceSource traceSource, string format, params object[] args)
		{
			traceSource.TraceEvent(TraceEventType.Verbose, 0, format, args);
		}

		public static void TraceData
		(this TraceSource traceSource, TraceEventType eventType, int id, params object[] data)
		{
			traceSource.TraceData(eventType, id, data);
		}

		public static void TraceData
		(this TraceSource traceSource, TraceEventType eventType, int id, object data)
		{
			traceSource.TraceData(eventType, id, data);
		}

		public static void TraceEvent
		(this TraceSource traceSource, TraceEventType eventType, int id)
		{
			traceSource.TraceEvent(eventType, id);
		}

		public static void TraceEvent
		(this TraceSource traceSource, TraceEventType eventType, int id, string message)
		{
			traceSource.TraceEvent(eventType, id, message);
		}

		public static void TraceEvent
		(this TraceSource traceSource, TraceEventType eventType,
		int id, string format, params object[] args)
		{
			traceSource.TraceEvent(eventType, id, format, args);
		}

		public static void TraceTransfer
		(this TraceSource traceSource, int id, string message, Guid relatedActivityId)
		{
			traceSource.TraceTransfer(id, message, relatedActivityId);
		}

	}
}
