using System;
using System.Runtime.InteropServices;
using Tesseract.Controls;
using Tesseract.Theming;

namespace Tesseract.Backends
{
	public class GtkThemer: ThemerBase
	{
		/*Gdk.Drawable gdkDrawable;
		
		Gtk.Button gtkButton;
		IntPtr gtkButtonStyle;
		
		[DllImport("libgtk-x11-2.0.so")]
		static extern IntPtr gtk_style_attach (IntPtr raw, IntPtr window);
		
		[DllImport("libgtk-x11-2.0.so")]
		static extern void gtk_widget_ensure_style (IntPtr raw);

		[DllImport("libgtk-x11-2.0.so")]
		static extern IntPtr gtk_rc_get_style (IntPtr widget);
		
		public GtkThemer()
		{
			gtkButton = new Gtk.Button();
			gtk_widget_ensure_style(gtkButton.Handle);
			gtkButtonStyle = gtk_rc_get_style(gtkButton.Handle);
		}
		
		public override void InitButton (Button btn)
		{
			
		}
		
		public override void RenderButton(Button btn, IGraphics g)
		{
			if ((btn.Background != null) || (btn.ActiveBackground != null) || (btn.OverBackground != null)
			    || (btn.DownBackground != null) || (btn.OverDownBackground != null))
			{
				base.RenderButton(btn, g);
				return;
			}
			
			Cairo.Context cc = ((CairoGraphics)g).context;
			
			int w = (int)btn.Path.W.Pixels;
			int h = (int)btn.Path.H.Pixels;
			
			gdkDrawable = new Gdk.Pixmap(null, w, h, 24);
			
			Gtk.Style style = gtkButton.Style;
			
			Gtk.StateType stateType = Gtk.StateType.Normal;
			Gtk.ShadowType shadowType = Gtk.ShadowType.Out;
			string detail = "buttondefault";
			
			if (btn.MouseOver)
			{
				stateType = Gtk.StateType.Active;
				shadowType = Gtk.ShadowType.In;
				detail = "button";
			}
			
			int l = 175;
			int t = 125;
			Gtk.Style.PaintBox(style, gdkDrawable, stateType, shadowType, Gdk.Rectangle.Zero, gtkButton, detail, l, t, w, h);
			
			Gdk.Pixbuf pb = Gdk.Pixbuf.FromDrawable(gdkDrawable, gdkDrawable.Colormap, l, t, 0, 0, w, h);
			
			byte[] buffer = pb.SaveToBuffer("bmp");
			
			System.IO.File.WriteAllBytes("test.bmp", buffer);
			
			cc.Pattern = new Cairo.SurfacePattern(new Cairo.ImageSurface(ref buffer, Cairo.Format.ARGB32, w, h, 0));
			cc.Rectangle(0, 0, btn.Path.W, btn.Path.H);
			cc.Fill();
			
			style.Detach();
			
			pb.Dispose();
			gdkDrawable.Dispose();
		}*/

	}
}
