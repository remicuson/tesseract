using System;
using System.Collections.Generic;
using System.Text;
using Tesseract.Graphics;

namespace Tesseract.Theming
{
    internal class XMLStyleLoader: XMLLoader
    {
        public override void HandleChild(object parent, object child)
        {
            if (parent is LinearGradient && child is GradientStop)
                ((LinearGradient)parent).Add((GradientStop)child);
            else if (parent is RadialGradient && child is GradientStop)
                ((RadialGradient)parent).Add((GradientStop)child);
            else
                base.HandleChild(parent, child);
        }
    }
}
