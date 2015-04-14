using Groll.Schule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Groll.Schule.SchulDB.Pages
{
    /// <summary>
    /// Interaktionslogik für UserDetailsPage.xaml
    /// </summary>
    public partial class VorlagenPage : Page, ISchulDBPage
    {      

        #region ViewModel
        private Groll.Schule.SchulDB.ViewModels.VorlagenVM viewModel;

        public Groll.Schule.SchulDB.ViewModels.VorlagenVM ViewModel
        {
            get
            {
                if (viewModel == null)
                {
                    viewModel = this.FindResource("ViewModel") as Groll.Schule.SchulDB.ViewModels.VorlagenVM;
                    if (viewModel == null)
                        throw new ResourceReferenceKeyNotFoundException();
                }
                return viewModel;
            }
        }
        #endregion

        public VorlagenPage()
        {
            InitializeComponent();
        }

        public void SetMainWindow(MainWindow x)
        {
            // Save MainWindow handle
            this.Tag = x;            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {            
        }

        public void OnDatabaseChanged()
        {
           
        }

     
     
    }
}
