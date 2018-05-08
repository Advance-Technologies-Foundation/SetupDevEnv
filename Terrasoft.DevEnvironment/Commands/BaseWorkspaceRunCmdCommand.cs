using System.IO;

namespace Terrasoft.DevEnvironment.Commands
{
	public abstract class BaseWorkspaceConsoleRunCmdCommand : BaseRunCmdCommand
	{
		protected abstract string OperationName { get; set; }

		protected override string GetLocationText() {
			var projectDirectoryPath = Path.Combine(Context.Settings.ProjectsPath, Context.ProjectDirectoryName);
			return Path.Combine(projectDirectoryPath, BpmonlineConstants.WebAppDebugRelativePath);
		}

		protected override string GetExecuteCommandText() {
			var projectDirectoryPath = Path.Combine(Context.Settings.ProjectsPath, Context.ProjectDirectoryName);
			var webApplicationPath = Path.Combine(projectDirectoryPath, BpmonlineConstants.WebAppRelativePath);
			var command = $@"Terrasoft.Tools.WorkspaceConsole.exe --operation={OperationName} --workspaceName=Default --webApplicationPath=""{webApplicationPath}"" --autoExit=true";
			return command;
		}
	}
}