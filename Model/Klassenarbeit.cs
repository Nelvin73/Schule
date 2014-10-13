using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groll.Schule.Model
{
    public class KlassenarbeitsNote
    {

        public virtual Klassenarbeit Klassenarbeit { get; set; }
        public virtual Schueler Schüler { get; set; }
        
        public bool HatMitgeschrieben {get; set;}

        public int? Punkte { get; set; }
        public int? Note { get; set; }
        public string Kommentar {get; set;}

        public KlassenarbeitsNote() { }
        
        public override string ToString()
        {
            if (Schüler != null )
                return Schüler.DisplayName + ": " + (HatMitgeschrieben ? Note + " (" + Punkte + " Punkte)" : "nicht mitgeschrieben");
            else
                return "";
        }
    }    
    
    public class Klassenarbeit
    {
        
        public int KlassenarbeitId { get; set; }
        public virtual Klasse Klasse { get; set; }
        public string Name { get; set; }
        public virtual Fach Fach { get; set; }
        public int GesamtPunkte { get; set; }
        public string Punkteschlüssel { get; set; }

        public ObservableCollection<KlassenarbeitsNote> Noten { get; set; }

        public Klassenarbeit() { }
        
        public override string ToString()
        {
            return Name ?? "";
        }

    }
}
