using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groll.Schule.Model
{
    public enum Geschlecht {männlich, weiblich}
    public enum DisplayNameFormat {VornameNachname = 1, NachnameVorname = 2}

    public class Schueler
    {
        // Default Anzeigeformat des Namens (statisch für alle Personen)
        public static DisplayNameFormat DefaultDisplayNameFormat = DisplayNameFormat.VornameNachname;

        public int ID { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public DateTime? Geburtsdatum { get; set; }
        public Geschlecht Geschlecht { get; set; }
        public string Bemerkung { get; set; }
        public Adresse Adresse { get; set; }
        public byte[] Foto { get; set; }
        public bool Inaktiv { get; set; }    // Yes = aktuell nicht mehr in der Schule

        public virtual List<Klasse> Klassen { get; set; }
     
        public Schueler()
        {
            Adresse = new Adresse();
        }

        // Berechnete Member
        public int? Alter
        {
            get { return !Geburtsdatum.HasValue ? null : (int?) ((DateTime.Now - Geburtsdatum.Value).Days / 365); } 
        }

        public Klasse GetKlasse(int SchuljahrId)
        {
            return Klassen.FirstOrDefault(x => x.SchuljahrId == SchuljahrId);

        }

        public string DisplayName { get { return GetDisplayName(); } }
        
        public string GetDisplayName(DisplayNameFormat NameFormat = 0)
        {
            return (NameFormat == 0 ? DefaultDisplayNameFormat : NameFormat) == DisplayNameFormat.NachnameVorname ? Nachname + ", " + Vorname : Vorname + " " + Nachname; 
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }    
  
    public class Adresse
    {
        public string Strasse { get; set; }
        public int? PLZ { get; set; }
        public string Ort { get; set; }

        public Adresse()
        {
            Ort = Strasse = "";
        }
    }
    
}
