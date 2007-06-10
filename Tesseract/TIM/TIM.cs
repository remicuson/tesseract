using System;
using System.Xml;
using Tesseract.Controls;

namespace Tesseract.TIM
{
	public static class TIM
	{
		internal static Control currentControl;
		
		public static T Load<T>(XmlDocument xml)
		{
			TIMLoader l = new TIMLoader(xml);
			T obj = Activator.CreateInstance<T>();
			l.Load(obj);
			return obj;
		}
		
		public static T Load<T>(string filename)
		{
			XmlDocument xml = new XmlDocument();
			xml.Load(filename);
			return Load<T>(xml);
		}
	}
}
