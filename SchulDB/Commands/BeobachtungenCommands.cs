using Groll.Schule.SchulDB.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Groll.Schule.SchulDB.Commands
{
    
    public class BeobachtungenCommands : SchuleCommandsBase
    {
        private DelegateCommand changeHistoryView;
        private DelegateCommand clearInput;
        private DelegateCommand addComment;
        private DelegateCommand insertText;
        private DelegateCommand insertTextbaustein ;
        private DelegateCommand exportToWord;
        private static RoutedUICommand updateView = new RoutedUICommand("UpdateBeobachtungenView", "UpdateBeobachtungenView", typeof(BeobachtungenCommands));
        private static RoutedUICommand editModeChanged = new RoutedUICommand("EditModeChanged", "EditModeChanged", typeof(BeobachtungenCommands));

        public BeobachtungenCommands()
        {
            ChangeHistoryView = ClearInput = AddComment = InsertText = InsertTextbaustein = ExportToWord =
                new DelegateCommand((o) => ExecuteCommand(o), (o) => CanExecute(o));
        }

        public DelegateCommand ChangeHistoryView
        {
            get { return changeHistoryView; }
            set { changeHistoryView = value; OnPropertyChanged(); }
        }

        public DelegateCommand ClearInput
        {
            get { return clearInput; }
            set { clearInput = value; OnPropertyChanged(); }
        }

        public DelegateCommand AddComment
        {
            get { return addComment; }
            set { addComment = value; OnPropertyChanged(); }
        }

        public DelegateCommand InsertText
        {
            get { return insertText; }
            set { insertText = value; OnPropertyChanged(); }
        }

        public DelegateCommand InsertTextbaustein
        {
            get { return insertTextbaustein; }
            set { insertTextbaustein = value; OnPropertyChanged(); }
        }

        public DelegateCommand ExportToWord
        {
            get { return exportToWord; }
            set { exportToWord = value; OnPropertyChanged(); }
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
