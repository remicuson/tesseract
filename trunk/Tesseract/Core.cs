using System;
using System.Reflection;
using Tesseract.Backends;
using Tesseract.Controls;
using Tesseract.Theming;

namespace Tesseract
{
	public static class Core
	{
		internal static IBackend backend;
		internal static IGraphics internalGraphics;
        internal static IThemer defaultThemer;
		
		public static void Init()
		{
			Debug.Info("Tesseract v" + Assembly.GetExecutingAssembly().GetName().Version.ToString());
			Debug.Indent();

            TypeStore.Init();

			SelectBackend();
			
			backend.Init();
			internalGraphics = backend.InternalGraphics();
            defaultThemer = new Theming.DefaultThemer();
		}
		
		public static void Done()
		{
			internalGraphics.Dispose();
			backend.Done();
		
			Debug.DeIndent();
			Debug.Info("Tesseract Done");
		}
		
		public static void Run(Window w)
		{
			Debug.Info("Beginning Main Loop");
			backend.Run(w.backendWindow);
		}
		
		static void SelectBackend()
		{
			Type[] types = Assembly.GetExecutingAssembly().GetTypes();
			
			foreach (Type t in types)
			{
				if (t.GetInterface("IBackend") != null)
				{
					IBackend b = (IBackend)Activator.CreateInstance(t);
					
					if (!b.CanUse())
						Debug.Info("Unable to use backend " + t.Name);
					else
					{
						backend = b;
						Debug.Info("Using backend " + t.Name);
						return;
					}
				}
			}
			
			Debug.Fatal("Unable to find suitable backend");
		}
	}
}