using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Groll.Schule.SchuleDBWeb.Zum_Testen
{
    public partial class Test1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SetDate_Click(object sender, EventArgs e)
        {
            textB.Text = DateTime.Now.ToString();
        }
    }
}