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
using Groll.Schule.Model;
using System.Collections.ObjectModel;

namespace Groll.UserControls
{
    /// <summary>
    /// Interaktionslogik für StundenplanGrid.xaml
    /// </summary>
    public partial class StundenplanGrid : UserControl
    {

        private ComboBox auswahlBox;

        #region properties
       

        public Unterrichtsstunde SelectedStunde
        {
            get { return (Unterrichtsstunde)GetValue(SelectedStundeProperty); }
            set { SetValue(SelectedStundeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedStunde.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedStundeProperty =
            DependencyProperty.Register("SelectedStunde", typeof(Unterrichtsstunde), typeof(StundenplanGrid), new PropertyMetadata(null));

        

        public ObservableCollection<Fach> Fächerliste
        {
            get { return (ObservableCollection<Fach>)GetValue(FächerlisteProperty); }
            set { SetValue(FächerlisteProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Fächerliste.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FächerlisteProperty =
            DependencyProperty.Register("Fächerliste", typeof(ObservableCollection<Fach>), typeof(StundenplanGrid), new PropertyMetadata(null));

        


        public ObservableCollection<int> PausenStunden
        {
            get { return (ObservableCollection<int>)GetValue(PausenStundenProperty); }
            set { SetValue(PausenStundenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PausenStunden.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PausenStundenProperty =
            DependencyProperty.Register("PausenStunden", typeof(ObservableCollection<int>), typeof(StundenplanGrid),
            new PropertyMetadata(new ObservableCollection<int>(), new PropertyChangedCallback(UpdateAfterPropertyChanged)));


        public int DisplayedStunden
        {
            get { return (int)GetValue(DisplayedStundenProperty); }
            set { SetValue(DisplayedStundenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayedStunden.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayedStundenProperty =
            DependencyProperty.Register("DisplayedStunden", typeof(int), typeof(StundenplanGrid),
            new PropertyMetadata(8, new PropertyChangedCallback(UpdateAfterPropertyChanged)));

        private static void UpdateAfterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Recreate Display            
            StundenplanGrid g = d as StundenplanGrid;
            g.CreateGrid();
        }



        public bool ShowSaturday
        {
            get { return (bool)GetValue(ShowSaturdayProperty); }
            set { SetValue(ShowSaturdayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowSaturday.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowSaturdayProperty =
            DependencyProperty.Register("ShowSaturday", typeof(bool), typeof(StundenplanGrid),
            new PropertyMetadata(true, new PropertyChangedCallback(UpdateAfterPropertyChanged)));



        public Stundenplan Stundenplan
        {
            get { return (Stundenplan)GetValue(StundenplanProperty); }
            set { SetValue(StundenplanProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Stundenplan.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StundenplanProperty =
            DependencyProperty.Register("Stundenplan", typeof(Stundenplan), typeof(StundenplanGrid),
            new PropertyMetadata(null, new PropertyChangedCallback(UpdateAfterPropertyChanged)));


        #endregion

        public StundenplanGrid()
        {
            InitializeComponent();
            PausenStunden.CollectionChanged += PausenStunden_CollectionChanged;
            CreateGrid();
        }

        void PausenStunden_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CreateGrid();
        }


        private void CreateGrid()
        {
            // Define rows and columns
            gridStundenplan.ColumnDefinitions.Clear();
            gridStundenplan.RowDefinitions.Clear();
            gridStundenplan.Children.Clear();
            auswahlBox = null;
            
            ContentControl c;
            Style pausenStyle = FindResource("PausenRowStyle") as Style;
            Style wdHeaderStyle = FindResource("WochentagHeaderStyle") as Style;
            Style hrHeaderStyle = FindResource("StundenHeaderStyle") as Style;
            Style stundenInfoStyle = FindResource("StundenInfoStyle") as Style;            
            
            #region ColumnHeaders
            gridStundenplan.RowDefinitions.Add(
                  new RowDefinition() { Height = GridLength.Auto });
            
            for (int i = 0; i < (ShowSaturday ? 7 : 6); i++)
            {
                
                gridStundenplan.ColumnDefinitions.Add(
                  new ColumnDefinition()
                  {
                      Width = (i == 0 ? GridLength.Auto : new GridLength(1, GridUnitType.Star)),
                      MinWidth = 50
                  });

                if (i > 0)
                {
                    c = new ContentControl()
                        {
                            Style = wdHeaderStyle,
                            Content = Enum.GetName(typeof(Wochentag), i)
                        };

                    gridStundenplan.Children.Add(c);
                    Grid.SetColumn(c, i);
                    Grid.SetRow(c, 0);
                }
            }

            #endregion

            #region Rows
           
            for (int hour = 1, row = 1; hour < DisplayedStunden + 1; hour++, row++)
            {
                // Stunden-Header
                gridStundenplan.RowDefinitions.Add(
                   new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

                c = new ContentControl()
                {
                    Style = hrHeaderStyle,
                    Content = hour.ToString()
                };
                gridStundenplan.Children.Add(c);
                Grid.SetColumn(c, 0);
                Grid.SetRow(c, row);

                // Stunden
                if (Stundenplan != null)
                    for (int i = 1; i < (ShowSaturday ? 7 : 6); i++)
                    {
                        c = new ContentControl()
                        {
                            Style = stundenInfoStyle,
                            Content = Stundenplan.GetStunde(i, row),
                            Tag = new Stundenbezeichnung() {  Stunde = hour, Tag = (Wochentag) i}
                        };                        
                        c.MouseLeftButtonDown += Stunde_MouseLeftButtonDown;
                        gridStundenplan.Children.Add(c);   
                        Grid.SetColumn(c, i);
                        Grid.SetRow(c, row);
                    } 

                // nach der Stunde eine Pause ?
                if (PausenStunden != null && PausenStunden.Contains(hour))
                {
                    gridStundenplan.RowDefinitions.Add( new RowDefinition() { Height = GridLength.Auto });
                    c = new ContentControl()
                    {
                        Style = pausenStyle,
                        Content = "Pause"
                    };
                    Grid.SetColumnSpan(c, ShowSaturday ? 7 : 6);
                    Grid.SetRow(c, ++row);
                    gridStundenplan.Children.Add(c);                    
                    
                 } 
            }

            #endregion





        }

        private void Stunde_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Dropdown anzeigen
            ContentControl c = (sender as ContentControl);
            if (c == null)
                return;

            SelectedStunde = c.Content as Unterrichtsstunde;
            if (SelectedStunde == null)
            {
                var b =  (Stundenbezeichnung) c.Tag ;
                SelectedStunde = new Unterrichtsstunde()
                    {
                        Stundenplan = Stundenplan,
                        Tag = b.Tag,
                     Stunde = b.Stunde,
                        Fach = null
                    };
                Stundenplan.Stunden.Add(SelectedStunde);
                c.Content = SelectedStunde;
            }
            
            if (auswahlBox == null)
            {
                // Auswahlbox erstellen
                auswahlBox = new ComboBox();
                auswahlBox.Visibility = System.Windows.Visibility.Collapsed;
                auswahlBox.Style = FindResource("FachSelectionCbStyle") as Style;
                auswahlBox.SetBinding(ComboBox.ItemsSourceProperty, "Fächerliste");
                auswahlBox.SetBinding(ComboBox.SelectedItemProperty,
                    new Binding("SelectedStunde.Fach") { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(StundenplanGrid), 1) });               
                
                gridStundenplan.Children.Add(auswahlBox);
            }
          
            // an richtige Position verschieben
            Grid.SetRow(auswahlBox, Grid.GetRow(c));
            Grid.SetColumn(auswahlBox, Grid.GetColumn(c));
            auswahlBox.Visibility = System.Windows.Visibility.Visible;
            auswahlBox.IsDropDownOpen = true;

        }

    }
}
