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
    public class ApplicationMenuVM : RibbonTabVM
    {
        UowSchuleDB uow;

        #region Properties für Bindings

        public string CurrentDbType
        {
            get
            {
                if (uow == null)
                    return "";

                return uow.CurrentDbType.ToString();
            }
        }

        public string CurrentDbFile
        {
            get
            {
                if (uow == null)
                    return "";

                return uow.CurrentDbFilename.ToString();
            }
        }
        #endregion
      
        #region Konstruktor
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="ribbonVM">Root Element</param>
        public ApplicationMenuVM()          
        {
            Label = "";
            SmallImageSourceFile = LargeImageSourceFile = "Images/DB.ico";
            OnDatabaseChanged();             
        }
        #endregion

        #region Database-Handling
        public override void OnDatabaseChanged()
        {
            // invalidate all database relevant properties
            uow = RibbonVM.UnitOfWork;
            OnPropertyChanged("CurrentDbType");
            OnPropertyChanged("CurrentDbFile");            
            base.OnDatabaseChanged();
        }
      
        #endregion

    }
}
