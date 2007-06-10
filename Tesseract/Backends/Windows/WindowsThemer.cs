using System;
using Tesseract.Backends;
using Tesseract.Controls;
using Tesseract.Theming;

namespace Tesseract.Backends
{
    public class WindowsThemer : ThemerBase
    {
        public override void InitButton(Button btn)
        {
        }

        public override void RenderButton(Button btn, IGraphics g)
        {
            if (btn.ActiveBackground != null || btn.Background != null ||
                btn.DownBackground != null || btn.OverBackground != null ||
                btn.OverDownBackground != null)
            {
                base.RenderButton(btn, g);
                return;
            }

            System.Drawing.Graphics wing = ((WindowsGraphics)g).graphics;
            System.Drawing.Rectangle bounds = new System.Drawing.Rectangle(0, 0, (int)btn.Path.W + 1, (int)btn.Path.H + 1);

            System.Windows.Forms.VisualStyles.PushButtonState state =
                btn.MouseDown ? System.Windows.Forms.VisualStyles.PushButtonState.Pressed :
                btn.MouseOver ? System.Windows.Forms.VisualStyles.PushButtonState.Hot :
                System.Windows.Forms.VisualStyles.PushButtonState.Normal;

            System.Windows.Forms.ButtonRenderer.DrawButton(wing, bounds, state);
        }
    }
}
