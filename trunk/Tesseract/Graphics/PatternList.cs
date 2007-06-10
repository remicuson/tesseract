using System;
using System.Collections.Generic;
using Tesseract.Controls;
using Tesseract.Backends;

namespace Tesseract.Graphics
{
	public class PatternList: List<Pattern>
	{
		public PatternList(Control c)
		{
			this.control = c;
		}
		
		Control control;
		public Control Control
		{
			get { return control; }
			set { control = value; }
		}
		
		public new void Add(Pattern item)
		{
			base.Add(item);
			control.AutoReRender();
		}

        public void Render(IGraphics g)
        {
            foreach (Pattern p in this)
            {
                p.Apply(g, control.Path.W, control.Path.H);
                
                if (p.Type == PatternType.Fill)
                    g.Fill();
                else
                    g.Stroke();
            }
        }
	}
}
