using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groll.Schule.Model
{
    public class Beobachtung
    {
        public int BeobachtungId { get; set; }               
        public DateTime? Datum { get; set; }
        public string Text { get; set; }
        public virtual Fach Fach { get; set; }
        public int SchuljahrId { get; set; }
        public virtual Schuljahr Schuljahr { get; set; }
        public virtual Schueler Schueler { get; set; }
    }
}
