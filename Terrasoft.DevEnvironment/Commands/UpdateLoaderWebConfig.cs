namespace Terrasoft.DevEnvironment.Commands {
	using Managers;
	using System.IO;

	public class UpdateLoaderWebConfig : BaseCommand {

		protected override void InternalExecute(Context context) {
			Logger.WriteCommand("Update loader web.config");
			var tsManager = new TerrasoftManager();
			var webConfigFilePath = Path.Combine(Context.Settings.ProjectsPath, Context.ProjectDirectoryName,
				BpmonlineConstants.WebAppLoaderWebConfigRelativePath);
			tsManager.UpdateFileDesignMode(webConfigFilePath, true);
			tsManager.UpdateDbGeneralSection(webConfigFilePath);
			Logger.WriteCommandSuccess();
		}

	}

}
