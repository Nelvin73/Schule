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
        private Border GetBorder(string border)
        {
            border += "0000";
            Thickness t = new Thickness();
            t.Left = border[0] - '0';
            t.Top = border[1] - '0';
            t.Right = border[2] - '0';
            t.Bottom = border[3] - '0';

            return new Border()
            {
                Padding = new Thickness(10),
                BorderBrush = this.Foreground,
                BorderThickness = t,                
            };
        }

        private void CreateGrid()
        {
            Grid grid = myGRID;
            grid.Children.Clear();

            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Add(new RowDefinition() { });
            grid.RowDefinitions.Add(new RowDefinition() { });

            for (int i = 0; i < Data.Count + 2; i++)            
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star)});            

            // Linker und rechter Überstand
            Border b = GetBorder("0022");            
            Grid.SetColumn(b, 0);
            Grid.SetRow(b, 0);
            grid.Children.Add(b);

            b = GetBorder("2002");                        
            Grid.SetColumn(b, Data.Count + 1);
            Grid.SetRow(b, 0);
            grid.Children.Add(b);

            b = GetBorder("0220");
            Grid.SetColumn(b, 0);
            Grid.SetRow(b, 1);
            grid.Children.Add(b);

            b = GetBorder("2200");
            Grid.SetColumn(b, Data.Count + 1);
            Grid.SetRow(b, 1);
            grid.Children.Add(b);
            
            int j = 1;
            foreach (var d in Data)
            {
                // Header
                b = GetBorder("2022");                
                b.Child = new TextBlock() { Text = d.Key, HorizontalAlignment = System.Windows.HorizontalAlignment.Center, VerticalAlignment = System.Windows.VerticalAlignment.Center };                
                Grid.SetColumn(b, j);
                Grid.SetRow(b, 0);
                grid.Children.Add(b);

                // Data
                b = GetBorder("2020");
                b.Child = new TextBlock() { Text = d.Value, HorizontalAlignment = System.Windows.HorizontalAlignment.Center, VerticalAlignment = System.Windows.VerticalAlignment.Center };                                             
                Grid.SetColumn(b, j);
                Grid.SetRow(b, j);
                grid.Children.Add(b);
                j++;

            }
            

        }



    }
}
