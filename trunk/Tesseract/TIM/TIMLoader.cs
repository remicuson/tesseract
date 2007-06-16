using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Xml;
using Tesseract.Controls;
using Tesseract.Geometry;
using Tesseract.Graphics;

namespace Tesseract.TIM
{
	internal class TIMLoader: XMLLoader
	{
		XmlElement xmlRoot;
		XmlElement xmlRootControl;
		
		Dictionary<string, Type> typeDict = new Dictionary<string, Type>();
		
		public TIMLoader(XmlDocument xml)
		{
			this.xmlRoot = xml.DocumentElement;
			
			if (xmlRoot.Name.ToLower() != "tim")
			{
				Debug.Error("Unable to find TIM root element");
				return;
			}
			
			foreach (XmlNode n in xmlRoot)
				if (n.NodeType == XmlNodeType.Element)
					xmlRootControl = (XmlElement)n;
		}
		
		public void Load(object obj)
		{
            Load(xmlRootControl, obj);
            TIM.currentControl = null;
		}
		
		public override void Load(XmlNode xml, object o)
		{
			if (o is Control)
				TIM.currentControl = (Control)o;

            if (xml.Attributes != null)
            {
                foreach (XmlNode n in xml.Attributes)
                {
                    if (LoadProperty(o, n))
                        continue;
                }
            }

            foreach (XmlNode n in xml)
            {
                if (n.NodeType == XmlNodeType.Comment)
                    continue;

                if (n.NodeType == XmlNodeType.Text)
                {
                    LoadText(o, n.Value);
                    continue;
                }

                if (LoadProperty(o, n))
                    continue;

                object child = base.Load(n);

                if (child != null)
                    HandleChild(o, child);
            }
		}
		
		void LoadText(object obj, string text)
		{
			if (!(obj is Control))
				return;
			
			Label lbl = new Label();
			lbl.Display = DisplayMode.Flow;
			lbl.Text = text;
			((Control)obj).Children.Add(lbl);
		}

        public override void HandleChild(object parent, object child)
        {
            if (parent is Control && child is Control)
                ((Control)parent).Children.Add((Control)child);
            else
                base.HandleChild(parent, child);
        }
	}
}
