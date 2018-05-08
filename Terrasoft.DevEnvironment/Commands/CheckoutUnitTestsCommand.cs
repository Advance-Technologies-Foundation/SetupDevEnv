namespace Terrasoft.DevEnvironment.Commands {
	using Managers;
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	public class CheckoutUnitTestsCommand : BaseCommand {

		private void CheckoutProjects(IEnumerable<string> packages) {
			var svnManager = new SvnManager(Context.Settings.SvnUserName, Context.Settings.SvnUserPassword);
			foreach (var package in packages) {
				var projectDirectoryPath = Path.Combine(Context.Settings.ProjectsPath, Context.ProjectDirectoryName);
				var testDirectoryPath = Path.Combine(projectDirectoryPath, BpmonlineConstants.ConfigurationCsUnitTestsPath, package);
				if (Directory.Exists(testDirectoryPath)) {
					Directory.Delete(testDirectoryPath, true);
				}
				Directory.CreateDirectory(testDirectoryPath);
				var projectUrl = Context.Settings.UnitTestsPath + "/" + package;
				svnManager.Checkout(projectUrl, testDirectoryPath);
			}
		}

		private List<string> SplitPackegesSetting(string value) {
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
				var pkg = tsManager.GetPackeges(databaseManager);
				return pkg;
			} else {
				return SplitPackegesSetting(Context.Settings.Packages);
			}
		}

		private List<string> GetUnitTestProjects(List<string> devPackeges) {
			var svnPath = Context.Settings.UnitTestsPath;
			var projectTestSuffix = ".UnitTests";
			var tests = Context.Settings.CSUnitTestsProjects;
			return GetTestsPackages(devPackeges, tests, projectTestSuffix, svnPath);
		}

		private List<string> GetTestsPackages(List<string> packeges, string testPackagesSetting, string testPackegeSuffix, string svnUrl) {
			var result = new List<string>();
			var svnManager = new SvnManager(Context.Settings.SvnUserName, Context.Settings.SvnUserPassword);
			var directories = svnManager.GetDirectories(svnUrl);
			if (string.Equals(testPackagesSetting.Trim(), "*")) {
				foreach (var packege in packeges) {
					var testPackageName = packege + testPackegeSuffix;
					if (directories.Contains(testPackageName)) {
						result.Add(testPackageName);
					}
				}
				return result;
			} else {
				var packages = SplitPackegesSetting(testPackagesSetting);
				foreach (var package in packages) {
					if (directories.Contains(package)) {
						result.Add(package);
					}
				}
				return result;
			}
		}

		private List<string> AddRequaredProjects(List<string> packages) {
			var requared = new string[] {
				"UnitTest"
			};
			return packages.Concat(requared).ToList();
		}

		protected override void InternalExecute(Context context) {
			Logger.WriteCommand("Download C# unit test projects");
			var packages = GetDevPackages();
			var csprojects = GetUnitTestProjects(packages);
			csprojects = AddRequaredProjects(csprojects);
			CheckoutProjects(csprojects);
			Logger.WriteCommandSuccess();
		}

	}

}
