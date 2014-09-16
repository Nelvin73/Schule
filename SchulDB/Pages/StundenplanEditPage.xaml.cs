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
    public partial class StundenplanEditPage : Page, ISchulDBPage
    {
        private ListBox dragSource = null;
        private Point dragStartPoint = new Point();
        private Schueler currentClickedSchüler = null;
        private bool DragMoveStarted = false;
       

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

        public StundenplanEditPage()
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
           
        }

        #region Drag & Drop

        // Check if Drag & Drop is initiated
        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Handle Doubleclick
            if (e.ClickCount > 1)
                return;

            // Remember DragStartPosition
            dragSource = (ListBox) sender;
            dragStartPoint = e.GetPosition(dragSource);
            
            // if this element is selected, supress click to allow multiple selections
            var listItem = dragSource.ContainerFromElement((Visual) e.OriginalSource) as FrameworkElement;
            if (listItem == null)
                return;

            currentClickedSchüler = dragSource.ItemContainerGenerator.ItemFromContainer(listItem) as Schueler;
            
            if (dragSource.SelectedItems.Contains(currentClickedSchüler))
            {
                // Prevent further handling to allow Drag and Drop of multi-selections  
                // Remember to handle it later with DragMoveStarted flag
                e.Handled = true;
                DragMoveStarted = true;
            }

        }

       

        private void ListBox_MouseMove(object sender, MouseEventArgs e)
        {
                        
            if (dragSource == null)
                    return;

            // Check if Drag is started            
            Vector v = e.GetPosition(dragSource) - dragStartPoint;            
            if (e.LeftButton == MouseButtonState.Pressed && (Math.Abs(v.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(v.Y) > SystemParameters.MinimumVerticalDragDistance ))
            {
                if (currentClickedSchüler != null)
                {
                    // Check if multiple items are selected
                    object items = (dragSource.SelectedItems.Count > 1 ? dragSource.SelectedItems.Cast<Schueler>().ToList() : new List<Schueler>() { currentClickedSchüler });                    
                    
                    DragDrop.DoDragDrop(dragSource, new DataObject("Schüler",  items), DragDropEffects.Move | DragDropEffects.None);
                }
            }

        }
                            

        // Drag & Drop wird versucht
        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            ListBox ziel = (ListBox)sender;

            // Get dropped Schüler
            IList<Schueler> s = e.Data.GetData("Schüler") as IList<Schueler>;

            if (s == null)
                return;

            foreach (Schueler i in s)
            {
                if (ziel.Tag.ToString() == "F")
                    ViewModel.RemoveFromCurrentClass(i);
                else
                    ViewModel.AddToCurrentClass(i);
            }
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

        #endregion      

        private void Schülerliste_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var l = (ListBox)sender;
            if (l.Tag.ToString() == "F")
                ViewModel.AddToCurrentClass(l.SelectedItem as Schueler);
            else
                ViewModel.RemoveFromCurrentClass(l.SelectedItem as Schueler);
  
                      }
    
     
    }
}
