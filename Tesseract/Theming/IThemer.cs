using System;
using Tesseract.Backends;
using Tesseract.Controls;

namespace Tesseract.Theming
{
	public interface IThemer: IDisposable
	{
        void InitControl(Control c);
        
        string[] GetStyles(Control c);
        void SetStyle(Control c, string style);

        void RenderControl(Control c, IGraphics g);
	}
}
