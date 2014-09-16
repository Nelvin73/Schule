using System.Windows.Controls.Ribbon;
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
using Groll.Schule.SchulDB.Pages;
using Groll.Schule.SchulDB.Commands;
using Groll.Schule.SchulDB.ViewModels;

namespace Groll.Schule.SchulDB
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        #region ViewModel

       
        private MainWindowVM mainViewModel;
        public MainWindowVM MainViewModel
        {
            get
            {
                if (mainViewModel == null)
                    mainViewModel = this.FindResource("MainViewModel") as MainWindowVM;
                return mainViewModel;
            }
        }
        
       
        #endregion

        public MainWindow()
        {
            try
            {
                InitializeComponent();         
            }
            catch (Exception e)
            {
                
                throw;
            }
                        
        }

       


    }
}
