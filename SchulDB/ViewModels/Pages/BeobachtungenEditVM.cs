using Groll.Schule.SchulDB.Helper;
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
    /// ViewModel für die Beobachtungs-Eingabe-Seite
    /// </summary>
    public class BeobachtungenEditVM : BeobachtungenBaseVM
    {
        // internal Member
       
        #region Properties
        private Beobachtung editedBeobachtung;

        public Beobachtung EditedBeobachtung
        {
            get { return editedBeobachtung; }
            set
            {
                editedBeobachtung = value;
                // Update Selected Values
                if (value == null)
                {
                    ResetBeobachtung();
                }
                else
                {
                    SelectedSchüler = editedBeobachtung.Schueler;
                    SelectedFach = editedBeobachtung.Fach ?? Fächerliste.First(x => x.FachId == -1000);
                    SelectedSchuljahr = editedBeobachtung.Schuljahr;
                    SelectedKlasse = editedBeobachtung.Klasse;
                    BeoDatum = editedBeobachtung.Datum;
                    BeoText = editedBeobachtung.Text;

                }
            }
        }

        public bool IsSchülerChanged
        {
            get { return editedBeobachtung.Schueler != SelectedSchüler; }
        }

        public bool IsSchuljahrChanged
        {
            get { return editedBeobachtung.Schuljahr != SelectedSchuljahr; }
        }


        private bool isEditMode;

        public bool IsEditMode
        {
            get { return isEditMode; }
            set
            {
                if (isEditMode != value)
                { isEditMode = value; OnPropertyChanged(); }
            }
        }
        
               
        #endregion

        //  Konstructor
        public BeobachtungenEditVM()
        {
        }

        #region Verhalten bei Änderungen der Auswahl
                  
        #endregion

        public void ResetBeobachtung()
        {
            SelectedSchüler = null;
            SelectedFach = null;
            SelectedSchuljahr = null;
            SelectedKlasse = null;
            BeoDatum = null;
            BeoText = "";

        }
        // Commands

        #region Public Interface für Commands    
        public bool UpdateBeobachtung(Beobachtung beo = null)
        {
            // Wenn beo = null, wird EditedBeobachtung genommen
            Beobachtung b = beo ?? EditedBeobachtung;
            if (b == null)
                return false;

            b.Datum = BeoDatum;
            b.Fach = (SelectedFach.FachId == -1000) ? null : SelectedFach;            
            b.Schueler = SelectedSchüler;
            b.Schuljahr = SelectedSchuljahr;
            b.Text = BeoText;

            UnitOfWork.Beobachtungen.Save();

            return true;
        }
        #endregion
    }
}
