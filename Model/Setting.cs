using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groll.Schule.Model
{
    public class Setting
    {
        public string Key { get; set; }
        public string ValueType { get; set; }

        public int IntValue { get; set; }
        public string StringValue { get; set; }
        public bool BoolValue { get; set; }

        public void SetValue(object value)
        {
            switch (value.GetType().Name)
            {
                case "Int16":
                case "Int32":
                    ValueType = "int";
                    IntValue = (int)value;
                    break;

                case "Boolean":
                    ValueType = "bool";
                    BoolValue = (bool)value;
                    break;

                default:
                    ValueType = "string";
                    StringValue = value.ToString();
                    break;
            }           
        }

        public object GetValue()
        {
            switch (ValueType)
            {
                case "int":
                   return IntValue;                   

                case "bool":
                    ValueType = "bool";
                    return BoolValue;                  

                default:
                    return StringValue;                    
            }           
        }

        public string GetString(string Default = "")
        {
            return (ValueType == "string" ? StringValue : Default);            
        }

        public int GetInt(int Default = 0)
        {
            return (ValueType == "int" ? IntValue : Default);            
        }

        public bool GetBool(bool Default = false)
        {
            return (ValueType == "bool" ? BoolValue : Default);            
        }

        public Setting() { }

        public Setting(string Key, bool Value)
        {
            SetValue(Value);
            this.Key = Key;
        }
        
        public Setting(string Key, string Value)
        {
            SetValue(Value);
            this.Key = Key;
        }
        
        public Setting(string Key, int Value)
        {
            SetValue(Value);
            this.Key = Key;
        }
        
    }
}
