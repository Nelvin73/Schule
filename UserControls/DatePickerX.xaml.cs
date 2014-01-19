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
    public partial class DatePickerX : DatePicker
    {

        public static RoutedCommand DatePickerXButton = new RoutedCommand();        

        protected override void OnInitialized(EventArgs e)
        {
            // Datumsformat ändern (damit Wochentag angezeigt wird)
            CultureInfo ci = new CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy (ddd)";
            ci.DateTimeFormat.DateSeparator = ".";
            Thread.CurrentThread.CurrentCulture = ci;
                               
            base.OnInitialized(e);

            this.Style = FindResource("DatePickerXStyle") as Style;


            SelectedDate = DateTime.Now;
            SelectedDateFormat = DatePickerFormat.Short;
            MouseWheel += picker_MouseWheel;  
            
        }

        #region Navigation
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
            if (SelectedDate != null)
                SelectedDate = SelectedDate.Value.AddDays(i);
        }

        public void SetNextDay() { AddDays(1); }

        public void SetPrevDay() { AddDays(-1); }

        public void SetNextWeek() { AddDays(7); }

        public void SetPrevWeek() { AddDays(-7); }

        public void SetNextMonth()
        {           
            if (SelectedDate != null)
                SelectedDate = SelectedDate.Value.AddMonths(1);
        }

        public void SetPrevMonth()
        {
            if (SelectedDate != null)
                SelectedDate = SelectedDate.Value.AddMonths(1);
        }

        #endregion

        #region WatermarkText
        public string WatermarkText
        {
            get { return (string)GetValue(WatermarkTextProperty); }
            set { SetValue(WatermarkTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WatermarkText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WatermarkTextProperty = 
            DependencyProperty.Register("WatermarkText", typeof(string), typeof(DatePickerX), new PropertyMetadata(""));
      
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
        #endregion

        private void userControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Wasserzeichen setzen
            if (WatermarkText != "")
            {
                var datePickerTextBox = GetFirstChildOfType<System.Windows.Controls.Primitives.DatePickerTextBox>(this);
                if (datePickerTextBox != null)
                {
                    var partWatermark = datePickerTextBox.Template.FindName("PART_Watermark", datePickerTextBox) as ContentControl;
                    if (partWatermark != null)
                        partWatermark.Content = WatermarkText;
                }
            }                                        
        }        

        // Command
        private void DatePickerXButton_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            switch (e.Parameter.ToString())
            {
                case "1":
                    // PrevWeek
                    SetPrevWeek();
                    break;

                case "2":
                    // PrevDay
                    SetPrevDay();
                    break;
                
                case "3":
                    // Clear
                    SelectedDate = null;
                    break;
              
                case "4":
                    // NextDay
                    SetNextDay();
                    break;

                case "5":
                    // NextWeek
                    SetNextWeek();
                    break;
                
            }
        }

        #region ShowNavigationButtons
        public bool ShowNavigationButtons
        {
            get { return (bool)GetValue(ShowNavigationButtonsProperty); }
            set { SetValue(ShowNavigationButtonsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowNavigationButtons.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowNavigationButtonsProperty =
            DependencyProperty.Register("ShowNavigationButtons", typeof(bool), typeof(DatePickerX), new PropertyMetadata(true) );

        #endregion

 
        public DatePickerX()
        {
            InitializeComponent();
        }
   
    }
}
