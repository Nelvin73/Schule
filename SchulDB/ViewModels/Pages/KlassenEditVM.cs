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

        private List<Klasse> klassenListe;
        private List<Schueler> schülerListe;
        private List<Schueler> freieSchülerListe;
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

        // Liste der Schüler / z.B. für Dropdown oder Liste                       
        public List<Schueler> SchülerListe
        {
            get { return schülerListe; }
            set
            {
                if (schülerListe == value)
                    return;
                schülerListe = value; OnPropertyChanged();              
            }
        }              
    
        // Liste der Schüler ohne Klasse                       
        public List<Schueler> FreieSchülerListe
        {
            get { return freieSchülerListe; }
            set
            {
                if (freieSchülerListe == value)
                    return;
                freieSchülerListe = value; OnPropertyChanged();              
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
            if (selectedKlasse == null)
                SchülerListe = new List<Schueler>();
            else
                SchülerListe = SelectedKlasse.Schueler;            
        }

        #endregion
       
        public override void RefreshData()
        {
            base.RefreshData();
            
            // Initialisierung
            if (UnitOfWork != null)
            {
                // Aktuelle Klassen holen
                KlassenListe = UnitOfWork.Klassen.GetList();
                
                // Freie Schüler holen
             //   var sj = UnitOfWork.Settings["Global.AktuellesSchuljahr"].GetInt(Schuljahr.GetCurrent().Startjahr);
            //    FreieSchülerListe = UnitOfWork.Schueler.GetList().Where(x => x.GetKlasse(sj) == null).ToList();
            
            }
        } 


        // Public methods


        public bool AddToCurrentClass(Schueler s)
        {
            if (!SelectedKlasse.Schueler.Contains(s))
            {
                SelectedKlasse.Schueler.Add(s);
            }
            return false;

        }

        public bool RemoveFromCurrentClass(Schueler s)
        {
            // In DB löschen
            UnitOfWork.Klassen.GetById(SelectedKlasse.KlasseId).Schueler.Remove(s);
            UnitOfWork.Save();

            var sj = UnitOfWork.Settings["Global.AktuellesSchuljahr"].GetInt(Schuljahr.GetCurrent().Startjahr);
            FreieSchülerListe = UnitOfWork.Schueler.GetList().Where(x => x.GetKlasse(sj) == null).ToList();
            return false;
        }

         /*
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
