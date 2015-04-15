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
using Groll.Schule.OutputTools;

namespace Groll.Schule.SchulDB.ViewModels
{
    /// <summary>
    /// ViewModel für die Klassenbearbeitungs-Seite
    /// </summary>
    public class VorlagenVM : SchuleViewModelBase
    {
        // internal Member
        private List<IOutputTemplate> vorlagenListe;        
        private IOutputTemplate selectedVorlage;
     
        #region Properties

        private List<string> groups;

        public List<string> Groups
        {
            get { return groups; }
            set
            {
                groups = value;
                OnPropertyChanged();

                if (groups.Count > 0)
                    SelectedGroup = groups[0];
                else
                    SelectedGroup = null;
                
            }
        }

        private List<string> subgroups;

        public List<string> SubGroups
        {
            get { return subgroups; }
            set
            {
                subgroups = value; OnPropertyChanged();

                if (subgroups.Count > 0)
                    SelectedSubgroup = subgroups[0];
                else
                    SelectedSubgroup = null;
            }
        }

        private string selectedGroup;

        public string SelectedGroup
        {
            get { return selectedGroup; }
            set
            {
                selectedGroup = value;
                OnPropertyChanged();
                OnGroupChanged();
            }
        }

        private string selectedSubgroup;

        public string SelectedSubgroup
        {
            get { return selectedSubgroup; }
            set
            {
                selectedSubgroup = value;
                OnPropertyChanged();
                OnSubGroupChanged();
            }
        }

        // Liste der Klassen / z.B. für Dropdown oder Liste        
        public List<IOutputTemplate> Vorlagen
        {
            get { return vorlagenListe; }
            set
            {
                if (vorlagenListe == value)
                    return;
                vorlagenListe = value; OnPropertyChanged();
                if (vorlagenListe.Count > 0)
                    SelectedVorlage = vorlagenListe[0];
                else
                    SelectedVorlage = null;
            }
        }

        public IOutputTemplate SelectedVorlage
        {
            get { return selectedVorlage; }
            set
            {
                if (selectedVorlage == value)
                    return;
                selectedVorlage = value; 
                OnPropertyChanged(); 
                OnSelectedVorlageChanged();
            }
        }

        #endregion

        //  Konstructor
        public VorlagenVM()
        {
            Start = new DelegateCommand((x) => SelectedVorlage.Start(UnitOfWork));
            ShowSettings = new DelegateCommand( (x) => SelectedVorlage.ShowConfig(UnitOfWork));
       
        }

        #region Verhalten bei Änderungen der Auswahl       

        
        private void OnGroupChanged()
        {
            UpdateSubGroups();

        }

        private void OnSubGroupChanged()
        {
            // Update Liste
            Vorlagen = 
                (from t in OutputTemplateManager.Templates
                where (selectedGroup == null || selectedGroup == "" || t.Group == selectedGroup) && (selectedSubgroup == null || selectedSubgroup == "" || t.SubGroup == selectedSubgroup)
                select t).ToList();                
        }

        protected virtual void OnSelectedVorlageChanged()
        {
            // nichts nötig, da mit ObservableCollection auf SelectedKlasse.Schüler gebunden wird
        }

        #endregion

        public override void RefreshData()
        {
            // Daten neu einlesen
            base.RefreshData();
            UpdateGroups();                                  
        }

        private void UpdateGroups()
        {         
            Groups = OutputTemplateManager.GetGroups();                            
        }

        private void UpdateSubGroups()
        {
            var k = OutputTemplateManager.GetSubGroups(selectedGroup);
            
            // Wenn mehrere Gruppen existieren, eine leere Auswahl ermöglichen                
            if (k.Count > 1 && !k.Contains(""))
            {
                k.Insert(0, "");
            }                      

            SubGroups = k;
        }

        #region Commands        
        public DelegateCommand Start { get; private set; }
        public DelegateCommand ShowSettings { get; private set; }

       
        #endregion

    }
}
