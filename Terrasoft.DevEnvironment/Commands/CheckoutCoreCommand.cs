namespace Terrasoft.DevEnvironment.Commands {
	using Managers;
	using System.IO;

	public class CheckoutCoreCommand : BaseCommand {

		protected override void InternalExecute(Context context) {
			Logger.WriteCommand("Checkout core");
			var projectDirectoryPath = Path.Combine(Context.Settings.ProjectsPath, Context.ProjectDirectoryName);
			var svnManager = new SvnManager(context.Settings.SvnUserName, context.Settings.SvnUserPassword);
			svnManager.Checkout(Context.Settings.CorePath, Context.BuildCoreRevision, projectDirectoryPath);
			Logger.WriteCommandSuccess();
		}

	}

}
