using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Groll.Schule.SchulDB.Pages
{
    public interface ISchulDBPage
    {
        void SetMainWindow(MainWindow x);

        void OnDatabaseChanged();
      //   bool OnDisplayed();
        
    }
}
