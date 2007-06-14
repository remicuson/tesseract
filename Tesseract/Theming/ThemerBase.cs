using System;
using Tesseract.Backends;
using Tesseract.Controls;
using Tesseract.Geometry;
using Tesseract.Graphics;

namespace Tesseract.Theming
{
	public class ThemerBase: IThemer
	{
		public ThemerBase()
		{
		}
		
		public void Dispose()
		{
			
		}
		
		public virtual void InitControl(Control c)
		{

		}

        public virtual string[] GetStyles(Control c)
        {
            return new string[0];
        }

        public virtual void SetStyle(Control c, string style)
        {

        }

		public virtual void RenderControl(Control c, IGraphics g)
		{

		}
	}
}
