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


namespace Groll.Schule.SchulDB.ViewModels
{
    public class RibbonTabBeobachtungenVM : RibbonTabVM
    {
        private ObservableCollection<Beobachtung> beobachtungenCollection;

        #region Properties für Bindings
        
        /// <summary>
        /// Liste der 10 letzten Beobachtungen
        /// </summary>
        public List<Beobachtung> Last10Beobachtungen
        {
            get
            {
                if (UnitOfWork == null || UnitOfWork.CurrentDbType == UowSchuleDB.DatabaseType.None)
                    return new List<Beobachtung>();

                if (beobachtungenCollection == null)
                {
                    // Liste der Beobachtungen laden und benachrichtigen lassen, wenn diese sich ändert.
                    beobachtungenCollection = UnitOfWork.Beobachtungen.GetObservableCollection();
                    beobachtungenCollection.CollectionChanged += beo_CollectionChanged;
                }

                return beobachtungenCollection.Reverse().DistinctBy( x => x.Text).Take(10).ToList();                
            }

        }
       

        #region Properties für den ExportFilter Button
    
        private List<RibbonButtonVM> exportFilterItemSource;

        public List<RibbonButtonVM> ExportFilterItemSource
        {
            get { return exportFilterItemSource; }
            set { exportFilterItemSource = value; OnPropertyChanged(); }
        }
        
        private RibbonButtonVM exportFilterSelected;

        public RibbonButtonVM ExportFilterSelected
        {
            get { return exportFilterSelected; }
            set { exportFilterSelected = value; OnPropertyChanged(); }
        }

        private bool groupBySchüler;

        public bool GroupBySchüler
        {
            get { return groupBySchüler; }
            set { groupBySchüler = value; }
        }

        System.Windows.Input.RoutedCommand s = Commands.BeobachtungenCommands.ExportBeobachtungen;
       
        #endregion

        

        #endregion

        #region Konstruktor
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="ribbonVM">Root Element</param>
        public RibbonTabBeobachtungenVM(RibbonVM ribbonVM) : base(ribbonVM)
        {
            Header = "Beobachtungen";
            
            // Exportfilter Button           
            ExportFilterItemSource = new List<RibbonButtonVM>()
            {
                new RibbonButtonVM()
                {
                    Header = "Alles",
                    LongHeader = "Alles Beobachtungen\nexportieren",
                    Tag = "ALL",
                    LargeImageSourceFile = "Aktenschrank.ico",                    
                    IsSelected = true
                },
                new RibbonButtonVM()
                {
                    Header = "Schuljahr", 
                    LongHeader = "Beobachtungen aus dem\naktuellen Schuljahr\nexportieren",
                    Tag = "SJ",
                    LargeImageSourceFile = "neues-jahr-2012.jpg",                                       
                },
                new RibbonButtonVM()
                {
                    Header = "Klasse",   
                    LongHeader = "Beobachtungen von Schülern\nder ausgewählten Klasse\nexportieren",
                    Tag = "KL",
                    LargeImageSourceFile = "Klasse.ico",                                        
                },
                new RibbonButtonVM()
                {
                    Header = "Schüler",  
                    LongHeader = "Alle Beobachtungen des\nmarkierten Schülers\nexportieren",
                    Tag = "SCH",
                    LargeImageSourceFile = "Schüler.ico",                                        
                },
                new RibbonButtonVM()
                {
                    Header = "Schüler (Schuljahr)", 
                    LongHeader = "Beobachtungen des\nmarkierten Schülers aus\ndiesem Schuljahr",
                    Tag = "SSJ",
                    LargeImageSourceFile = "Schüler.ico",                                        
                }
            };

            ExportFilterSelected = ExportFilterItemSource[0];

        


            

            GroupBySchüler = true;
                      
        }
        #endregion

        #region Database-Handling
        public override void OnDatabaseChanged()
        {
            // invalidate all database relevant properties
            beo_CollectionChanged(null, null);
            base.OnDatabaseChanged();
        }

        void beo_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Beobachtungen changed
            beobachtungenCollection = null;
            OnPropertyChanged("Last10Beobachtungen");
        }
        #endregion

    }
}
