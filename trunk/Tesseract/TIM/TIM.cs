using System;
using System.Xml;
using Tesseract.Controls;

namespace Tesseract.TIM
{
    /// <summary>
    /// Provides methods used to load and save TIM markup
    /// </summary>
	public static class TIM
	{
		internal static Control currentControl;
		
        /// <summary>
        /// Load the TIM markup contained in the given xml document
        /// </summary>
        /// <typeparam name="T">The type of control to be loaded</typeparam>
        /// <param name="xml">The xml document containing the TIM markup</param>
        /// <returns>The root control defined by the TIM markup & given type</returns>
		public static T Load<T>(XmlDocument xml)
		{
			TIMLoader l = new TIMLoader(xml);
			T obj = Activator.CreateInstance<T>();
			l.Load(obj);
			return obj;
		}
		
        /// <summary>
        /// Load the TIM markup contained in an xml document of the given filename
        /// </summary>
        /// <typeparam name="T">The type of control to be loaded</typeparam>
        /// <param name="filename">The path to an xml document containing TIM markup</param>
        /// <returns>The root control defined by the TIM markup & given type</returns>
		public static T Load<T>(string filename)
		{
			XmlDocument xml = new XmlDocument();
			xml.Load(filename);
			return Load<T>(xml);
		}
	}
}
