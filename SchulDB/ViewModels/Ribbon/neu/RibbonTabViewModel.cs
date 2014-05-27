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
    public class RibbonTabViewModel : SchuleViewModelBase
    {

        #region Private Fields
        private RibbonViewModel RibbonVM;
        private bool isSelected = false;
        private bool isVisible = true;
        private string label = null;
        private string contextualTabGroupHeader;
        #endregion

        #region Properties

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

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value; OnPropertyChanged();
                }
            }
        }

        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                if (isVisible != value)
                { isVisible = value; OnPropertyChanged(); }
            }
        }

        public string Label
        {
            get { return label; }
            set
            {
                if (label != value)
                { label = value; OnPropertyChanged(); }
            }
        }

        #endregion      


        #region Element retrieval
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
        #endregion       
        
        public RibbonTabViewModel(RibbonViewModel Ribbon = null)
        {
            // Set RibbonViewModel to Default, if it is null
            RibbonVM = Ribbon ?? RibbonViewModel.Default;
            
            // Im Designermode anzeigen
            if (App.Current.MainWindow == null)
            {
                IsVisible = true;
                IsSelected = true;
            }
        }
                     
    }
}
