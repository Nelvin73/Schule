﻿using System;
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
    public class RibbonTabBeobachtungenVM : RibbonTabViewModel
    {
        private ObservableCollection<Beobachtung> beobachtungenCollection;
        private ObservableCollection<Textbaustein> texteCollection;
        
        private RibbonMenuItemVM selectedGrouping;
        private RibbonMenuItemVM selectedSorting;
        private bool paragraphAfterEveryEntry = false;
        private bool repeatSameName = false;


        #region Properties für Bindings

        #region Ribbon Elements
        // History-View Menu
        public RibbonSelectedItemMenuItemVM HistoryViewMenuButton
        {
            get
            {
                string Key = "HistoryViewMenuButton";
                RibbonSelectedItemMenuItemVM t = GetElement(Key) as RibbonSelectedItemMenuItemVM;
                if (t == null)
                {
                    t = new RibbonSelectedItemMenuItemVM()
                    {
                        
                        ItemsSource = new List<RibbonMenuItemVM>()
                        {
                            new RibbonMenuItemVM
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
                            new RibbonMenuItemVM()
                            {
                                Tag = "Schüler",
                                LongHeader = "Die letzten Beobachtungen des ausgewählten Schülers",                                
                                SmallImageSourceFile = "Schüler.ico",
                                LargeImageSourceFile = "Schüler.ico",   
                                ToolTipTitle = "Ausgewählter Schüler",
                                ToolTipDescription = "Zeigt die letzten Beobachtungen des ausgewählten Schülers an.",
                                //ToolTipImageSourceFile = "Schüler.ico"                          
                            },
                            new RibbonMenuItemVM()
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
                    SetElement(Key, t);
                }

                return t;
            }
        }

        /// <summary>
        /// Button zum Wechseln zur View-Page
        /// </summary>
        public RibbonItemViewModel ViewBeobachtungenButton
        {
            get
            {
                string Key = "ViewBeobachtungenButton";
                RibbonItemViewModel t = GetElement(Key) as RibbonItemViewModel;

                return t ?? SetElement(Key,
                    new RibbonItemViewModel()
                    {
                        Label = "Beobachtungen anzeigen",
                        LargeImageSourceFile = "Word_Doc1.ico",
                        Command = MainWindowViewModel.Command_Navigate,
                        CommandParameter = "beobachtungenansicht",
                        ToolTipTitle = "Geht zur Beobachtungen-Ansicht",
                        ToolTipDescription = "Ausgabe der Beobachtungen mit Möglichkeit zur nachträglichen Änderung.",
                        ToolTipImageSourceFile = "Word_Doc1.ico"
                    });                               
            }
        }

        /// <summary>
        /// Button zum Start des Exports
        /// </summary>
        public RibbonItemViewModel ExportButton
        {
            get
            {
                string Key = "ExportButton";
                RibbonItemViewModel t = GetElement(Key);

                return t ?? SetElement(Key,
                    new RibbonItemViewModel()
                    {
                        Label = "Exportieren",
                        LargeImageSourceFile = "Word_Doc2.ico",                        
                        ToolTipTitle = "Startet den Exportvorgang",
                        ToolTipDescription = "Exportiert die Beobachtungen nach Word,\nentsprechend der eingestellten Vorgaben.",
                        ToolTipImageSourceFile = "Word_Doc1.ico"
                    });                               
            }
        }

        /// <summary>
        /// Menu zum Setzen des Filters
        /// </summary>
        public RibbonSelectedItemMenuItemVM FilterMenuButton
        {
            get
            {
                string Key = "FilterMenuButton";
                RibbonSelectedItemMenuItemVM t = GetElement(Key) as RibbonSelectedItemMenuItemVM;
                if (t == null)
                {
                    t = new RibbonSelectedItemMenuItemVM()
                    {
                        ItemsSource = new List<RibbonMenuItemVM>()
                        {
                            new RibbonMenuItemVM()
                            {
                                Label = "Alles",
                                LongHeader = "Alle Beobachtungen exportieren",
                                Tag = "ALL",
                                LargeImageSourceFile = "Aktenschrank.ico",                    
                                ToolTipTitle = "Alle Beobachtungen",
                                ToolTipDescription = "Exportiert alle vorhandenen Schülerbeobachtungen.",
                                ToolTipImageSourceFile = "Word_Doc1.ico"                  
                            },
                            new RibbonMenuItemVM()
                            {
                                Label = "Schuljahr", 
                                LongHeader = "Alle Beobachtungen des aktuellen Schuljahrs",
                                Tag = "SJ",
                                LargeImageSourceFile = "neues-jahr-2012.jpg",   
                                ToolTipTitle = "Aktuelles Schuljahr",
                                ToolTipDescription = "Alle Beobachtungen aus dem aktuellen Schuljahr werden exportiert.",
                                ToolTipImageSourceFile = "Word_Doc1.ico"                          
                            },
                            new RibbonMenuItemVM()
                            {
                                Label = "Klasse",   
                                LongHeader = "Beobachtungen von Schülern der ausgewählten \nKlasse exportieren",
                                Tag = "KL",
                                LargeImageSourceFile = "Klasse.ico",   
                                ToolTipTitle = "Ausgewählte Klasse",
                                ToolTipDescription = "Nur Beobachtungen der ausgewählten Klasse werden exportiert.",
                                ToolTipImageSourceFile = "Word_Doc1.ico"                   
                            },                
                            new RibbonMenuItemVM()
                            {
                                Label = "Schüler", 
                                LongHeader = "Beobachtungen des markierten Schülers\n aus diesem Schuljahr",
                                Tag = "SSJ",
                                LargeImageSourceFile = "Schüler.ico",                       
                                ToolTipTitle = "Ausgewählter Schüler (nur aktuelles Schuljahr)",
                                ToolTipDescription = "Aktuelle Beobachtungen des ausgewählten Schülers werden exportiert.\n\nNur aktuelles Schuljahr!",
                                ToolTipImageSourceFile = "Word_Doc1.ico"     
                            },
                            new RibbonMenuItemVM()
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
                    SetElement(Key, t);
                }

                return t;
            }
        }

        public RibbonMenuItemVM GroupingSettingsMenuButton
        {
            get
            {
                string Key = "GroupingSettingsMenuButton";
                RibbonMenuItemVM t = GetElement(Key) as RibbonMenuItemVM;
                if (t == null)
                {
                    t = new RibbonMenuItemVM()
                    {
                        Label = "Gruppieren",
                        ItemsSource = new List<RibbonMenuItemVM>()
                        {
                            new RibbonMenuItemVM()
                            {
                                Label = "Schüler",
                                LongHeader = "Gruppiert nach Schüler",
                                Tag = "S",
                                LargeImageSourceFile = "Schüler.ico",                    
                                SmallImageSourceFile = "Schüler.ico",                    
                                IsSelected = true,                                                 
                            },
                            new RibbonMenuItemVM()
                            {
                                Label = "Datum",
                                LongHeader = "Gruppiert nach Datum",
                                Tag = "D",
                                LargeImageSourceFile = "Kalender3.png",                    
                                SmallImageSourceFile = "Kalender3.png",                              
                            }
                        }
                    };
                    t.SelectedItem = SelectedGrouping = t.ItemsSource[0];
                    SetElement(Key, t);
                }

                return t;
            }
        }

        public RibbonMenuItemVM SortSettingsMenuButton
        {
            get
            {
                string Key = "SortSettingsMenuButton";
                RibbonMenuItemVM t = GetElement(Key) as RibbonMenuItemVM;
                if (t == null)
                {
                    t = new RibbonMenuItemVM()
                    {
                        Label = "Sortieren",
                        StaysOpenOnClick = true,
                        ItemsSource = new List<RibbonMenuItemVM>()
                        {
                            new RibbonMenuItemVM()
                            {
                                Label = "Absteigend",
                                LongHeader = "Absteigend- Neueste Einträge zuerst",
                                Tag = "DESC",
                        //        LargeImageSourceFile = "SortDesc.ico",                    
                         //       SmallImageSourceFile = "SortDesc.ico",                    
                                IsSelected = true,                                                 
                            },
                            new RibbonMenuItemVM()
                            {
                                Label = "Aufsteigend",
                                LongHeader = "Aufsteigend - Älteste Einträge zuerst",
                                Tag = "ASC",
                        //        LargeImageSourceFile = "SortAsc.ico",                    
                         //       SmallImageSourceFile = "SortAsc.ico",                              
                            }
                        }
                    };

                    t.SelectedItem = SelectedSorting = t.ItemsSource[0];
                    SetElement(Key, t);
                }

                return t;
            }
        }
     
        public RibbonMenuItemVM TextBreakKlasseMenuButton
        {
            get
            {
                string Key = "TextBreakKlasseMenuButton";
                RibbonMenuItemVM t = GetElement(Key) as RibbonMenuItemVM;
                if (t == null)
                {
                    t = new RibbonMenuItemVM()
                    {
                        Label = "Klasse",
                        StaysOpenOnClick = true,
                        ItemsSource = new List<RibbonMenuItemVM>()
                        {
                            new RibbonMenuItemVM()
                            {
                                Label = "Seite",
                                LongHeader = "Jede Klasse auf einer neuen Seite\nanfangen lassen",
                                Tag = "Seite",
                                IsSelected = true,                                                 
                            },
                            new RibbonMenuItemVM()
                            {
                                Label = "Absatz",
                                LongHeader = "Fortlaufend\n(neuer Absatz)",
                                Tag = "Absatz",
                            }
                        }
                    };
                    t.SelectedItem =  t.ItemsSource[0];
                    SetElement(Key, t);
                }

                return t;
            }
        }

        public RibbonMenuItemVM TextBreakSchülerMenuButton
        {
            get
            {
                string Key = "TextBreakSchülerMenuButton";
                RibbonMenuItemVM t = GetElement(Key) as RibbonMenuItemVM;
                if (t == null)
                {
                    t = new RibbonMenuItemVM()
                    {
                        Label = "Schüler",
                        StaysOpenOnClick = true,
                        ItemsSource = new List<RibbonMenuItemVM>()
                        {
                            new RibbonMenuItemVM()
                            {
                                Label = "Seite",
                                LongHeader = "Jeden Schüler auf einer neuen Seite\nanfangen lassen",
                                Tag = "Seite",
                                IsSelected = true,                                                 
                            },
                            new RibbonMenuItemVM()
                            {
                                Label = "Absatz",
                                LongHeader = "Fortlaufend\n(neuer Absatz)",
                                Tag = "Absatz",
                            },
                            new RibbonMenuItemVM()
                            {
                                Label = "Fortlaufend",
                                LongHeader = "Fortlaufend\n (ohne Trennung)",
                                Tag = "None",
                            }
                        }
                    };
                    t.SelectedItem =  t.ItemsSource[0];
                    SetElement(Key, t);
                }

                return t;
            }
        }

        public RibbonMenuItemVM TextBreakDatumMenuButton
        {
            get
            {
                string Key = "TextBreakDatumMenuButton";
                RibbonMenuItemVM t = GetElement(Key) as RibbonMenuItemVM;
                if (t == null)
                {
                    t = new RibbonMenuItemVM()
                    {
                        Label = "Datum",
                        StaysOpenOnClick = true,
                        ItemsSource = new List<RibbonMenuItemVM>()
                        {                      
                            new RibbonMenuItemVM()
                            {
                                Label = "Seite",
                                LongHeader = "Neues Datum in einer neuen Seite\nanfangen lassen",
                                Tag = "Seite",                                                                              
                            },
                            new RibbonMenuItemVM()
                            {
                                Label = "Absatz",
                                LongHeader = "Neuer Absatz nach neuem Datum",
                                Tag = "Absatz",                                
                            },
                            new RibbonMenuItemVM()
                            {
                                Label = "Fortlaufend",
                                LongHeader = "Fortlaufend\n (ohne Trennung)",
                                Tag = "None",
                                IsSelected = true
                            }
                        }
                    };
                    t.SelectedItem = t.ItemsSource[02];
                    SetElement(Key, t);
                }

                return t;
            }
        }
        #endregion

        #region Settings
        private Reports.BeobachtungenExport.TextBreakType ConvertToTextBreakType(object o)
        {
            switch ((o ?? "").ToString())
            {
                case "Seite":
                    return Reports.BeobachtungenExport.TextBreakType.Page;
                case "Absatz":
                    return Reports.BeobachtungenExport.TextBreakType.Paragraph;
                default:
                    return Reports.BeobachtungenExport.TextBreakType.None;
            }
        }

        public Reports.BeobachtungenExport.BeobachtungenExportSettings ExportSettings
        {
            get
            {
                var s = new Reports.BeobachtungenExport.BeobachtungenExportSettings();
                s.DateSortDirection = (SelectedSorting.Tag ?? "ASC").ToString() == "ASC" ? System.ComponentModel.ListSortDirection.Ascending : System.ComponentModel.ListSortDirection.Descending;
                s.TextBreakNewKlasse = ConvertToTextBreakType(SelectedTextBreakKlasse.Tag);
                s.TextBreakNewSchüler = ConvertToTextBreakType(SelectedTextBreakSchüler.Tag);
                s.TextBreakNewDatum = ConvertToTextBreakType(SelectedTextBreakDatum.Tag);
                s.GroupBy = (SelectedGrouping.Tag ?? "").ToString() == "S" ? Reports.BeobachtungenExport.GroupByType.GroupBySchüler : Reports.BeobachtungenExport.GroupByType.GroupByDatum;
                s.ParagraphAfterEveryEntry = ParagraphAfterEveryEntry;
                s.RepeatSameName = RepeatSameName;

               
                return s;
            }
        }
        public RibbonMenuItemVM SelectedGrouping
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
                        SelectedTextBreakSchüler = TextBreakSchülerMenuButton.ItemsSource[0];
                        SelectedTextBreakDatum = TextBreakDatumMenuButton.ItemsSource[2];
                    }else
                    {
                        SelectedTextBreakSchüler = TextBreakSchülerMenuButton.ItemsSource[2];
                        SelectedTextBreakDatum = TextBreakDatumMenuButton.ItemsSource[0];

                    }
                }
            }
        }

        public RibbonMenuItemVM SelectedSorting
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

        public RibbonMenuItemVM SelectedTextBreakKlasse
        {
            get
            {
                return TextBreakKlasseMenuButton.SelectedItem;                
            }
            set
            {
                if (TextBreakKlasseMenuButton.SelectedItem != value)
                {
                    TextBreakKlasseMenuButton.SelectedItem = value;
                    OnPropertyChanged();
                }
            }
        }

        public RibbonMenuItemVM SelectedTextBreakSchüler
        {
            get
            {
                return TextBreakSchülerMenuButton.SelectedItem;
            }
            set
            {
                if (TextBreakKlasseMenuButton.SelectedItem != value)
                {
                    TextBreakSchülerMenuButton.SelectedItem = value;
                    OnPropertyChanged();
                }
            }
        }

        public RibbonMenuItemVM SelectedTextBreakDatum
        {
            get
            {
                return TextBreakDatumMenuButton.SelectedItem;
            }
            set
            {
                if (TextBreakKlasseMenuButton.SelectedItem != value)
                {
                    TextBreakDatumMenuButton.SelectedItem = value;
                    OnPropertyChanged();
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
                if (UnitOfWork == null || UnitOfWork.ConnectionString == "")
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

        public List<Textbaustein> Textbausteine
        {
            get
            {
                if (UnitOfWork == null || UnitOfWork.ConnectionString  == "")
                    return new List<Textbaustein>();

                if (texteCollection == null)
                {
                    // Liste der Beobachtungen laden und benachrichtigen lassen, wenn diese sich ändert.
                    texteCollection = UnitOfWork.Textbausteine.GetObservableCollection();
                    texteCollection.CollectionChanged += txt_CollectionChanged;
                }

                return texteCollection.ToList();
            }

        }
       
        #endregion
        
        #endregion

        #region Command Implementation
        #endregion

        #region Konstruktor
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="RibbonViewModel">Root Element</param>
        public RibbonTabBeobachtungenVM() : base()
        {
            Label = "Beobachtungen eingeben";            
            
          //  IsVisible = true;  // per Default unsichtbar
            ContextualTabGroupHeader = "Beobachtungen";
           
        }
        #endregion

        #region Database-Handling
        public override void OnDatabaseChanged()
        {
            // invalidate all database relevant properties
            beobachtungenCollection = null;
            texteCollection = null;
            OnPropertyChanged("Textbausteine");
            OnPropertyChanged("Last10Beobachtungen");
            base.OnDatabaseChanged();
        }
        
        void txt_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Textbausteine changed            
            OnPropertyChanged("Textbausteine");
        }
        
        void beo_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Beobachtungen changed            
            OnPropertyChanged("Last10Beobachtungen");
        }
        #endregion

    }
}
