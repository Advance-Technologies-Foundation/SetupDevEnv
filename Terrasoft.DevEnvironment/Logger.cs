using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terrasoft.DevEnvironment {

	public class Logger {

		internal void WriteCommand(string value) {
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write($"{value}... ");
			Console.ResetColor();
		}

		internal void WriteCommandAddition(string value) {
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write($"{value}");
			Console.ResetColor();
		}

		internal void WriteCommandAdditionLine(string value) {
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine($"{value}");
			Console.ResetColor();
		}

		internal void WriteCommandSuccess() {
			Console.ForegroundColor = ConsoleColor.DarkGreen;
			Console.WriteLine("   Done");
			Console.ResetColor();
		}

		internal void Write(string value) {
			Console.WriteLine(value);
		}

		internal void WriteUserQuestion(string value) {
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write(value);
			Console.ResetColor();
		}

		internal void WriteError(Exception value) {
			Console.WriteLine(Environment.NewLine);
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(value is SoftCommonException ? value.Message : value.ToString());
			Console.ResetColor();
		}

	}

}
