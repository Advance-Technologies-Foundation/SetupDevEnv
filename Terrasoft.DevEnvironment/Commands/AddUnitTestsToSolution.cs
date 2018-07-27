namespace Terrasoft.DevEnvironment.Commands
{
	using System;
	using System.IO;
	using System.Linq;


	public class AddUnitTestsToSolution : BaseCommand
	{
		private void AddToSolution() {
			var projectDirectoryPath = Path.Combine(Context.Settings.ProjectsPath, Context.ProjectDirectoryName);
			var testDirectoryPath = Path.Combine(projectDirectoryPath, BpmonlineConstants.ConfigurationCsUnitTestsPath);
			var processInfo = new System.Diagnostics.ProcessStartInfo("dotnet.exe");
			processInfo.WorkingDirectory = testDirectoryPath;
			var directories = Directory.GetDirectories(testDirectoryPath, "*.UnitTests")
				.Union(Directory.GetDirectories(testDirectoryPath, "UnitTest"));
			foreach (var dir in directories) {
				var projectFile = Directory.GetFiles(dir, "*.csproj").FirstOrDefault();
				if (string.IsNullOrEmpty(projectFile)) {
					continue;
				}
				processInfo.Arguments = $"sln add \"{ projectFile  }\"";
				var process = System.Diagnostics.Process.Start(processInfo);
				process.WaitForExit();
				if (process.ExitCode != 0) {
					throw new Exception($"Error while {GetType().Name}");
				}
			}
		}

		protected override void InternalExecute(Context context) {
			Logger.WriteCommand("Add Unit Tests to Solution");

			AddToSolution();

			Logger.WriteCommandSuccess();
		}
	}
}
