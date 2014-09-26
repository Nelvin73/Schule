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

        public List<int> StundenzahlenListe { get; private set; }      

        private object stundenanzahl;        
        public object Stundenanzahl
        {
            get
            {
                return stundenanzahl;
            }

            set
            {
                if (value != null && stundenanzahl != value)
                {
                    stundenanzahl = value;
                    OnPropertyChanged();
                    
                    // Inform Page-ViewModels
                    if (SetStundenanzahl != null)
                        SetStundenanzahl.Execute(stundenanzahl);
                }
            }
        }
     
        private bool showSamstag;
        public bool ShowSamstag
        {
            get
            {
                return showSamstag;
            }

            set
            {
                if (showSamstag != value)
                {
                    showSamstag = value;
                    OnPropertyChanged();

                    // Inform Page-ViewModels
                    if (SetShowSamstag != null)
                        SetShowSamstag.Execute(value);
                }
            }
        }

        private string pausenStunden;
        public string PausenStunden
        {
            get
            {
                return pausenStunden;
            }

            set
            {
                if (pausenStunden != value)
                {
                    pausenStunden = value;
                    OnPropertyChanged();

                    // Inform Page-ViewModels
                    if (SetPausenstunden != null)
                        SetPausenstunden.Execute(value);
                }
            }
        }
        
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

        public override void RefreshData()
        {
            StundenzahlenListe = new List<int>(){ 6, 8, 10, 12};
            Stundenanzahl = (int) Settings.Get("Stundenplan.Anzeige.AnzahlStunden", typeof(int), 6, true);
            PausenStunden = Settings.Get("Stundenplan.Anzeige.Pausenstunden", typeof(string)).ToString();
            ShowSamstag = (bool) Settings.Get("Stundenplan.Anzeige.ShowSamstag", typeof(bool), false, true);
            base.RefreshData();
        }

        #region Commands
        public DelegateCommand SetStundenanzahl { get;  set; }
        public DelegateCommand SetShowSamstag { get; set; }
        public DelegateCommand SetPausenstunden { get; set; }

        #endregion
    }
}
