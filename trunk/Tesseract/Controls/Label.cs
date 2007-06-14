using System;
using Tesseract.Backends;
using Tesseract.Geometry;
using Tesseract.Graphics;

namespace Tesseract.Controls
{
	public class Label: Control
	{
		public Label()
		{
			this.AutoSize = true;
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
		
		public override void HandleAutoSize()
		{
            Font.Apply(Core.internalGraphics);

			double w = Core.internalGraphics.TextWidth(text);
			double h = Core.internalGraphics.TextHeight(text);
			
			if (this.Display == DisplayMode.Flow)
			{
				for (int i = 0; i < flowChunks.Length; i++)
				{
					w = Math.Max(w, (renderFlowChunkLocations[i].RealL - renderLocation.RealL) + flowChunks[i].W);
					h = Math.Max(h, (renderFlowChunkLocations[i].RealT - renderLocation.RealT) + flowChunks[i].H);
				}
			}
			
			this.Path.W = w;
			this.Path.H = h;
		}
		
		internal Rectangle[] flowChunks;
		public override Rectangle[] GetFlowChunks()
		{
			string[] words = text.Trim().Split(new char[] { ' ' });
			
			if (words.Length == 0)
				return new Rectangle[] { new Rectangle(this.Padding.L + this.Padding.R, this.Padding.T + this.Padding.B) };
			
			double spaceWidth = 0;
			
			flowChunks = new Rectangle[words.Length];

            Font.Apply(Core.internalGraphics);

			for (int i = 0; i < flowChunks.Length; i++)
				flowChunks[i] = new Rectangle(Core.internalGraphics.TextWidth(words[i]) + spaceWidth, Core.internalGraphics.TextHeight(words[i]));
			
			return flowChunks;
		}

	}
}
