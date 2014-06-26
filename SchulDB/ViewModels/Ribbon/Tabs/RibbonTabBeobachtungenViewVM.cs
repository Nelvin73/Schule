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

        private bool newPageOnSchüler;
        private bool editMode;

        public bool NewPageOnSchüler
        {
            get { return newPageOnSchüler; }
            set
            {
                if (newPageOnSchüler != value)
                { newPageOnSchüler = value; OnPropertyChanged(); }
            }
        }

       
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
