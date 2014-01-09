using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.Model;
using Groll.Schule.DataAccess;

namespace Groll.Schule.DataManager.Repositories
{
    public class SettingsRepository : RepositoryBase<Setting>
    {
        public SettingsRepository(SchuleContext context) : base(context) { }

        public Setting this[string Key]
        {
            get
            {
                return context.Settings.Find(Key);
            }

            set
            {
                var i = context.Settings.Find(Key);
                if (i != null)
                    i.SetValue(value);
            }
        }
    }
}
