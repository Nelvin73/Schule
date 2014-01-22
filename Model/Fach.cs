using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groll.Schule.Model
{
    public class Fach
    {
        public int FachId { get; set; }
        public string Name { get; set; }
        public bool Inaktiv { get; set; }    // Yes = aktuell nicht benötigt (hidden)

        public Fach() { }
        public Fach(string Name) { this.Name = Name; }

        public override string ToString()
        {
            return Name;
        }
    }
}
