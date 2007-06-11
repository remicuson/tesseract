using System;
using Tesseract.Events;
using Tesseract.Geometry;
using Tesseract.Controls;

namespace Tesseract.Backends
{
	public interface IWindow
	{
        Window Window { get; set; }

		string Title { get; set; }
		
		double L { get; set; }
		double T { get; set; }
		
		double W { get; set; }
		double H { get; set; }
		
		double DpiX { get; }
		double DpiY { get; }

		bool Framed { get; set; }
		
		void ReRender();
		void ReRender(double L, double T, double R, double B);
		
		event EventHandler<MouseEventArgs> MouseMove;
		event EventHandler<MouseEventArgs> MousePress;
		event EventHandler<MouseEventArgs> MouseRelease;
		event EventHandler<RenderEventArgs> Render;
		event EventHandler Resize;
	}
}
