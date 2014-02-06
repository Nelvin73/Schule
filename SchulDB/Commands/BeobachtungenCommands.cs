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

        private static RoutedUICommand historyViewChanged = new RoutedUICommand("HistoryViewChanged", "ChangeHistoryView", typeof(BeobachtungenCommands));
        private static RoutedUICommand clearInput = new RoutedUICommand("ClearInput", "ClearInput", typeof(BeobachtungenCommands));
        private static RoutedUICommand add = new RoutedUICommand("Add", "Add", typeof(BeobachtungenCommands));
        private static RoutedUICommand insertText = new RoutedUICommand("InsertText", "InsertText", typeof(BeobachtungenCommands));
        private static RoutedUICommand export = new RoutedUICommand("ExportBeobachtungen", "ExportBeobachtungen", typeof(BeobachtungenCommands));


        public static RoutedUICommand HistoryViewChanged
        {
            get { return historyViewChanged; }
        }
        public static RoutedUICommand ClearInput
        {
            get { return clearInput; }
        }

        public static RoutedUICommand Add
        {
            get { return add; }
        }

        public static RoutedUICommand InsertText
        {
            get { return insertText; }
        }

        public static RoutedUICommand ExportBeobachtungen
        {
            get { return export; }
        }

    }
}
