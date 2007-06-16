using System;
using System.Collections.Generic;
using System.Text;
using Tesseract.Graphics;

namespace Tesseract.Geometry
{
    public class TextPath: Path
    {
        public TextPath(string Text, Font Font)
        {
            this.text = Text;
            this.font = Font;
        }

        public TextPath(string Text) : this(Text, new Font()) { }
        public TextPath() : this("") { }

        string text;
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        Font font;
        public Font Font
        {
            get { return font; }
            set { font = value; }
        }

        public override Distance W
        {
            get
            {
                font.Apply(Core.internalGraphics);
                return Core.internalGraphics.TextWidth(text);
            }
            set { }
        }

        public override Distance H
        {
            get
            {
                font.Apply(Core.internalGraphics);
                return Core.internalGraphics.TextHeight(text);
            }
            set { }
        }

        public override void Apply(Tesseract.Backends.IGraphics g)
        {
            font.Apply(g);
            g.Text(text, 0, 0);
        }
    }
}
