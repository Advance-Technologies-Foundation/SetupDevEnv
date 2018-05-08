using System.Dynamic;

namespace Terrasoft.DevEnvironment {
	using System.Configuration;

	public class Settings {
		

		public string SvnUserName { get; internal set; }
		public string SvnUserPassword { get; internal set; }
		public string TeamCityUrl { get; internal set; }
		public string TeamCityBuildsConfigurationUrl { get; set; }
		public string ProjectsPath { get; internal set; }
		public string BuidPath { get; internal set; }
		public string CorePath { get; internal set; }
		public string PackageStorePath { get; internal set; }
		public string MSSSQLConnectionString { get; internal set; }
		public string DatabaseNamePattern { get; internal set; }
		public string RedisConnectionString { get; internal set; }
		public string SharedDirectoryPath { get; internal set; }
		public string Packages { get; internal set; }
		public string JSUnitTestsPackages { get; internal set; }
		public string DBUnitTestsPackages { get; internal set; }
		public string CSUnitTestsProjects { get; internal set; }
		public bool ClearProjectDirectory { get; internal set; }
		public bool ClearSvnAuthenticationCache { get; internal set; }
		public bool UpdateWorkspace { get; internal set; }
		public bool BuildWorkspace { get; internal set; }
		public string DfsBuildsDirectoryPath { get;  internal set; }
		public string InfrastructureConsolePath { get; internal set; }
		public string UnitTestsPath { get; internal set; }
		public string TSqltPath { get; internal set; }

		public void FillSettingsFromConfig() {
			SvnUserName = ConfigurationManager.AppSettings["SvnUserName"];
			SvnUserPassword = ConfigurationManager.AppSettings["SvnUserPassword"];
			TeamCityUrl = ConfigurationManager.AppSettings["TeamCityUrl"];
			TeamCityBuildsConfigurationUrl = ConfigurationManager.AppSettings["TeamCityBuildsConfigurationUrl"];
			ProjectsPath = ConfigurationManager.AppSettings["ProjectsPath"];
			CorePath = ConfigurationManager.AppSettings["CorePath"];
			PackageStorePath = ConfigurationManager.AppSettings["PackageStorePath"];
			MSSSQLConnectionString = ConfigurationManager.AppSettings["MSSSQLConnectionString"];
			DatabaseNamePattern = ConfigurationManager.AppSettings["DatabaseNamePattern"];
			RedisConnectionString = ConfigurationManager.AppSettings["RedisConnectionString"];
			SharedDirectoryPath = ConfigurationManager.AppSettings["SharedDirectoryPath"];
			Packages = ConfigurationManager.AppSettings["Packages"];
			JSUnitTestsPackages = ConfigurationManager.AppSettings["JSUnitTestsPackages"];
			DBUnitTestsPackages = ConfigurationManager.AppSettings["DBUnitTestsPackages"];
			CSUnitTestsProjects = ConfigurationManager.AppSettings["CSUnitTestsProjects"];
			DfsBuildsDirectoryPath = ConfigurationManager.AppSettings["DfsBuildsDirectoryPath"];
			InfrastructureConsolePath = ConfigurationManager.AppSettings["InfrastructureConsolePath"];
			UnitTestsPath = ConfigurationManager.AppSettings["UnitTestsPath"];
			TSqltPath = ConfigurationManager.AppSettings["TSqltPath"];
			ClearProjectDirectory = bool.Parse(ConfigurationManager.AppSettings["ClearProjectDirectory"]);
			ClearSvnAuthenticationCache = bool.Parse(ConfigurationManager.AppSettings["ClearSvnAuthenticationCache"]);
			UpdateWorkspace = bool.Parse(ConfigurationManager.AppSettings["UpdateWorkspace"]);
			BuildWorkspace = bool.Parse(ConfigurationManager.AppSettings["BuildWorkspace"]);
		}

	}

}
