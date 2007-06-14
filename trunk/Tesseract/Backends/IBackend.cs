using System;
using Tesseract.Theming;

namespace Tesseract.Backends
{
    /// <summary>
    /// Interface implemented by platform specific backends
    /// </summary>
	public interface IBackend
	{
		bool CanUse();
	
		void Init();
		IWindow CreateWindow();
		void Run(IWindow w);
		void Done();
		
		IGraphics InternalGraphics();
	}
}
