using System;
using System.Collections.Generic;
using Tesseract.Geometry;
using Tesseract.Graphics;

namespace Tesseract.Backends
{
	public class WindowsGraphics: IGraphics
	{
		internal System.Drawing.Graphics graphics;
	
		public WindowsGraphics(System.Drawing.Graphics g, double w, double h)
		{
			graphics = g;
			
			graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
			graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
			graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			
			//graphics.TranslateTransform(1, 0);
		}
		
		public WindowsGraphics(): this(System.Drawing.Graphics.FromImage(new System.Drawing.Bitmap(10, 10)), 10, 10) { }
		
		public void Dispose()
		{
			//graphics.Dispose();
		}
		
		/* Operations */
		public void Display(PatternList plist, Path path)
		{
			System.Drawing.Drawing2D.GraphicsPath pth = GetPath(path);
			Display(plist, pth, path.W, path.H);
			pth.Dispose();
		}
		
		void Display(PatternList plist, System.Drawing.Drawing2D.GraphicsPath path, double w, double h)
		{
			Path prevClip = this.Clip;
			
			foreach (Pattern p in plist)
			{
				if (p.Clip == PatternClip.Control)
					this.Clip = plist.Control.Path;
				else if (p.Clip == PatternClip.None)
					this.Clip = null;
                else if (p.Clip == PatternClip.Parent)
                {
                    if (plist.Control.Parent == null)
                    {
                        this.Clip = null;
                    }
                    else
                    {
                        Translate(-plist.Control.renderLocation.RealL.Pixels, -plist.Control.renderLocation.RealT.Pixels);
                        this.Clip = plist.Control.Parent.Path;
                        Translate(plist.Control.renderLocation.RealL.Pixels, plist.Control.renderLocation.RealT.Pixels);
                    }
                }

				if (p.Type == PatternType.Fill)
				{
					System.Drawing.Brush b = GetBrush(p, w, h);
					graphics.FillPath(b, path);
					b.Dispose();
				}
				else if (p.Type == PatternType.Stroke)
				{
					System.Drawing.Brush b = GetBrush(p, w, h);
					System.Drawing.Pen pen = new System.Drawing.Pen(b, (float)p.StrokeSize.Pixels);
					
					if (p is Dash)
					{
                        pen.DashPattern = new float[] { (float)((Dash)p).Size, (float)((Dash)p).Size / 2 };
					}

					graphics.DrawPath(pen, path);
					
					pen.Dispose();
					b.Dispose();
				}
			}
			
			this.Clip = prevClip;
		}
		
		public void DisplayText(PatternList plist, Font font, string str)
		{
			System.Drawing.Drawing2D.GraphicsPath gpath = new System.Drawing.Drawing2D.GraphicsPath();
			System.Drawing.Font f = GetFont(font);
			System.Drawing.StringFormat format = new System.Drawing.StringFormat();

            gpath.AddString(str, f.FontFamily, (int)f.Style, f.Size, new System.Drawing.Point(0, 0), format);
			
			Display(plist, gpath, TextWidth(font, str), TextHeight(font, str));
			
			gpath.Dispose();
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
				
				graphics.ResetClip();
				
				if (clip != null)
					graphics.SetClip(GetPath(clip));
			}
		}

		/* Text */
		public double TextWidth(Font font, string str)
		{
			return graphics.MeasureString(str, GetFont(font)).Width;
		}
		
		public double TextHeight(Font font, string str)
		{
			return graphics.MeasureString(str, GetFont(font)).Height;
		}
		
		/* Transformations */
		public void Translate(Location l)
		{
			graphics.TranslateTransform((float)l.RealL, (float)l.RealT);
		}
		
		public void Translate(double x, double y)
		{
            graphics.TranslateTransform((float)x, (float)y);
		}
		
		/* Utility Methods */
		System.Drawing.Drawing2D.GraphicsPath GetPath(Path path)
		{
			System.Drawing.Drawing2D.GraphicsPath gpath = new System.Drawing.Drawing2D.GraphicsPath();
			
			if (path is Rectangle)
				gpath.AddRectangle(new System.Drawing.RectangleF(0, 0, (float)path.W - 1, (float)path.H - 1));
			else if (path is RoundedRectangle)
			{
                RoundedRectangle rr = (RoundedRectangle)path;

                gpath.AddArc(0, 0, (float)rr.RTL, (float)rr.RTL, 180, 90);
                gpath.AddArc((float)(path.W - rr.RTR - 1), 0, (float)rr.RTR, (float)rr.RTR, 270, 90);
                gpath.AddArc((float)(path.W - rr.RBR - 1), (float)(path.H - rr.RBR - 1), (float)rr.RBR, (float)rr.RBR, 0, 90);
                gpath.AddArc(0, (float)(path.H - rr.RBL - 1), (float)rr.RBL, (float)rr.RBL, 90, 90);
			}
			
			return gpath;
		}
		
		System.Drawing.Brush GetBrush(Pattern p, double w, double h)
		{
			System.Drawing.Brush brush = null;
			
			if (p is Solid)
				brush = new System.Drawing.SolidBrush(GetColor(((Solid)p).Color));
			else if (p is LinearGradient)
			{
				double x2 = ((LinearGradient)p).Orientation == LinearGradientOrientation.Vertical ? 0 : w;
				double y2 = ((LinearGradient)p).Orientation == LinearGradientOrientation.Horizontal ? 0 : h;
				
				brush = new System.Drawing.Drawing2D.LinearGradientBrush(
					new System.Drawing.Point(0, 0),
					new System.Drawing.Point((int)x2, (int)y2),
					System.Drawing.Color.Transparent,
					System.Drawing.Color.Transparent);
				
				System.Drawing.Drawing2D.ColorBlend cblend = new System.Drawing.Drawing2D.ColorBlend(((LinearGradient)p).Stops.Count);
				
				for (int i = 0; i < ((LinearGradient)p).Stops.Count; i++)
				{
					cblend.Colors[i] = GetColor(((LinearGradient)p).Stops[i].Color);
					cblend.Positions[i] = (float)((LinearGradient)p).Stops[i].Stop;
				}
				
				((System.Drawing.Drawing2D.LinearGradientBrush)brush).InterpolationColors = cblend;
			}
			else if (p is RadialGradient)
			{
				Console.WriteLine("WindowsGraphics: RadialGradient Not Implemented");
			}
			
			return brush;
		}
		
		System.Drawing.Color GetColor(Color c)
		{
			return System.Drawing.Color.FromArgb((int)(c.A * alphaMultiplier * 255), (int)(c.R * 255), (int)(c.G * 255), (int)(c.B * 255));
		}
		
		System.Drawing.Font GetFont(Font f)
		{
			System.Drawing.Font font = new System.Drawing.Font(f.Family, (float)f.Size, System.Drawing.GraphicsUnit.Pixel);
			return font;
		}
	}
}
