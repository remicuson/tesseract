using System;
using Tesseract.Theming;

namespace Tesseract.Backends
{
	public interface IBackend
	{
		bool CanUse();
	
		void Init();
		IWindow CreateWindow();
		void Run(IWindow w);
		void Done();
		
		IGraphics InternalGraphics();
		
		IThemer NativeThemer();
	}
}
