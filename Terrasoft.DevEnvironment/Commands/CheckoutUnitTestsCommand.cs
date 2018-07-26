namespace Terrasoft.DevEnvironment.Commands
{
	using Managers;
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	public class CheckoutUnitTestsCommand : BaseCommand
	{

		private void CheckoutProjects(Dictionary<string, List<string>> repos) {
			var svnManager = new SvnManager(Context.Settings.SvnUserName, Context.Settings.SvnUserPassword);
			foreach (var repo in repos) {
				foreach (var package in repo.Value) {
					var projectDirectoryPath = Path.Combine(Context.Settings.ProjectsPath, Context.ProjectDirectoryName);
					var testDirectoryPath = Path.Combine(projectDirectoryPath, BpmonlineConstants.ConfigurationCsUnitTestsPath, package);
					if (Directory.Exists(testDirectoryPath)) {
						Directory.Delete(testDirectoryPath, true);
					}
					Directory.CreateDirectory(testDirectoryPath);
					var projectUrl = repo.Key + "/" + package;
					svnManager.Checkout(projectUrl, testDirectoryPath);
				}
			}
		}

		private List<string> SplitConfigSetting(string value) {
			if (!string.IsNullOrEmpty(value)) {
				var separators = new char[] { ',', ' ', ';' };
				var packages = value.Split(separators, StringSplitOptions.RemoveEmptyEntries)
					.Distinct().ToList();
				return packages;
			} else {
				return new List<string>();
			}
		}

		private List<string> GetDevPackages() {
			if (string.Equals(Context.Settings.Packages.Trim(), "*")) {
				var databaseManager = new DbManager();
				databaseManager.MSSSQLConnectionString = Context.Settings.MSSSQLConnectionString;
				databaseManager.DataBase = Context.DatabaseName;
				var tsManager = new TerrasoftManager();
				var pkg = tsManager.GetPackages(databaseManager);
				return pkg;
			} else {
				return SplitConfigSetting(Context.Settings.Packages);
			}
		}

		private Dictionary<string, List<string>> GetUnitTestProjects(List<string> devPackages) {
			var unitTestsPath = Context.Settings.UnitTestsPath;
			var projectTestSuffix = ".UnitTests";
			var tests = Context.Settings.CSUnitTestsProjects;
			Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
			foreach (var svnPath in SplitConfigSetting(unitTestsPath)) {
				result.Add(svnPath, GetTestsPackages(devPackages, tests, projectTestSuffix, svnPath));
			}
			return result;
		}

		private List<string> GetTestsPackages(List<string> packages, string testPackagesSetting, string testPackageSuffix, string svnUrl) {
			var result = new List<string>();
			var svnManager = new SvnManager(Context.Settings.SvnUserName, Context.Settings.SvnUserPassword);
			var directories = svnManager.GetDirectories(svnUrl);
			if (string.Equals(testPackagesSetting.Trim(), "*")) {
				foreach (var package in packages) {
					var testPackageName = package + testPackageSuffix;
					if (directories.Contains(testPackageName)) {
						result.Add(testPackageName);
					}
				}
				foreach (var package in GetRequiredTestPackages()) {
					if (directories.Contains(package) && !result.Contains(package)) {
						result.Add(package);
					}
				}
				return result;
			} else {
				var splitPackages = SplitConfigSetting(testPackagesSetting);
				foreach (var package in splitPackages.Concat(GetRequiredTestPackages()).Distinct()) {
					if (directories.Contains(package)) {
						result.Add(package);
					}
				}
				return result;
			}
		}

		private string[] GetRequiredTestPackages() => new string[] {
				"UnitTest"
		};
		
		protected override void InternalExecute(Context context) {
			Logger.WriteCommand("Download C# unit test projects");
			var packages = GetDevPackages();
			var csprojects = GetUnitTestProjects(packages);
			CheckoutProjects(csprojects);
			Logger.WriteCommandSuccess();
		}

	}

}
