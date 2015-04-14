using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Groll.Schule.OutputTools
{
    public static class XElementExt
    {



        /// <summary>
        /// Returns the value of an XML-Attribute as string
        /// If the object is NULL, or the Attribute does not exist the default value will be returned.     
        /// </summary>        
        /// <param name="x"></param>
        /// <param name="Attribute">Attribute Name</param>
        /// <param name="DefaultValue">Default Value</param>
        /// <returns>Value of the Attribute as string or default Value</returns>
        public static string AttributeValue(this XElement x, string Attribute, string DefaultValue = "")
        {
            if (x == null)
                return DefaultValue;

            return (string)x.Attribute(Attribute) ?? DefaultValue;
        }

        /// <summary>
        /// Returns the value of an XML-Attribute
        /// If the object is NULL, the Attribute does not exist or cannot be converted
        /// to the type T, the default value will be returned.     
        /// </summary>
        /// <typeparam name="T">Ziel-Typ für die Umwandlung</typeparam>
        /// <param name="x"></param>
        /// <param name="Attribute">Attribute Name</param>
        /// <param name="DefaultValue">Default Value</param>
        /// <returns>Value of the Attribute as type T or default Value</returns>
        public static T AttributeValue<T>(this XElement x, string Attribute, T DefaultValue = default(T))
        {
            if (x == null)
                return DefaultValue;

            var v = (string) x.Attribute(Attribute);

            if (!string.IsNullOrEmpty(v))
            {
                try
                {
                    if (typeof(T) == typeof(string))
                        return (T) (object) v;

                    return (T)Convert.ChangeType(v, typeof(T));
                }
                catch
                { 
                    return DefaultValue; 
                }
            }

            return DefaultValue;

        }

    }
}
