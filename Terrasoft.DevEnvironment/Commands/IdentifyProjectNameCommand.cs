using System;
using System.IO;
using System.Linq;

namespace Terrasoft.DevEnvironment.Commands {
	public class IdentifyProjectNameCommand : BaseCommand {

		protected override void InternalExecute(Context context) {
			Logger.WriteUserQuestion("Please, enter new environment directory name: ");
			var projectName = Console.ReadLine();
			var projectPath = Path.Combine(Context.Settings.ProjectsPath, projectName);
			if (Directory.Exists(projectPath) && Directory.EnumerateFiles(projectPath).Count() != 0) {
				if (context.Settings.ClearProjectDirectory) {
					Logger.WriteUserQuestion($"Force clear catalog {projectPath}... ");
					Directory.Delete(projectPath, true);
					Logger.WriteCommandSuccess();
				} else {
					throw new Exception("Folder alredy exists");
				}
			}
			Directory.CreateDirectory(projectPath);
			Context.ProjectDirectoryName = projectName;
		}
	}
}
