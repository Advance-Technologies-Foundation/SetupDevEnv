namespace Terrasoft.DevEnvironment.Commands {
	using Managers;
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	public class CheckoutPackagesCommand : BaseCommand {

		private string ShortcutFileName = "InfrastructureConsole_DownloadPackages.bat";

		private string CreateShortcutContent(IEnumerable<string> packages) {
			var workingCopyPath = Path.Combine(Context.Settings.ProjectsPath, Context.ProjectDirectoryName, BpmonlineConstants.ConfigurationPkgRelativePath);
			var consolePath = Path.Combine(Context.Settings.ProjectsPath, InfrastructureConsoleConstants.DirectoryName, InfrastructureConsoleConstants.ConsoleRelativePath);
			var packagesList = string.Join(",", packages);
			var line1 = $"{consolePath} ";
			var line2 = $"operation=DownloadPackages repositoryUri={Context.Settings.PackageStorePath} ";
			var line3 = $"workingCopyPath=\"{workingCopyPath}\" packages={packagesList} ";
			var line4 = $"threadCount=10 version=7.8.0 updateExists=true downloadDependOnPackages=false ";
			var line5 = $"userName={Context.Settings.SvnUserName} password={Context.Settings.SvnUserPassword}";
			return line1 + line2 + line3 + line4 + line5;
		}

		private string SaveShortcut(string content) {
			var path = Path.Combine(Context.Settings.ProjectsPath, Context.ProjectDirectoryName, ShortcutFileName);
			if (File.Exists(path)) {
				File.Delete(path);
			}
			File.AppendAllText(path, content);
			return path;
		}

		private void RunInfrastructureConsoleDownloadPackages(string filePath) {
			Logger.WriteCommandAddition($" Run '{ShortcutFileName}'");
			var processInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/c " + filePath);
			var process = System.Diagnostics.Process.Start(processInfo);
			process.WaitForExit();
			if (process.ExitCode != 0) {
				throw new Exception("Error while 'DownloadPackages'");
			};
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

		private List<string> AddСompulsoryPackages(List<string> packages, string package) {
			if (packages.Any() && !packages.Contains(package)) {
				packages.Add(package);
			}
			return packages;
		}

		private List<string> GetDbTestPackages(List<string> softkeyPackages) {
			var svnPath = Context.Settings.PackageStorePath;
			var jsTestSuffix = ".DBTests";
			var tests = Context.Settings.DBUnitTestsPackages;
			var packages = GetTestsPackages(softkeyPackages, tests, jsTestSuffix, svnPath);
			return packages;
		}

		private List<string> GetTestsPackages(List<string> devPackeges, string testPackagesSetting, string testPackegeSuffix, string svnUrl) {
			if (string.Equals(testPackagesSetting.Trim(), "*")) {
				var result = new List<string>();
				var svnManager = new SvnManager(Context.Settings.SvnUserName, Context.Settings.SvnUserPassword);
				var directorues = svnManager.GetDirectories(svnUrl);
				foreach (var packege in devPackeges) {
					var testPackageName = packege + testPackegeSuffix;
					if (directorues.Contains(testPackageName)) {
						result.Add(testPackageName);
					}
				}
				return result;
			} else {
				return SplitPackegesSetting(testPackagesSetting);
			}
		}

		protected override void InternalExecute(Context context) {
			Logger.WriteCommand("Create InfrastructureConsole shortcut");
			var packages = GetDevPackages();
			var dbTestsPackages = GetDbTestPackages(packages);
			var allPackages = packages.Concat(dbTestsPackages);
			var shortcutContent = CreateShortcutContent(allPackages);
			var path = SaveShortcut(shortcutContent);
			Logger.WriteCommandAddition($"Shortcut path: {path}");
			if (packages.Any()) {
				RunInfrastructureConsoleDownloadPackages(path);
			}
			if (dbTestsPackages.Any()) {
				(new StartCommand(Context, null))
					.SetNext(new InstallTsqltCommand())
					.Execute();
			}
			Logger.WriteCommandSuccess();
		}

	}

}
