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
            set { newPageOnSchüler = value; }
        }
        


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
                        LargeImageSourceFile = "Word_Doc2.ico",
                        Command = BeobachtungenCommands.ExportBeobachtungen,
                        ToolTipTitle = "Startet den Exportvorgang",
                        ToolTipDescription = "Exportiert die Beobachtungen nach Word,\nentsprechend der eingestellten Vorgaben.",
                        ToolTipImageSourceFile = "Word_Doc1.ico"
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
                                LargeImageSourceFile = "Aktenschrank.ico",                    
                                ToolTipTitle = "Alle Beobachtungen",
                                ToolTipDescription = "Exportiert alle vorhandenen Schülerbeobachtungen.",
                                ToolTipImageSourceFile = "Word_Doc1.ico"                  
                            },
                            new RibbonMenuEntryVM()
                            {
                                Label = "Schuljahr", 
                                LongHeader = "Alle Beobachtungen des aktuellen Schuljahrs",
                                Tag = "SJ",
                                LargeImageSourceFile = "neues-jahr-2012.jpg",   
                                ToolTipTitle = "Aktuelles Schuljahr",
                                ToolTipDescription = "Alle Beobachtungen aus dem aktuellen Schuljahr werden exportiert.",
                                ToolTipImageSourceFile = "Word_Doc1.ico"                          
                            },
                            new RibbonMenuEntryVM()
                            {
                                Label = "Klasse",   
                                LongHeader = "Beobachtungen von Schülern der ausgewählten \nKlasse exportieren",
                                Tag = "KL",
                                LargeImageSourceFile = "Klasse.ico",   
                                ToolTipTitle = "Ausgewählte Klasse",
                                ToolTipDescription = "Nur Beobachtungen der ausgewählten Klasse werden exportiert.",
                                ToolTipImageSourceFile = "Word_Doc1.ico"                   
                            },                
                            new RibbonMenuEntryVM()
                            {
                                Label = "Schüler", 
                                LongHeader = "Beobachtungen des markierten Schülers\n aus diesem Schuljahr",
                                Tag = "SSJ",
                                LargeImageSourceFile = "Schüler.ico",                       
                                ToolTipTitle = "Ausgewählter Schüler (nur aktuelles Schuljahr)",
                                ToolTipDescription = "Aktuelle Beobachtungen des ausgewählten Schülers werden exportiert.\n\nNur aktuelles Schuljahr!",
                                ToolTipImageSourceFile = "Word_Doc1.ico"     
                            },
                            new RibbonMenuEntryVM()
                            {
                                Label = "Schüler",  
                                LongHeader = "Beobachtungen des markierten Schülers",
                                Tag = "SCH",
                                LargeImageSourceFile = "Schüler.ico",  
                                ToolTipTitle = "Ausgewählter Schüler",
                                ToolTipDescription = "Beobachtungen des ausgewählten Schülers werden exportiert.\n\nAlle Schuljahre!",
                                ToolTipImageSourceFile = "Word_Doc1.ico"                                 
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
                        Label = "Gruppieren",
                     //   StaysOpenOnClick = true,
                        ItemsSource = new List<RibbonMenuEntryVM>()
                        {
                            new RibbonMenuEntryVM()
                            {
                                Label = "Schüler",
                                LongHeader = "Gruppiert nach Schüler",
                                Tag = "S",
                                LargeImageSourceFile = "Schüler.ico",                    
                                SmallImageSourceFile = "Schüler.ico",                    
                                IsSelected = true,                                                 
                            },
                            new RibbonMenuEntryVM()
                            {
                                Label = "Datum",
                                LongHeader = "Gruppiert nach Datum",
                                Tag = "D",
                                LargeImageSourceFile = "Kalender3.png",                    
                                SmallImageSourceFile = "Kalender3.png",                              
                            }
                        }
                    };
                    t.SelectedItem = t.ItemsSource[0];
                    SetElement(Key, (RibbonBaseVM)t);
                }

                return t;
            }
        }
  
        public RibbonMenuEntryVM SortSettingsMenuButton
        {
            get
            {
                string Key = "SortSettingsMenuButton";
                RibbonMenuEntryVM t = GetElement(Key) as RibbonMenuEntryVM;
                if (t == null)
                {
                    t = new RibbonMenuEntryVM()
                    {
                        Label = "Sortieren",
                        StaysOpenOnClick = true,
                        ItemsSource = new List<RibbonMenuEntryVM>()
                        {
                            new RibbonMenuEntryVM()
                            {
                                Label = "Absteigend",
                                LongHeader = "Absteigend- Neueste Einträge zuerst",
                                Tag = "DESC",
                        //        LargeImageSourceFile = "SortDesc.ico",                    
                         //       SmallImageSourceFile = "SortDesc.ico",                    
                                IsSelected = true,                                                 
                            },
                            new RibbonMenuEntryVM()
                            {
                                Label = "Aufsteigend",
                                LongHeader = "Aufsteigend - Älteste Einträge zuerst",
                                Tag = "ASC",
                        //        LargeImageSourceFile = "SortAsc.ico",                    
                         //       SmallImageSourceFile = "SortAsc.ico",                              
                            }
                        }
                    };

                    t.SelectedItem = t.ItemsSource[0];
                    SetElement(Key, (RibbonBaseVM)t);
                }

                return t;
            }
        }
       
        public RibbonMenuSelectedItemEntryVM HistoryViewMenuButton
        {
            get
            {
                string Key = "HistoryViewMenuButton";
                RibbonMenuSelectedItemEntryVM t = GetElement(Key) as RibbonMenuSelectedItemEntryVM;
                if (t == null)
                {
                    t = new RibbonMenuSelectedItemEntryVM()
                    {
                        
                        ItemsSource = new List<RibbonMenuEntryVM>()
                        {
                            new RibbonMenuEntryVM()
                            {                                
                                Tag = "ID",
                                LongHeader = "Die zuletzt eingegebenen Beobachtungen",                                                                
                                SmallImageSourceFile = "Keys.ico",
                                LargeImageSourceFile = "Keys.ico",                    
                                IsSelected = true,              
                                ToolTipTitle = "Die zuletzt eingegebenen Beobachtungen",
                                ToolTipDescription = "Zeigt die zuletzt eingegebenen Beobachtungen an.",
                                ToolTipImageSourceFile = "Keys.ico"                  
                            },
                            new RibbonMenuEntryVM()
                            {
                                Tag = "Schüler",
                                LongHeader = "Die letzten Beobachtungen des ausgewählten Schülers",                                
                                SmallImageSourceFile = "Schüler.ico",
                                LargeImageSourceFile = "Schüler.ico",   
                                ToolTipTitle = "Ausgewählter Schüler",
                                ToolTipDescription = "Zeigt die letzten Beobachtungen des ausgewählten Schülers an.",
                                //ToolTipImageSourceFile = "Schüler.ico"                          
                            },
                            new RibbonMenuEntryVM()
                            {
                                Tag = "Datum",                                
                                LongHeader = "Aktuellste Beobachtungen nach Datum",
                                SmallImageSourceFile = "Kalender3.png",
                                LargeImageSourceFile = "Kalender3.png",   
                                ToolTipTitle = "Anzeige nach Datum",
                                ToolTipDescription = "Die aktuellsten Beobachtungen werden nach Datum sortiert angezeigt.",
                                //ToolTipImageSourceFile = "Kalender3.png"                   
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
                    if (selectedGrouping.Tag.ToString() == "S")
                    {
                        TextBreakSchuelerMenuButton.ItemsSource.ForEach(x => x.IsSelected = false);
                        TextBreakDatumMenuButton.ItemsSource.ForEach(x => x.IsSelected = false);
                        SelectedTextBreakSchueler = TextBreakSchuelerMenuButton.ItemsSource[0];
                        SelectedTextBreakDatum = TextBreakDatumMenuButton.ItemsSource[2];
                    }else
                    {
                       TextBreakSchuelerMenuButton.ItemsSource.ForEach(x => x.IsSelected = false);
                        TextBreakDatumMenuButton.ItemsSource.ForEach(x => x.IsSelected = false);
                        SelectedTextBreakSchueler = TextBreakSchuelerMenuButton.ItemsSource[2];
                        SelectedTextBreakDatum = TextBreakDatumMenuButton.ItemsSource[0];

                    }
                    OnPropertyChanged("TextBreakSchuelerMenuButton");
                    OnPropertyChanged("SelectedTextBreakDatum");
                }
            }
        }

        public RibbonMenuEntryVM SelectedSorting
        {
            get
            {
                return selectedSorting;
            }
            set
            {
                if (selectedSorting != value)
                {
                    selectedSorting = value; OnPropertyChanged();
                }
            }
        }

        public bool ParagraphAfterEveryEntry
        {
            get
            {
                return paragraphAfterEveryEntry;
            }
            set
            {
                if (paragraphAfterEveryEntry != value)
                {
                    paragraphAfterEveryEntry = value; OnPropertyChanged();
                }
            }
        }

        public bool RepeatSameName
        {
            get
            {
                return repeatSameName;
            }
            set
            {
                if (repeatSameName != value)
                {
                    repeatSameName = value; OnPropertyChanged();
                }
            }
        }
        
        public RibbonMenuEntryVM SelectedTextBreakKlasse
        {
            get
            {
                return selectedTextBreakKlasse;
            }
            set
            {
                if (selectedTextBreakKlasse != value)
                {
                    selectedTextBreakKlasse = value; OnPropertyChanged();
                }
            }
        }

        public RibbonMenuEntryVM SelectedTextBreakSchueler
        {
            get
            {
                return selectedTextBreakSchueler;
            }
            set
            {
                if (selectedTextBreakSchueler != value)
                {
                    selectedTextBreakSchueler = value; OnPropertyChanged();
                }
            }
        }

        public RibbonMenuEntryVM SelectedTextBreakDatum
        {
            get
            {
                return selectedTextBreakDatum;
            }
            set
            {
                if (selectedTextBreakDatum != value)
                {
                    selectedTextBreakDatum = value; OnPropertyChanged();
                }
            }
        }


        public RibbonMenuEntryVM TextBreakKlasseMenuButton
        {
            get
            {
                string Key = "TextBreakKlasseMenuButton";
                RibbonMenuEntryVM t = GetElement(Key) as RibbonMenuEntryVM;
                if (t == null)
                {
                    t = new RibbonMenuEntryVM()
                    {
                        Label = "Klasse",
                        StaysOpenOnClick = true,
                        ItemsSource = new List<RibbonMenuEntryVM>()
                        {
                            new RibbonMenuEntryVM()
                            {
                                Label = "Seite",
                                LongHeader = "Jede Klasse auf einer neuen Seite\nanfangen lassen",
                                Tag = "Seite",
                                IsSelected = true,                                                 
                            },
                            new RibbonMenuEntryVM()
                            {
                                Label = "Absatz",
                                LongHeader = "Fortlaufend\n(neuer Absatz)",
                                Tag = "Absatz",
                            }
                        }
                    };
                    t.SelectedItem = t.ItemsSource[0];
                    SetElement(Key, (RibbonBaseVM)t);
                }

                return t;
            }
        }

        public RibbonMenuEntryVM TextBreakSchuelerMenuButton
        {
            get
            {
                string Key = "TextBreakSchuelerMenuButton";
                RibbonMenuEntryVM t = GetElement(Key) as RibbonMenuEntryVM;
                if (t == null)
                {
                    t = new RibbonMenuEntryVM()
                    {
                        Label = "Schueler",
                        StaysOpenOnClick = true,
                        ItemsSource = new List<RibbonMenuEntryVM>()
                        {
                            new RibbonMenuEntryVM()
                            {
                                Label = "Seite",
                                LongHeader = "Jeden Schüler auf einer neuen Seite\nanfangen lassen",
                                Tag = "Seite",
                                IsSelected = true,                                                 
                            },
                            new RibbonMenuEntryVM()
                            {
                                Label = "Absatz",
                                LongHeader = "Fortlaufend\n(neuer Absatz)",
                                Tag = "Absatz",
                            },
                            new RibbonMenuEntryVM()
                            {
                                Label = "Fortlaufend",
                                LongHeader = "Fortlaufend\n (ohne Trennung)",
                                Tag = "None",
                            }}};

                    t.SelectedItem = t.ItemsSource[0];
                    SetElement(Key, (RibbonBaseVM)t);
                }

                return t;
            }
        }
  
        public RibbonMenuEntryVM TextBreakDatumMenuButton
        {
            get
            {
                string Key = "TextBreakDatumMenuButton";
                RibbonMenuEntryVM t = GetElement(Key) as RibbonMenuEntryVM;
                if (t == null)
                {
                    t = new RibbonMenuEntryVM()
                    {
                        Label = "Datum",
                        StaysOpenOnClick = true,
                        ItemsSource = new List<RibbonMenuEntryVM>()
                        {                      
                            new RibbonMenuEntryVM()
                            {
                                Label = "Seite",
                                LongHeader = "Neues Datum in einer neuen Seite\nanfangen lassen",
                                Tag = "Seite",                                                                              
                            },
                            new RibbonMenuEntryVM()
                            {
                                Label = "Absatz",
                                LongHeader = "Neuer Absatz nach neuem Datum",
                                Tag = "Absatz",                                
                            },
                            new RibbonMenuEntryVM()
                            {
                                Label = "Fortlaufend",
                                LongHeader = "Fortlaufend\n (ohne Trennung)",
                                Tag = "None",
                                IsSelected = true
                            }
                        }
                    };
                    t.SelectedItem = t.ItemsSource[0];
                    SetElement(Key, (RibbonBaseVM)t);
                }

                return t;
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
       

      
  
               
       
        

        #endregion

        #region Konstruktor
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="ribbonVM">Root Element</param>
        public RibbonTabBeobachtungenViewVM(RibbonVM ribbonVM = null)
            : base(ribbonVM)
        {
            Label = "Beobachtungen";
            IsVisible = true;  // per Default unsichtbar
       //     ContextualTabGroupHeader = "Beobachtungen bearbeiten";
            
           
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
