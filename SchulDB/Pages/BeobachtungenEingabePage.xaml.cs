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
    public partial class BeobachtungenEingabePage : Page, ISchulDBPage
    {
        public BeobachtungenEingabePage()
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

        

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var j = this.FindResource("ViewModel") as Groll.Schule.SchulDB.Pages.ViewModels.BeobachtungenEingabeVM;
            j.BeoDatum = null;
            System.Diagnostics.Debug.WriteLine(j.Fächerliste.Count);
        }
    }
}
