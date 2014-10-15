using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Groll.UserControls
{
    /// <summary>
    /// Interaktionslogik für WerteVerteilungControl.xaml
    /// </summary>
    public partial class WerteVerteilungControl : UserControl
    {
        private Dictionary<string, string> _Data2;
        public Dictionary<string, string> Data2
        {
            get
            {
                return _Data2;
            }
            set {

                _Data2 = value;
                UpdateGrid(this, new DependencyPropertyChangedEventArgs());
            }
        }
        public Dictionary<string, string> Data
        {
            get { return (Dictionary<string, string>)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(Dictionary<string, string>), typeof(WerteVerteilungControl),
            new PropertyMetadata(new Dictionary<string, string>(), UpdateGrid,  XXXXX));

        private static object XXXXX(DependencyObject d, object baseValue)
        {
            int i = 1;
            return baseValue;
        }

        private static void UpdateGrid(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
 	        // Werte wurden geändert .. neu aufbauen
            var c = d as WerteVerteilungControl;
            if (c == null)
                return;

            c.CreateGrid();
            

        


        }

       

        

        
        
        public WerteVerteilungControl()
        {
            InitializeComponent();
            Data2 = new Dictionary<string, string>();
            CreateGrid();
        }

        private void CreateGrid()
        {
            UniformGrid grid = myGRID;
            grid.Children.Clear();
            grid.Columns = Data.Count + 2;
            grid.Rows = 2;


            Border b = new Border()
            {
                Padding = new Thickness(10),
                BorderBrush = this.Foreground,
                BorderThickness = new Thickness(2, 0, 2, 2)
            };

            Grid.SetColumn(b, 0);
            Grid.SetRow(b, 0);
            grid.Children.Add(b);

            b = new Border()
            {
                Padding = new Thickness(10),
                BorderBrush = this.Foreground,
                BorderThickness = new Thickness(2, 0, 2, 2)
            };

            Grid.SetColumn(b, grid.Columns - 1);
            Grid.SetRow(b, 0);
            grid.Children.Add(b);

          


            
            int i = 1;
            foreach (var d in Data)
            {
                // Header
                b = new Border()
                {
                    Padding = new Thickness(10),
                    BorderBrush = this.Foreground,
                    BorderThickness = new Thickness(2,0, (i > 0 && i < Data.Count -1 ? 2 : 0), 2)
                };
                

                TextBlock t = new TextBlock()
                {
                    Text = d.Key
                };
                
                Grid.SetColumn(b, i);
                Grid.SetRow(b, 0);
                b.Child = t;
                grid.Children.Add(b);

                // Data
                b = new Border()
                {
                    Padding = new Thickness(10),
                    BorderBrush = this.Foreground,
                    BorderThickness = new Thickness(2, 0, (i > 0 && i < Data.Count - 1 ? 2 : 0), 0)
                };

                t = new TextBlock()
                {
                    Text = d.Value
                };

                Grid.SetColumn(b, i);
                Grid.SetRow(b, 1);
                b.Child = t;
                grid.Children.Add(b);


            }
            

        }



    }
}
