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
        private ObservableCollection<Schueler> freieSchülerListe;
        private Klasse selectedKlasse;
     
        #region Properties

        public Stundenplan Stundenplan
        {
            get
            {                                    

                return stundenplan;

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

       // Liste der Schüler ohne Klasse                       
        public ObservableCollection<Schueler> FreieSchülerListe
        {
            get { return freieSchülerListe; }
            set
            {
                if (freieSchülerListe == value)
                    return;
                freieSchülerListe = value; OnPropertyChanged();              
            }
        }              

        #endregion

        //  Konstructor
        public StundenplanEditVM()
        {
            AddClassCommand = new DelegateCommand((object x) => AddKlasse(x));
            DeleteClassCommand = new DelegateCommand((object x) => DeleteKlasse());
            
        }

        #region Verhalten bei Änderungen der Auswahl       

         protected virtual void OnSelectedKlasseChanged()
        {
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
                stundenplan = UnitOfWork.Stundenpläne.Get(x => x.Klasse == selectedKlasse);
                //    int sj = Settings.ActiveSchuljahr.Startjahr;
                        
                // Aktuelle Klassen holen
              //  KlassenListe = UnitOfWork.Klassen.GetList().Where(x => x.SchuljahrId == sj).ToList();   
                
                // Freie Schüler holen
              //  FreieSchülerListe = new ObservableCollection<Schueler>(UnitOfWork.Schueler.GetList().Where(x => !x.Inaktiv && x.GetKlasse(sj) == null));                
            }
        }


        #region Commands
        public DelegateCommand AddClassCommand { get; private set; }
        public DelegateCommand DeleteClassCommand { get; private set; }

        public void AddKlasse(object Name)
        {
            string n = Name as string;
            if (!string.IsNullOrWhiteSpace(n))
            {
                CreateKlasse(n);
            }
        }

        public void DeleteKlasse()
        {
            if (System.Windows.MessageBox.Show("Soll diese Klasse wirklich gelöscht werden ?\n\nDie Klassenzugehörigkeiten der Schüler geht damit verloren !", "Klasse löschen", System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Warning) == System.Windows.MessageBoxResult.OK)
            {
                // Verbundene Schüler löschen
                SelectedKlasse.Schueler.Clear();
                UnitOfWork.Klassen.Delete(SelectedKlasse);
                UnitOfWork.Save();
                RefreshData();
            }
        }

        #endregion


        // Public methods

        public bool AddToCurrentClass(Schueler s)
        {
            if (selectedKlasse == null || s == null)
            {
                // Keine Klasse ausgewählt oder invalider Schüler
                return false;
            }

            if (!SelectedKlasse.Schueler.Contains(s))
            {
                // Schüler hinzufügen und speichern
                SelectedKlasse.Schueler.Add(s);
                FreieSchülerListe.Remove(s);                
                UnitOfWork.Save();                                
            }
            return true;

        }

        public bool RemoveFromCurrentClass(Schueler s)
        {
            // In DB löschen
            SelectedKlasse.Schueler.Remove(s);
            UnitOfWork.Save();
            FreieSchülerListe.Add(s);
            return true;
        }

        public void CreateKlasse(string Name)
        {
            // Check if exist
            Klasse x = UnitOfWork.Klassen.Get(Name, Settings.ActiveSchuljahr);
            if (x == null)
            {
                x = UnitOfWork.Klassen.Add(Name, Settings.ActiveSchuljahr);
                UnitOfWork.Save();
                RefreshData();                
            }
            SelectedKlasse = x;
        }                     
    }
}
