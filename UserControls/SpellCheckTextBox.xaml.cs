using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;

namespace Groll.UserControls
{
    public partial class SpellCheckTextBox : TextBox
    {
                
        ContextMenu contextMenu = new ContextMenu();

        public SpellCheckTextBox()
            : base()
        {
            this.ContextMenu = contextMenu;
            this.SpellCheck.IsEnabled = true;
            this.SpellCheck.SpellingReform = SpellingReform.Postreform;
            
            if (System.IO.File.Exists("Wörterbuch.lex"))
                this.SpellCheck.CustomDictionaries.Add(new Uri("Wörterbuch.lex", UriKind.Relative)); 

            this.Language = System.Windows.Markup.XmlLanguage.GetLanguage("DE");
            this.Initialized += (s, e) =>
                ContextMenuOpening += new ContextMenuEventHandler(SpellCheckTextBox_ContextMenuOpening);
        }

        private void SpellCheckTextBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            // Define new Context Menu
            contextMenu.Items.Clear();

            // Check for spelling errors
            SpellingError error = this.GetSpellingError(this.CaretIndex);
            if (error != null)
            {
                var sugg = error.Suggestions.ToList();
                if (sugg.Count > 0)
                {
                    // Add Suggestions
                    foreach (string suggession in sugg)
                    {
                        contextMenu.Items.Add(GetMenu(suggession, EditingCommands.CorrectSpellingError, this, true));
                    }
                }
                else
                    contextMenu.Items.Add(GetMenu("(keine Vorschläge)", ApplicationCommands.NotACommand, this));
                // Add Separator
                contextMenu.Items.Add(new Separator());

                // Add Ignore All
                contextMenu.Items.Add(GetMenu("Alle ignorieren", EditingCommands.IgnoreSpellingError, this));

                // Add to dic.                  
                var dicItem = new MenuItem() { Header = "Zum Wörterbuch hinzufügen" };
                dicItem.Command = EditingCommands.IgnoreSpellingError;
                dicItem.CommandTarget = this;
                dicItem.Click += (object o, RoutedEventArgs rea) =>
                {
                    this.AddToDictionary(this.Text.Substring(GetSpellingErrorStart(this.CaretIndex), GetSpellingErrorLength(this.CaretIndex)));
                };
                contextMenu.Items.Add(dicItem);
            }

            // Add Separator
            contextMenu.Items.Add(new Separator());

            // Add Cut, Copy, Paste
            contextMenu.Items.Add(new MenuItem() { Command = ApplicationCommands.Cut });
            contextMenu.Items.Add(new MenuItem() { Command = ApplicationCommands.Copy });
            contextMenu.Items.Add(new MenuItem() { Command = ApplicationCommands.Paste });
        }

        private void AddToDictionary(string p)
        {
            // Save string in Dictionary File
            try
            {
                var file = System.IO.File.AppendText("Wörterbuch.lex");
                file.WriteLine(p);
                file.Close();

                // Check if Wörterbuch already used
                if (this.SpellCheck.CustomDictionaries.Count == 0)
                    this.SpellCheck.CustomDictionaries.Add(new Uri("Wörterbuch.dic", UriKind.Relative)); 

            }
            catch (Exception)
            {
                // Failed to write to dic ... ignore error ...                
            }
            
        }

        private MenuItem GetMenu(string header, ICommand command, TextBoxBase target, bool Bold = false)
        {
            MenuItem item = new MenuItem();
            item.Header = header;
            item.FontWeight = Bold ? FontWeights.Bold : FontWeights.Normal;
            item.Command = command;
            item.CommandParameter = header;
            item.CommandTarget = target;           
            return item;
        }
    }
}

