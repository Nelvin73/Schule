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
    public class RibbonTabStandardVM : RibbonTabVM
    {
      
        #region Properties für Bindings
/*
        /// <summary>
        /// Button zum Start des Exports
        /// </summary>
        public RibbonBaseVM ExportButton
        {
            get
            {
                string Key = "ExportButton";
                RibbonBaseVM t = GetElement(Key);

                if (t == null)
                {
                    t = SetElement(Key, new RibbonBaseVM()
                    {
                        Label = "Exportieren",
                        LargeImageSourceFile = "Word_Doc2.ico",
                        Command = BeobachtungenCommands.ExportBeobachtungen,
                        ToolTipTitle = "Startet den Exportvorgang",
                        ToolTipDescription = "Exportiert die Beobachtungen nach Word,\nentsprechend der eingestellten Vorgaben.",
                        ToolTipImage = new Uri("Word_Doc1.ico", UriKind.Relative)
                    });

                }
                return t;
            }
        }       


        public RibbonBaseVM SelectedExportFilter
        {
            get { return selectedExportFilter; }
            set
            {
                if (selectedExportFilter != value)
                { selectedExportFilter = value; OnPropertyChanged(); }
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


        public List<RibbonMenuEntryVM> ExportFilterItemSource
        {
            get
            {
                if (exportFilterItemSource == null)
                {
                    exportFilterItemSource = new List<RibbonMenuEntryVM>()
                    {
                    new RibbonMenuEntryVM()
                    {
                        Label = "Alles",
                        LongHeader = "Alle Beobachtungen exportieren",
                        Tag = "ALL",
                        LargeImageSourceFile = "Aktenschrank.ico",                    
                        IsSelected = true,              
                        ToolTipTitle = "Alle Beobachtungen",
                        ToolTipDescription = "Exportiert alle vorhandenen Schülerbeobachtungen.",
                        ToolTipImage = new Uri("Word_Doc1.ico", UriKind.Relative)                    
                    },
                    new RibbonMenuEntryVM()
                    {
                        Label = "Schuljahr", 
                        LongHeader = "Alle Beobachtungen des aktuellen Schuljahrs",
                        Tag = "SJ",
                        LargeImageSourceFile = "neues-jahr-2012.jpg",   
                        ToolTipTitle = "Aktuelles Schuljahr",
                        ToolTipDescription = "Alle Beobachtungen aus dem aktuellen Schuljahr werden exportiert.",
                        ToolTipImage = new Uri("Word_Doc1.ico", UriKind.Relative)                              
                    },
                    new RibbonMenuEntryVM()
                    {
                        Label = "Klasse",   
                        LongHeader = "Beobachtungen von Schülern der ausgewählten \nKlasse exportieren",
                        Tag = "KL",
                        LargeImageSourceFile = "Klasse.ico",   
                        ToolTipTitle = "Ausgewählte Klasse",
                        ToolTipDescription = "Nur Beobachtungen der ausgewählten Klasse werden exportiert.",
                        ToolTipImage = new Uri("Word_Doc1.ico", UriKind.Relative)                      
                    },                
                    new RibbonMenuEntryVM()
                    {
                        Label = "Schüler", 
                        LongHeader = "Beobachtungen des markierten Schülers\n aus diesem Schuljahr",
                        Tag = "SSJ",
                        LargeImageSourceFile = "Schüler.ico",                       
                        ToolTipTitle = "Ausgewählter Schüler (nur aktuelles Schuljahr)",
                        ToolTipDescription = "Aktuelle Beobachtungen des ausgewählten Schülers werden exportiert.\n\nNur aktuelles Schuljahr!",
                        ToolTipImage = new Uri("Word_Doc1.ico", UriKind.Relative)     
                    },
                    new RibbonMenuEntryVM()
                    {
                        Label = "Schüler",  
                        LongHeader = "Beobachtungen des markierten Schülers",
                        Tag = "SCH",
                        LargeImageSourceFile = "Schüler.ico",  
                        ToolTipTitle = "Ausgewählter Schüler",
                        ToolTipDescription = "Beobachtungen des ausgewählten Schülers werden exportiert.\n\nAlle Schuljahre!",
                        ToolTipImage = new Uri("Word_Doc1.ico", UriKind.Relative)                                 
                    }};
                }
                SelectedExportFilter = exportFilterItemSource[0];
                return exportFilterItemSource;
            }
        }

       
        private bool groupBySchüler;

        public bool GroupBySchüler
        {
            get { return groupBySchüler; }
            set { groupBySchüler = value; }
        }

        System.Windows.Input.RoutedCommand s = Commands.BeobachtungenCommands.ExportBeobachtungen;
       
        #endregion

        */

        #endregion

        #region Konstruktor
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="RibbonViewModel">Root Element</param>
        public RibbonTabStandardVM(RibbonViewModel RibbonViewModel = null)
            : base(RibbonViewModel)
        {
            Label = "Standard";
            IsSelected = true;                             
        }
        #endregion

        #region Database-Handling
        public override void OnDatabaseChanged()
        {
            // invalidate all database relevant properties
            base.OnDatabaseChanged();
        }

      
        #endregion

    }
}
