using System;
using System.IO;

namespace Terrasoft.DevEnvironment.Commands
{
	public class  BuildWorkspace : BaseWorkspaceConsoleRunCmdCommand
	{
		protected override string ShortcutFileName { get; set; } = "AutoBuildWorkspace.bat";
		protected override string RunMessageText { get; set; } = "Start Build configuration workspace.";
		protected override string OperationName { get; set; } = "BuildWorkspace";

		protected override bool ExecuteCommand() {
			return Context.Settings.BuildWorkspace;
		}

		
	}
}