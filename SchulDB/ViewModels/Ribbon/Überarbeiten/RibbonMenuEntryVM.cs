using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.SchulDB.Helper;
using Groll.Schule.DataManager;
using Groll.Schule.Model;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Input;


namespace Groll.Schule.SchulDB.ViewModels
{
    public class RibbonMenuEntryVM : RibbonBaseVM
    {        
 
        private string longHeader = "";
        private List<RibbonMenuEntryVM> itemsSource;
        private RibbonMenuEntryVM selectedItem;
        private bool staysOpenOnClick = false;
        /// <summary>
        /// Längerer Text, z.B. für Gallery
        /// </summary>
        public string LongHeader
        {
            get { return longHeader; }
            set { if (longHeader != value) { longHeader = value; OnPropertyChanged(); } }
        }

        
        public List<RibbonMenuEntryVM> ItemsSource
        {
            get { return itemsSource; }
            set { if (itemsSource != value) { itemsSource = value; OnPropertyChanged(); } }
        }

        public RibbonMenuEntryVM SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value; OnPropertyChanged();
                }
            }
        }
        public bool StaysOpenOnClick
        {
            get { return staysOpenOnClick; }
            set
            {
                if (staysOpenOnClick != value)
                {
                    staysOpenOnClick = value; OnPropertyChanged();
                }
            }
        }
    }
}
