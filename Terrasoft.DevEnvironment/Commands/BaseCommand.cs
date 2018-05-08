using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terrasoft.DevEnvironment.Commands {

	public class BaseCommand {

		protected Logger Logger = null;

		protected Context Context = null;

		protected BaseCommand Next = null;

		protected TimeSpan Elapsed = TimeSpan.Zero;

		public virtual string CommandName {
			get {
				return GetType().Name;
			}
		}

		protected virtual void InternalExecute(Context context) {

		}

		public void Execute() {
			var stopWatch = Stopwatch.StartNew();
			InternalExecute(Context);
			stopWatch.Stop();
			Elapsed = TimeSpan.FromTicks(stopWatch.Elapsed.Ticks);
			Next?.Execute();
		}

		public BaseCommand SetNext(BaseCommand command) {
			Next = command;
			command.Context = Context;
			command.Logger = Logger;
			return Next;
		}

		public (TimeSpan executionTotal, string executionReporting) GetExecutionTimeСomposite() {
			var currentСommandName = CommandName.PadRight(50, ' ');
			var currentСommandElapsed = $"{Elapsed.TotalMinutes:0} min {Elapsed.Seconds} sec";
			var currentReporting = $"{currentСommandName} - {currentСommandElapsed}{Environment.NewLine}";
			if (Next != null) {
				var (nextElapsed, nextReporting) = Next.GetExecutionTimeСomposite();
				return (Elapsed.Add(nextElapsed), currentReporting + nextReporting);
			}
			return (Elapsed, currentReporting);
		}

	}

}
