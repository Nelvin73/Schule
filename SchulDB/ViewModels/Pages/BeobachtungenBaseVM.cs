using Groll.Schule.SchulDB.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.Model;
using Groll.Schule.DataManager;
using System.Collections.ObjectModel;

namespace Groll.Schule.SchulDB.ViewModels
{
    /// <summary>
    /// ViewModel für die Beobachtungs-Eingabe-Seite
    /// </summary>
    public class BeobachtungenBaseVM : ObservableObject
    {
        // internal Member
        private UowSchuleDB uow;
        private DateTime? beoDatum;
        private string beoText;
        private Schuljahr selectedSchuljahr;
        private Klasse selectedKlasse;
        private Schueler selectedSchüler;
        private Fach selectedFach;
        private ObservableCollection<Fach> fächerListe;        
        private ObservableCollection<Klasse> klassenListe;
        private ObservableCollection<Schueler> schülerListe;
        private ObservableCollection<Schuljahr> schuljahrListe;
              
        #region Properties

        // Unit of Work
        public UowSchuleDB UnitOfWork
        {
            get { return uow; }
            set
            {
                if (uow == value)
                    return;
                uow = value; OnUnitOfWorkChanged();
            }
        }

        public Schuljahr SelectedSchuljahr
        {
            get { return selectedSchuljahr; }
            set
            {
                if (selectedSchuljahr == value)
                    return;
                selectedSchuljahr = value; OnPropertyChanged(); OnSelectedSchuljahrChanged();
            }
        }

        public ObservableCollection<Schuljahr> SchuljahrListe
        {
            get { return schuljahrListe; }
            set
            {
                if (schuljahrListe == value)
                    return;
                schuljahrListe = value; OnPropertyChanged();
                if (schuljahrListe.Count > 0)
                {
                    // Default = Aktuelles Schuljahr
                    var sj = uow.Settings["Global.AktuellesSchuljahr"];
                    if (sj != null)
                    {
                        SelectedSchuljahr = uow.Schuljahre.GetById(sj.GetInt(Schuljahr.GetCurrent().Startjahr));
                    }
                    if (selectedSchuljahr == null)
                        SelectedSchuljahr = schuljahrListe[0];
                }
                else
                    SelectedSchuljahr = null;
            }
        }
           
        
        // Datum der Beobachtungen (NULL = Allgemein)        
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
        public string BeoText
        {
            get { return beoText; }
            set
            {
                if (beoText == value)
                    return;
                beoText = value; OnPropertyChanged();
            }
        }

        // Liste der Fächer / z.B. für Dropdown oder Liste        
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

        protected virtual void OnUnitOfWorkChanged()
        {
            uow.DatabaseChanged += uow_DatabaseChanged;
            RefreshData();
        }

        /// <summary>
        /// EventHandler für das DatabaseChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void uow_DatabaseChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        protected virtual void RefreshData()
        {
            // Initialisierung
            if (uow != null)
            {
                // Aktuelle Fächer holen
                var fl = new ObservableCollection<Fach>(uow.Fächer.GetActiveFächer());
                fl.Insert(0, new Fach("<kein Fach>") { FachId = -1000 });
                Fächerliste = fl;

                // SCHULJAHRE LISTE LADEN
                SchuljahrListe = uow.Schuljahre.GetObservableCollection();                                               
            }
        }

        protected virtual void OnSelectedSchuljahrChanged()
        {
            // Ein anderes Schuljahr wurde ausgewählt => Klassenliste aktualisieren
            if (selectedSchuljahr == null)
                KlassenListe = new ObservableCollection<Klasse>();
            else
                // Klassen des Schuljahres holen
                KlassenListe = new ObservableCollection<Klasse>(uow.Klassen.GetList(k => k.Schuljahr.Startjahr == selectedSchuljahr.Startjahr));

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
            OnUnitOfWorkChanged();
        }

        #endregion
    }
}
