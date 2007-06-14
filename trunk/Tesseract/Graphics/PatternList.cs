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

        public PatternList() : this(null) { }
		
		Control control;
		public Control Control
		{
			get { return control; }
			set { control = value; }
		}
		
		public new void Add(Pattern item)
		{
			base.Add(item);

            if (control != null)
    			control.AutoReRender();
		}

        public void Render(IGraphics g)
        {
            foreach (Pattern p in this)
            {
                if (control != null)
                    p.Apply(g, control.Path.W, control.Path.H);
                else
                    p.Apply(g, 1, 1);

                if (p.Type == PatternType.Fill)
                    g.Fill();
                else
                    g.Stroke();
            }
        }
	}
}
