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
	public class TIMLoader
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
			
			GenerateTypeList(Assembly.GetExecutingAssembly());
			GenerateTypeList(Assembly.GetEntryAssembly());
		}
		
		public void Load(object obj)
		{
			Load(obj, xmlRootControl);
            TIM.currentControl = null;
		}
		
		public void Load(object obj, XmlNode xml)
		{
			if (obj is Control)
				TIM.currentControl = (Control)obj;
			
			if (xml.Attributes != null)
			{
				foreach (XmlNode n in xml.Attributes)
				{
					if (LoadProperty(obj, n))
						continue;
				}
			}
			
			foreach (XmlNode n in xml)
			{
				if (n.NodeType == XmlNodeType.Text)
				{
					LoadText(obj, n.Value);
					continue;
				}
				if (LoadProperty(obj, n))
					continue;
				
				if (FindType(n.LocalName) != null)
				{
					object childobj = Activator.CreateInstance(FindType(n.LocalName));
					Load(childobj, n);
					AddChild(childobj, obj);
				}
			}
		}
		
		bool LoadProperty(object obj, XmlNode xml)
		{
			PropertyInfo pinfo = obj.GetType().GetProperty(xml.LocalName);
			
			if (pinfo == null)
				return false;
				
			TypeConverter tc = TypeDescriptor.GetConverter(pinfo.PropertyType);
			if (tc.CanConvertFrom(typeof(string)) && !string.IsNullOrEmpty(NodeValue(xml)))
			{
				pinfo.SetValue(obj, tc.ConvertFrom(NodeValue(xml)), null);
			}
			else
			{
				object val = pinfo.GetValue(obj, null);
			
				if (val is PatternList)
					val = new PatternList(((PatternList)val).Control);
				else if ((val == null) || (val.GetType().GetInterface("IList") != null))
					val = Activator.CreateInstance(pinfo.PropertyType);
				
				Load(val, xml);
			
				pinfo.SetValue(obj, val, null);
			}
			
			return true;
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
		
		void AddChild(object child, object parent)
		{
            if (parent is Control && child is Control)
                ((Control)parent).Children.Add((Control)child);
            else if (parent is LinearGradient && child is GradientStop)
                ((LinearGradient)parent).Add((GradientStop)child);
            else if (parent is RadialGradient && child is GradientStop)
                ((RadialGradient)parent).Add((GradientStop)child);
            else if (parent.GetType().GetInterface("IList") != null)
                parent.GetType().GetMethod("Add").Invoke(parent, new object[] { child });
		}
		
		void GenerateTypeList(Assembly a)
		{
			Type[] types = a.GetTypes();
			
			foreach (Type type in types)
			{
				if (typeDict.ContainsKey(type.Name))
					continue;
				
				typeDict.Add(type.Name, type);
			}
		}
		
		Type FindType(string Name)
		{
			if (typeDict.ContainsKey(Name))
				return typeDict[Name];
			
			Debug.Error("Unable to Find Type: " + Name);
			return null;
		}
		
		string NodeValue(XmlNode n)
		{
			if (n.NodeType == XmlNodeType.Attribute)
				return n.Value;
			
			return n.InnerText;
		}
	}
}
