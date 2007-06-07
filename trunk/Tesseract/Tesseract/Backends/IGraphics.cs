using System;
using Tesseract.Geometry;
using Tesseract.Graphics;

namespace Tesseract.Backends
{
	public enum GraphicsOperator { Over, Source }
	
	public interface IGraphics: IDisposable
	{
		/* Operations */
		void Display(PatternList plist, Path path);
		void DisplayText(PatternList plist, Font font, string str);

		/* Settings */
		double AlphaMultiplier { get; set; }
		Path Clip { get; set; }

		/* Text */
		double TextWidth(Font font, string str);
		double TextHeight(Font font, string str);
		
		/* Transformations */
		void Translate(Location l);
		void Translate(double x, double y);
	}
}
