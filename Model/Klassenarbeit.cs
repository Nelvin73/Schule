using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groll.Schule.Model
{
    public class KlassenarbeitsNote : ObservableObject
    {

        private double? _Punkte = null;
        private int? _Note = null ;
        private bool _OhneWertung = false;
        
        public int KlassenarbeitId { get; set; }
        public int SchülerId { get; set; }
        public virtual Klassenarbeit Klassenarbeit { get; set; }
        public virtual Schueler Schüler { get; set; }
        
        public bool HatMitgeschrieben {
            get
            {
                return Punkte.HasValue || Note.HasValue;
            }
            set
            {
                if (Note == null && Punkte == null)
                    return;

                Note = null;
                Punkte = null;
                OnPropertyChanged();                
            }
        }
        
        public bool OhneWertung
        {
            get
            {
                return _OhneWertung;
            }
            set
            {
                if (_OhneWertung == value)
                    return;

                _OhneWertung = value;
                OnPropertyChanged();

                if (Klassenarbeit != null)
                    Klassenarbeit.RecalcStatistik();
            }
        }             
       
        public double? Punkte
        {
            get { return _Punkte; }
            set
            {
                if (_Punkte != value)
                {
                    _Punkte = value;
                    OnPropertyChanged();
                    if (Klassenarbeit != null)
                        Note = Klassenarbeit.BerechneNote(_Punkte);
                }
            }
        }        

        public int? Note
        {
            get { return _Note; }
            set
            {
                if (_Note != value)
                {
                    _Note = value;
                    OnPropertyChanged();
                    if (Klassenarbeit != null)
                        Klassenarbeit.RecalcStatistik();
                }
            }
        }
        
        public string Kommentar {get; set;}

        public KlassenarbeitsNote() {  }
        
        public override string ToString()
        {
            if (Schüler != null )
                return Schüler.DisplayName + ": " + (HatMitgeschrieben ? Note + " (" + Punkte + " Punkte)" : "nicht mitgeschrieben");
            else
                return "";
        }
    }

    public class Klassenarbeit : ObservableObject
    {
        
        private double? _Notenschnitt = null;
        private Dictionary<int, int> _Notenverteilung = null;
        
        #region Properties

        private string _Name = "";
        private int _GesamtPunkte;
        private string _Punkteschlüssel = "";
        private DateTime _Datum;
       
 
        public int KlassenarbeitId { get; set; }        
        public virtual Klasse Klasse { get; set; }
        public virtual Fach Fach { get; set; }

        public string Kommentar { get; set; }

        public ObservableCollection<KlassenarbeitsNote> Noten { get; set; }

        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    OnPropertyChanged();                    
                }
            }
        }

        public DateTime Datum
        {
            get { return _Datum; }
            set
            {
                if (_Datum != value)
                {
                    _Datum = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public int GesamtPunkte
        {
            get { return _GesamtPunkte; }
            set
            {
                if (_GesamtPunkte != value)
                {
                    _GesamtPunkte = value;
                    OnPropertyChanged();                   
                    SetDefaultPunkteschlüssel();
                }
            }
        }

        public  ObservableCollection<double> PunkteschlüsselListe
        {            
            get{
                double res;
                var ret =
                    from p in (Punkteschlüssel ?? "").Split(':')
                    select double.TryParse(p, out res) ? res : 0;
                return new ObservableCollection<double>(ret);
            }
            set{
                
                string x = "";
                foreach (var i in value)
                    x += ":" + i.ToString();
                
                Punkteschlüssel = x.Substring(1);                
            }
            
        }

        public string Punkteschlüssel
        {
            get { return _Punkteschlüssel; }
            set
            {
                if (_Punkteschlüssel != value)
                {
                    _Punkteschlüssel = value ?? "";
                    OnPropertyChanged();
                    OnPropertyChanged("PunkteschlüsselListe");
                    RecalcNoten();
                }
            }
        }

        #endregion


        #region Berechnete Properties
        public double? Notenschnitt
        {
            get
            {
                if (_Notenschnitt == null)
                {
                    _Notenschnitt = Noten.Where( x => !x.OhneWertung).Average(x => x.Note);                    
                }
                return _Notenschnitt;

            }
        }

        public Dictionary<int, int> Notenverteilung
        {
            get
            {
                if (_Notenverteilung == null)
                {
                    
                    var dict = new Dictionary<int,int>();

                    var res =
                        (from a in Noten
                         group a by a.Note into g
                         where g.Key != null
                         select new { Key = g.Key, Count = g.Count() }
                        ).ToDictionary(k => k.Key.Value, v => v.Count);

                    _Notenverteilung = res;                    
                    
                }
                return _Notenverteilung;

            }
        }

        #endregion
        public int? BerechneNote(double? Punkte)
        {
            if (Punkte == null)
                return null;

            int note = 6;
            foreach (double p in PunkteschlüsselListe)
            {
                if (Punkte >= p)
                    note--;
                else
                    break;
            }
            return note;

        }
      

        private void SetDefaultPunkteschlüssel()
        {
            ObservableCollection<double> l = new  ObservableCollection<double>();
            l.Add(Math.Round(_GesamtPunkte * 0.4, 0));
            l.Add(Math.Round(_GesamtPunkte * 0.5, 0));            
            l.Add(Math.Round(_GesamtPunkte * 0.7, 0));
            l.Add(Math.Round(_GesamtPunkte * 0.8, 0));
            l.Add(Math.Round(_GesamtPunkte * 0.95, 0));
            PunkteschlüsselListe = l;
        }

        #region Aktualisierungs-Routinen
        /// <summary>
        /// Berechnet alle Noten neu
        /// wird aufgerufen, wenn Punkteschlüssel sich ändern
        /// </summary>
        private void RecalcNoten()
        {
            // Noten nach Punkteschlüsseländerung neu berechnen
            foreach (KlassenarbeitsNote n in Noten)
            {
                n.Note = BerechneNote(n.Punkte);
            }
            RecalcStatistik();
        }

        /// <summary>
        /// Berechnet Statistik-Informationen neu
        /// wird aufgerufen, wenn sich irgend etwas ändert
        /// </summary>
        public void RecalcStatistik()
        {
            _Notenschnitt = null;
            _Notenverteilung = null;
            OnPropertyChanged("Notenschnitt");
            OnPropertyChanged("Notenverteilung");
        }


        #endregion

      
        public Klassenarbeit() 
        { 
            Noten = new ObservableCollection<KlassenarbeitsNote>();
            Datum = DateTime.Today;
        }
        
        public override string ToString()
        {
            return Name ?? "";
        }

    }
}
