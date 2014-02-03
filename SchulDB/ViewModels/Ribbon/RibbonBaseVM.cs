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
    public class RibbonBaseVM : ObservableObject
    {
        #region Private Fields
        private RibbonVM ribbonVM;                
        private bool isSelected = false;
        private bool isVisible = true;
        private string label = "";
        private string largeImageSourceFile = "";
        private string smallImageSourceFile = "";
        private string toolTipTitle = "";
        private string toolTipDescription = "";
        private Uri toolTipImage;
        private string toolTipFooterTitle = "";
        private string toolTipFooterDescription = "";
        private Uri toolTipFooterImage;
        private ICommand command;
        private string keyTip = "";
        private object tag;
        #endregion

        #region Selection, Visibility, Label
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value; OnPropertyChanged();
                }
            }
        }
        
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                if (isVisible != value)
                { isVisible = value; OnPropertyChanged(); }
            }
        }

        public string Label
        {
            get { return label; }
            set
            {
                if (label != value)
                { label = value; OnPropertyChanged(); }
            }
        }

        #endregion

        #region Images-Sources
        // Properties for Binding
        public ImageSource LargeImageSource
        {
            get
            {
                return GetImageSourceFromRessourceString(largeImageSourceFile);
            }
        }

        public ImageSource SmallImageSource
        {
            get { return GetImageSourceFromRessourceString(smallImageSourceFile); }
        }

        public ImageSource ImageSource  // Link to SmallImageSource
        {
            get
            {
                return GetImageSourceFromRessourceString(smallImageSourceFile);
            }
        }

        // Properties for Configuration
        public string LargeImageSourceFile
        {
            get { return largeImageSourceFile; }
            set
            {
                if (largeImageSourceFile != value)
                {
                    largeImageSourceFile = value;
                    OnPropertyChanged();
                    OnPropertyChanged("LargeImageSource");
                }
            }
        }

        public string SmallImageSourceFile
        {
            get { return smallImageSourceFile; }
            set
            {
                if (smallImageSourceFile != value)
                {
                    smallImageSourceFile = value;
                    OnPropertyChanged();
                    OnPropertyChanged("SmallImageSource");
                    OnPropertyChanged("ImageSource");
                }
            }
        }

        // Hilfsfunktion
        private ImageSource GetImageSourceFromRessourceString(string URI)
        {
            return new System.Windows.Media.Imaging.BitmapImage(new Uri( URI, UriKind.Relative));
        }

        #endregion

        #region ToolTips
        public string ToolTipTitle
        {
            get
            {
                return toolTipTitle;
            }

            set
            {
                if (toolTipTitle != value)
                {
                    toolTipTitle = value;
                    OnPropertyChanged();
                }
            }
        }        

        public string ToolTipDescription
        {
            get
            {
                return toolTipDescription;
            }

            set
            {
                if (toolTipDescription != value)
                {
                    toolTipDescription = value;
                    OnPropertyChanged();
                }
            }
        }

        public Uri ToolTipImage
        {
            get
            {
                return toolTipImage;
            }

            set
            {
                if (toolTipImage != value)
                {
                    toolTipImage = value;
                    OnPropertyChanged();
                }
            }
        }
        

        public string ToolTipFooterTitle
        {
            get
            {
                return toolTipFooterTitle;
            }

            set
            {
                if (toolTipFooterTitle != value)
                {
                    toolTipFooterTitle = value;
                    OnPropertyChanged();
                }
            }
        }
       
        public string ToolTipFooterDescription
        {
            get
            {
                return toolTipFooterDescription;
            }

            set
            {
                if (toolTipFooterDescription != value)
                {
                    toolTipFooterDescription = value;
                    OnPropertyChanged();
                }
            }
        }
      
        public Uri ToolTipFooterImage
        {
            get
            {
                return toolTipFooterImage;
            }

            set
            {
                if (toolTipFooterImage != value)
                {
                    toolTipFooterImage = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        public ICommand Command
        {
            get
            {
                return command;
            }

            set
            {
                if (command != value)
                {
                    command = value;
                    OnPropertyChanged();
                }
            }
        }       

        public string KeyTip
        {
            get
            {
                return keyTip;
            }

            set
            {
                if (keyTip != value)
                {
                    keyTip = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public object Tag
        {
            get { return tag; }
            set { tag = value; OnPropertyChanged(); }
        }
    }
}
