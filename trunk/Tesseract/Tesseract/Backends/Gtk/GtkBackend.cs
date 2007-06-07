using System;
using Tesseract.Theming;

namespace Tesseract.Backends
{
	public class GtkBackend: IBackend
	{
		GtkWindow mainWindow;
		
		public GtkBackend()
		{
		}
		
		public bool CanUse()
		{
			if (Environment.OSVersion.Platform != PlatformID.Unix)
				return false;
			
			string[] args = Environment.GetCommandLineArgs();
			return Gtk.Application.InitCheck("Tesseract", ref args);
		}
	
		public void Init()
		{
			Gtk.Application.Init();
		}
		
		public IWindow CreateWindow()
		{
			return new GtkWindow();
		}
		
		public void Run(IWindow w)
		{
			mainWindow = (GtkWindow)w;
			mainWindow.Show();
			
			mainWindow.DeleteEvent += delegate { Done(); };
			
			done = false;
			Gtk.Application.Run();
		}
		
		bool done;
		public void Done()
		{
			if (!done)
				Gtk.Application.Quit();
			
			done = true;
		}
		
		public IGraphics InternalGraphics()
		{
			return new CairoGraphics();
		}
		
		public IThemer NativeThemer()
		{
			return new GtkThemer();
		}
	}
}
