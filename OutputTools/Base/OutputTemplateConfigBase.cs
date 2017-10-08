using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groll.Schule.OutputTools
{
    public abstract class OutputTemplateConfigBase : IOutputTemplateConfig
    {
        public OutputTemplateConfigBase()
        {            
            RestoreDefaultValues();            
        }

        public virtual void LoadFromDatabase(DataManager.SchuleUnitOfWork UnitofWork)
        {
            var s = UnitofWork.Settings["OutputTemplate.Settings." + GetType().FullName];
            string settings = s == null ? "" : s.GetString();            
            if (!String.IsNullOrEmpty(settings))
            {
                SetSettingsFromString(settings);
            }
        }

        public virtual void SaveToDatabase(DataManager.SchuleUnitOfWork UnitofWork)
        {
            string settings = GetSettingsAsString();
            if (!String.IsNullOrEmpty(settings))

            {
                UnitofWork.Settings["OutputTemplate.Settings." + GetType().FullName] = new Model.Setting("OutputTemplate.Settings." + GetType().FullName, settings);
                UnitofWork.Save();
            }   
            
        }

        public abstract string GetSettingsAsString();
        public abstract void SetSettingsFromString(string Settings);
        
        public virtual void RestoreDefaultValues()
        {            
        }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
