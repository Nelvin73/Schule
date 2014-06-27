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
        #region Properties für Bindings

        public bool NewPageOnSchüler
        {
            get { return UpdateViewButton.IsChecked; }
            set
            {
                if (UpdateViewButton.IsChecked != value)
                { UpdateViewButton.IsChecked = value; OnPropertyChanged(); }
            }
        }

       
        public bool EditMode
        {
            get { return EditModeButton.IsChecked; }
            set
            {
                if (EditModeButton.IsChecked != value)
                { EditModeButton.IsChecked = value; OnPropertyChanged(); }
            }
        }

        
        #region Ribbon Elements

        public RibbonBaseVM UpdateViewButton
        {
            get
            {
                string Key = "UpdateViewButton";
                RibbonBaseVM t = GetElement(Key) as RibbonBaseVM;
                if (t == null)
                {
                    t = new RibbonBaseVM()
                    {
                        Label = "Schüler auf neuer Seite",                                               
                    };                 
                    SetElement(Key, t);
                }
                return t;                
            }
        }

        public RibbonBaseVM EditModeButton
        {
            get
            {
                string Key = "EditModeButton";
                RibbonBaseVM t = GetElement(Key) as RibbonBaseVM;
                if (t == null)
                {
                    t = new RibbonBaseVM()
                    {
                        Label = "Ändern",
                    };
                    SetElement(Key, t);
                }
                return t;
            }
        }

        #endregion


        #endregion

        #region Konstruktor
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="RibbonViewModel">Root Element</param>
        public RibbonTabBeobachtungenViewVM(RibbonViewModel RibbonViewModel = null)
            : base(RibbonViewModel)
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
            base.OnDatabaseChanged();
        }
       
        #endregion

    }
}
