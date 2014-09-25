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
        private Stundenplan stundenplan;
        private List<Klasse> klassenListe;
        private ObservableCollection<Fach> fächerListe;
        private Klasse selectedKlasse;
     
        #region Properties


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

     
        #endregion

        //  Konstructor
        public StundenplanEditVM()
        {
        
           }

        void Stunden_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            
        }

        #region Verhalten bei Änderungen der Auswahl       

         protected virtual void OnSelectedKlasseChanged()
        {
            LoadStundenplan();
               
            // nichts nötig, da mit ObservableCollection auf SelectedKlasse.Schüler gebunden wird
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

                // Freie Schüler holen
              //  FreieSchülerListe = new ObservableCollection<Schueler>(UnitOfWork.Schueler.GetList().Where(x => !x.Inaktiv && x.GetKlasse(sj) == null));                
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

       
        #endregion


        // Public methods

         
    }
}
