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
        private bool isSelected = false;
        private bool isVisible = true;
        private string label = null;
        private string largeImageSourceFile = "";
        private string smallImageSourceFile = "";
        private string toolTipTitle = "";
        private string toolTipDescription = "";
        private string toolTipImageSourceFile = "";
        private string toolTipFooterTitle = "";
        private string toolTipFooterDescription = "";
        private string toolTipFooterImageSourceFile = "";
        private ICommand command;
        private object commandParameter;
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
        public Uri LargeImageSource
        {
            get
            {
                return CreateUri(largeImageSourceFile);
            }
        }

        public Uri SmallImageSource
        {
            get
            {
                return CreateUri(smallImageSourceFile);
            }
        }

        public Uri ImageSource  // Link to SmallImageSource
        {
            get
            {
                return CreateUri(smallImageSourceFile);
            }
        }

        public Uri ToolTipFooterImageSource
        {
            get
            {
                return CreateUri(toolTipFooterImageSourceFile);
            }            
        }

        public Uri ToolTipImageSource
        {
            get
            {
                return CreateUri(toolTipImageSourceFile);
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

        public string ToolTipImageSourceFile
        {
            get { return toolTipImageSourceFile; }
            set
            {
                if (toolTipImageSourceFile != value)
                {
                    toolTipImageSourceFile = value;
                    OnPropertyChanged();
                    OnPropertyChanged("ToolTipImageSourceFile");
                }
            }
        }

        public string ToolTipFooterImageSourceFile
        {
            get { return toolTipFooterImageSourceFile; }
            set
            {
                if (toolTipFooterImageSourceFile != value)
                {
                    toolTipFooterImageSourceFile = value;
                    OnPropertyChanged();
                    OnPropertyChanged("ToolTipImageSourceFile");
                }
            }
        }
       
        // Hilfsfunktion
        private Uri CreateUri(string Filename)
        {
            return string.IsNullOrWhiteSpace(Filename) ? null : new Uri("/Images/" + Filename, UriKind.Relative);
        }

        private ImageSource GetImageSourceFromRessourceString(string URI)
        {
            if (string.IsNullOrEmpty(URI))
                return null;
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
    
        public object CommandParameter
        {
            get
            {
                return commandParameter;
            }

            set
            {
                if (commandParameter != value)
                {
                    commandParameter = value;
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
