using System;
using Tesseract.Backends;

namespace Tesseract.Graphics
{
	public class Font
	{
        internal static string defaultFamily = "sans-serif";
        internal static double defaultSize = 12;

		public Font()
		{
            family = Font.defaultFamily;
            size = Font.defaultSize;
		}
		
		string family;
		public string Family
		{
			get { return family; }
			set { family = value; }
		}
		
		double size;
		public double Size
		{
			get { return size; }
			set { size = value; }
		}

        public void Apply(IGraphics g)
        {
            g.FontFamily = family;
            g.FontSize = size;
        }
	}
}
