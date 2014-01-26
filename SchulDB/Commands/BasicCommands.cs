using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Groll.Schule.SchulDB.Commands
{
    public class BasicCommands
    {
        private static RoutedUICommand navigateToPage = new RoutedUICommand("NavigateTo", "NavigateTo", typeof(BasicCommands));
        private static RoutedUICommand dumpContext = new RoutedUICommand("DumpContext", "DumpContext", typeof(BasicCommands));   

        
        public static RoutedUICommand NavigateTo
        {
            get { return navigateToPage; }
        }

        public static RoutedUICommand DumpContext
        {
            get { return dumpContext; }
        }


        public static void CanExecute_TRUE(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            // e.Handled = true;
        }


    }
}
