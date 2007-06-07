using System;
using Tesseract.Backends;
using Tesseract.Controls;
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
		
		/* Init Methods */
		public virtual void InitControl(Control c)
		{
			c.Background = new PatternList(c);
			c.ActiveBackground = new PatternList(c);
		}
		
		public virtual void InitMouseStateControl(MouseStateControl c)
		{
			InitControl(c);
			
			c.DownBackground = new PatternList(c);
			c.OverBackground = new PatternList(c);
			c.OverDownBackground = new PatternList(c);
		}
		
		public virtual void InitButton(Button btn)
		{
			InitMouseStateControl(btn);
			
			btn.Background.Add(new LinearGradient(Colors.Honeydew2, Colors.Honeydew3));
			btn.OverBackground.Add(new LinearGradient(Colors.Honeydew, Colors.Honeydew2));
			btn.DownBackground.Add(new LinearGradient(Colors.Honeydew3, Colors.Honeydew4));
			
			btn.Background.Add(new Solid(Colors.Gray, PatternType.Stroke));
		}
		
		public virtual void InitFrame(Frame frm)
		{
			InitMouseStateControl(frm);
			
			frm.Background.Add(new Solid(Colors.LightGray));
			frm.Background.Add(new Solid(Colors.Gray, PatternType.Stroke));
		}
		
		public virtual void InitHBox(HBox hbx)
		{
			InitControl(hbx);
		}
		
		public virtual void InitLabel(Label lbl)
		{
			InitControl(lbl);
			
			lbl.TextFill = new PatternList(lbl);
			lbl.TextFill.Add(new Solid(Colors.Black));
		}
		
		public virtual void InitVBox(VBox vbx)
		{
			InitControl(vbx);
		}
		
		public virtual void InitWindow(Window wnd)
		{
			InitMouseStateControl(wnd);
			
			wnd.Background.Add(new Solid(Colors.White));
		}
		
		/* Render Methods */
		public virtual void RenderControl(Control c, IGraphics g)
		{
			g.Display(c.Background, c.Path);
			
			if (c.ActiveOpacity > 0)
			{
				g.AlphaMultiplier *= c.ActiveOpacity;
				g.Display(c.ActiveBackground, c.Path);
				g.AlphaMultiplier /= c.ActiveOpacity;
			}
		}
		
		public virtual void RenderMouseStateControl(MouseStateControl c, IGraphics g)
		{
			RenderControl(c, g);
			
			if (c.DownOpacity > 0)
			{
				g.AlphaMultiplier *= c.DownOpacity;
				g.Display(c.DownBackground, c.Path);
				g.AlphaMultiplier /= c.DownOpacity;
				
				if (c.OverDownOpacity > 0)
				{
					g.AlphaMultiplier *= c.OverDownOpacity;
					g.Display(c.OverDownBackground, c.Path);
					g.AlphaMultiplier /= c.OverDownOpacity;
				}
			}
			else if (c.OverOpacity > 0)
			{
				g.AlphaMultiplier *= c.OverOpacity;
				g.Display(c.OverBackground, c.Path);
				g.AlphaMultiplier /= c.OverOpacity;
			}
		}
		
		public virtual void RenderButton(Button btn, IGraphics g)
		{
			RenderMouseStateControl(btn, g);
		}
		
		public virtual void RenderFrame(Frame frm, IGraphics g)
		{
			RenderMouseStateControl(frm, g);
		}
		
		public virtual void RenderHBox(HBox hbx, IGraphics g)
		{
			RenderControl(hbx, g);
		}
		
		public virtual void RenderLabel(Label lbl, IGraphics g)
		{
			if (string.IsNullOrEmpty(lbl.Text))
				return;
			
			double tx = (lbl.Path.W - g.TextWidth(lbl.Font, lbl.Text)) / 2;
			double ty = (lbl.Path.H - g.TextHeight(lbl.Font, lbl.Text)) / 2;
			
			g.Translate(tx, ty);
			g.DisplayText(lbl.TextFill, lbl.Font, lbl.Text);
			g.Translate(-tx, -ty);
		}
		
		public virtual void RenderVBox(VBox vbx, IGraphics g)
		{
			RenderControl(vbx, g);
		}
		
		public virtual void RenderWindow(Window wnd, IGraphics g)
		{
			RenderMouseStateControl(wnd, g);
		}
	}
}
