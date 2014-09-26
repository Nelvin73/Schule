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
    /// ViewModel für die Beobachtungs-Eingabe-Seite
    /// </summary>
    public class BeobachtungenBaseVM : SchuleViewModelBase
    {
        // internal Member
        private ObservableCollection<Schuljahr> schuljahrListe;
              
        #region Properties
        
        // Datum der Beobachtungen (NULL = Allgemein)        
        private DateTime? beoDatum;
        public DateTime? BeoDatum
        {
            get { return beoDatum; }
            set
            {
                if (beoDatum == value)
                    return;
                beoDatum = value; OnPropertyChanged();
            }
        }

        // Beobachtungstext        
        private string beoText;
        public string BeoText
        {
            get { return beoText; }
            set
            {
                if (beoText == value)
                    return;

                beoText = value; 
                SchuleCommands.Beobachtungen.AddComment.RaiseCanExecuteChanged();   // Update Command
                OnPropertyChanged();
            }
        }

        // Liste der Fächer / z.B. für Dropdown oder Liste        
        private ObservableCollection<Fach> fächerListe;        
        public ObservableCollection<Fach> Fächerliste
        {
            get { return fächerListe; }
            set
            {
                if (fächerListe == value)
                    return;
                fächerListe = value; OnPropertyChanged();
                if (fächerListe.Count > 0)
                    SelectedFach = fächerListe[0];
                else
                    SelectedFach = null;
            }
        }

        // Liste der Klassen / z.B. für Dropdown oder Liste        
        private ObservableCollection<Klasse> klassenListe;
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

        // Liste der Schüler / z.B. für Dropdown oder Liste                       
        private ObservableCollection<Schueler> schülerListe;
        public ObservableCollection<Schueler> SchülerListe
        {
            get { return schülerListe; }
            set
            {
                if (schülerListe == value)
                    return;
                schülerListe = value; OnPropertyChanged();
                if (schülerListe.Count > 0)
                    SelectedSchüler = schülerListe[0];
                else
                    SelectedSchüler = null;
            }
        }              

        private Klasse selectedKlasse;
        public Klasse SelectedKlasse
        {
            get { return selectedKlasse; }
            set
            {
                if (selectedKlasse == value)
                    return;
                selectedKlasse = value; OnPropertyChanged(); OnSelectedKlasseChanged();
            }
        }

        private Schueler selectedSchüler;
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

        private Fach selectedFach;
        public Fach SelectedFach
        {
            get { return selectedFach; }
            set
            {
                if (selectedFach == value)
                    return;
                selectedFach = value; OnPropertyChanged();
            }
        }

        #endregion

        //  Konstructor
        public BeobachtungenBaseVM()
        {
            BeoDatum = DateTime.Now;
            beoText = "";            
        }

        #region Verhalten bei Änderungen der Auswahl       

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

                // Klassen holen                
                Schuljahr activeSJ = Settings.ActiveSchuljahr;
                if (activeSJ == null)
                    KlassenListe = new ObservableCollection<Klasse>();
                else
                    // Klassen des Schuljahres holen
                    KlassenListe = new ObservableCollection<Klasse>(UnitOfWork.Klassen.GetList(k => k.Schuljahr.Startjahr == activeSJ.Startjahr));
                
            }
        } 
       
        protected virtual void OnSelectedSchuljahrChanged()
        {
           

        }

        protected virtual void OnSelectedKlasseChanged()
        {
            // Eine Klasse wurde ausgewählt => Schülerliste aktualisieren
            if (selectedKlasse == null)
                SchülerListe = new ObservableCollection<Schueler>();
            else
                SchülerListe = new ObservableCollection<Schueler>(SelectedKlasse.Schueler);


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
            return selectedSchüler != null && beoText.Length > 0;
        }     
      
        public void Refresh()
        {
            RefreshData();
        }

        #endregion
    }
}
