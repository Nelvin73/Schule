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
    public class RibbonButtonVM : ObservableObject
    {        

 // TODO Headertext (kurz) für Button und mehrzeileige Erklärung für  Galerie 

        private bool isSelected = false;
        private bool isVisible = true;
        private string header = "";
        private string longHeader = "";
        private string largeImageSourceFile = null;
        private string smallImageSourceFile = null;
        private object tag;

        public object Tag
        {
            get { return tag; }
            set { tag = value; OnPropertyChanged();}
        }

        private ICommand command;

        public ICommand Command
        {
            get { return command; }
            set { command = value; }
        }
        

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; OnPropertyChanged(); }
        }
        
        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; OnPropertyChanged(); }
        }

        public string Header
        {
            get { return header; }
            set { header = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Längerer Text, z.B. für Gallery
        /// </summary>
        public string LongHeader
        {
            get { return longHeader; }
            set { longHeader = value; OnPropertyChanged(); }
        }

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

        public string LargeImageSourceFile
        {
            get { return largeImageSourceFile; }
            set { largeImageSourceFile = value; 
                OnPropertyChanged();
                OnPropertyChanged("LargeImageSource"); 
            }
        }

        public string SmallImageSourceFile
        {
            get { return smallImageSourceFile; }
            set { smallImageSourceFile = value; 
                OnPropertyChanged();
                OnPropertyChanged("SmallImageSource");
            }
        }

        private List<RibbonButtonVM> itemsSource;

        public List<RibbonButtonVM> ItemsSource
        {
            get { return itemsSource; }
            set { itemsSource = value; }
        }
        

        private ImageSource GetImageSourceFromRessourceString(string URI)
        {                                   
            return new System.Windows.Media.Imaging.BitmapImage(new Uri("Images/" + URI, UriKind.Relative));            
        }
    }
}
