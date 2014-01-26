﻿using Groll.Schule.Model;
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
using Groll.Schule.SchulDB.Commands;

namespace Groll.Schule.SchulDB.Pages
{
    /// <summary>
    /// Interaktionslogik für UserDetailsPage.xaml
    /// </summary>

    public partial class BeobachtungenEingabePage : Page, ISchulDBPage
    {

        #region ViewModel
        private Groll.Schule.SchulDB.Pages.ViewModels.BeobachtungenEingabeVM viewModel;

        public Groll.Schule.SchulDB.Pages.ViewModels.BeobachtungenEingabeVM ViewModel
        {
            get
            {
                if (viewModel == null)
                {
                    viewModel = this.FindResource("ViewModel") as Groll.Schule.SchulDB.Pages.ViewModels.BeobachtungenEingabeVM;
                    if (viewModel == null)
                        throw new ResourceReferenceKeyNotFoundException();
                }
                return viewModel;
            }
        }
        #endregion

        public BeobachtungenEingabePage()
        {
            InitializeComponent();

            // Command Bindings
            this.CommandBindings.AddRange(new List<CommandBinding>
                {
                    new CommandBinding(BeobachtungenCommands.ClearInput, Executed_ClearInput, BasicCommands.CanExecute_TRUE),
                    
                });
        }



        #region ICommand implementierungen
        private void Executed_ClearInput(object sender, ExecutedRoutedEventArgs e)
        {
            // Eingabe im Model löschen
            ViewModel.ClearInput();

            // Anzeigefilter zurücksetzen
            Filter.Text = "";

            // Falls Textfeld Fokus hat, auch den angezeigten Text löschen
            if (txtBeoText.IsFocused)
                txtBeoText.Text = "";
        }

        #endregion


        public void SetMainWindow(MainWindow x)
        {
            // Save MainWindow handle
            this.Tag = x;
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ViewModel.AddCurrentComment();
        }

        // Liste neuladen beim Ändern des Filters
        private void Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            var s = TryFindResource("SchülerListeViewSource") as CollectionViewSource;
            s.View.Refresh();
        }

        // Filter
        void s_Filter(object sender, FilterEventArgs e)
        {
            var i = e.Item as Schueler;
            e.Accepted = i != null && (Filter.Text == "" || i.DisplayName.ToLower().Contains(Filter.Text.ToLower()));
        }


        #region Navigation / Initialization
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            // Navigated away from Page
            // Hide Context Tab
            (Tag as MainWindow).RibbonVM.ShowBeobachtungenTab = false;
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            // Page is first time initialized
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Navigated toward Page
            var rvm = (Tag as MainWindow).RibbonVM;
            rvm.ShowBeobachtungenTab = true;
            rvm.BeobachtungenIsSelected = true;
            txtBeoText.Focus();
        }
        #endregion

    }
}
