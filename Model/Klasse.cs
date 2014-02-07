using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groll.Schule.Model
{
    public class Klasse
    {
        public int KlasseId { get; set; }
        public string Name { get; set; }
        public int SchuljahrId { get; set; }

        public virtual List<Schueler> Schueler { get; set; }        
        public virtual Schuljahr Schuljahr { get; set; }

        public override string ToString()
        {
            return "Klasse " + (Name ?? "")  + " in Schuljahr " + (Schuljahr == null ? "" : Schuljahr.ToString());
        }
    }
}
