using System;
using Tesseract.Backends;
using Tesseract.Graphics;

namespace Tesseract.Controls
{
	public class Frame: Control
	{
		public Frame(): base()
		{

		}

        public override bool CanActivate()
        {
            return false;
        }
	}
}
