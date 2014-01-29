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
    public class BeobachtungenEingabeVM : ObservableObject
    {
        // internal Member
        private int currentSJ;
        private UowSchuleDB uow;
        private DateTime? beoDatum;
        private string beoText;
        private ObservableCollection<Fach> fächerListe;
        private Klasse selectedKlasse;
        private Schueler selectedSchüler;
        private Fach selectedFach;
        private ObservableCollection<Klasse> klassenListe;
        private ObservableCollection<Schueler> schülerListe;
        private List<Beobachtung> beobachtungenHistoryListe;
        private BeoHistoryMode beobachtungenHistoryMode;

        public enum BeoHistoryMode { LastEntered, SortDate, CurrentSchueler }

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
            }
        }

        // Liste der Beobachtungen / z.B. für Dropdown oder Liste                       
        public List<Beobachtung> BeobachtungenHistoryListe
        {
            get { return beobachtungenHistoryListe; }
            set
            {
                if (beobachtungenHistoryListe == value)
                    return;
                beobachtungenHistoryListe = value; OnPropertyChanged();
            }
        }

        // History-Modus
        public BeoHistoryMode BeobachtungenHistoryType
        {
            get { return beobachtungenHistoryMode; }
            set
            {
                if (beobachtungenHistoryMode == value)
                    return;
                beobachtungenHistoryMode = value; OnPropertyChanged(); UpdateHistory();
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
        public BeobachtungenEingabeVM()
        {
            BeoDatum = DateTime.Now;
            beoText = "";
        }

        #region Verhalten bei Änderungen der Auswahl

        private void OnUnitOfWorkChanged()
        {
            uow.DatabaseChanged += uow_DatabaseChanged;
            RefreshData();
        }

        /// <summary>
        /// EventHandler für das DatabaseChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void uow_DatabaseChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            // Initialisierung
            if (uow != null)
            {
                // Aktuelles Schuljahr holen                               
                var sj = uow.Settings["Global.AktuellesSchuljahr"];
                if (sj == null)
                    return;
                currentSJ = sj.GetInt(Schuljahr.GetCurrent().Startjahr);

                // Klassen des Schuljahres holen
                KlassenListe = new ObservableCollection<Klasse>(uow.Klassen.GetList(k => k.Schuljahr.Startjahr == currentSJ));

                // Aktuelle Fächer holen
                var fl = new ObservableCollection<Fach>(uow.Fächer.GetList(f => f.Inaktiv == false));
                fl.Insert(0, new Fach("<kein Fach>") { FachId = -1000 });
                Fächerliste = fl;

                UpdateHistory();
            }
        }


        private void OnSelectedKlasseChanged()
        {
            // Eine Klasse wurde ausgewählt => Schülerliste aktualisieren
            if (selectedKlasse == null)
                SchülerListe = new ObservableCollection<Schueler>();
            else
                SchülerListe = new ObservableCollection<Schueler>(SelectedKlasse.Schueler);


        }

        private void OnSelectedSchülerChanged()
        {
            // Ein Schüler wurde ausgewählt 
            int i = 0;  // Dummy
        }

        private void UpdateHistory()
        {
            switch (beobachtungenHistoryMode)
            {
                case BeoHistoryMode.LastEntered:
                    BeobachtungenHistoryListe = uow.Beobachtungen.GetObservableCollection().OrderByDescending(x => x.BeobachtungId).Take(10).ToList();
                    break;

                case BeoHistoryMode.SortDate:
                    BeobachtungenHistoryListe = uow.Beobachtungen.GetObservableCollection().OrderByDescending(x => x.Datum).Take(10).ToList();                    
                    break;

                case BeoHistoryMode.CurrentSchueler:
                    BeobachtungenHistoryListe = uow.Beobachtungen.GetObservableCollection().Where (s => s.Schueler == selectedSchüler).OrderByDescending(x => x.BeobachtungId).Take(10).ToList();                    
                    break;
            }

        }



        #endregion


        // Commands

        #region Public Interface für Commands
        public bool ValidateCurrent()
        {
            return selectedSchüler != null && beoText.Length > 0 && currentSJ > 0;

        }
        public void AddCurrentComment(bool clear = true)
        {
            // Save current comment
            Beobachtung b = new Beobachtung()
            {
                Datum = beoDatum,
                Fach = (selectedFach == null || selectedFach.FachId == -1000) ? null : selectedFach,
                Text = beoText,
                SchuljahrId = currentSJ,
                Schueler = selectedSchüler
            };

            uow.Beobachtungen.Add(b);
            uow.Save();
            UpdateHistory();

            if (clear)
                BeoText = "";
        }

        public void ClearInput()
        {
            BeoDatum = DateTime.Now;
            BeoText = "";
        }

        public void Refresh()
        {
            OnUnitOfWorkChanged();
        }

        #endregion
    }
}
