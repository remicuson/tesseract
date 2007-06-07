using System;
using Tesseract.Backends;
using Tesseract.Graphics;

namespace Tesseract.Controls
{
	public class Label: Control
	{
		public Label()
		{
			this.AutoSize = true;
			
			Core.themer.InitLabel(this);
		}
		
		string text;
		public string Text
		{
			get { return text; }
			set { text = value; }
		}
		
		PatternList textfill;
		public PatternList TextFill
		{
			get { return textfill; }
			set { textfill = value; }
		}
		
		public override void RenderControl(IGraphics g)
		{
			Core.themer.RenderLabel(this, g);
		}
		
		public override void HandleAutoSize()
		{
			this.Path.W = Core.internalGraphics.TextWidth(Font, text);
			this.Path.H = Core.internalGraphics.TextHeight(Font, text);
		}
	}
}
