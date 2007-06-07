using System;

namespace Tesseract
{
	public static class Debug
	{
		static string indent;
		
		static void Write(string prefix, string s)
		{
			string[] lines = s.Split(new char[] { '\n' });
			
			bool first = true;
			foreach (string line in lines)
			{
				Console.WriteLine(indent + (first ? prefix + ": " : "- ") + line);
				first = false;
			}
		}
		
		public static void Info(string s)
		{
			Write("Info", s);
		}
		
		public static void Error(string s)
		{
			Write("ERROR", s);
		}
		
		public static void Fatal(string s)
		{
			Write("FATAL", s);
			Write("FATAL", "Exiting...");
			Environment.Exit(0);
		}
		
		public static void Indent()
		{
			indent += "\t";
		}
		
		public static void DeIndent()
		{
			indent = indent.Substring(0, indent.Length - 1);
		}
	}
}
