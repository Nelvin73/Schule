using Groll.Schule.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;


namespace Groll.Schule.SchuleDBWeb.Pages
{
    public partial class Schueler : System.Web.UI.Page
    {

        private SchuleUnitOfWork uow = null;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            // if logged on, set info from database
            if (User.Identity.IsAuthenticated)
            {
                string userID = User.Identity.GetUserId();
                if (!String.IsNullOrWhiteSpace(userID))
                {
                    uow = new SchuleUnitOfWork(Schule.Datenbank.SchuleContext.AttachLocalDBFile("|DataDirectory|\\" + userID + ".mdf"));

                   
                }
            
            ListView1.SelectMethod = "SelectMethod";
            }
        }

        public List<Groll.Schule.Model.Schueler> SelectMethod()
        {
            return uow.Schueler.GetList();
        }
           

        
    }
}