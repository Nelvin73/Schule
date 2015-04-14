using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groll.Schule.OutputTools.Arbeitsblätter
{
    public class GrundrechnenÜbungConfig : IOutputTemplateConfig
    {
        IList<GrundrechenArten> SelectedOperators;


        public void LoadFromDatabase(DataManager.UowSchuleDB UnitofWork)
        {
            throw new NotImplementedException();
        }

        public void SaveToDatabase(DataManager.UowSchuleDB UnitofWork)
        {
            throw new NotImplementedException();
        }

        public void RestoreDefaultValues(DataManager.UowSchuleDB UnitofWork)
        {
            SelectedOperators = new List<GrundrechenArten> () 
            {
              GrundrechenArten.Plus, GrundrechenArten.Minus, GrundrechenArten.Mal, GrundrechenArten.Geteilt
            }


        }
    }
}
