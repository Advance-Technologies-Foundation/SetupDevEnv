namespace Terrasoft.DevEnvironment.Managers {
	using System.Net;
	using System.Xml;

	public class TeamCityManager {

		public XmlDocument GetDocumnt(string url) {
			XmlDocument doc = new XmlDocument();
			WebClient client = new WebClient();
			string response = client.DownloadString(url);
			doc.LoadXml(response);
			return doc;
		}

	}

}
