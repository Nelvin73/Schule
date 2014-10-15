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
    public class KlassenarbeitEditVM : SchuleViewModelBase
    {
        // internal Member
        private List<Klasse> klassenListe;
        private Klassenarbeit selectedKlassenarbeit;
        private ObservableCollection<Klassenarbeit> klassenArbeiten;
        private Klasse selectedKlasse;
        private List<Fach> fächerListe;
      

        #region Properties

        // Liste der Klassen / z.B. für Dropdown oder Liste        
        public List<Klasse> KlassenListe
        {
            get { return klassenListe; }
            set
            {
                if (klassenListe == value)
                    return;
                klassenListe = value; OnPropertyChanged();
                if (klassenListe.Count > 0)
                    SelectedKlasse = klassenListe[0];
                else
                    SelectedKlasse = null;
            }
        }

        public Klasse SelectedKlasse
        {
            get { return selectedKlasse; }
            set
            {
                if (selectedKlasse == value)
                    return;
                selectedKlasse = value;
                OnPropertyChanged();
                OnSelectedKlasseChanged();
            }
        }
        
        public Klassenarbeit SelectedKlassenarbeit
        {
            get { return selectedKlassenarbeit; }
            set
            {
                if (selectedKlassenarbeit == value)
                    return;
                selectedKlassenarbeit = value;
                OnPropertyChanged();
                OnSelectedKlassenarbeitChanged();
            }
        }


        // Liste der Schüler ohne Klasse                       
        public ObservableCollection<Klassenarbeit> Klassenarbeiten
        {
            get { return klassenArbeiten; }
            set
            {
                if (klassenArbeiten == value)
                    return;
                klassenArbeiten = value; OnPropertyChanged();
                if (klassenArbeiten.Count > 0)
                    SelectedKlassenarbeit = klassenArbeiten[0];
                else
                    SelectedKlassenarbeit = null;
            }
        }

        public List<Fach> Fächerliste
        {
            get { return fächerListe; }
            set
            {
                if (fächerListe == value)
                    return;
                fächerListe = value; OnPropertyChanged();
/*                if (fächerListe.Count > 0)
                    SelectedFach = fächerListe[0];
                else
                    SelectedFach = null;  */
            }
        }

        
        
        #endregion

        //  Konstructor
        public KlassenarbeitEditVM()
        {
            AddArbeitCommand = new DelegateCommand((object x) => AddArbeit(x));            
        }


        #region Verhalten bei Änderungen der Auswahl

        protected virtual void OnSelectedKlasseChanged()
        {
            Klassenarbeiten = new ObservableCollection<Klassenarbeit>(UnitOfWork.Klassenarbeiten.GetList().Where(x => x.Klasse == SelectedKlasse));
            // nichts nötig, da mit ObservableCollection auf SelectedKlasse.Schüler gebunden wird
        }

        protected void OnSelectedKlassenarbeitChanged()
        {
            // Für alle Schüler Informationen einpflegen, falls noch nicht vorhanden
            if (selectedKlassenarbeit == null)
                return;

            var missing = selectedKlasse.Schueler.Except
                (from n in SelectedKlassenarbeit.Noten
                select n.Schüler);

            if (missing.Count() > 0)
            {
                foreach (var s in missing)
                {
                    SelectedKlassenarbeit.Noten.Add(new KlassenarbeitsNote()
                    {
                        Schüler = s,
                        HatMitgeschrieben = false,
                        Note = null,
                        Punkte = null
                    });
                }

                UnitOfWork.Save();
            }
        }

        #endregion
                                      
        public override void RefreshData()
        {
            // Lädt sämtliche Daten neu, z.B. wenn DB oder aktuelles Schuljahr geändert wurde
            base.RefreshData();
            
            // Initialisierung
            if (UnitOfWork != null)
            {                
                int sj = Settings.ActiveSchuljahr.Startjahr;
                        
                // Aktuelle Klassen holen
                KlassenListe = UnitOfWork.Klassen.GetList().Where(x => x.SchuljahrId == sj).ToList();                   

                // Fächer holen
                Fächerliste = UnitOfWork.Fächer.GetActiveFächer();
            }
        }


        #region Commands
        public DelegateCommand AddArbeitCommand { get; private set; }

        private object AddArbeit(object x)
        {
            // var k = UnitOfWork.Klassenarbeiten.Add(new Klassenarbeit());            
            var k = UnitOfWork.Klassenarbeiten.Add(new Klassenarbeit());
            Klassenarbeiten.Add(k);
            k.Name = "Neue Klassenarbeit";
            k.Klasse = SelectedKlasse;
            UnitOfWork.Save();
            
            SelectedKlassenarbeit = k;
            return k;
        }


        #endregion
   



    }
}
