using Groll.Schule.SchulDB.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.Model;
using Groll.Schule.DataManager;
using System.Collections.ObjectModel;
using Groll.Schule.SchulDB.Commands;

namespace Groll.Schule.SchulDB.ViewModels
{
    /// <summary>
    /// ViewModel für die Klassenbearbeitungs-Seite
    /// </summary>
    public class StundenplanEditVM : SchuleViewModelBase
    {
        // internal Member
        private Klasse selectedKlasse;
        private int anzeigeAnzahlStunden;
        private bool showSamstag;
        private ObservableCollection<int> pausenstunden;
     
        #region Properties

        private ObservableCollection<Fach> fächerListe;
        public ObservableCollection<Fach> Fächerliste
        {
            get
            {
                return fächerListe;
            }

            set
            {
                if (fächerListe == value)
                    return;
                fächerListe = value; OnPropertyChanged();
            }
        }

        private Stundenplan stundenplan;
        public Stundenplan Stundenplan
        {
            get
            {                                    

                return stundenplan;

            }

            set
            {
                if (stundenplan == value)
                    return;
                stundenplan = value;                
                stundenplan.Stunden.CollectionChanged += Stunden_CollectionChanged;
                OnPropertyChanged();                
            }
        }

        // Liste der Klassen / z.B. für Dropdown oder Liste        
        private List<Klasse> klassenListe;
        public List<Klasse> KlassenListe
        {
            get { return klassenListe; }
            set
            {
                if (klassenListe == value)
                    return;
                klassenListe = value; OnPropertyChanged();
                if (klassenListe.Count > 0)
                    SelectedKlasse = klassenListe[0];
                else
                    SelectedKlasse = null;
            }
        }

        public Klasse SelectedKlasse
        {
            get { return selectedKlasse; }
            set
            {
                if (selectedKlasse == value)
                    return;
                selectedKlasse = value; 
                OnPropertyChanged(); 
                OnSelectedKlasseChanged();
            }
        }

        public int AnzeigeAnzahlStunden
        {
            get
            {
                return anzeigeAnzahlStunden;
            }

            set
            {
                if (anzeigeAnzahlStunden == value)
                    return;

                anzeigeAnzahlStunden = value;
                Settings.Set("Stundenplan.Anzeige.AnzahlStunden", typeof(int), anzeigeAnzahlStunden);
                OnPropertyChanged();
            }
        }

        public ObservableCollection<int> Pausenstunden
        {
            get
            {
                return pausenstunden;
            }

            set
            {
                if (pausenstunden == value)
                    return;

                pausenstunden = value;
                Settings.Set("Stundenplan.Anzeige.Pausenstunden", typeof(int), PausenstundenListToString(value));
                OnPropertyChanged();
            }
        }

        public bool ShowSamstag
        {
            get
            {
                return showSamstag;
            }

            set
            {
                if (showSamstag == value)
                    return;

                showSamstag = value;
                Settings.Set("Stundenplan.Anzeige.ShowSamstag", typeof(bool), value);
                OnPropertyChanged();
            }
        }

        #endregion

        //  Konstructor
        public StundenplanEditVM()
        {
            // Commands des Ribbons verbinden
            Ribbon.TabStundenplan.SetStundenanzahl = new DelegateCommand((c) => SetStundenAnzahl(c)) ;
            Ribbon.TabStundenplan.SetShowSamstag = new DelegateCommand((c) => SetShowSamstag(c));
            Ribbon.TabStundenplan.SetPausenstunden = new DelegateCommand((c) => SetPausenstunden(c));
        }      

        

        #region Hilfsfunktionen
        private ObservableCollection<int> PausenstundenStringToList(string s)
        {
            ObservableCollection<int> p = new ObservableCollection<int>();
            int i = 0;
            foreach (string n in s.Split(','))
            {
                if (int.TryParse(n, out i))
                    p.Add(i);
            }

            return p;
        }

        private string PausenstundenListToString(ObservableCollection<int> l)
        {
            return string.Join(",", l);
        }

        #endregion

        #region Verhalten bei Änderungen der Auswahl

        protected virtual void OnSelectedKlasseChanged()
        {
            LoadStundenplan();              
        }

        void Stunden_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            int i = 1;
        }

        #endregion
       
        public override void RefreshData()
        {
            // Lädt sämtliche Daten neu, z.B. wenn DB oder aktuelles Schuljahr geändert wurde
            base.RefreshData();
            
            // Initialisierung
            if (UnitOfWork != null)
            {
                int sj = Settings.ActiveSchuljahr.Startjahr;
                        
                // Aktuelle Klassen holen
                KlassenListe = UnitOfWork.Klassen.GetList().Where(x => x.SchuljahrId == sj).ToList();   
                
                LoadStundenplan();
                
                // Fächer laden
                Fächerliste = UnitOfWork.Fächer.GetObservableCollection();

                // Stundenplan-Konfig
                AnzeigeAnzahlStunden = (int)Settings.Get("Stundenplan.Anzeige.AnzahlStunden", typeof(int), 6, true);
                ShowSamstag = (bool) Settings.Get("Stundenplan.Anzeige.ShowSamstag", typeof(bool), false, true);
                Pausenstunden = PausenstundenStringToList(Settings.Get("Stundenplan.Anzeige.Pausenstunden", typeof(string)).ToString());                               
            }
        }

        private void LoadStundenplan()
        {
            if (selectedKlasse != null)
            {
                Stundenplan s = UnitOfWork.Stundenpläne.GetList().Where(x => x.Klasse == selectedKlasse).FirstOrDefault();
                if (s == null)
                {
                    // leeren default Stundenplan für diese Klasse erzeugen
                    s = UnitOfWork.Stundenpläne.Create();
                    s.Klasse = selectedKlasse;
                    UnitOfWork.Stundenpläne.Add(s);
                    UnitOfWork.Save();
                }

                Stundenplan = s;
            }

        }

        #region Commands
        public DelegateCommand AddClassCommand { get; private set; }
        public DelegateCommand DeleteClassCommand { get; private set; }

        private void SetShowSamstag(object c)
        {
            ShowSamstag = (bool)c;
        }

        private void SetStundenAnzahl(object c)
        {
            int i = 0;

            if (c is Int16)
                i = (Int16)c;
            else

                if (!int.TryParse((string)c.ToString(), out i))
                    return;

            // Save new value
            AnzeigeAnzahlStunden = i;
        }

        private void SetPausenstunden(object c)
        {
            // Save new value
            Pausenstunden = PausenstundenStringToList(c.ToString()); ;
        }
       
        #endregion


        // Public methods

         
    }
}
