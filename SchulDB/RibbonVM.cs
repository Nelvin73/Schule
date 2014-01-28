using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.SchulDB.Helper;

namespace Groll.Schule.SchulDB
{
    public class RibbonVM : ObservableObject
    {
        public MainWindow MainWindow { get; set; }
        
        #region CurrentDB info properties        
        private string currentDBtype = "";
        public string CurrentDBtype
        {
            get { return currentDBtype; }
            set { currentDBtype = value; OnPropertyChanged(); }
        }

        private string currentDdFile = "";
        public string CurrentDbFile
        {
            get { return currentDdFile; }
            set { currentDdFile = value; OnPropertyChanged(); }
        }
        #endregion
      
        #region ShowContextTabgroups

        private bool showBeobachtungenTab;
        public bool ShowBeobachtungenTab
        {
            get { return showBeobachtungenTab; }
            set { showBeobachtungenTab = value; OnPropertyChanged(); }
        }

        #endregion


        #region IsSelected
        private bool beobachtungenIsSelected;

        public bool BeobachtungenIsSelected
        {
            get { return beobachtungenIsSelected; }
            set { beobachtungenIsSelected = value; OnPropertyChanged(); }
        }

        private bool defaultIsSelected;

        public bool DefaultIsSelected
        {
            get { return defaultIsSelected; }
            set { defaultIsSelected = value; OnPropertyChanged(); }
        }


        #endregion


        public RibbonVM()
        {
            if (App.Current.MainWindow == null)
            {
                // Designer mode!
                ShowBeobachtungenTab = true;
                BeobachtungenIsSelected = true;
            }
            else
            {
                // Normal Mode
                ShowBeobachtungenTab = false;
            }
        }
    }
}
