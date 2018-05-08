namespace Terrasoft.DevEnvironment.Managers {
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Xml;

    public class TerrasoftManager
    {

		public class BpmonlineAppStructure {
			public string LoaderPhysicalPath { get; set; }
			public string ApplicationPhysicalPath { get; set; }
			public string ConnectionStringPath { get; set; }
			public string LoaderWebConfigPath { get; set; }
			public string DbDeployPath { get; set; }
			public string ApplicationName { get; set; }
		}

		public string GetDatabaseBackupFilePath(string buildPath) {
			var databaseFolderPath = Path.Combine(buildPath, "db");
			var files = Directory.GetFiles(databaseFolderPath);
			if (!files.Any()) {
				throw new Exception("Db not found for deploy");
			}
			return files[0];
		}

		public void UpdateDbConnectionString(string connectionStringsConfigPath, string connectionString) {
			XmlDocument doc = new XmlDocument();
			doc.Load(connectionStringsConfigPath);
			XmlNode root = doc.DocumentElement;
			XmlNode myNode = root.SelectSingleNode("//connectionStrings/add[@name='db']");
			myNode.Attributes["connectionString"].Value = connectionString;
			doc.Save(connectionStringsConfigPath);
		}

		public void UpdateRedisConnectionString(string connectionStringsConfigPath, string connectionString) {
			XmlDocument doc = new XmlDocument();
			doc.Load(connectionStringsConfigPath);
			XmlNode root = doc.DocumentElement;
			XmlNode myNode = root.SelectSingleNode("//connectionStrings/add[@name='redis']");
			myNode.Attributes["connectionString"].Value = connectionString;
			doc.Save(connectionStringsConfigPath);
		}

		public void UpadteFileDesignMode(string webConfigPath, bool value) {
			XmlDocument doc = new XmlDocument();
			doc.Load(webConfigPath);
			var appSettingsNode = doc.DocumentElement.SelectSingleNode($"//fileDesignMode");
			appSettingsNode.Attributes["enabled"].Value = value ? "true" : "false";
			doc.Save(webConfigPath);
		}

		public void UpdateWCConnectionString(string connectionStringsConfigPath, string connectionString) {
			XmlDocument doc = new XmlDocument();
			doc.Load(connectionStringsConfigPath);
			XmlNode root = doc.DocumentElement;
			XmlNode myNode = root.SelectSingleNode("//configuration/connectionStrings/add[@name='db']");
			myNode.Attributes["connectionString"].Value = connectionString;
			doc.Save(connectionStringsConfigPath);
		}

		public void UpadteFileDesignModeWorkspaceConsole(string webConfigPath, bool value) {
			XmlDocument doc = new XmlDocument();
			doc.Load(webConfigPath);
			var appSettingsNode = doc.DocumentElement.SelectSingleNode($"//fileDesignMode");
			appSettingsNode.Attributes["enabled"].Value = value ? "true" : "false";
			doc.Save(webConfigPath);
		}

		public void SetWorkspaceConsolePrefer32Bit(string projectFilePath, bool value) {
			XmlDocument doc = new XmlDocument();
			doc.Load(projectFilePath);
			XmlNode root = doc.DocumentElement;
			var nodeValue = value ? "true" : "false";
			//https://stackoverflow.com/questions/12607895/cant-get-xmldocument-selectnodes-to-retrieve-any-of-my-nodes
			var nodes = root.SelectNodes("//*[local-name()='PropertyGroup']/*[local-name()='Prefer32Bit']");
			foreach (XmlNode node in nodes) {
				node.InnerText = nodeValue;
			}
			doc.Save(projectFilePath);
		}

		public string GetClearCultureScript() {
			var sqlText = @"
				declare tname_iter CURSOR LOCAL FORWARD_ONLY READ_ONLY STATIC FOR
				
				SELECT OBJECT_NAME(parent_object_id) as [Name]
				FROM sys.foreign_keys
				WHERE OBJECT_NAME(referenced_object_id) = 'SysCulture' and delete_referential_action = 0

				DECLARE @tableName nvarchar(max);
				DECLARE @sqlStmt nvarchar(max);
				
				open tname_iter
				FETCH NEXT FROM tname_iter INTO @tableName
				    WHILE @@FETCH_STATUS=0
				    BEGIN
				              set @sqlStmt = 'delete from ' + @tableName + ' where SysCultureId in 
								(select id from sysculture where name not in (N''ru-RU'',N''en-US''))'
				              exec(@sqlStmt);
				        FETCH NEXT FROM tname_iter INTO @tableName 
				    END
				close tname_iter;
				deallocate tname_iter;
				delete from sysculture where name not in (N'ru-RU',N'en-US')";
			return sqlText;
		}

		public string CreatePrepareDevDatabaseScript(string packageStorePath) {
			var sqlText = $@"
				INSERT INTO SysRepository ([Name], [Address], [IsActive]) 
				VALUES ('ps', '{packageStorePath}', 1);

				UPDATE SysSettingsValue
				SET TextValue = ''
				WHERE SysSettingsId IN (SELECT Id FROM SysSettings WHERE Code LIKE 'SchemaNamePrefix')

				UPDATE SysSettingsValue
				SET TextValue = '0.0.0.0'
				WHERE SysSettingsId IN (SELECT Id FROM SysSettings WHERE Code LIKE 'ConfigurationVersion')

				UPDATE [SysWorkspace]
				SET [AssemblyData] = NULL";
			return sqlText;
		}

		public string CreatePrepareDevPackageScript(string packageName) {
			var sqlText = $@"
				UPDATE SysPackage
				SET
					InstallType = 0,
					Maintainer = (SELECT TextValue FROM SysSettingsValue WHERE SysSettingsId = 
									(SELECT [Id] FROM SysSettings WHERE [Code] LIKE 'Maintainer' AND [IsPersonal] = 0) 
									AND SysAdminUnitId = 'A29A3BA5-4B0D-DE11-9A51-005056C00008'),
					SysRepositoryId = (SELECT TOP 1 Id FROM SysRepository WHERE [Name] = 'ps')
				WHERE [Name] IN (
					'{packageName}'
				)";
			return sqlText;
		}

		public List<string> GetPackeges(DbManager dbManager) {
			var result = new List<string>();
			var sqlQuery = @"SELECT [Name] 
				FROM SysPackage
				WHERE[Name] != 'Custom'";
			dbManager.ExecSqlReader(sqlQuery, (r) => {
				while (r.Read()) {
					result.Add((string)r[0]);
				}
				r.Close();
			});
			return result;
		}

	}

}
