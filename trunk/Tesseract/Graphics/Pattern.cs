using System;
using Tesseract.Backends;
using Tesseract.Geometry;

namespace Tesseract.Graphics
{
	public enum PatternClip { None, Control, Parent }
	public enum PatternType { Fill, Stroke }
	
    /// <summary>
    /// The base of all pattern classes
    /// </summary>
	public class Pattern
	{
		Distance strokesize = 1;
        /// <summary>
        /// The size at which to stroke a path using this pattern
        /// </summary>
		public Distance StrokeSize
		{
			get { return strokesize; }
			set { strokesize = value; }
		}
		
		PatternClip clip = PatternClip.Parent;
        /// <summary>
        /// Specifies what area drawing should be clipped to whilst using this pattern
        /// </summary>
		public PatternClip Clip
		{
			get { return clip; }
			set { clip = value; }
		}
		
		PatternType type = PatternType.Fill;
        /// <summary>
        /// Specifies whether this pattern is used to fill or stroke a path
        /// </summary>
		public PatternType Type
		{
			get { return type; }
			set { type = value; }
		}

        /// <summary>
        /// Apply the pattern to the given backend graphics interface, for drawing an area of the given size
        /// </summary>
        /// <param name="g">The backend graphics interface to use</param>
        /// <param name="W">The width of the area to be drawn</param>
        /// <param name="H">The height of the area to be drawn</param>
        public virtual void Apply(IGraphics g, double W, double H)
        {

        }
	}
}
