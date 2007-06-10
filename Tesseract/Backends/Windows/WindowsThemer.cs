using System;
using Tesseract.Backends;
using Tesseract.Controls;
using Tesseract.Theming;
using Tesseract.Graphics;

namespace Tesseract.Backends
{
    public class WindowsThemer : ThemerBase
    {
        public WindowsThemer()
        {
            Font.defaultFamily = System.Drawing.SystemFonts.DefaultFont.FontFamily.Name;
            Font.defaultSize = 12; //System.Drawing.SystemFonts.DefaultFont.SizeInPoints;
        }

        public override void InitButton(Button btn)
        {
        }

        public override void RenderButton(Button btn, IGraphics g)
        {
            System.Drawing.Graphics wing = ((WindowsGraphics)g).graphics;
            System.Drawing.Rectangle bounds = new System.Drawing.Rectangle(0, 0, (int)btn.Path.W, (int)btn.Path.H);
            System.Windows.Forms.VisualStyles.PushButtonState state = System.Windows.Forms.VisualStyles.PushButtonState.Default;

            if (btn.MouseDown)
            {
                if (btn.DownBackground != null)
                    base.RenderButton(btn, g);
                else
                    state = System.Windows.Forms.VisualStyles.PushButtonState.Pressed;
            }
            else if (btn.MouseOver)
            {
                if (btn.OverBackground != null)
                    base.RenderButton(btn, g);
                else
                    state = System.Windows.Forms.VisualStyles.PushButtonState.Hot;
            }
            else if (btn.Active)
            {
                if (btn.ActiveBackground != null)
                    base.RenderButton(btn, g);
                else
                    state = System.Windows.Forms.VisualStyles.PushButtonState.Normal;
            }
            else
            {
                if (btn.Background != null)
                    base.RenderButton(btn, g);
                else
                    state = System.Windows.Forms.VisualStyles.PushButtonState.Normal;
            }

            if (state != System.Windows.Forms.VisualStyles.PushButtonState.Default)
            {
                System.Windows.Forms.ButtonRenderer.DrawButton(wing, bounds, state);

                if (btn.Active)
                    System.Windows.Forms.ControlPaint.DrawFocusRectangle(wing, bounds);
            }
        }
    }
}
