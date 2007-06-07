using System;
using Tesseract;
using Tesseract.Controls;
using Tesseract.Geometry;
using Tesseract.Graphics;
using Tesseract.TIM;

namespace TestApp
{
	public class Test: Window
	{
		public static void Main()
		{
			Core.Init();
			Window w = TIM.Load<Test>("MyGUI.xml");
			Core.Run(w);
			Core.Done();
		}
	}
}
