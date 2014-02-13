using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.SchulDB.Helper;
using Groll.Schule.DataManager;
using Groll.Schule.Model;
using System.Collections.ObjectModel;
using System.Windows.Controls.Ribbon;
using System.Windows.Media;
using Groll.Schule.SchulDB.Commands;


namespace Groll.Schule.SchulDB.ViewModels
{
    public class RibbonTabBeobachtungenViewVM : RibbonTabVM
    {
        private ObservableCollection<Beobachtung> beobachtungenCollection;
        private RibbonBaseVM selectedExportFilter;
        private List<RibbonMenuEntryVM> exportFilterItemSource;
        private RibbonMenuEntryVM selectedGrouping;
        private RibbonMenuEntryVM selectedSorting;
        private RibbonMenuEntryVM selectedTextBreakKlasse;
        private RibbonMenuEntryVM selectedTextBreakSchueler;
        private RibbonMenuEntryVM selectedTextBreakDatum;
        private bool paragraphAfterEveryEntry = false;
        private bool repeatSameName = false;


        private bool newPageOnSchüler;

        public bool NewPageOnSchüler
        {
            get { return newPageOnSchüler; }
            set
            {
                if (newPageOnSchüler != value)
                { newPageOnSchüler = value; OnPropertyChanged(); }
            }
        }

        private bool editMode;

        public bool EditMode
        {
            get { return editMode; }
            set
            {
                if (editMode != value)
                { editMode = value; OnPropertyChanged(); }
            }
        }

        #region Properties für Bindings

   

        #endregion

        #region Konstruktor
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="ribbonVM">Root Element</param>
        public RibbonTabBeobachtungenViewVM(RibbonVM ribbonVM = null)
            : base(ribbonVM)
        {
            Label = "Beobachtungen ansehen / bearbeiten";
            IsVisible = false;  // per Default unsichtbar
            ContextualTabGroupHeader = "Beobachtungen";
            
           
           // SelectedExportFilter = ExportFilterItemSource[0];
                
                      
        }
        #endregion

        #region Database-Handling
        public override void OnDatabaseChanged()
        {
            // invalidate all database relevant properties
            beobachtungenCollection = null;
            OnPropertyChanged("Last10Beobachtungen");
            base.OnDatabaseChanged();
        }

        void beo_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Beobachtungen changed            
            OnPropertyChanged("Last10Beobachtungen");
        }
        #endregion

    }
}
