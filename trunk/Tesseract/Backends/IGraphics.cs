using System;
using Tesseract.Geometry;
using Tesseract.Graphics;

namespace Tesseract.Backends
{
	public interface IGraphics: IDisposable
	{
        /* Operations */
        void Clip();
        void Fill();
        void Stroke();

        /* Path */
        void ClearPath();
        void ClosePath();

        void Rectangle(double L, double T, double R, double B);
        void RoundedRectangle(double L, double T, double R, double B, double RTL, double RTR, double RBL, double RBR);
        void Ellipse(double L, double T, double R, double B);
        void Text(string str, double X, double Y);

        void Line(double X1, double Y1, double X2, double Y2);
        void Bezier(double X1, double Y1, double X2, double Y2, double X3, double Y3, double X4, double Y4);

        /* Pattern */
        void Solid(double A, double R, double G, double B);
        void LinearGradient(double X1, double Y1, double X2, double Y2, double[] S, double[] A, double[] R, double[] G, double[] B);
        void RadialGradient(double X1, double Y1, double R1, double X2, double Y2, double R2, double[] S, double[] A, double[] R, double[] G, double[] B);
        void Dash(double Size);

		/* Settings */
		double AlphaMultiplier { get; set; }
        double StrokeSize { get; set; }

        /* State */
        void Save();
        void Restore();

		/* Text */
        string FontFamily { get; set; }
        double FontSize { get; set; }

		double TextWidth(string str);
		double TextHeight(string str);
		
		/* Transformations */
        void Rotate(double R);
        void Scale(double X, double Y);
		void Translate(double X, double Y);
	}
}
