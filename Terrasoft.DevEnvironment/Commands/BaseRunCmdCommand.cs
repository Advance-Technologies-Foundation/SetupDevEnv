using System;
using System.IO;

namespace Terrasoft.DevEnvironment.Commands
{
	public abstract class BaseRunCmdCommand : BaseCommand
	{
		protected abstract string ShortcutFileName { get; set; }
		protected virtual string RunMessageText { get; set; } = "Run command";
		protected virtual bool WaitForExit { get; set; } = true;

		protected abstract string GetExecuteCommandText();

		protected abstract string GetLocationText();

		protected virtual string GetShortcutPuth() {
			return Path.Combine(Context.Settings.ProjectsPath, Context.ProjectDirectoryName, ShortcutFileName);
		}

		private void DeleteShortcut() {
			var fullPath = GetShortcutPuth();
			if (File.Exists(fullPath)) {
				File.Delete(fullPath);
			}
		}

		protected virtual void CreateShortcut() {
			var line1 = $@"CD {GetLocationText()}" + Environment.NewLine;
			var line2 = GetExecuteCommandText();
			DeleteShortcut();
			File.AppendAllText(GetShortcutPuth(), line1 + line2);
		}

		protected abstract bool ExecuteCommand();

		protected override void InternalExecute(Context context) {
			if (!ExecuteCommand()) {
				return;
			}
			Logger.WriteCommand(RunMessageText);
			var path = GetShortcutPuth();
			if (!File.Exists(path)) {
				Logger.WriteCommand($"Create {ShortcutFileName} file.");
				CreateShortcut();
			}
			var processInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/c " + path);
			var process = System.Diagnostics.Process.Start(processInfo);
			if (WaitForExit) {
				process.WaitForExit();
				if (process.ExitCode != 0) {
					DeleteShortcut();
					throw new Exception($"Error while {GetType().Name}");
				}
			}
			DeleteShortcut();
			Logger.WriteCommandSuccess();
		}


	}
}