using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Tesseract
{
    /// <summary>
    /// Used to find types from their names
    /// </summary>
    internal static class TypeStore
    {
        static Dictionary<string, Type> typeDict;

        /// <summary>
        /// Create & populate the type dictionary
        /// </summary>
        public static void Init()
        {
            typeDict = new Dictionary<string, Type>();

            AppendTypeList(Assembly.GetExecutingAssembly());
            AppendTypeList(Assembly.GetEntryAssembly());
        }

        /// <summary>
        /// Append all types found in the given assembly to the type dictionary
        /// </summary>
        /// <param name="a">The assembly to look for types inside</param>
        static void AppendTypeList(Assembly a)
        {
            Type[] types = a.GetTypes();

            foreach (Type type in types)
            {
                if (typeDict.ContainsKey(type.Name))
                    continue;

                typeDict.Add(type.Name, type);
            }
        }

        /// <summary>
        /// Find a type of the given name
        /// </summary>
        /// <param name="Name">The name of the type to find</param>
        /// <returns>The type if found, otherwise null</returns>
        public static Type Find(string Name)
        {
            if (typeDict.ContainsKey(Name))
                return typeDict[Name];

            return null;
        }
    }
}
