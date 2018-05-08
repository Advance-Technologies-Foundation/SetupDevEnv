using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terrasoft.DevEnvironment.Managers
{
	public class CleanManager
	{

		public CleanManager(Context context, Logger logger) {
			Context = context;
			Logger = logger;
		}

		public Context Context { get; }
		public Logger Logger { get; }

		public void CleanLocalCopyBuild() {
			if (File.Exists(Context.LocalCopyBuildPath)) {
				Logger.WriteCommandAdditionLine($"Remove: {Context.LocalCopyBuildPath} ");
				File.Delete(Context.LocalCopyBuildPath);
				Logger.WriteCommandSuccess();
			}
		}

		public void CleanTempUnzippedBuildDirectory() {
			if (Directory.Exists(Context.TempUnzippedBuildDirectory)) {
				Logger.WriteCommandAdditionLine($"Remove: {Context.TempUnzippedBuildDirectory} ");
				Directory.Delete(Context.TempUnzippedBuildDirectory, true);
				Logger.WriteCommandSuccess();
			}
		}
	}
}
