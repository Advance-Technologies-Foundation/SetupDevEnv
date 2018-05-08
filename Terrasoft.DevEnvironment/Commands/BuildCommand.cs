namespace Terrasoft.DevEnvironment.Commands {
	using System;
	using System.IO;
	using System.Reflection;

	public class BuildCommand : BaseCommand {

		private string ShortcutFileName = "BuildSolution_Debug.bat";

		private bool IsMsBuildExists() {
			return File.Exists(CommonConstants.MsBuildPath);
		}

		private string CreateBuildCommand() {
			var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var nugeExetPath = Path.Combine(assemblyFolder, "lib", "nuget.exe");
			var solutionPath = Path.Combine(Context.Settings.ProjectsPath, Context.ProjectDirectoryName, BpmonlineConstants.SolutionPath);
			string command = string.Empty;
			command += $"\"{nugeExetPath}\" restore \"{solutionPath}\"" + Environment.NewLine;
			command += $"\"{CommonConstants.MsBuildPath}\" ";
			command += $"\"{solutionPath}\" ";
			command += $"/t:Clean;Restore;Rebuild /p:Configuration=Debug /m /v:m /ignore:*.Tests.csproj";
			return command;
		}

		private string SaveBuildFileCommend() {
			string content = CreateBuildCommand();
			var path = Path.Combine(Context.Settings.ProjectsPath, Context.ProjectDirectoryName, ShortcutFileName);
			if (File.Exists(path)) {
				File.Delete(path);
			}
			File.AppendAllText(path, content);
			return path;
		}

		protected override void InternalExecute(Context context) {
			Logger.WriteCommand("Start build solution");
			if (!IsMsBuildExists()) {
				Logger.WriteCommandAddition("MSBuild not fount, command skiped.");
				return;
			}
			var command = SaveBuildFileCommend();
			var processInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/c " + command);
			var process = System.Diagnostics.Process.Start(processInfo);
			process.WaitForExit();
			if (process.ExitCode != 0) {
				throw new Exception("Error while 'Build solution'");
			};
			Logger.WriteCommandSuccess();
		}

	}

}
