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
    public class RibbonTabBeobachtungenVM : RibbonTabVM
    {
        private ObservableCollection<Beobachtung> beobachtungenCollection;
        private RibbonBaseVM selectedExportFilter;
        private List<RibbonMenuEntryVM> exportFilterItemSource;
        private RibbonMenuEntryVM selectedGrouping;

        #region Properties für Bindings

        /// <summary>
        /// Button zum Start des Exports
        /// </summary>
        public RibbonBaseVM ExportButton
        {
            get
            {
                string Key = "ExportButton";
                RibbonBaseVM t = GetElement(Key);

                return t ?? SetElement(Key, 
                    new RibbonBaseVM()
                    {
                        Label = "Exportieren",
                        LargeImageSourceFile = "Images/Word_Doc2.ico",
                        Command = BeobachtungenCommands.ExportBeobachtungen,
                        ToolTipTitle = "Startet den Exportvorgang",
                        ToolTipDescription = "Exportiert die Beobachtungen nach Word,\nentsprechend der eingestellten Vorgaben.",
                        ToolTipImageSourceFile = "Images/Word_Doc1.ico"
                    });                               
            }
        }

        /// <summary>
        /// Menu zum Setzen des Filters
        /// </summary>
        public RibbonMenuSelectedItemEntryVM FilterMenuButton
        {
            get
            {
                string Key = "FilterMenuButton";
                RibbonMenuSelectedItemEntryVM t = GetElement(Key) as RibbonMenuSelectedItemEntryVM;
                if (t == null)
                {
                    t = new RibbonMenuSelectedItemEntryVM()
                    {
                        ItemsSource = new List<RibbonMenuEntryVM>()
                        {
                            new RibbonMenuEntryVM()
                            {
                                Label = "Alles",
                                LongHeader = "Alle Beobachtungen exportieren",
                                Tag = "ALL",
                                LargeImageSourceFile = "Images/Aktenschrank.ico",                    
                                IsSelected = true,              
                                ToolTipTitle = "Alle Beobachtungen",
                                ToolTipDescription = "Exportiert alle vorhandenen Schülerbeobachtungen.",
                                ToolTipImageSourceFile = "Images/Word_Doc1.ico"                  
                            },
                            new RibbonMenuEntryVM()
                            {
                                Label = "Schuljahr", 
                                LongHeader = "Alle Beobachtungen des aktuellen Schuljahrs",
                                Tag = "SJ",
                                LargeImageSourceFile = "Images/neues-jahr-2012.jpg",   
                                ToolTipTitle = "Aktuelles Schuljahr",
                                ToolTipDescription = "Alle Beobachtungen aus dem aktuellen Schuljahr werden exportiert.",
                                ToolTipImageSourceFile = "Images/Word_Doc1.ico"                          
                            },
                            new RibbonMenuEntryVM()
                            {
                                Label = "Klasse",   
                                LongHeader = "Beobachtungen von Schülern der ausgewählten \nKlasse exportieren",
                                Tag = "KL",
                                LargeImageSourceFile = "Images/Klasse.ico",   
                                ToolTipTitle = "Ausgewählte Klasse",
                                ToolTipDescription = "Nur Beobachtungen der ausgewählten Klasse werden exportiert.",
                                ToolTipImageSourceFile = "Images/Word_Doc1.ico"                   
                            },                
                            new RibbonMenuEntryVM()
                            {
                                Label = "Schüler", 
                                LongHeader = "Beobachtungen des markierten Schülers\n aus diesem Schuljahr",
                                Tag = "SSJ",
                                LargeImageSourceFile = "Images/Schüler.ico",                       
                                ToolTipTitle = "Ausgewählter Schüler (nur aktuelles Schuljahr)",
                                ToolTipDescription = "Aktuelle Beobachtungen des ausgewählten Schülers werden exportiert.\n\nNur aktuelles Schuljahr!",
                                ToolTipImageSourceFile = "Images/Word_Doc1.ico"     
                            },
                            new RibbonMenuEntryVM()
                            {
                                Label = "Schüler",  
                                LongHeader = "Beobachtungen des markierten Schülers",
                                Tag = "SCH",
                                LargeImageSourceFile = "Images/Schüler.ico",  
                                ToolTipTitle = "Ausgewählter Schüler",
                                ToolTipDescription = "Beobachtungen des ausgewählten Schülers werden exportiert.\n\nAlle Schuljahre!",
                                ToolTipImageSourceFile = "Images/Word_Doc1.ico"                                 
                            }
                        }
                    };
                    t.SelectedItem = t.ItemsSource[0];
                    SetElement(Key, (RibbonBaseVM)t);
                }

                return t;
            }
        }

        public RibbonMenuEntryVM GroupingSettingsMenuButton
        {
            get
            {
                string Key = "GroupingSettingsMenuButton";
                RibbonMenuEntryVM t = GetElement(Key) as RibbonMenuEntryVM;
                if (t == null)
                {
                    t = new RibbonMenuEntryVM()
                    {
                        Label = "Gruppierung",
                        ItemsSource = new List<RibbonMenuEntryVM>()
                        {
                            new RibbonMenuEntryVM()
                            {
                                Label = "Schüler",
                                LongHeader = "Gruppiert nach Schüler",
                                Tag = "S",
                                LargeImageSourceFile = "Images/Schüler.ico",                    
                                SmallImageSourceFile = "Images/Schüler.ico",                    
                                IsSelected = true,              
                               // ToolTipTitle = "Alle Beobachtungen",
                               // ToolTipDescription = "Exportiert alle vorhandenen Schülerbeobachtungen.",
                               // ToolTipImageSourceFile = "Images/Word_Doc1.ico"                  
                            },
                            new RibbonMenuEntryVM()
                            {
                                Label = "Datum",
                                LongHeader = "Gruppiert nach Datum",
                                Tag = "D",
                                LargeImageSourceFile = "Images/Aktenschrank.ico",                    
                                SmallImageSourceFile = "Images/Aktenschrank.ico",                              
                            }
                        }
                    };
                    t.SelectedItem = t.ItemsSource[0];
                    SetElement(Key, (RibbonBaseVM)t);
                }

                return t;
            }
        }


        public RibbonMenuEntryVM SelectedGrouping
        {
            get
            {
                return selectedGrouping;
            }
            set
            {
                if (selectedGrouping != value)
                {
                    selectedGrouping = value; OnPropertyChanged();
                }
            }
        }

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
        public RibbonTabBeobachtungenVM(RibbonVM ribbonVM = null) : base(ribbonVM)
        {
            Label = "Beobachtungen";
            IsVisible = false;  // per Default unsichtbar
            ContextualTabGroupHeader = "Beobachtungen";
            
           
           // SelectedExportFilter = ExportFilterItemSource[0];

        

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
