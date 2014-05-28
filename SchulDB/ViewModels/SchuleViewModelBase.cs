﻿using Groll.Schule.SchulDB.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.Model;
using Groll.Schule.DataManager;
using System.Collections.ObjectModel;

namespace Groll.Schule.SchulDB.ViewModels
{
    /// <summary>
    /// ViewModel-Basisklasse
    /// </summary>
    public class SchuleViewModelBase : ObservableObject
    {
              
        #region Properties

        #region Unit of Work
        
        private UowSchuleDB unitOfWork;

        public UowSchuleDB UnitOfWork
        {
            get
            {
                if (unitOfWork == null)
                { 
                    // Try to get UnitOfWork Global Ressource; if not successful, it stays <null>
                    unitOfWork = System.Windows.Application.Current.TryFindResource("UnitOfWork") as UowSchuleDB;
                    unitOfWork.DatabaseChanged += unitOfWork_DatabaseChanged;
                    OnDatabaseChanged();
                }
                return unitOfWork;
            }          
        }

        
        #endregion       

        #endregion

        //  Konstructor
        public SchuleViewModelBase()
        {          
        }

        #region Verhalten bei Änderung der Datenbank
      

        /// <summary>
        /// EventHandler für das DatabaseChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void unitOfWork_DatabaseChanged(object sender, EventArgs e)
        {
            OnDatabaseChanged();
            
        }

        public virtual void OnDatabaseChanged()
        {
            RefreshData();
        }

        public virtual void RefreshData() { }

       


        #endregion


        // Commands

        #region Public Interface für Commands
       
        #endregion
    }
}
