using System;
using Tesseract.Backends;
using Tesseract.Events;
using Tesseract.Geometry;
using Tesseract.Graphics;

namespace Tesseract.Controls
{
	public enum DisplayMode { Block, Inline, Flow }
	
	public class Control
	{
		public event EventHandler<RenderEventArgs> Render;
		
		internal Location renderLocation;
		
		public Control()
		{
			children = new ChildList(this);
			path = new Rectangle(100, 100);
			margin = new Margin(this, 0, 0, 0, 0);
			padding = new Padding(this, 0, 0, 0, 0);
		}
		
		bool autosize;
		public bool AutoSize
		{
			get { return autosize; }
			set { autosize = value; HandleAutoSize(); }
		}
		
		string id;
		public string ID
		{
			get { return id; }
			set { id = value; }
		}
		
		DisplayMode display = DisplayMode.Inline;
		public DisplayMode Display
		{
			get { return display; }
			set { display = value; }
		}
		
		ChildList children;
		public ChildList Children
		{
			get { return children; }
		}
		
		Control parent;
		public Control Parent
		{
			get { return parent; }
			set { parent = value; }
		}
		
		public Window Window
		{
			get
			{
				Control c = this;
				
				while ((c != null) && !(c is Window))
					c = c.Parent;
				
				return (Window)c;
			}
		}
		
		Location location;
		public virtual Location Location
		{
			get { return location; }
			set
			{
				location = value;
				location.Control = this;
				display = DisplayMode.Block;
				OnMove();
			}
		}
		
		Margin margin;
		public virtual Margin Margin
		{
			get { return margin; }
			set { margin = value; }
		}
		
		Padding padding;
		public virtual Padding Padding
		{
			get { return padding; }
			set { padding = value; }
		}
		
		public Location OffsetLocation
		{
			get
			{
				Location l = new Location(this, renderLocation.RealL, renderLocation.RealT, null, null);
				Control p = this.Parent;
				
				while (p != null)
				{
					l.Offset(p.renderLocation.RealL, p.renderLocation.RealT);
					p = p.Parent;
				}
				
				return l;
			}
		}
		
		Path path;
		public virtual Path Path
		{
			get { return path; }
			set
			{
				path = value;
				path.Control = this;
				OnResize();
			}
		}
		
		Font font = null;
		public Font Font
		{
			get
			{
				if (font != null)
					return font;
				
				if (parent != null)
					return parent.Font;
				
				return new Font();
			}
			set { font = value; }
		}
		
		bool mouseover;
		public virtual bool MouseOver
		{
			get { return mouseover; }
			set { mouseover = value; }
		}
		
		bool mousedown;
		public virtual bool MouseDown
		{
			get { return mousedown; }
			set { mousedown = value; }
		}
		
		bool visible = true;
		public bool Visible
		{
			get { return visible; }
			set { visible = value; }
		}
		
		PatternList background;
		public PatternList Background
		{
			get { return background; }
			set { background = value; }
		}
		
		PatternList activebackground;
		public PatternList ActiveBackground
		{
			get { return activebackground; }
			set { activebackground = value; }
		}
		
		double activeopacity = 0;
		public double ActiveOpacity
		{
			get { return activeopacity; }
			set { activeopacity = value; }
		}
		
		bool autoactiveopacity = true;
		public bool AutoActiveOpacity
		{
			get { return autoactiveopacity; }
			set { autoactiveopacity = value; }
		}
		
		bool active;
		public virtual bool Active
		{
			get { return active; }
			set
			{
				active = value;
				
				if (autoactiveopacity)
					activeopacity = value ? 1 : 0;
			}
		}
		
		internal void AutoReRender()
		{
			this.ReRender();
		}
		
		public virtual void ReRender()
		{
			if (this.Window != null)
				this.Window.ReRender(OffsetLocation.L, OffsetLocation.T, OffsetLocation.L + path.W, OffsetLocation.T + path.H);	
		}
		
		public virtual void OnRender(RenderEventArgs e)
		{
            e.Graphics.Save();

            try
            {
                this.renderLocation.HandleRel();

                if (display != DisplayMode.Flow)
                    e.Graphics.Translate(this.renderLocation.RealL, this.renderLocation.RealT);

                if (this.visible)
                {
                    if (this.Render != null)
                        this.Render(this, e);
                    else
                        RenderControl(e.Graphics);
                }

                this.Path.Apply(e.Graphics);
                e.Graphics.Clip();

                foreach (Control child in children)
                    child.OnRender(e);
            }
            finally
            {
                e.Graphics.Restore();
            }
		}
		
