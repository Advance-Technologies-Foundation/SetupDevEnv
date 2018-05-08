namespace Terrasoft.DevEnvironment.Managers {
	using Microsoft.SqlServer.Management.Common;
	using Microsoft.SqlServer.Management.Smo;
	using System;
	using System.Data.SqlClient;
	using System.Threading.Tasks;

	public class DbManager {

		SqlConnection con = null;

		private string _MSSSQLConnectionString;

		
		public string MSSSQLConnectionString {
			set {
				_MSSSQLConnectionString = value;
			}
		}

		private string _DataBase;

		public string DataBase {
			set {
				_DataBase = value;
			}
		}

		private string DevMSSSQLConnectionString {
			get {
				if (string.IsNullOrEmpty(_DataBase)) {
					throw new Exception("Database name not set");
				}
				return CreateConnectionString(_MSSSQLConnectionString, _DataBase);
			}
			set {
				_MSSSQLConnectionString = value;
			}
		}

		private string MasterMSSSQLConnectionString {
			get {
				if (string.IsNullOrEmpty(_MSSSQLConnectionString)) {
					throw new Exception("Database ConnectionString not set");
				}
				return CreateConnectionString(_MSSSQLConnectionString, "master");
			}
		}

		private string CreateUniqueName(string dbName) {
			var resultDbName = string.Empty;
			using (con = new SqlConnection(MasterMSSSQLConnectionString)) {
				con.Open();
				for (int i = 0; i < 100; i++) {
					var dbSuffix = i > 0 ? $"_{i}" : string.Empty;
					resultDbName = $"{dbName}{dbSuffix}";
					var sqlCommand = $"SELECT name FROM master.sys.databases WHERE name = N'{resultDbName}'";
					SqlCommand cmd = new SqlCommand(sqlCommand, con);
					var result = cmd.ExecuteScalar();
					if (result == null) {
						return resultDbName;
					}
				}
			}
			throw new Exception("Can'not create unique db name");
		}

		private void GetServerDataAndLogPath(out string dataPath, out string logPath) {
			using (var con = new SqlConnection(MasterMSSSQLConnectionString)) {
				var serverConnection = new ServerConnection(con);
				var server = new Server(serverConnection);
				dataPath = string.IsNullOrEmpty(server.Settings.DefaultFile) ? server.MasterDBPath : server.Settings.DefaultFile;
				logPath = string.IsNullOrEmpty(server.Settings.DefaultLog) ? server.MasterDBLogPath : server.Settings.DefaultLog;
			}
		}

		public void RestoreDb(string dbBackupPath, string dbName, string dataDir, string logDir) {
			var sqlCommand = $@"restore database [{dbName}] from disk = '{dbBackupPath}'
				with move 'TSOnline_Data' to '{dataDir}\{dbName}.mdf',
				move 'TSOnline_Log' to '{logDir}\{dbName}.ldf'";
			using (con = new SqlConnection(MasterMSSSQLConnectionString)) {
				con.Open();
				SqlCommand cmd = new SqlCommand(sqlCommand, con);
				cmd.CommandTimeout = 600;
				cmd.ExecuteNonQuery();
			}
		}

		public string RestoreBpmonlineDbWithUniqueName(string bakFilePath, string dbName) {
			var uniqueDbName = CreateUniqueName(dbName);
			var dataFolderPath = string.Empty;
			var logFolderPath = string.Empty;
			GetServerDataAndLogPath(out dataFolderPath, out logFolderPath);
			if (string.IsNullOrEmpty(dataFolderPath) || string.IsNullOrEmpty(logFolderPath)) {
				throw new Exception("Can'not determinate sql server data or log folder path");
			}
			RestoreDb(bakFilePath, uniqueDbName, dataFolderPath, logFolderPath);
			return uniqueDbName;
		}

		public void ExecSqlScript(string sqlCommand) {
			using (con = new SqlConnection(DevMSSSQLConnectionString)) {
				con.Open();
				SqlCommand cmd = new SqlCommand(sqlCommand, con);
				cmd.CommandTimeout = 0;
				cmd.ExecuteNonQuery();
			}
		}

		public void ExecSqlScript2(string sqlCommand) {
			using (SqlConnection conn = new SqlConnection(DevMSSSQLConnectionString)) {
				Server db = new Server(new ServerConnection(conn));
				db.ConnectionContext.ExecuteNonQuery(sqlCommand);
			}
		}

		public void ExecSqlReader(string sqlCommand, Action<SqlDataReader> action) {
			using (con = new SqlConnection(DevMSSSQLConnectionString)) {
				con.Open();
				SqlCommand cmd = new SqlCommand(sqlCommand, con);
				action.Invoke(cmd.ExecuteReader());
			}
		}

		public void Ping() {
			Task t = Task.Run(() => {
				using (con = new SqlConnection(MasterMSSSQLConnectionString)) {
					con.Open();
					SqlCommand cmd = new SqlCommand("SELECT @@VERSION", con);
					cmd.ExecuteNonQuery();
				}
			});
			if (!t.Wait(TimeSpan.FromMilliseconds(3000)))
				throw new Exception("The timeout interval elapsed.");
		}

		public static string CreateConnectionString(string connectionStringPattern, string databaseName) {
			return connectionStringPattern.Replace("##dbname##", databaseName);
		}

	}


}
