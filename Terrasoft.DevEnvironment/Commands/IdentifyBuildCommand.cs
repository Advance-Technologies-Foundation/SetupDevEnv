namespace Terrasoft.DevEnvironment.Commands
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Xml;
	using Terrasoft.DevEnvironment.Managers;

	public class IdentifyBuildCommand : BaseCommand
	{

		protected override void InternalExecute(Context context) {
			var buildsForUser = new Dictionary<string, string>();
			var buildsPageSize = getBuildsPageSize();
			var skipBuildsCount = 0;
			var selectedIndex = -1;
			var tcManager = new TeamCityManager();
			do {
				Logger.WriteCommand("Determination builds");
				Logger.WriteUserQuestion($"Page {Math.Round(skipBuildsCount / buildsPageSize * 1.0)}");
				var restApiUrl = string.Format(Context.Settings.TeamCityBuildsConfigurationUrl, skipBuildsCount);
				var response = tcManager.GetDocumnt(Context.Settings.TeamCityUrl + restApiUrl);
				var buildsInResponse = response.SelectNodes("builds/*").GetEnumerator();
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
				Logger.WriteUserQuestion("Enter a 'number' to select build, or '+', '-' to navigate between pages: ");

				var input = Console.ReadLine();

				if (input == "+") {
					skipBuildsCount += buildsPageSize;
				} else if (input == "-") {
					if (skipBuildsCount < buildsPageSize) {
						skipBuildsCount = buildsPageSize;
					}
					skipBuildsCount -= buildsPageSize;
				} else {
					selectedIndex = Int32.Parse(input);
					break;
				}
				buildsForUser.Clear();
				Console.Clear();
			} while (selectedIndex < 0);

			Context.TeamCityBuildHref = buildsForUser.ElementAt(selectedIndex).Value;
		}

		private int getBuildsPageSize() {
			var temp = Context.Settings.TeamCityBuildsConfigurationUrl;
			var startIndex = temp.IndexOf("count:") + 6;
			temp = temp.Substring(startIndex);
			var numberString = temp.Substring(0, temp.IndexOf(','));
			return Int32.Parse(numberString);
		}
	}

}
