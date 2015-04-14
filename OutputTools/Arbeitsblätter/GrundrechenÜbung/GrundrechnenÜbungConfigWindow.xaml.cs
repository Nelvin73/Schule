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
using System.Windows.Shapes;

namespace Groll.Schule.OutputTools.Arbeitsblätter
{
    /// <summary>
    /// Interaktionslogik für GrundrechnenÜbungConfigWindow.xaml
    /// </summary>
    public partial class GrundrechnenÜbungConfigWindow : Window
    {
        public GrundrechnenÜbungConfig Config { get; set; }

        public GrundrechnenÜbungConfigWindow()
        {
            InitializeComponent();
        }
        
        public GrundrechnenÜbungConfigWindow(GrundrechnenÜbungConfig Config)
        {
            this.Config = Config;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Open file dialog to select new template
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Arbeitsblatt-Infos (*.arb)|*.arb";
            dlg.InitialDirectory = System.IO.Path.Combine(Environment.CurrentDirectory, "Arbeitsblätter\\GrundrechenÜbung");
            if (dlg.ShowDialog(this) == true)
            {
                string path = dlg.FileName;
                if (path.StartsWith(Environment.CurrentDirectory + "\\"))
                    path = path.Replace(Environment.CurrentDirectory + "\\", "");
                FilePathTextBox.Text = Config.FilePath = path;
            }
        }
    }
}
