namespace Terrasoft.DevEnvironment.Commands {
	using Terrasoft.DevEnvironment.Managers;

	public class DeleteSvnAuthenticationCachedItemsCommand : BaseCommand {

		protected override void InternalExecute(Context context) {
			if (!context.Settings.ClearSvnAuthenticationCache) {
				return;
			}
			Logger.WriteCommand("Delete Svn authentication cached items");
			var svnManager = new SvnManager(context.Settings.SvnUserName, context.Settings.SvnUserPassword);
			svnManager.DeletenAuthenticationCachedItems();
			Logger.WriteCommandSuccess();
		}

	}

}
