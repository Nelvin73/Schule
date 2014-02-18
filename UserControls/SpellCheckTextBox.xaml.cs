using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;

namespace Groll.UserControls
{
    public partial class SpellCheckTextBox : TextBox
    {
        ContextMenu _menu;
        private ContextMenu menu
        {
            get
            {
                if (_menu == null)
                {
                    _menu = new ContextMenu();
                    _menu.Items.Add(new MenuItem() { Command = ApplicationCommands.Cut });
                    _menu.Items.Add(new MenuItem() { Command = ApplicationCommands.Copy });
                    _menu.Items.Add(new MenuItem() { Command = ApplicationCommands.Paste });
                }
                return _menu;
            }
        }


        public SpellCheckTextBox()
            : base()
        {
            this.ContextMenu = menu;
            this.SpellCheck.IsEnabled = true;
            this.SpellCheck.SpellingReform = SpellingReform.Postreform;
            this.Language = System.Windows.Markup.XmlLanguage.GetLanguage("DE");
            this.Initialized += (s, e) =>
                ContextMenuOpening += new ContextMenuEventHandler(SpellCheckTextBox_ContextMenuOpening);
        }

        private void SpellCheckTextBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            // Define new Context Menu
                       
            /*
             *  Vorschläge (Fett)   oder  (no spelling suggestions)
             * -----------------
             * Ignore all
             * ------------------
             * Cut
             * Copy
             * Paste
             * */
            this.ClearSpellCheckMenuItems(menu);
            int catatPos = this.CaretIndex;
     
            SpellingError error = this.GetSpellingError(catatPos);
            if (error != null)
            {
                this.ContextMenu.Items.Insert(0, new Separator());
                MenuItem item = this.GetMenu("Ignore All", EditingCommands.IgnoreSpellingError, this);
                item.Tag = "S";
                this.ContextMenu.Items.Insert(0, item);
                foreach (string suggession in error.Suggestions)
                {
                    item = this.GetMenu(suggession, EditingCommands.CorrectSpellingError, this);
                    item.Tag = "S";
                    this.ContextMenu.Items.Insert(0, item);
                }

            }
        }
        private MenuItem GetMenu(string header, ICommand command, TextBoxBase target)
        {
            MenuItem item = new MenuItem();
            item.Header = header;
            item.Command = command;
            item.CommandParameter = header;
            item.CommandTarget = target;
            return item;
        }
        private void ClearSpellCheckMenuItems(ContextMenu menu)
        {
            foreach (var item in menu.Items.Cast<MenuItem>().ToList())
            {               
                if (item != null && item.Tag != null)
                    menu.Items.Remove(item);
            }
        }
    }
}

