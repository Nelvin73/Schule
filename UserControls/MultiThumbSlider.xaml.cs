using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaktionslogik für MultiThumbSlider.xaml
    /// </summary>
    public partial class MultiThumbSlider : UserControl
    {
        private static bool ReInitializing = false;

        public MultiThumbSlider()
        {
            InitializeComponent();
            Values = new ObservableCollection<double>();
            RecreateSliders();
        }


        #region Values             
        public ObservableCollection<double> Values
        {
            get { return (ObservableCollection<double>)GetValue(ValuesProperty); }
            set { SetValue(ValuesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Values.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValuesProperty =
            DependencyProperty.Register("Values", typeof(ObservableCollection<double>), typeof(MultiThumbSlider), new PropertyMetadata(new ObservableCollection<double>(), new PropertyChangedCallback(ValuesChanged), new CoerceValueCallback(ValidateValues)));

        private static object ValidateValues(DependencyObject d, object baseValue)
        {
 	        var j = d as MultiThumbSlider;
            var l = baseValue as ObservableCollection<double>;

            if (j == null || l == null)
                return baseValue;

            // Add values until count of thumbs
            for (int i = l.Count; i < j.ThumbCount; i++)
                l.Add(0);

            // return sorted list 
            return new ObservableCollection<double>(l.OrderBy( x => x).Take(j.ThumbCount));

        }

        private static void ValuesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var j = d as MultiThumbSlider;
            if (j == null || ReInitializing)
                return;

            ReInitializing = true;
            var sliders = j.SliderContainer.Children.OfType<Slider>().ToList();

            for (int i = 0; i < sliders.Count && i <j.Values.Count; i++ )                
            {
                sliders[i].Value = j.Values[i];                
            }

            ReInitializing = false;
        }

        


        public bool ShowValue
        {
            get { return (bool)GetValue(ShowValueProperty); }
            set { SetValue(ShowValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowValueProperty =
            DependencyProperty.Register("ShowValue", typeof(bool), typeof(MultiThumbSlider), new PropertyMetadata(true));

        

        #endregion

        #region Ticks behavior

        public double TickFrequency
        {
            get { return (double)GetValue(TicksFrequencyProperty); }
            set { SetValue(TicksFrequencyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TicksFrequency.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TicksFrequencyProperty =
            DependencyProperty.Register("TickFrequency", typeof(double), typeof(MultiThumbSlider), new PropertyMetadata(10D, new PropertyChangedCallback(OnTickChanged)));

       
        public bool IsTickVisible
        {
            get { return (bool)GetValue(IsTicksVisibleProperty); }
            set { SetValue(IsTicksVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsTicksVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsTicksVisibleProperty =
            DependencyProperty.Register("IsTickVisible", typeof(bool), typeof(MultiThumbSlider), new PropertyMetadata(true, new PropertyChangedCallback(OnTickChanged)));


        public bool IsSnapToTickEnabled
        {
            get { return (bool)GetValue(IsSnapToTickEnabledProperty); }
            set { SetValue(IsSnapToTickEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSnapToTicksEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSnapToTickEnabledProperty =
            DependencyProperty.Register("IsSnapToTickEnabled", typeof(bool), typeof(MultiThumbSlider), new PropertyMetadata(true, new PropertyChangedCallback(OnTickChanged)));
        
 
        private static void OnTickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Ticks changed
            var j = d as MultiThumbSlider;
            if (j == null)
                return;

            foreach (var sl in j.SliderContainer.Children.OfType<Slider>())
            {
                sl.TickFrequency = j.TickFrequency;
                sl.IsSnapToTickEnabled = j.IsSnapToTickEnabled;
                sl.TickPlacement = j.IsTickVisible ? TickPlacement.BottomRight : TickPlacement.None;
            }
        }
        #endregion


        #region MinValue / MaxValue

        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(MultiThumbSlider), new PropertyMetadata(0D, new PropertyChangedCallback(OnMinMaxChanged)));

             
        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(MultiThumbSlider), new PropertyMetadata(100D, new PropertyChangedCallback(OnMinMaxChanged)));


        private static void OnMinMaxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var x = d as MultiThumbSlider;
            x.RecreateSliders();
            x.InvalidateVisual();
        }

        #endregion

        #region ThumbCount
        public int ThumbCount
        {
            get { return (int)GetValue(ThumbCountProperty); }
            set { SetValue(ThumbCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ThumbCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbCountProperty =
            DependencyProperty.Register("ThumbCount", typeof(int), typeof(MultiThumbSlider),
            new PropertyMetadata(2, new PropertyChangedCallback(OnThumbCountChanged), new CoerceValueCallback(OnCoerceThumbCount)));

        private static object OnCoerceThumbCount(DependencyObject d, object baseValue)
        {
            if (!(baseValue is int))
                return 2;

            int i = (int)baseValue;
           
            if (i > 5)
                return 5;
            if (i < 1)
                return 1;
            else return i;           
        }

        private static void OnThumbCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Anzahl Thumbs geändert => Steuerelement neu aufbauen
            if (e.NewValue != e.OldValue)
            {
                var x = d as MultiThumbSlider;                
                x.RecreateSliders();
                x.InvalidateVisual();
            }
        }
        #endregion

        void RecreateSliders()
        {
            ReInitializing = true;

            SliderContainer.Children.Clear();
            if (Values == null)
                Values = new ObservableCollection<double>();

            for (int i = 0; i < ThumbCount; i++)
            {
                var s = new Slider()
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Center,
                    Value = MinValue + ((double) MaxValue - MinValue) / (ThumbCount + 1D) * (i + 1D),                    
                    Template = (ControlTemplate) FindResource("SingleSlider"),
                    Minimum = MinValue,
                    Maximum = MaxValue,
                    Tag = i,
                    TickFrequency = this.TickFrequency,
                    IsSnapToTickEnabled = this.IsSnapToTickEnabled,
                    TickPlacement = IsTickVisible ? TickPlacement.BottomRight : TickPlacement.None
                };

                s.ValueChanged += s_ValueChanged;
                s.Tag = SliderContainer.Children.Add(s);

                if (i >= Values.Count)
                    Values.Add(s.Value);
                else
                    s.Value = Values[i];
            }

            ReInitializing = false;
        }

        void s_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Der Wert eines Sliders wurde geändert
            var s = e.Source as Slider;
            if (s == null || ReInitializing)
                return;
            
            int index = (int)s.Tag;
            // Umweg nötig, damit CollectionChanged ausgelöst wird
            ReInitializing = true;
            Values[index] = (double) e.NewValue;
            Values = new ObservableCollection<double>(Values);
            ReInitializing = false;

            if (e.NewValue > e.OldValue)
            {
                // Check ob Slider rechts existiert
                if (SliderContainer.Children.Count > (int) s.Tag + 1)
                {
                    var sp = SliderContainer.Children[(int)s.Tag + 1] as Slider;
                    if (sp != null && e.NewValue >= sp.Value)
                        // Slider nach rechts mitschieben
                        sp.Value = e.NewValue;
                }
            }

            else if (e.NewValue < e.OldValue)
            {
                // Check ob Slider links existiert
                if ((int) s.Tag > 0)
                {
                    var sp = SliderContainer.Children[(int)s.Tag - 1] as Slider;
                    if (sp != null && e.NewValue <= sp.Value)
                        // Slider nach rechts mitschieben
                        sp.Value = e.NewValue;

                }
            }         

            
        }
       


        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Breite geändert
            if (!e.WidthChanged)
                return;

            foreach (var c in SliderContainer.Children)
            {
                var s = c as Slider;
                if (s != null)
                    s.Width = e.NewSize.Width;

            }

        }



    }
}
