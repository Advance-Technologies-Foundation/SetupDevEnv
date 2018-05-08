using System.IO;

namespace Terrasoft.DevEnvironment.Commands
{
	public class RunTsLib : BaseRunCmdCommand
	{
		protected override string ShortcutFileName { get; set; } = "RunTSLib.bat";
		protected override bool WaitForExit { get; set; } = false;
		protected override string GetExecuteCommandText() {
			return "TSLib.cmd";
		}

		protected override string GetLocationText() {
			return Path.Combine(Context.Settings.ProjectsPath, Context.ProjectDirectoryName);
		}

		protected override bool ExecuteCommand() {
			return true;
		}

		protected override string RunMessageText { get; set; } = "Run TSLib.cmd";
	}
}