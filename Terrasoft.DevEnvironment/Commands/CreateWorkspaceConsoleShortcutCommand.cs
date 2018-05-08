namespace Terrasoft.DevEnvironment.Commands {
	using System;
	using System.IO;

	public class CreateWorkspaceConsoleShortcutCommand : BaseCommand {

		private string fileNameTempalte = "WorkspaceConsole_{0}.bat";

		private void CreateAndSaveShortcut(string binFolder, string appPath, string operation, string saveFolder) {
			var line1 = $@"CD {binFolder}" + Environment.NewLine;
			var line2 = $@"Terrasoft.Tools.WorkspaceConsole.exe --operation={operation} --workspaceName=Default --webApplicationPath=""{appPath}""";
			var fileName = string.Format(fileNameTempalte, operation);
			var fullPath = Path.Combine(saveFolder, fileName);
			if (File.Exists(fullPath)) {
				File.Delete(fullPath);
			}
			File.AppendAllText(fullPath, line1 + line2);
		}

		protected override void InternalExecute(Context context) {
			Logger.WriteCommand("Create WorkspaceConsole shortcuts");
			var projectDirectoryPath = Path.Combine(Context.Settings.ProjectsPath, Context.ProjectDirectoryName);
			var debugFolderPath = Path.Combine(projectDirectoryPath, BpmonlineConstants.WebAppDebugRelativePath);
			var webApplicationPath = Path.Combine(projectDirectoryPath, BpmonlineConstants.WebAppRelativePath);
			CreateAndSaveShortcut(debugFolderPath, webApplicationPath, "BuildWorkspace", projectDirectoryPath);
			CreateAndSaveShortcut(debugFolderPath, webApplicationPath, "RebuildWorkspace", projectDirectoryPath);
			CreateAndSaveShortcut(debugFolderPath, webApplicationPath, "UpdateWorkspaceSolution", projectDirectoryPath);
			Logger.WriteCommandSuccess();
		}

	}

}
