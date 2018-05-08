using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Terrasoft.DevEnvironment.Commands;
using Terrasoft.DevEnvironment.Managers;

namespace Terrasoft.DevEnvironment {

	public class EnvironmentManager {

		private Context _context = null;

		private Logger _log = new Logger();

		private BaseCommand _createCommand = null;

		private void PrintResultInfo() {
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine();
			Console.WriteLine("***************************");
			Console.WriteLine("For correct debug mode UNCHECK following parameter in VS:");
			Console.WriteLine("Tools -> Options -> Debugging -> General -> Suppress JIT optimization on module load (Managed only)");
			Console.WriteLine("***************************");
			Console.WriteLine();
			Console.WriteLine("***************************");
			Console.WriteLine("Next: ");
			Console.WriteLine("1) Rebuild solution ");
			Console.WriteLine("2) Run 'Update packages from file system' ");
			Console.WriteLine("3) Run 'WorkspaceConsole_UpdateWorkspaceSolution.bat'");
			Console.WriteLine("4) Run 'WorkspaceConsole_RebuildWorkspace.bat'");
			Console.WriteLine("***************************");
			Console.ResetColor();
			var executionTime = _createCommand.GetExecutionTimeСomposite().executionReporting;
			var executionTotal = _createCommand.GetExecutionTimeСomposite().executionTotal;
			Console.WriteLine(executionTime);
			Console.WriteLine($"TOTAL: {executionTotal.TotalMinutes:0} min {executionTotal.Seconds} sec");
		}

		private void CleanUp() {
			_log.WriteCommand("Clean temp data");
			var cleanManager = new CleanManager(_context, _log);
			cleanManager.CleanLocalCopyBuild();
			cleanManager.CleanTempUnzippedBuildDirectory();
			_log.WriteCommandSuccess();
		}

		private void InternalCreate() {
			_createCommand = new StartCommand(_context, _log);
			_createCommand
				.SetNext(new ValidationCommand())
				.SetNext(new IdentifyProjectNameCommand())
				.SetNext(new IdentifyBuildCommand())
				.SetNext(new IdentifyBuildPropertiesCommand())
				.SetNext(new DownloadBuildCommand())
				.SetNext(new UnzipBuildCommand())
				.SetNext(new RestoreDatabaseCommand())
				.SetNext(new CheckoutCoreCommand())
				.SetNext(new UpdateDatabaseConnectionStringCommand())
				.SetNext(new UpdateRedisConnectionStringCommand())
				.SetNext(new EnableFileDesignModeCommand())
				.SetNext(new PrepareDevDatabaseCommand())
				.SetNext(new ClearCulturesFromDatabaseCommand())
				.SetNext(new UnlockDevPackagesInDatabaseCommand())
				.SetNext(new CreateWorkspaceConsoleShortcutCommand())
				.SetNext(new UpdateWCDatabaseConnectionStringCommand())
				.SetNext(new EnableFileDesignModeWorkspaceConsoleCommand())
				.SetNext(new SetWorkspaceConsolePrefer32BitCommand())
				.SetNext(new CheckoutPackagesCommand())
				.SetNext(new CheckoutUnitTestsCommand())
				.SetNext(new BuildCommand())
				.SetNext(new UpdateWorkspace())
				.SetNext(new BuildWorkspace())
				.SetNext(new RunTsLib());
			_createCommand.Execute();
		}

		public void Create(Context context) {
			_context = context;
			var isSuccess = false;
			try {
				InternalCreate();
				isSuccess = true;
			} catch (Exception ex) {
				_log.WriteError(ex);
			} finally {
				StartCommand startCommand = new StartCommand(_context, _log);
				startCommand
					.SetNext(new DeleteTempFilesCommand())
					.SetNext(new DeleteSvnAuthenticationCachedItemsCommand());
				startCommand.Execute();
				if (isSuccess) {
					PrintResultInfo();
					Console.WriteLine("Work successfully done");
				}
			}
			Console.ReadLine();
		}

	}

}
