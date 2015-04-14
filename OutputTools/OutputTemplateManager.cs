using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groll.Schule.OutputTools.Arbeitsblätter;


namespace Groll.Schule.OutputTools
{
    public static class OutputTemplateManager
    {

        private static List<IOutputTemplate> templates;

        public static IReadOnlyCollection<IOutputTemplate> Templates
        {
            get { return templates.AsReadOnly(); }            
        }
               

        static OutputTemplateManager()
        {
            templates = new List<IOutputTemplate>();
            Register(new GrundrechnenÜbungOutput());
            Register(new GrundrechnenÜbungOutput("Test1","oijpoijopijopij\noüijpoijopij", "Arbeitsblätter", "Deutsch"));            
        }

        public static void Register(IOutputTemplate template)
        {
            templates.Add(template);
        }

        public static void Remove(IOutputTemplate template)
        {
            templates.Remove(template);
        }

        public static List<string> GetGroups()
        {            
            var g = from t in Templates                                        
                    group t by t.Group into i
                    orderby i.Key
                    select i.Key;
            return g.ToList();        
        }

        public static List<string> GetSubGroups(string Group)
        {            
            var sg = (from t in Templates
                    where t.Group == Group
                    group t by t.SubGroup into i
                    orderby i.Key
                    select i.Key).ToList();

            // Wenn ausschließlich leere Subgroup existiert, leere Liste zurückgeben.
            if (sg.Count == 1 && sg[0] == "")
                sg.Clear();

            return sg;
           
        }



    }
}
