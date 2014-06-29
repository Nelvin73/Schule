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
    // Spezielles RibbonMenuItemVM, was die Eigenschaften von SelectedItems spiegelt (für Auswahl Buttons)
    public class RibbonSelectedItemMenuItemVM : RibbonMenuItemVM
    {

        private RibbonMenuItemVM selectedItem;

        new public string LongHeader
        {
            get { return (SelectedItem ?? new RibbonMenuItemVM()).LongHeader; }
        }

        new public string Label
        {
            get { return (SelectedItem ?? new RibbonMenuItemVM()).Label; }
        }

        new public string ToolTipDescription
        {
            get { return (SelectedItem ?? new RibbonMenuItemVM()).ToolTipDescription; }
        }

        new public string ToolTipFooterDescription
        {
            get { return (SelectedItem ?? new RibbonMenuItemVM()).ToolTipFooterDescription; }
        }

        new public string ToolTipFooterTitle
        {
            get { return (SelectedItem ?? new RibbonMenuItemVM()).ToolTipFooterTitle; }
        }

        new public string ToolTipTitle
        {
            get { return (SelectedItem ?? new RibbonMenuItemVM()).ToolTipTitle; }
        }

        new public Uri ToolTipFooterImageSource
        {
            get { return (SelectedItem ?? new RibbonMenuItemVM()).ToolTipFooterImageSource; }
        }

        new public Uri ToolTipImageSource
        {
            get { return (SelectedItem ?? new RibbonMenuItemVM()).ToolTipImageSource; }
        }

        new public Uri SmallImageSource
        {
            get { return (SelectedItem ?? new RibbonMenuItemVM()).SmallImageSource; }
        }

        new public Uri LargeImageSource
        {
            get { return (SelectedItem ?? new RibbonMenuItemVM()).LargeImageSource; }
        }

        new public object Tag
        {
            get { return (SelectedItem ?? new RibbonMenuItemVM()).Tag; }
        }

        new public RibbonMenuItemVM SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value; OnPropertyChanged();
                    OnPropertyChanged("LongHeader");
                    OnPropertyChanged("Label");
                    OnPropertyChanged("ToolTipDescription");
                    OnPropertyChanged("ToolTipFooterDescription");
                    OnPropertyChanged("ToolTipFooterTitle");
                    OnPropertyChanged("ToolTipTitle");
                    OnPropertyChanged("ToolTipFooterImageSource");
                    OnPropertyChanged("ToolTipImageSource");
                    OnPropertyChanged("SmallImageSource");
                    OnPropertyChanged("LargeImageSource");
                    OnPropertyChanged("Tag");   
                }
            }
        }


    }
}
