using System;
using System.Runtime.InteropServices;
using Tesseract.Events;

namespace Tesseract.Backends
{
	public class GtkWindow: Gtk.Window, IWindow
	{
		public event EventHandler<MouseEventArgs> MouseMove;
		public event EventHandler<MouseEventArgs> MousePress;
		public event EventHandler<MouseEventArgs> MouseRelease;
		public event EventHandler<RenderEventArgs> Render;
		public new event EventHandler Resize;
		
		bool useGlitz;
		bool doubleBuffer = true;
		
		public GtkWindow(): base("Tesseract")
		{
			base.AppPaintable = true;
			base.SetPosition(Gtk.WindowPosition.Center);
			
			base.Events |= Gdk.EventMask.PointerMotionMask;
			base.Events |= Gdk.EventMask.ButtonPressMask;
			base.Events |= Gdk.EventMask.ButtonReleaseMask;
			
			OnScreenChanged(this.Screen);
			useGlitz = !supportAlpha; // Because XGL Interferes With Glitz
			                          // Better would be to find out if direct rendering is available
			useGlitz |= Environment.CommandLine.Contains("--use-glitz");
			useGlitz &= !Environment.CommandLine.Contains("--no-glitz");
			this.DoubleBuffered = !useGlitz;
		}
		
		public double L
		{
			get
			{
				int l = 0, t = 0;
				base.GetPosition(out l, out t);
				return l;
			}
			set { }
		}
		
		public double T
		{
			get
			{
				int l = 0, t = 0;
				base.GetPosition(out l, out t);
				return t;
			}
			set { }
		}
		
		public double W
		{
			get
			{
				int w = 0, h = 0;
				base.GetSize(out w, out h);
				return w;
			}
			set { base.WidthRequest = (int)value; }
		}
		
		public double H
		{
			get
			{
				int w = 0, h = 0;
				base.GetSize(out w, out h);
				return h;
			}
			set { base.HeightRequest = (int)value; }
		}
		
		public double DpiX
		{
			get { return base.Screen.Width / base.Screen.WidthMm; }
		}
		
		public double DpiY
		{
			get { return base.Screen.Height / base.Screen.HeightMm; }
		}
		
		public bool Framed
		{
			get { return base.HasFrame; }
			set { base.HasFrame = value; }
		}
		
		public void ReRender()
		{
			base.QueueDraw();
		}
		
		public void ReRender(double L, double T, double R, double B)
		{
			base.QueueDrawArea(
				(int)Math.Min(L, R),
				(int)Math.Min(T, B),
				(int)(Math.Max(L, R) - Math.Min(L, R)),
				(int)(Math.Max(T, B) - Math.Min(T, B)));
		}
		
		protected override bool OnExposeEvent(Gdk.EventExpose e)
		{
			if (this.Render != null)
			{
				CairoGraphics g = useGlitz ? new CairoGraphics(glitzSurface) : new CairoGraphics(e.Window);
	
				if (supportAlpha)
				{
					g.context.Save();
					g.context.Operator = Cairo.Operator.Source;
					g.context.Color = new Cairo.Color(0, 0, 0, 0);
					g.context.Fill();
					g.context.Restore();
				}
				
				this.Render(this, new RenderEventArgs(g));
				g.Dispose();
			}
			
			if (useGlitz)
			{
				ggs.Flush ();

				if (doubleBuffer)
					ggd.SwapBuffers();
				else
					ggd.Flush();
			}
			
			return true;
		}
		
		bool supportAlpha;
		protected override void OnScreenChanged(Gdk.Screen previous_screen)
		{
			base.OnScreenChanged(previous_screen);
			
			supportAlpha = this.Screen.RgbaColormap != null;
			this.Colormap = supportAlpha ? this.Screen.RgbaColormap : this.Screen.RgbColormap;
			
			if (!supportAlpha)
				Debug.Info("Screen Doesn't Support Alpha Channel");
		}
		
		protected override void OnSizeAllocated(Gdk.Rectangle allocation)
		{
			base.OnSizeAllocated(allocation);
			
			if (this.Resize != null)
				this.Resize(this, EventArgs.Empty);
				
			InitGlitz();
		}
		
		protected override void OnRealized()
		{
			base.OnRealized();
			InitGlitz();
		}

		
		protected override bool OnMotionNotifyEvent(Gdk.EventMotion evnt)
		{
			base.OnMotionNotifyEvent(evnt);
			
			if (this.MouseMove != null)
				this.MouseMove(this, new MouseEventArgs(MouseButton.None, evnt.X, evnt.Y));
			
			return true;
		}

