using System;
using System.Collections.Generic;
using Tesseract;
using Tesseract.TIM;
using Tesseract.Controls;

namespace TestApp
{
    class Program: Window
    {
        [STAThread]
        static void Main(string[] args)
        {
            string timFile = args.Length > 0 ? args[0] : "LayoutTest.xml";

            Core.Init();
            Core.Run(TIM.Load<Program>(timFile));
            Core.Done();
        }
    }
}