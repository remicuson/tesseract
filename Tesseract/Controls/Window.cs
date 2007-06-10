using System;
using Tesseract.Backends;
using Tesseract.Events;
using Tesseract.Geometry;
using Tesseract.Graphics;

namespace Tesseract.Controls
{
	public class Window: MouseStateControl
	{
		internal IWindow backendWindow;
		
		internal Control mouseOverControl;
		internal Control mouseDownControl;
		
		public Window()
		{
            backendWindow = Core.backend.CreateWindow();
            backendWindow.Render += new EventHandler<RenderEventArgs>(backendRender);
            backendWindow.Resize += new EventHandler(backendResize);
            backendWindow.MouseMove += new EventHandler<MouseEventArgs>(backendMouseMove);
            backendWindow.MousePress += new EventHandler<MouseEventArgs>(backendMousePress);
            backendWindow.MouseRelease += new EventHandler<MouseEventArgs>(backendMouseRelease);
			
            this.Path = new Rectangle(400, 300);
            
            this.renderLocation = new Location(this, 0, 0, null, null);
            this.Font = new Font();
            
            Core.themer.InitWindow(this);
		}

        void backendMouseMove(object sender, MouseEventArgs e)
        {
            Control orig = mouseOverControl;
            _MouseMoveRecursive(this, e.X, e.Y);

            if (orig != mouseOverControl)
            {
                if (orig != null)
                    orig.OnMouseLeave();

                if (mouseOverControl != null)
                    mouseOverControl.OnMouseEnter();
            }

            if (mouseOverControl != null)
                mouseOverControl.OnMouseMove(new MouseEventArgs(MouseButton.None, e.X - mouseOverControl.OffsetLocation.RealL, e.Y - mouseOverControl.OffsetLocation.RealT));
        }

        void backendMousePress(object sender, MouseEventArgs e)
        {
            if (mouseOverControl != null)
            {
                mouseDownControl = mouseOverControl;
                mouseOverControl.OnMousePress(new MouseEventArgs(e.Button, e.X - mouseOverControl.OffsetLocation.RealL, e.Y - mouseOverControl.OffsetLocation.RealT));
            }
        }

        void backendMouseRelease(object sender, MouseEventArgs e)
        {
            if (mouseDownControl != null)
            {
                mouseDownControl.OnMouseRelease(new MouseEventArgs(e.Button, e.X - mouseDownControl.OffsetLocation.RealL, e.Y - mouseOverControl.OffsetLocation.RealT));
                mouseDownControl = null;
            }
        }

        void backendRender(object sender, RenderEventArgs e)
        {
            OnRender(e);
        }

        void backendResize(object sender, EventArgs e)
        {
            base.Path = new Rectangle(backendWindow.W, backendWindow.H);
        }
		
		public double DpiX
		{
			get { return backendWindow.DpiX; }
		}
		
		public double DpiY
		{
			get { return backendWindow.DpiY; }
		}
		
		public string Title
		{
			get { return backendWindow.Title; }
			set { backendWindow.Title = value; }
		}
		
		public override Path Path
		{
			get { return base.Path; }
			set
			{
				base.Path = value;
				backendWindow.W = value.W.Pixels;
				backendWindow.H = value.H.Pixels;
			}
		}
		
		public override void ReRender()
		{
			backendWindow.ReRender();
		}
		
		public void ReRender(double L, double T, double R, double B)
		{
			backendWindow.ReRender(L, T, R, B);
		}
		
		bool _MouseMoveRecursive(Control c, double X, double Y)
		{
			if (!c.Visible)
				return false;
			
			if (c.Path.Contains(X, Y))
			{
				bool overchild = false;
				
				foreach (Control child in c.Children)
				{
					bool cr = _MouseMoveRecursive(child, X - child.renderLocation.RealL, Y - child.renderLocation.RealT);
					
					if (cr)
						cr &= !c.StealChildMouse(child, X, Y);
					
					overchild |= cr;
				}
				
				if (!overchild)
					mouseOverControl = c;
				
				return true;
			}
			
			return false;
		}
		
		public Control GetControlByID(string id)
		{
			return _GetControlByIDRecursive(id, this);
		}
		
		Control _GetControlByIDRecursive(string id, Control c)
		{
			if (c.ID == id)
				return c;
				
			foreach (Control child in c.Children)
			{
				Control tmp = _GetControlByIDRecursive(id, child);
				
				if (tmp != null)
					return tmp;
			}
			
			return null;
		}

        public override void RenderControl(IGraphics g)
        {
            Core.themer.RenderWindow(this, g);
        }
	}
}
