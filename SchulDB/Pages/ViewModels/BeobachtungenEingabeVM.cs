using Groll.Schule.SchulDB.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.Model;
using Groll.Schule.DataManager;
using System.Collections.ObjectModel;

namespace Groll.Schule.SchulDB.Pages.ViewModels
{
    /// <summary>
    /// ViewModel für die Beobachtungs-Eingabe-Seite
    /// </summary>
    public class BeobachtungenEingabeVM : ObservableObject
    {
        // internal Member
        private int currentSJ;
        private UowSchuleDB uow;
        private DateTime? beoDatum;
        private string beoText;
        private ObservableCollection<Fach> fächerListe;
        private Klasse selectedKlasse;
        private Schueler selectedSchüler;
        private Fach selectedFach;
        private ObservableCollection<Klasse> klassenListe;
        private ObservableCollection<Schueler> schülerListe;

        #region Properties

        // Unit of Work
        public UowSchuleDB UnitOfWork
        {
            get { return uow; }
            set { uow = value; OnUnitOfWorkChanged(); }
        }
        
        // Datum der Beobachtungen (NULL = Allgemein)        
        public DateTime? BeoDatum
        {
            get { return beoDatum; }
            set { beoDatum = value; OnPropertyChanged(); }
        }

        // Beobachtungstext        
        public string BeoText
        {
            get { return beoText; }
            set { beoText = value; OnPropertyChanged(); }
        }

        // Liste der Fächer / z.B. für Dropdown oder Liste        
        public ObservableCollection<Fach> Fächerliste
        {
            get { return fächerListe; }
            set { fächerListe = value; OnPropertyChanged(); }
        }   
     
        // Liste der Klassen / z.B. für Dropdown oder Liste        
        public ObservableCollection<Klasse> KlassenListe
        {
            get { return klassenListe; }
            set
            {
                klassenListe = value; OnPropertyChanged();
                if (klassenListe.Count > 0)
                    SelectedKlasse = klassenListe[0];                  
            }
        }

        // Liste der Schüler / z.B. für Dropdown oder Liste                       
        public ObservableCollection<Schueler> SchülerListe
        {
            get { return schülerListe; }
            set { schülerListe = value; OnPropertyChanged();
            if (schülerListe.Count > 0)
                SelectedSchüler = schülerListe[0]; 
            }
        }

        public Klasse SelectedKlasse
        {
            get { return selectedKlasse; }
            set { selectedKlasse = value; OnPropertyChanged(); OnSelectedKlasseChanged(); }
        }       

        public Schueler SelectedSchüler
        {
            get { return selectedSchüler; }
            set { selectedSchüler = value; OnPropertyChanged(); OnSelectedSchülerChanged(); }
        }

        public Fach SelectedFach
        {
            get { return selectedFach; }
            set { selectedFach = value; OnPropertyChanged(); }
        } 
             
        #endregion

        //  Konstructor
        public BeobachtungenEingabeVM()
        {           
        }

        #region Verhalten bei Änderungen der Auswahl

        private void OnUnitOfWorkChanged()
        {
            // Initialisierung
            if (uow != null)
            {
                // Aktuelles Schuljahr holen
                int currentSJ = uow.Settings["Global.AktuellesSchuljahr"].GetInt(Schuljahr.GetCurrent().Startjahr);

                // Klassen des Schuljahres holen
                KlassenListe = new ObservableCollection<Klasse>(uow.Klassen.GetList(k => k.Schuljahr.Startjahr == currentSJ));

                // Aktuelle Fächer holen
                Fächerliste = new ObservableCollection<Fach>(uow.Fächer.GetList(f => f.Inaktiv == false));
            }
        }

        private void OnSelectedKlasseChanged()
        { 
            // Eine Klasse wurde ausgewählt => Schülerliste aktualisieren
            SchülerListe = new ObservableCollection<Schueler>(SelectedKlasse.Schueler);
            
            
        }

        private void OnSelectedSchülerChanged()
        {
            // Ein Schüler wurde ausgewählt 
            int i = 0;  // Dummy
        }
        


        private void UpdateSchülerListe()
        {

        }

        #endregion


        // Commands
        public void AddCurrentComment()
        {
            Beobachtung b = new Beobachtung()
            {
                 Datum = beoDatum,
                 Fach = selectedFach, 
                 Text = beoText, 
                 Schuljahr = ScurrentSJ,
                 Schueler = selectedSchüler};
                
                uow.Beobachtungen.Add(b);

        }

    }
}
