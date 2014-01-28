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
    public partial class SchuelerDetailsPage : Page, ISchulDBPage
    {
        public SchuelerDetailsPage()
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
            CollectionViewSource schuelerSource = this.FindResource("schuelerViewSource") as CollectionViewSource;
            var UOW = (this.FindResource("UnitOfWork")) as Groll.Schule.DataManager.UowSchuleDB;
            schuelerSource.Source = UOW.Schueler.GetObservableCollection();
        }

        public void OnDatabaseChanged()
        {
            throw new NotImplementedException();
        }
    }
}
