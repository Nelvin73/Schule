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
    public class KlassenarbeitEditVM : SchuleViewModelBase
    {
        // internal Member
        private List<Klasse> klassenListe;        
        private ObservableCollection<Schueler> freieSchülerListe;
        private Klasse selectedKlasse;
     
        #region Properties

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
        public KlassenarbeitEditVM()
        {
             
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
                int sj = Settings.ActiveSchuljahr.Startjahr;
                        
                // Aktuelle Klassen holen
                KlassenListe = UnitOfWork.Klassen.GetList().Where(x => x.SchuljahrId == sj).ToList();   
                
                   }
        }


    
        }
}
