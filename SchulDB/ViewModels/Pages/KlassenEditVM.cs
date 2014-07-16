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
    public class KlassenEditVM : SchuleViewModelBase
    {
        // internal Member

        private ObservableCollection<Klasse> klassenListe;
        private ObservableCollection<Schueler> schülerListe;
        private Klasse selectedKlasse;

        /*private DateTime? beoDatum;
        private string beoText;
        private Schuljahr selectedSchuljahr;
        
        private Schueler selectedSchüler;
        private Fach selectedFach;
        
        */      
        #region Properties

        // Liste der Klassen / z.B. für Dropdown oder Liste        
        public ObservableCollection<Klasse> KlassenListe
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

        
      
       

        // Liste der Schüler / z.B. für Dropdown oder Liste                       
        public ObservableCollection<Schueler> SchülerListe
        {
            get { return schülerListe; }
            set
            {
                if (schülerListe == value)
                    return;
                schülerListe = value; OnPropertyChanged();
               /*
                if (schülerListe.Count > 0)
                    SelectedSchüler = schülerListe[0];
                else
                    SelectedSchüler = null;
                * */
            }
        }              

      /*
        public Schueler SelectedSchüler
        {
            get { return selectedSchüler; }
            set
            {
                if (selectedSchüler == value)
                    return;
                selectedSchüler = value; OnPropertyChanged(); OnSelectedSchülerChanged();
            }
        }
        */
        #endregion

        //  Konstructor
        public KlassenEditVM()
        {
            
        }

        #region Verhalten bei Änderungen der Auswahl       

         protected virtual void OnSelectedKlasseChanged()
        {
            // Eine Klasse wurde ausgewählt => Schülerliste aktualisieren
           /*
             if (selectedKlasse == null)
                SchülerListe = new ObservableCollection<Schueler>();
            else
                SchülerListe = new ObservableCollection<Schueler>(SelectedKlasse.Schueler);

             */
        }

        #endregion
        /*
 * 
        public override void RefreshData()
        {
            base.RefreshData();
            
            // Initialisierung
            if (UnitOfWork != null)
            {
                // Aktuelle Fächer holen
                var fl = new ObservableCollection<Fach>(UnitOfWork.Fächer.GetActiveFächer());
                fl.Insert(0, new Fach("<kein Fach>") { FachId = -1000 });
                Fächerliste = fl;

                // SCHULJAHRE LISTE LADEN
                SchuljahrListe = UnitOfWork.Schuljahre.GetObservableCollection();                                               
            }
        } 
       
        protected virtual void OnSelectedSchuljahrChanged()
        {
            // Ein anderes Schuljahr wurde ausgewählt => Klassenliste aktualisieren
            if (selectedSchuljahr == null)
                KlassenListe = new ObservableCollection<Klasse>();
            else
                // Klassen des Schuljahres holen
                KlassenListe = new ObservableCollection<Klasse>(UnitOfWork.Klassen.GetList(k => k.Schuljahr.Startjahr == selectedSchuljahr.Startjahr));

        }

       

        protected virtual void OnSelectedSchülerChanged()
        {
            // Ein Schüler wurde ausgewählt  
            SchuleCommands.Beobachtungen.AddComment.RaiseCanExecuteChanged();
        }

       


        #endregion


        // Commands

        #region Public Interface für Commands
        public virtual bool ValidateCurrent()
        {
            return selectedSchüler != null && beoText.Length > 0 && selectedSchuljahr != null;
        }     
      
        public void Refresh()
        {
            RefreshData();
        }

       
 * */ 
    }
}
