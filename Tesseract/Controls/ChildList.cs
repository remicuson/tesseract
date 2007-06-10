using System;
using System.Collections.Generic;

namespace Tesseract.Controls
{
	public class ChildList: List<Control>
	{
		public ChildList(Control parent)
		{
			this.parent = parent;
		}
		
		Control parent;
		public Control Parent
		{
			get { return parent; }
			set { parent = value; }
		}
		
		public new void Add(Control item)
		{
			base.Add(item);
			item.Parent = parent;
			
			parent.OnChildAdded(item);
			item.OnParented(parent);
		}
		
		public new void Remove(Control item)
		{
			base.Remove(item);
			
			if (item.Parent == parent)
				item.Parent = null;
			
			parent.OnChildRemoved(item);
		}
	}
}
