using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Groll.UserControls
{
    /// <summary>
    /// Interaktionslogik für DatePickerEx.xaml
    /// </summary>
    public partial class DatePickerEx : UserControl
    {

        // wichtigste Standardeigenschaften an DatePicker weitergeben
        

public DateTime? SelectedDate
{
    get { return (DateTime?)GetValue(SelectedDateProperty); }
    set { SetValue(SelectedDateProperty, value); }
}

// Using a DependencyProperty as the backing store for SelectedDate.  This enables animation, styling, binding, etc...
public static readonly DependencyProperty SelectedDateProperty = 
    DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(DatePickerEx), new PropertyMetadata(DateTime.Now));




        #region
        public string WatermarkText
        {
            get { return (string)GetValue(WatermarkTextProperty); }
            set { SetValue(WatermarkTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WatermarkText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WatermarkTextProperty = 
            DependencyProperty.Register("WatermarkText", typeof(string), typeof(DatePickerEx), new PropertyMetadata(""));        
        #endregion


        private void picker_Loaded(object sender, RoutedEventArgs e)
        {
            // Watermark
            if (WatermarkText == "")
                return;

            var datePickerTextBox = GetFirstChildOfType<System.Windows.Controls.Primitives.DatePickerTextBox>(picker);
            if (datePickerTextBox == null)
                return;
           
            var partWatermark = datePickerTextBox.Template.FindName("PART_Watermark", datePickerTextBox) as ContentControl;
            if (partWatermark == null)
                return;

            partWatermark.Content = WatermarkText;  
            
            var clearButton = picker.Template.FindName("PART_ClearButton", picker) as Button ;
            if (clearButton == null)
                return;

            clearButton.Click += clearButton_Click;
                 
        }

        void clearButton_Click(object sender, RoutedEventArgs e)
        {
            picker.SelectedDate = null;
        }

        

        #region ShowNavigationButtons
        public bool ShowNavigationButtons1
        {
            get { return (bool)GetValue(ShowNavigationButtons1Property); }
            set { SetValue(ShowNavigationButtons1Property, value); }
        }

        // Using a DependencyProperty as the backing store for ShowNavigationButtons.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowNavigationButtons1Property =
            DependencyProperty.Register("ShowNavigationButtons1", typeof(bool), typeof(DatePickerEx), new PropertyMetadata(true) );

        #endregion


        public DatePickerEx()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            // Datumsformat ändern (damit Wochentag angezeigt wird)
            CultureInfo ci = new CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy (ddd)";
            ci.DateTimeFormat.DateSeparator = ".";
            Thread.CurrentThread.CurrentCulture = ci;

          

           
 /*
        private static void OnWatermarkChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var datePicker = dependencyObject as DatePicker;
            if (datePicker == null)
                return;

            if ((e.NewValue != null) && (e.OldValue == null))
                datePicker.Loaded += DatePickerLoaded;
            else if ((e.NewValue == null) && (e.OldValue != null))
                datePicker.Loaded -= DatePickerLoaded;
        }
 
        private static void DatePickerLoaded(object sender, RoutedEventArgs e)
        {*/
            
          
            base.OnInitialized(e);

            picker.SelectedDate = DateTime.Now;
            picker.SelectedDateFormat = DatePickerFormat.Short;
            picker.MouseWheel += picker_MouseWheel;  
        }


          private static T GetFirstChildOfType<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject == null)
                return null;

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
            {
                var child = VisualTreeHelper.GetChild(dependencyObject, i);
                var result = (child as T) ?? GetFirstChildOfType<T>(child);
                if (result != null)
                    return result;
            }            
            return null;
        }
    

        void picker_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Mausrad             
            if (e.Delta == 0)
                return;

            int direction = (e.Delta > 0 ? 1 : -1);
            
            // Mit SHIFT = 1 Monat
            if (Keyboard.IsKeyDown(Key.LeftShift))
                if (direction > 0)
                    SetNextMonth();
                else
                    SetPrevMonth();

            // Mit CTRL = 1 Woche  
            else if (Keyboard.IsKeyDown(Key.LeftCtrl))
                AddDays(7 * direction);

            else
                AddDays(direction);                    
        }

        private void AddDays(int i)
        {
            DateTime? s = picker.SelectedDate;
            if (s != null)
                picker.SelectedDate = s.Value.AddDays(i);
        }

        public void SetNextDay() { AddDays(1); }

        public void SetPrevDay() { AddDays(-1); }

        public void SetNextWeek() { AddDays(7); }

        public void SetPrevWeek() { AddDays(-7); }

        public void SetNextMonth() {
            DateTime? s = picker.SelectedDate;
            if (s != null)
                picker.SelectedDate = s.Value.AddMonths(1);
        }

        public void SetPrevMonth()
        {
            DateTime? s = picker.SelectedDate;
            if (s != null)
                picker.SelectedDate = s.Value.AddMonths(1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == btNextDay)
                AddDays(1);
            else if (sender == btNextWeek)
                AddDays(7);
            else if (sender == btPrevDay)
                AddDays(-1);
            else
                AddDays(-7);
        }
    }
}
