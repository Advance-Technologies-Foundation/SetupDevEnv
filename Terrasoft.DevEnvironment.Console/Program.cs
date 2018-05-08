namespace Terrasoft.DevEnvironment.Console {

	class Program {

		static void Main(string[] args) {
			var settings = new Settings();
			settings.FillSettingsFromConfig();
			var context = new Context(settings);
			var environmentManager = new EnvironmentManager();
			environmentManager.Create(context);
		}

	}

}
