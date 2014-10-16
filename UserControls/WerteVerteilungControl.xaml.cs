﻿using System;
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
       

        private Border AddBorder(int Left, int Top, int Right, int Bottom, int row, int column)
        {
            Thickness t = new Thickness(Left, Top, Right, Bottom);
            Border b = new Border()
            {
                Padding = new Thickness(10),
                BorderBrush = this.Foreground,
                BorderThickness = t                
            };
             
            Grid.SetColumn(b, column);
            Grid.SetRow(b, row);
            myGRID.Children.Add(b);
            return b;            
        }

        private void CreateGrid()
        {
            Border b;
            Grid grid = myGRID;
            grid.Children.Clear();

            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Add(new RowDefinition() { });
            grid.RowDefinitions.Add(new RowDefinition() { });
             
            int j = 0;
            foreach (var d in Data)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star)});            
            
                // Header
                AddBorder(j == 0 ? 2 : 0, 0, 2, 2, 0, j).Child = new TextBlock() 
                    { Text = d.Key, HorizontalAlignment = System.Windows.HorizontalAlignment.Center, VerticalAlignment = System.Windows.VerticalAlignment.Center };                                
                
                // Data
                AddBorder(j == 0 ? 2 : 0, 0, 2, 0, 1, j).Child = new TextBlock() 
                    { Text = d.Value, HorizontalAlignment = System.Windows.HorizontalAlignment.Center, VerticalAlignment = System.Windows.VerticalAlignment.Center };                                             
                j++;

            }
            

        }
      

    }
}