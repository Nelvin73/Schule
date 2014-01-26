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
            ShowBeobachtungenTab = false;
        }
    }
}
