﻿using System;
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
    public partial class KlassenDetailsPage : Page, ISchulDBPage
    {
        public KlassenDetailsPage()
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
            System.Windows.Data.CollectionViewSource schuelerSource =
                ((System.Windows.Data.CollectionViewSource)(this.FindResource("klasseViewSource")));

            var UOW = (this.FindResource("UnitOfWork")) as Groll.Schule.DataManager.UowSchuleDB;            
            schuelerSource.Source = UOW.Klassen.GetObservableCollection();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var UOW = (this.FindResource("UnitOfWork")) as Groll.Schule.DataManager.UowSchuleDB;
            // UOW.Klassen.Create().Name = "Test";
            int i = 0;
        }
    }
}
