namespace Terrasoft.DevEnvironment.Commands {
	using System;
	using Managers;

	public class IdentifyBuildPropertiesCommand : BaseCommand {

		protected override void InternalExecute(Context context) {
			Logger.WriteCommand("Determination build properties");
			var teamcityManager = new TeamCityManager();
			var teamcityBuildResultingUrl = $"{Context.Settings.TeamCityUrl}{Context.TeamCityBuildHref}/resulting-properties";
			var response = teamcityManager.GetDocumnt(teamcityBuildResultingUrl);
			var buildPath = response.SelectSingleNode("//property[@name='BuildPath']/@value").Value;
			var coreRevision = response.SelectSingleNode("//property[@name='dep.PrepareApplicationCore780.VCSRevision']/@value").Value;
			Context.DfsBuildPath = buildPath;
			Context.BuildCoreRevision = Int32.Parse(coreRevision);
			Logger.WriteCommandAddition($"CoreRevision: {Context.BuildCoreRevision}, BuildPath: {Context.DfsBuildPath}");
			Logger.WriteCommandSuccess();
		}

	}

}
