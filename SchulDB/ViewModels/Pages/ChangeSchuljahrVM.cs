using Groll.Schule.SchulDB.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.Model;
using Groll.Schule.DataManager;
using System.Collections.ObjectModel;
using Groll.Schule.SchulDB.Commands;

namespace Groll.Schule.SchulDB.ViewModels
{
    /// <summary>
    /// ViewModel für die Klassenbearbeitungs-Seite
    /// </summary>
    public class ChangeSchuljahrVM : SchuleViewModelBase
    {
        // internal Member

        private List<Schuljahr> schuljahrListe;
        private Schuljahr selectedSchuljahr;
        private int sliderMin;        
        private int sliderMax;
     
        #region Properties

        // Liste der Schuljahre
        public List<Schuljahr> Schuljahrliste
        {
            get { return schuljahrListe; }
            set
            {
                if (schuljahrListe != value)
                {
                    schuljahrListe = value;
                    OnPropertyChanged();

                    if (schuljahrListe.Count > 0)
                        SelectedSchuljahr = schuljahrListe[0];
                    else
                        SelectedSchuljahr = null;
                }
            }
        }

        public Schuljahr SelectedSchuljahr
        {
            get { return selectedSchuljahr; }
            set
            {
                if (selectedSchuljahr != value)
                {
                    selectedSchuljahr = value;
                    OnPropertyChanged();
                }
            }
        }

        public int SliderMinValue
        {
            get { return sliderMin; }
            set
            {
                if (sliderMin != value)
                {
                    sliderMin = value;
                    OnPropertyChanged();                    
                }
            }
        }

        public int SliderMaxValue
        {
            get { return sliderMax; }
            set
            {
                if (sliderMax != value)
                {
                    sliderMax = value;
                    OnPropertyChanged();
                }
            }
        }
       
        #endregion

        //  Konstructor
        public ChangeSchuljahrVM()
        {
            ChangeSchuljahrCommand = new DelegateCommand(ChangeSchuljahr);
        }
      
       
        public override void RefreshData()
        {
            base.RefreshData();
            
            // Initialisierung
            if (UnitOfWork != null)
            {
                // Aktuelle Klassen holen
                Schuljahrliste = UnitOfWork.Schuljahre.GetList();
                
                // Slider einstellen
                int curr = Schuljahr.GetCurrent().Startjahr;
                int min = schuljahrListe.Min(x => x.Startjahr);
                int max = schuljahrListe.Max( x => x.Startjahr);
                SelectedSchuljahr = Settings.ActiveSchuljahr;

                sliderMin = Math.Min(min, curr) - 1;
                sliderMax = Math.Max(max, curr) + 1;
            }
        }

        
        public DelegateCommand ChangeSchuljahrCommand { get; set; }
        private void ChangeSchuljahr(object x)
        {
            int i = selectedSchuljahr.Startjahr;
            
            // Check if already in DB
            if (UnitOfWork.Schuljahre.GetById(i) == null)
            {
                UnitOfWork.Schuljahre.Add(selectedSchuljahr);
                UnitOfWork.Save();
            }

            Settings.ActiveSchuljahr = selectedSchuljahr;
            UnitOfWork.TriggerDatabaseChangedEvent();
        }
    }
}
