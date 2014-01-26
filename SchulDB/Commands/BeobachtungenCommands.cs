using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Groll.Schule.SchulDB.Commands
{
    public class BeobachtungenCommands
    {
        private static RoutedUICommand clearInput = new RoutedUICommand("ClearInput", "ClearInput", typeof(BeobachtungenCommands));



        public static RoutedUICommand ClearInput
        {
            get { return clearInput; }
        }





    }
}
