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
    // Spezielles RibbonMenuEntryVM, was die Eigenschaften von SelectedItems spiegelt (für Auswahl Buttons)
    public class RibbonMenuSelectedItemEntryVM : RibbonMenuEntryVM
    {

        private RibbonMenuEntryVM selectedItem;

        new public string LongHeader
        {
            get { return (SelectedItem ?? new RibbonMenuEntryVM()).LongHeader; }
        }

        new public string Label
        {
            get { return (SelectedItem ?? new RibbonMenuEntryVM()).Label; }
        }

        new public string ToolTipDescription
        {
            get { return (SelectedItem ?? new RibbonMenuEntryVM()).ToolTipDescription; }
        }

        new public string ToolTipFooterDescription
        {
            get { return (SelectedItem ?? new RibbonMenuEntryVM()).ToolTipFooterDescription; }
        }

        new public string ToolTipFooterTitle
        {
            get { return (SelectedItem ?? new RibbonMenuEntryVM()).ToolTipFooterTitle; }
        }

        new public string ToolTipTitle
        {
            get { return (SelectedItem ?? new RibbonMenuEntryVM()).ToolTipTitle; }
        }

        new public Uri ToolTipFooterImageSource
        {
            get { return (SelectedItem ?? new RibbonMenuEntryVM()).ToolTipFooterImageSource; }
        }

        new public Uri ToolTipImageSource
        {
            get { return (SelectedItem ?? new RibbonMenuEntryVM()).ToolTipImageSource; }
        }

        new public Uri SmallImageSource
        {
            get { return (SelectedItem ?? new RibbonMenuEntryVM()).SmallImageSource; }
        }

        new public Uri LargeImageSource
        {
            get { return (SelectedItem ?? new RibbonMenuEntryVM()).LargeImageSource; }
        }

        new public object Tag
        {
            get { return (SelectedItem ?? new RibbonMenuEntryVM()).Tag; }
        }

        new public RibbonMenuEntryVM SelectedItem
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
