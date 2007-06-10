using System;
using System.Collections.Generic;
using Tesseract.Controls;

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
	}
}
