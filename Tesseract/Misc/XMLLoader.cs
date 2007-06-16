using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.ComponentModel;

namespace Tesseract
{
    internal class XMLLoader
    {
        public XMLLoader()
        {

        }

        public virtual object Load(XmlNode xml)
        {
            Type t = TypeStore.Find(xml.LocalName);

            if (t == null)
                return null;

            object obj = Activator.CreateInstance(t);
            Load(xml, obj);
            return obj;
        }

        public virtual void Load(XmlNode xml, object o)
        {
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

                if (LoadProperty(o, n))
                    continue;

                object child = Load(n);

                if (child != null)
                    HandleChild(o, child);
            }
        }

        public virtual bool LoadProperty(object obj, XmlNode xml)
        {
            try
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

                    if ((val == null) || ((val.GetType().GetInterface("IList") != null) && (xml.Prefix != "append")))
                        val = Activator.CreateInstance(pinfo.PropertyType);
                    else if ((xml.ChildNodes.Count == 1) && (TypeStore.Find(xml.ChildNodes[0].LocalName) != null))
                    {
                        xml = xml.ChildNodes[0];
                        val = Activator.CreateInstance(TypeStore.Find(xml.LocalName));

                    }

                    Load(xml, val);

                    pinfo.SetValue(obj, val, null);
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.Error(string.Format("Unable to load property '{0}'\n", xml.Name, ex.Message));
                return false;
            }
        }

        public virtual void HandleChild(object parent, object child)
        {
            if (parent.GetType().GetInterface("IList") != null)
                parent.GetType().GetMethod("Add").Invoke(parent, new object[] { child });
        }

        #region Utility Methods

        protected virtual string NodeValue(XmlNode n)
        {
            if (n.NodeType == XmlNodeType.Attribute)
                return n.Value;

            return n.InnerText;
        }

        #endregion
    }
}
