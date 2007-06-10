using System;
using Tesseract.Backends;
using Tesseract.Geometry;

namespace Tesseract.Graphics
{
	public enum PatternClip { None, Control, Parent }
	public enum PatternType { Fill, Stroke }
	
	public class Pattern
	{
		Measurement strokesize = 1;
		public Measurement StrokeSize
		{
			get { return strokesize; }
			set { strokesize = value; }
		}
		
		PatternClip clip = PatternClip.Parent;
		public PatternClip Clip
		{
			get { return clip; }
			set { clip = value; }
		}
		
		PatternType type = PatternType.Fill;
		public PatternType Type
		{
			get { return type; }
			set { type = value; }
		}

        public virtual void Apply(IGraphics g, double W, double H)
        {

        }
	}
}
