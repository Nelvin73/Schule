using Groll.Schule.Model;
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

namespace Groll.Schule.SchulDB.Pages
{
    /// <summary>
    /// Interaktionslogik für UserDetailsPage.xaml
    /// </summary>
    public partial class KlassenEditPage : Page, ISchulDBPage
    {
        private ListBox dragSource = null;
        private Point dragStartPoint = new Point();

        #region ViewModel
        private Groll.Schule.SchulDB.ViewModels.KlassenEditVM viewModel;

        public Groll.Schule.SchulDB.ViewModels.KlassenEditVM ViewModel
        {
            get
            {
                if (viewModel == null)
                {
                    viewModel = this.FindResource("ViewModel") as Groll.Schule.SchulDB.ViewModels.KlassenEditVM;
                    if (viewModel == null)
                        throw new ResourceReferenceKeyNotFoundException();
                }
                return viewModel;
            }
        }
        #endregion

        public KlassenEditPage()
        {
            InitializeComponent();
        }

        public void SetMainWindow(MainWindow x)
        {
            // Save MainWindow handle
            this.Tag = x;            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {            
        }

        public void OnDatabaseChanged()
        {
            throw new NotImplementedException();
        }

        private void ListBox_MouseMove(object sender, MouseEventArgs e)
        {
            // Remember which Listbox is initiator
            dragSource = (ListBox) sender;
            if (dragSource == null)
                    return;

            // Check if Drag is started            
            Vector v = e.GetPosition(dragSource) - dragStartPoint;            
            if (e.LeftButton == MouseButtonState.Pressed && (Math.Abs(v.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(v.Y) > SystemParameters.MinimumVerticalDragDistance ))
            {                                                
                Schueler s = GetSchuelerFromListBox(dragSource, e.GetPosition(dragSource));
                if (s != null)
                {
                    DragDrop.DoDragDrop(dragSource, new DataObject("Schüler",  s), DragDropEffects.Move | DragDropEffects.None);
                }
            }

        }

        // Check if Drag & Drop is initiated
        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Remember DragStartPosition
            dragStartPoint = e.GetPosition((ListBox)sender);
        }

        private Schueler GetSchuelerFromListBox(ListBox source, Point point)
        {            
            object data = DependencyProperty.UnsetValue;
                        
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                while (data == DependencyProperty.UnsetValue && element != source)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);                    
                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = VisualTreeHelper.GetParent(element) as UIElement;
                    }
                
                }
            
            }

            return data as Schueler;
        }
       

        // Drag & Drop wird versucht
        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            ListBox ziel = (ListBox)sender;

            // Get dropped Schüler
            Schueler s = e.Data.GetData("Schüler") as Schueler;

            if (s == null)
                return;

            if (ziel.Tag.ToString() == "F")
                ViewModel.RemoveFromCurrentClass(s);
            else
                ViewModel.AddToCurrentClass(s);
        }

        private void ListBox_DragEnter(object sender, DragEventArgs e)
        {
            
        }

        private void ListBox_DragLeave(object sender, DragEventArgs e)
        {
            
        }

        private void ListBox_DragOver(object sender, DragEventArgs e)
        {
            // Drag & Drop verbieten, wenn dieselbe ListBox
            if (sender == dragSource)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void ListBox_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
        
        }

     
    }
}
