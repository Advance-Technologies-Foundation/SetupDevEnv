namespace Terrasoft.DevEnvironment.Commands
{
	public class UpdateWorkspace : BaseWorkspaceConsoleRunCmdCommand
	{
		protected override string ShortcutFileName { get; set; } = "AutoUpdateWorkspace.bat";
		protected override string OperationName { get; set; } = "UpdateWorkspaceSolution";
		protected override string RunMessageText { get; set; } = "Start Update configuration workspace.";
		protected override bool ExecuteCommand() {
			return Context.Settings.UpdateWorkspace;
		}
	}
}