using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;

namespace Fonlow.Diagnostics
{
	/// <summary>
	/// Store a dictionary of TraceSource objects.
	/// </summary>
	public class TraceSources
	{
		ConcurrentDictionary<string, TraceSource> dic;

		TraceSources()
		{
			dic = new ConcurrentDictionary<string, TraceSource>();
			var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
			TraceSourceSection = config.GetSection("TraceSource");
		}

		public TraceSources(IConfigurationSection traceSourceSection)
		{
			dic = new ConcurrentDictionary<string, TraceSource>();
			TraceSourceSection = traceSourceSection;
		}

		public IConfigurationSection TraceSourceSection { get; private set; }

		/// <summary>
		/// Create a TraceSource in lazy way.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public TraceSource GetTraceSource(string name)
		{
			TraceSource r;
			if (dic.TryGetValue(name, out r))
				return r;

			if (TraceSourceSection != null)
			{
				var ss = TraceSourceSection.GetSection(name);
				if (ss != null)
				{
					var traceLevelText = ss["SourceLevels"];
					var traceLevel = StringToSourceLevels(traceLevelText);
					r = new TraceSource(name, traceLevel);
					if (traceLevel != SourceLevels.Off && listener != null)
					{
						r.Listeners.Add(listener);
					}
				}
				else
				{
					r = new TraceSource(name);
				}
			}
			else
			{
				r = new TraceSource(name);
			}

			dic.TryAdd(name, r);
			return r;
		}

		public void InitLoggerTraceListener(TraceListener listener)
		{
			this.listener = listener;
		}

		TraceListener listener;

		public bool IsTraceSourceEnabled(string name)
		{
			if (TraceSourceSection != null)
			{
				var ss = TraceSourceSection.GetSection(name);
				if (ss != null)
				{
					var traceLevelText = ss["SourceLevels"];
					var traceLevel = StringToSourceLevels(traceLevelText);
					return traceLevel != SourceLevels.Off;
				}
			}

			return false;
		}

		public TraceSource this[string name]
		{
			get
			{
				return GetTraceSource(name);
			}
		}

		public static TraceSources Instance { get { return Nested.instance; } }

		private static class Nested
		{
			static Nested()
			{
			}

			internal static readonly TraceSources instance = new TraceSources();
		}

		static SourceLevels StringToSourceLevels(string s)
		{
			switch (s)
			{
				case "All": return SourceLevels.All;
				case "Information": return SourceLevels.Information;
				case "Error": return SourceLevels.Error;
				case "Warning": return SourceLevels.Warning;
				case "Off": return SourceLevels.Off;
				case "Verbose": return SourceLevels.Verbose;
				case "Critical": return SourceLevels.Verbose;
				case "ActivityTracing": return SourceLevels.ActivityTracing;
				default:
					return SourceLevels.Off;
			}
		}
	}


}
