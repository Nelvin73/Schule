using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.SchulDB.Helper;
using Groll.Schule.DataManager;
using Groll.Schule.Model;
using System.Collections.ObjectModel;
using System.Windows.Controls.Ribbon;
using System.Windows.Media;
using Groll.Schule.SchulDB.Commands;


namespace Groll.Schule.SchulDB.ViewModels
{
    public class RibbonTabStundenplanVM : RibbonTabViewModel
    {      
        #region Properties für Bindings
     
        #endregion

        #region Konstruktor

        /// <summary>
        /// Konstruktor
        /// </summary>        
        public RibbonTabStundenplanVM() : base()
        {
            Label = "Stundenplan bearbeiten";
            ContextualTabGroupHeader = "Stundenplan";
                                      
        }
        #endregion

       
    }
}
