using System;
using Tesseract.Backends;

namespace Tesseract.Graphics
{
	public class Font
	{
		public Font()
		{
			
		}
		
		string family = "sans-serif";
		public string Family
		{
			get { return family; }
			set { family = value; }
		}
		
		double size = 12;
		public double Size
		{
			get { return size; }
			set { size = value; }
		}
	}
}
