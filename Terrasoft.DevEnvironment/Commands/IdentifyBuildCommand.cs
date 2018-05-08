namespace Terrasoft.DevEnvironment.Commands {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Xml;
	using Terrasoft.DevEnvironment.Managers;

	public class IdentifyBuildCommand : BaseCommand {

		protected override void InternalExecute(Context context) {
			Logger.WriteCommand("Determination builds");
			var tcManager = new TeamCityManager();
			var response = tcManager.GetDocumnt(Context.Settings.TeamCityUrl + Context.Settings.TeamCityBuildsConfigurationUrl);
			var buildsInResponse = response.SelectNodes("builds/*").GetEnumerator();
			var buildsForUser = new Dictionary<string, string>();
			while (buildsInResponse.MoveNext()) {
				var build = (XmlElement)buildsInResponse.Current;
				var number = build.GetAttribute("number");
				var href = build.GetAttribute("href");
				if (!buildsForUser.ContainsKey(number)) {
					buildsForUser.Add(number, href);
				}
			}
			Logger.WriteCommandSuccess();
			for (int i = 0; i < buildsForUser.Count; i++) {
				Logger.Write($"[{i}] - {buildsForUser.ElementAt(i).Key}");
			}
			Logger.WriteUserQuestion("Please, select build: ");
			var selectedIndex = Console.ReadLine();
			Context.TeamCityBuildHref = buildsForUser.ElementAt(Int32.Parse(selectedIndex)).Value;
		}

	}

}