		public virtual void RenderControl(IGraphics g)
		{
			
		}
		
		public virtual void OnChildAdded(Control child)
		{
			PositionChildren();
		}
		
		public virtual void OnChildRemoved(Control child)
		{
			PositionChildren();
		}
		
		public virtual void OnParented(Control parent)
		{
			PositionChildren();
		}
		
		public virtual void OnMouseEnter()
		{
			MouseOver = true;
			AutoReRender();
		}
		
		public virtual void OnMouseLeave()
		{
			MouseOver = false;
			AutoReRender();
		}
		
		public virtual void OnMouseMove(MouseEventArgs e)
		{
		
		}
		
		public virtual void OnMousePress(MouseEventArgs e)
		{
			MouseDown = true;
			AutoReRender();
		}
		
		public virtual void OnMouseRelease(MouseEventArgs e)
		{
			MouseDown = false;
			AutoReRender();
		}
		
		public virtual void OnMove()
		{
		
		}
		
		public virtual void OnResize()
		{
			PositionChildren();
		}
		
		public virtual bool StealChildMouse(Control child, Measurement X, Measurement Y)
		{
			return false;
		}
		
		internal Location[] renderFlowChunkLocations;
		
		public virtual Rectangle[] GetFlowChunks()
		{
			return new Rectangle[] { new Rectangle(path.W, path.H) };
		}
		
		protected virtual void PositionChildren()
		{
			double x = padding.L;
			double y = padding.T;
			double rowh = 0;

			foreach (Control child in children)
			{
                if (x < child.Margin.L)
                    x = child.Margin.L;
                if (y < child.Margin.T)
                    y = child.Margin.T;

				if (child.Display == DisplayMode.Flow)
				{
					Rectangle[] chunks = child.GetFlowChunks();
					child.renderFlowChunkLocations = new Location[chunks.Length];

					for (int i = 0; i < chunks.Length; i++)
					{
						Rectangle chunkRect = chunks[i];
						
						if (x + chunkRect.W >= this.path.W - Math.Max(this.Padding.R, child.Margin.R))
						{
							x = this.padding.L;
							y += rowh;
							rowh = 0;
						}

						child.renderFlowChunkLocations[i] = new Location(child, x, y, null, null);
						x += chunkRect.W;
						rowh = Math.Max(rowh, chunkRect.H);
					}
					
					child.renderLocation = (chunks.Length > 0) ? child.renderFlowChunkLocations[0] : new Location(child, 0, 0, null, null);
					
					continue;
				}

                if (x + child.path.W >= this.path.W - Math.Max(this.Padding.R, child.Margin.R))
				{
					x = this.padding.L;
					y += rowh;
					rowh = 0;
				}
				
				if ((child.Display == DisplayMode.Block) && (child.Location != null))
				{
					child.renderLocation = child.Location;
					continue;
				}
				
				if (child.Display == DisplayMode.Inline)
				{
					child.renderLocation = new Location(child, x, y, null, null);
                    x += child.Path.W;
					rowh = Math.Max(rowh, child.Path.H);
					continue;
				}
				
				if (child.Display == DisplayMode.Block)
				{
					child.renderLocation = new Location(child, x, y, null, null);
					x = this.Path.W + 1;
					rowh = Math.Max(rowh, child.Path.H);
					continue;
				}
				
				Debug.Error("Unable to Position Child");
				child.renderLocation = new Location(child, 0, 0, null, null);
			}
			
			HandleAutoSize();
		}
		
		bool autosizing;
		public virtual void HandleAutoSize()
		{
			if (autosizing || (!autosize))
				return;
				
			autosizing = true;
				
			double w = padding.L + padding.R;
			double h = padding.T + padding.B;
			
			foreach (Control child in children)
			{
                w = Math.Max(child.renderLocation.RealL + child.path.W + Math.Max(child.Margin.R, padding.R), w);
                h = Math.Max(child.renderLocation.RealT + child.path.H + Math.Max(child.Margin.B, padding.B), h);
			}

			this.Path.W = w;
			this.Path.H = h;
			
			autosizing = false;
		}
	}
}
