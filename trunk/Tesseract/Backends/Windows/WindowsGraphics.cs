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
			graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
			graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // Initial Values

            alphamultiplier.Add(1);
            brush.Add(new System.Drawing.SolidBrush(System.Drawing.Color.Black));
            dashsize.Add(0);
            fontfamily.Add("sans-serif");
            fontsize.Add(12);
            path.Add(new System.Drawing.Drawing2D.GraphicsPath());
            strokesize.Add(1);

            //graphics.SetClip(new System.Drawing.RectangleF(0, 0, (float)w, (float)h));
		}
		
		public WindowsGraphics(): this(System.Drawing.Graphics.FromImage(new System.Drawing.Bitmap(10, 10)), 10, 10) { }
		
		public void Dispose()
		{
			
		}

        #region Lists
        List<double> alphamultiplier = new List<double>();
        List<System.Drawing.Brush> brush = new List<System.Drawing.Brush>();
        List<System.Drawing.Region> clip = new List<System.Drawing.Region>();
        List<double> dashsize = new List<double>();
        List<string> fontfamily = new List<string>();
        List<double> fontsize = new List<double>();
        List<System.Drawing.Drawing2D.GraphicsPath> path = new List<System.Drawing.Drawing2D.GraphicsPath>();
        List<double> strokesize = new List<double>();
        List<System.Drawing.Drawing2D.Matrix> transform = new List<System.Drawing.Drawing2D.Matrix>();
        #endregion

        #region Operations

        public void Clip()
        {
            graphics.SetClip(path[path.Count - 1], System.Drawing.Drawing2D.CombineMode.Intersect);
        }

        public void Fill()
        {
            graphics.FillPath(brush[brush.Count - 1], path[path.Count - 1]);
        }

        public void Stroke()
        {
            System.Drawing.Pen p = new System.Drawing.Pen(brush[brush.Count - 1], (float)strokesize[strokesize.Count - 1]);

            if (dashsize[dashsize.Count - 1] > 0)
                p.DashPattern = new float[] { (float)dashsize[dashsize.Count - 1], (float)(dashsize[dashsize.Count - 1] / 2) };
            
            graphics.DrawPath(p, path[path.Count - 1]);
            p.Dispose();
        }

        #endregion

        #region Path
        public void ClearPath()
        {
            path[path.Count - 1].Reset();
        }

        public void ClosePath()
        {
            path[path.Count - 1].CloseAllFigures();
        }

        public void Rectangle(double L, double T, double R, double B)
        {
            path[path.Count - 1].AddRectangle(new System.Drawing.RectangleF((float)L, (float)T, (float)(R - L), (float)(B - T)));
        }

        public void RoundedRectangle(double L, double T, double R, double B, double RTL, double RTR, double RBL, double RBR)
        {
            path[path.Count - 1].StartFigure();

            try
            {
                path[path.Count - 1].AddArc((float)L, (float)T, (float)RTL, (float)RTL, 180, 90);
                path[path.Count - 1].AddArc((float)(R - RTR), (float)T, (float)RTR, (float)RTR, 270, 90);
                path[path.Count - 1].AddArc((float)(R - RBR), (float)(B - RBR), (float)RBR, (float)RBR, 0, 90);
                path[path.Count - 1].AddArc((float)L, (float)(B - RBL), (float)RBL, (float)RBL, 90, 90);
            }
            catch
            {
            }

            path[path.Count - 1].CloseFigure();
        }

        public void Ellipse(double L, double T, double R, double B)
        {
            path[path.Count - 1].AddEllipse((float)L, (float)T, (float)(R - L), (float)(B - T));
        }

        public void Text(string str, double X, double Y)
        {
            System.Drawing.StringFormat format = new System.Drawing.StringFormat();
            System.Drawing.FontFamily family = null;

            try
            {
                family = new System.Drawing.FontFamily(fontfamily[fontfamily.Count - 1]);
            }
            catch
            {
                family = System.Drawing.SystemFonts.DefaultFont.FontFamily;
            }

            path[path.Count - 1].AddString(str, family, 0, (float)fontsize[fontsize.Count - 1], new System.Drawing.Point((int)X, (int)Y), format);
        }

        public void Line(double X1, double Y1, double X2, double Y2)
        {
            path[path.Count - 1].AddLine((float)X1, (float)Y1, (float)X2, (float)Y2);
        }

        public void Bezier(double X1, double Y1, double X2, double Y2, double X3, double Y3, double X4, double Y4)
        {
            path[path.Count - 1].AddBezier((float)X1, (float)Y1, (float)X2, (float)Y2, (float)X3, (float)Y3, (float)X4, (float)Y4);
        }

        #endregion

        #region Pattern

        public void Solid(double A, double R, double G, double B)
        {
            Dash(0);

            if (brush[brush.Count - 1] != null)
                brush[brush.Count - 1].Dispose();

            brush[brush.Count - 1] = new System.Drawing.SolidBrush(GetColor(A, R, G, B));
        }

        public void LinearGradient(double X1, double Y1, double X2, double Y2, double[] S, double[] A, double[] R, double[] G, double[] B)
        {
            Dash(0);

            if (brush[brush.Count - 1] != null)
                brush[brush.Count - 1].Dispose();

            System.Drawing.Drawing2D.LinearGradientBrush lgb = new System.Drawing.Drawing2D.LinearGradientBrush(
                new System.Drawing.PointF((float)X1, (float)Y1),
                new System.Drawing.PointF((float)X2, (float)Y2),
                System.Drawing.Color.Transparent,
                System.Drawing.Color.Transparent);

            System.Drawing.Drawing2D.ColorBlend cblend = new System.Drawing.Drawing2D.ColorBlend(S.Length);

            for (int i = 0; i < S.Length; i++)
            {
                cblend.Positions[i] = (float)S[i];
                cblend.Colors[i] = GetColor(A[i], R[i], G[i], B[i]);
            }

            lgb.InterpolationColors = cblend;

            brush[brush.Count - 1] = lgb;
        }

        public void RadialGradient(double X1, double Y1, double R1, double X2, double Y2, double R2, double[] S, double[] A, double[] R, double[] G, double[] B)
        {
            Dash(0);

            Debug.Info("WindowsGraphics.RadialGradient Not Implemented");
        }

        public void Dash(double Size)
        {
            dashsize[dashsize.Count - 1] = Size;
        }

        #endregion

        #region Settings

        public double AlphaMultiplier
        {
            get { return alphamultiplier[alphamultiplier.Count - 1]; }
            set { alphamultiplier[alphamultiplier.Count - 1] = value; }
        }

        public double StrokeSize
        {
            get { return strokesize[strokesize.Count - 1]; }
            set { strokesize[strokesize.Count - 1] = value; }
        }

        #endregion

        #region State

        public void Save()
        {
            alphamultiplier.Add(AlphaMultiplier);
            brush.Add(brush[brush.Count - 1]);
            dashsize.Add(dashsize[dashsize.Count - 1]);
            fontfamily.Add(FontFamily);
            fontsize.Add(FontSize);
            path.Add(path[path.Count - 1]);
            strokesize.Add(strokesize[strokesize.Count - 1]);

            clip.Add(graphics.Clip);
            transform.Add(graphics.Transform);
        }

        public void Restore()
        {
            if (alphamultiplier.Count <= 1)
                return;

            alphamultiplier.RemoveAt(alphamultiplier.Count - 1);
            brush.RemoveAt(brush.Count - 1);
            dashsize.RemoveAt(dashsize.Count - 1);
            fontfamily.RemoveAt(fontfamily.Count - 1);
            fontsize.RemoveAt(fontsize.Count - 1);
            path.RemoveAt(path.Count - 1);
            strokesize.RemoveAt(strokesize.Count - 1);

            graphics.Transform = transform[transform.Count - 1];
            transform.RemoveAt(transform.Count - 1);

            graphics.Clip = clip[clip.Count - 1];
            clip.RemoveAt(clip.Count - 1);
        }
        #endregion

        #region Text

        public string FontFamily
        {
            get { return fontfamily[fontfamily.Count - 1]; }
            set { fontfamily[fontfamily.Count - 1] = value; }
        }

        public double FontSize
        {
            get { return fontsize[fontsize.Count - 1]; }
            set { fontsize[fontsize.Count - 1] = value; }
        }

        public double TextWidth(string str)
        {
            System.Drawing.Font font = new System.Drawing.Font(FontFamily, (float)FontSize, System.Drawing.GraphicsUnit.Pixel);
            double w = graphics.MeasureString(str, font).Width;
            font.Dispose();

            return w;
        }

        public double TextHeight(string str)
        {
            System.Drawing.Font font = new System.Drawing.Font(FontFamily, (float)FontSize, System.Drawing.GraphicsUnit.Pixel);
            double h = graphics.MeasureString(str, font).Height;
            font.Dispose();

            return h;
        }
        
        #endregion

        #region Transformations
        public void Rotate(double R)
        {
            graphics.RotateTransform((float)(R * (180 / Math.PI)));
        }

        public void Scale(double X, double Y)
        {
            graphics.ScaleTransform((float)X, (float)Y);
        }

        public void Translate(double X, double Y)
        {
            graphics.TranslateTransform((float)X, (float)Y);
        }

        #endregion

        #region Utility Methods

        System.Drawing.Color GetColor(double A, double R, double G, double B)
        {
            return System.Drawing.Color.FromArgb((int)(A * alphamultiplier[alphamultiplier.Count - 1] * 255),
                                                 (int)(R * 255),
                                                 (int)(G * 255),
                                                 (int)(B * 255));
        }

        #endregion
    }
}
