namespace Terrasoft.DevEnvironment {

	public class Context {

		public Settings Settings { get;  set; }
		public string TeamCityBuildHref { get; internal set; }
		public string DfsBuildPath { get; internal set; }
		public int BuildCoreRevision { get; internal set; }
		public string LocalCopyBuildPath { get; internal set; }
		public string TempUnzippedBuildDirectory { get; internal set; }
		public string ProjectDirectoryName { get; internal set; }
		public string DatabaseName { get; internal set; }

		public Context(Settings settings) {
			Settings = settings;
		}

	}

}
