using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Groll.Schule.SchuleDBWeb.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

using Microsoft.Owin.Security;
using System.Data.Common;
using Groll.Schule.DataManager;
using Groll.Schule.Model;


namespace Groll.Schule.SchuleDBWeb
{
    public partial class _Default : Page    
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

                    int sj = uow.Settings["Global.AktuellesSchuljahr"].GetInt();
                    if (sj != 0)
                    {
                        var label = loginView1.FindControl("aktSchuljahr") as Label;
                        if (label != null)
                            label.Text = new Schuljahr(sj).ToString();

                    }
                }
                
            }
        }
    }
}