		protected override bool OnButtonPressEvent(Gdk.EventButton evnt)
		{
			base.OnButtonPressEvent(evnt);
			
			MouseButton btn = evnt.Button == 1 ? MouseButton.Left :
							  evnt.Button == 2 ? MouseButton.Middle : MouseButton.Right;
			
			if (this.MousePress != null)
				this.MousePress(this, new MouseEventArgs(btn, evnt.X, evnt.Y));
			
			return true;
		}
		
		protected override bool OnButtonReleaseEvent(Gdk.EventButton evnt)
		{
			base.OnButtonReleaseEvent(evnt);
			
			MouseButton btn = evnt.Button == 1 ? MouseButton.Left :
							  evnt.Button == 2 ? MouseButton.Middle : MouseButton.Right;
			
			if (this.MouseRelease != null)
				this.MouseRelease(this, new MouseEventArgs(btn, evnt.X, evnt.Y));
			
			return true;
		}
		
		/* Glitz Stuff Below */
		
		static Cairo.Surface glitzSurface;

		[DllImport("libgdk-x11-2.0.so.0")]
		internal static extern IntPtr gdk_x11_get_default_xdisplay ();

		[DllImport("libgdk-x11-2.0.so.0")]
		internal static extern int gdk_x11_get_default_screen ();

		[DllImport("libgdk-x11-2.0.so.0")]
		internal static extern IntPtr gdk_x11_drawable_get_xdisplay (IntPtr handle);

		[DllImport("libgdk-x11-2.0.so.0")]
		internal static extern IntPtr gdk_drawable_get_visual (IntPtr handle);

		[DllImport("libgdk-x11-2.0.so.0")]
		internal static extern IntPtr gdk_x11_visual_get_xvisual (IntPtr handle);

		[DllImport("libgdk-x11-2.0.so.0")]
		internal static extern uint gdk_x11_drawable_get_xid (IntPtr handle);

		[DllImport("libgdk-x11-2.0.so.0")]
		internal static extern IntPtr gdkx_visual_get (uint visualid);

		[DllImport("X11")]
		internal static extern uint XVisualIDFromVisual(IntPtr visual);

		public static NDesk.Glitz.Surface ggs;
		public static NDesk.Glitz.Drawable ggd;

		void InitGlitz()
		{
			if (!useGlitz)
			{
				glitzSurface = null;
				return;
			}
			if (glitzSurface != null)
				return;
			if (this.GdkWindow == null)
				return;
			
			try
			{
				IntPtr x_drawable = this.GdkWindow.Handle;
				IntPtr dpy = gdk_x11_drawable_get_xdisplay(x_drawable);
				int scr = 0;

				IntPtr visual = gdk_drawable_get_visual(x_drawable);
				IntPtr Xvisual = gdk_x11_visual_get_xvisual(visual);
				uint XvisualID = XVisualIDFromVisual (Xvisual);
			
				IntPtr fmt = NDesk.Glitz.GlitzAPI.glitz_glx_find_drawable_format_for_visual (dpy, scr, XvisualID);

				uint win = gdk_x11_drawable_get_xid (x_drawable);
				uint w = (uint)W;
				uint h = (uint)H;

				IntPtr glitz_drawable = NDesk.Glitz.GlitzAPI.glitz_glx_create_drawable_for_window (dpy, scr, fmt, win, w, h);
				ggd = new NDesk.Glitz.Drawable (glitz_drawable);

				IntPtr glitz_format = ggd.FindStandardFormat (NDesk.Glitz.FormatName.ARGB32);

				ggs = new NDesk.Glitz.Surface (ggd, glitz_format, w, h, 0, IntPtr.Zero);
				ggs.Attach (ggd, doubleBuffer ? NDesk.Glitz.DrawableBuffer.BackColor : NDesk.Glitz.DrawableBuffer.FrontColor);

				glitzSurface = new Cairo.GlitzSurface(ggs.Handle);
			}
			catch (Exception ex)
			{
				Debug.Error("GtkWindow: Error Initializing Glitz, falling back to software rendering");
				Debug.Indent();
				Debug.Error(ex.ToString());
				Debug.DeIndent();
				
				glitzSurface = null;
				useGlitz = false;
				this.DoubleBuffered = true;
			}
		}
	}
}
