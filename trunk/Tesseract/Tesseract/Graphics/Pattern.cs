using System;
using Tesseract.Backends;
using Tesseract.Geometry;

namespace Tesseract.Graphics
{
	public enum PatternType { Fill, Stroke }
	
	public class Pattern
	{
		Measurement strokesize = 1;
		public Measurement StrokeSize
		{
			get { return strokesize; }
			set { strokesize = value; }
		}
		
		PatternType type = PatternType.Fill;
		public PatternType Type
		{
			get { return type; }
			set { type = value; }
		}
	}
}
