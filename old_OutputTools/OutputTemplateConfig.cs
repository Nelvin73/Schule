using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.DataManager;
using Groll.Schule.Model;

namespace Groll.Schule.OutputTools
{
    
    /// <summary>
    /// Interface für Konfigurations-Klassen von Output-Templates
    /// </summary>
    public interface IOutputTemplateConfig
    {
        // Load config from database
        void LoadFromDatabase(UowSchuleDB UnitofWork);

        // save config to database
        void SaveToDatabase(UowSchuleDB UnitofWork);

        // Load default
        void RestoreDefaultValues(UowSchuleDB UnitofWork);


    }
}
