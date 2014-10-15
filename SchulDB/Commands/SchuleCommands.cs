using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Groll.Schule.Model;

namespace Groll.Schule.SchulDB.Commands
{
    public class SchuleCommands
    {
        public static BeobachtungenCommands Beobachtungen = new BeobachtungenCommands();
    }


    public class SchuleCommandsBase : ObservableObject
    {
        protected bool CanExecute(object o)
        {
            return true;
        }

        protected void ExecuteCommand(object o)
        {
           
        }
    }

}