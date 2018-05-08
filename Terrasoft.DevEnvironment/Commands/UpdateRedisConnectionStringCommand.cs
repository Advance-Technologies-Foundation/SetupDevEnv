namespace Terrasoft.DevEnvironment.Commands {
	using System;
	using System.IO;
	using Terrasoft.DevEnvironment.Managers;

	public class UpdateRedisConnectionStringCommand : BaseCommand {

		protected override void InternalExecute(Context context) {
			Logger.WriteCommand("Update redis connection string");
			var tsManager = new TerrasoftManager();
			var connectionStringFilePath = Path.Combine(Context.Settings.ProjectsPath, Context.ProjectDirectoryName,
				@"TSBpm\Src\Lib\Terrasoft.WebApp.Loader\ConnectionStrings.config");
			var appConString = Context.Settings.RedisConnectionString.Replace("##dbname##", (new Random()).Next(1, 16).ToString());
			tsManager.UpdateRedisConnectionString(connectionStringFilePath, appConString);
			Logger.WriteCommandSuccess();
		}

	}

}
