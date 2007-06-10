using System;
using Tesseract.Backends;
using Tesseract.Controls;

namespace Tesseract.Theming
{
	public interface IThemer: IDisposable
	{
		void InitButton(Button btn);
		void InitFrame(Frame frm);
		void InitHBox(HBox hbx);
		void InitLabel(Label lbl);
		void InitVBox(VBox vbx);
		void InitWindow(Window wnd);
		
		void RenderButton(Button btn, IGraphics g);
		void RenderFrame(Frame frm, IGraphics g);
		void RenderHBox(HBox hbx, IGraphics g);
		void RenderLabel(Label lbl, IGraphics g);
		void RenderVBox(VBox vbx, IGraphics g);
		void RenderWindow(Window wnd, IGraphics g);
	}
}
