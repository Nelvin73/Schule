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
    public class BeobachtungenEingabeVM : BeobachtungenBaseVM
    {
        // internal Member
        private List<Beobachtung> beobachtungenHistoryListe;
        private BeoHistoryMode beobachtungenHistoryMode;
        private List<Schueler> selectedSchülerList;
        public enum BeoHistoryMode { LastEntered, SortDate, CurrentSchueler }

        #region Properties
       
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

        public List<Schueler> SelectedSchülerList
        {
            get { return selectedSchülerList; }
            set
            {
                selectedSchülerList = value;
            }
        }

        #endregion

        //  Konstructor
        public BeobachtungenEingabeVM()
        {
            // Define Commands
            MainWindowViewModel.Command_BeoClearInput = new DelegateCommand((a) => ClearInput());
            MainWindowViewModel.Command_BeoAdd = new DelegateCommand((a) => AddCurrentComment(a == null));         
        }

        #region Verhalten bei Änderungen der Auswahl        

        public override void RefreshData()
        {            
            base.RefreshData();
            UpdateHistory();
        }
                

        protected override void OnSelectedSchülerChanged()
        {
            base.OnSelectedSchülerChanged();
            UpdateHistory();         
        }
       

        private void UpdateHistory()
        {
            switch (beobachtungenHistoryMode)
            {
                case BeoHistoryMode.LastEntered:
                    BeobachtungenHistoryListe = UnitOfWork.Beobachtungen.GetObservableCollection().OrderByDescending(x => x.BeobachtungId).Take(10).ToList();
                    break;

                case BeoHistoryMode.SortDate:
                    BeobachtungenHistoryListe = UnitOfWork.Beobachtungen.GetObservableCollection().OrderByDescending(x => x.Datum).Take(10).ToList();                    
                    break;

                case BeoHistoryMode.CurrentSchueler:
                    BeobachtungenHistoryListe = UnitOfWork.Beobachtungen.GetObservableCollection().Where(s => s.Schueler == SelectedSchüler).OrderByDescending(x => x.BeobachtungId).Take(10).ToList();                    
                    break;
            }

        }

        #endregion


        // Commands

        #region Public Interface für Commands

        public void UpdateHistoryView()
        {

            string type = Ribbon.TabBeobachtungen.HistoryViewMenuButton.SelectedItem.Tag.ToString();
            switch (type)
            {
                case "Schüler":
                    BeobachtungenHistoryType = BeoHistoryMode.CurrentSchueler;
                    break;
                case "Datum":
                    BeobachtungenHistoryType = BeoHistoryMode.SortDate;
                    break;
                default: // "ID"
                    BeobachtungenHistoryType = BeoHistoryMode.LastEntered;
                    break;
            }            

        }

        public void AddCurrentComment(bool clear = true)
        {
            // Save current comment
            foreach (Schueler s in SelectedSchülerList)
            {            
                Beobachtung b = new Beobachtung()
                {
                    Datum = BeoDatum,
                    Fach = (SelectedFach == null || SelectedFach.FachId == -1000) ? null : SelectedFach,
                    Text = BeoText,
                    SchuljahrId = SelectedSchuljahr.Startjahr,
                    Schueler = s
                };
                UnitOfWork.Beobachtungen.Add(b);
            }
            
            UnitOfWork.Save();
            UpdateHistory();

            if (clear)
                BeoText = "";
        }

        public void ClearInput()
        {
            BeoDatum = DateTime.Now;
            BeoText = "";
        }       

        #endregion
    }
}
