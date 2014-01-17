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

namespace Groll.UserControls
{
    /// <summary>
    /// Interaktionslogik für DatePickerEx.xaml
    /// </summary>
    public partial class DatePickerEx : UserControl
    {        
        
        public DatePickerEx()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            picker.SelectedDate = DateTime.Now;
            
            picker.MouseWheel += picker_MouseWheel;
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
