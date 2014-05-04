using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groll.Schule.Model
{
    public class Textbaustein
    {
        string name;
        public int TextbausteinId { get; set; }
        public int UsageCount { get; set; }
        public string Name
        {
            get
            {
                return string.IsNullOrEmpty(name) ? ShortText : name;
            }
            set { name = value; }
        }

        public string Text { get; set; }        

        public Textbaustein() { }
        public Textbaustein(string Name, string Text) { this.Text = Text; name = Name; }

        public override string ToString()
        {
            return Text ?? "";
        }

        public string ShortText { get { return Text.Substring(0, 10) + " ..."; } }
    }
}
