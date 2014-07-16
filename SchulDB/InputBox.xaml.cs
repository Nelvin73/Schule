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

namespace Groll.Schule.SchulDB
{
    /// <summary>
    /// Interaktionslogik für InputBox.xaml
    /// </summary>
    public partial class InputBox : Window
    {
        public string Header { get; private set; }
        public string Info { get; private set; }
        public string DefaultValue { get; private set; }
        public string ReturnValue { get; private set; }        

        public InputBox()
        {
            InitializeComponent();
            ReturnValue = "";
        }


        public static  bool Show(string Header, string Info, string Title = "Bitte eingeben", string DefaultValue = "")
        {
            InputBox b = new InputBox();
            b.Title = Title;
            b.Header = Header;
            b.Info = Info;
            b.DefaultValue = DefaultValue;

            bool? res = b.ShowDialog();

            return res ?? false;
        
        
        }
    }
}
