﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.SchulDB.Helper;
using Groll.Schule.DataManager;
using Groll.Schule.Model;
using System.Collections.ObjectModel;


namespace Groll.Schule.SchulDB.ViewModels
{
    public class RibbonTabVM : RibbonBaseVM
    {        
        private RibbonVM ribbonVM;
        private string contextualTabGroupHeader;
        protected Dictionary<string, RibbonBaseVM> elementCache = new Dictionary<string,RibbonBaseVM>();

        protected RibbonBaseVM GetElement(string Key)
        {
            if (Key != null && elementCache.ContainsKey(Key))
                return elementCache[Key];
            else
                return null;            
        }

        protected RibbonBaseVM SetElement(string Key, RibbonBaseVM Element, bool Overwrite = true)
        {
            if (string.IsNullOrEmpty(Key) || Element == null)
                return null;

            if (Overwrite || !elementCache.ContainsKey(Key))
                elementCache.Add(Key, Element);

            return Element;
        }

        protected UowSchuleDB UnitOfWork
        {
            get
            {
                if (ribbonVM != null)
                    return ribbonVM.UnitOfWork;
                else return null;
            }
         }

        

        public RibbonTabVM(RibbonVM RibbonVM = null)
        {
            // Set RibbonVM to Default, if it is null
            ribbonVM = RibbonVM ?? RibbonVM.Default;
            
            // Im Designermode anzeigen
            if (App.Current.MainWindow == null)
            {
                IsVisible = true;
                IsSelected = true;
            }
        }
       
        /// <summary>
        /// Initialisierung wenn Datenbank sich geändert hat
        /// </summary>
        public virtual void OnDatabaseChanged()
        {

        }

        #region Binding Properties
        public string ContextualTabGroupHeader
        {
            get
            {
                return contextualTabGroupHeader;
            }
            set
            {
                if (contextualTabGroupHeader != value)
                {
                    contextualTabGroupHeader = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion
    }
}
