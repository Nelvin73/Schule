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
        private static RoutedUICommand add = new RoutedUICommand("Add", "Add", typeof(BeobachtungenCommands), new InputGestureCollection() { new KeyGesture(Key.Enter, ModifierKeys.Control)});
        private static RoutedUICommand insertText = new RoutedUICommand("InsertText", "InsertText", typeof(BeobachtungenCommands));
        private static RoutedUICommand insertTextbaustein = new RoutedUICommand("InsertTextbaustein", "InsertTextbaustein", typeof(BeobachtungenCommands));
        private static RoutedUICommand export = new RoutedUICommand("ExportBeobachtungen", "ExportBeobachtungen", typeof(BeobachtungenCommands));
        private static RoutedUICommand updateView = new RoutedUICommand("UpdateBeobachtungenView", "UpdateBeobachtungenView", typeof(BeobachtungenCommands));
        private static RoutedUICommand editModeChanged = new RoutedUICommand("EditModeChanged", "EditModeChanged", typeof(BeobachtungenCommands));


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

        public static RoutedUICommand InsertTextbaustein
        {
            get { return insertTextbaustein; }
        }

        public static RoutedUICommand ExportBeobachtungen
        {
            get { return export; }
        }

        public static RoutedUICommand UpdateBeobachtungenView
        {
            get { return updateView; }
        }
        public static RoutedUICommand EditModeChanged
        {
            get { return editModeChanged; }
        }

    }
}
