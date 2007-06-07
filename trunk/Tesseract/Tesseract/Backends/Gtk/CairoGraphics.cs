using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Tesseract.Geometry;
using Tesseract.Graphics;

namespace Tesseract.Backends
{
	public class CairoGraphics: IGraphics, IDisposable
	{
		internal Cairo.Context context;
		
		[DllImport("libgdk-x11-2.0.so")]
		internal static extern IntPtr gdk_cairo_create (IntPtr raw);

		public CairoGraphics(Gdk.Window w)
		{
			context = new Cairo.Context(gdk_cairo_create(w.Handle));
		}
		
		public CairoGraphics(Cairo.Surface s)
		{
			context = new Cairo.Context(s);
		}
		
		public CairoGraphics(): this(new Cairo.ImageSurface(Cairo.Format.ARGB32, 10, 10)) { }
		
		public void Dispose()
		{
			((IDisposable)context.Target).Dispose();
			((IDisposable)context).Dispose();
		}
		
		/* Operations */
		public void Display(PatternList plist, Path path)
		{
			if (path != null)
				ApplyPath(path);
			
			foreach (Pattern p in plist)
			{
				ApplyPattern(p, path.W, path.H);
				
				//Console.WriteLine("{0} {1}x{2} with {3}", p.Type, path.W.Pixels, path.H.Pixels, p);
				
				if (p.Type == PatternType.Fill)
					context.FillPreserve();
				else if (p.Type == PatternType.Stroke)
				{
					context.LineWidth = p.StrokeSize;
					context.StrokePreserve();
				}
			}
		}
		
		public void DisplayText(PatternList plist, Font font, string str)
		{
			ApplyFont(font);
			
			context.NewPath();
			context.TextPath(str);
			
			Display(plist, null);
		}

		/* Settings */
		double alphaMultiplier = 1;
		public double AlphaMultiplier
		{
			get { return alphaMultiplier; }
			set
			{
				alphaMultiplier = value;
				
				if (double.IsNaN(alphaMultiplier))
					alphaMultiplier = 1;
			}
		}
		
		Path clip;
		public Path Clip
		{
			get { return clip; }
			set
			{
				clip = value;
				
				context.ResetClip();
				
				if (clip != null)
				{
					ApplyPath(clip);
					context.ClipPreserve();
				}
			}
		}

		/* Text */
		public double TextWidth(Font font, string str)
		{
			ApplyFont(font);
			return context.TextExtents(str).Width;
		}
		
		public double TextHeight(Font font, string str)
		{
			ApplyFont(font);
			return context.TextExtents(str).Height;
		}
		
		/* Transformations */
		public void Translate(Location l)
		{
			context.Translate(l.RealL, l.RealT);
		}
		
		public void Translate(double x, double y)
		{
			context.Translate(x, y);
		}
		
		/* Utility Methods */
		
		double ToPoint5(double d)
		{
			return Math.Truncate(d) + 0.5;
		}
		
		void ApplyPath(Path p)
		{
			context.NewPath();
			
			if (p is Rectangle)
				context.Rectangle(0.5, 0.5, (int)(p.W - 1), (int)(p.H - 1));
			else if (p is RoundedRectangle)
			{
				RoundedRectangle rr = (RoundedRectangle)p;
				
				context.MoveTo(0.5, ToPoint5(rr.RTL));
				context.CurveTo(0.5, 0.5, 0.5, 0.5, ToPoint5(rr.RTL), 0.5);
				context.LineTo(ToPoint5(rr.W - rr.RTR - 1), 0.5);
				context.CurveTo(ToPoint5(rr.W - 1), 0.5, ToPoint5(rr.W - 1), 0.5, ToPoint5(rr.W - 1), ToPoint5(rr.RTR));
				context.LineTo(ToPoint5(rr.W - 1), ToPoint5(rr.H - rr.RBR - 1));
				context.CurveTo(ToPoint5(rr.W - 1), ToPoint5(rr.H - 1), ToPoint5(rr.W - 1), ToPoint5(rr.H - 1), ToPoint5(rr.W - rr.RBR - 1), ToPoint5(rr.H - 1));
				context.LineTo(ToPoint5(rr.RBL), ToPoint5(rr.H - 1));
				context.CurveTo(0.5, ToPoint5(rr.H - 1), 0.5, ToPoint5(rr.H - 1), 0.5, ToPoint5(rr.H - rr.RBL - 1));
				context.ClosePath();
			}
		}
		
		void ApplyPattern(Pattern p, double w, double h)
		{
			// Reset Dash
			context.SetDash(new double[0], 0);
			
			if (p is Dash)
			{
				context.Color = GetColor(((Dash)p).Color);
				context.SetDash(new double[] { ((Dash)p).Size }, 0);
			}
			else if (p is Solid)
				context.Color = GetColor(((Solid)p).Color);
			else if (p is LinearGradient)
			{
				double x2 = ((LinearGradient)p).Orientation == LinearGradientOrientation.Vertical ? 0 : w;
				double y2 = ((LinearGradient)p).Orientation == LinearGradientOrientation.Horizontal ? 0 : h;
				
				Cairo.LinearGradient grad = new Cairo.LinearGradient(0, 0, x2, y2);
				
				for (int i = 0; i < ((LinearGradient)p).Stops.Count; i++)
					grad.AddColorStop(((LinearGradient)p).Stops[i].Stop, GetColor(((LinearGradient)p).Stops[i].Color));
								
				context.Pattern = grad;
			}
			else if (p is RadialGradient)
			{
				RadialGradient rg = (RadialGradient)p;
				
				Cairo.RadialGradient grad = new Cairo.RadialGradient(rg.X1, rg.Y1, rg.R1, rg.X2, rg.Y2, rg.R2);
				
				for (int i = 0; i < rg.Stops.Count; i++)
					grad.AddColorStop(rg.Stops[i].Stop, GetColor(rg.Stops[i].Color));
								
				context.Pattern = grad;
			}
		}
		
		void ApplyFont(Font f)
		{
			context.SelectFontFace(f.Family, Cairo.FontSlant.Normal, Cairo.FontWeight.Normal);
			context.SetFontSize(f.Size);
		}
		
		Cairo.Color GetColor(Color c)
		{
			return new Cairo.Color(c.R, c.G, c.B, c.A * alphaMultiplier);
		}
	}
}
