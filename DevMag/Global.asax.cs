
using SitefinityWebApp.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Data.Configuration;
using Telerik.Sitefinity.Modules.Ecommerce;

namespace SitefinityWebApp
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Bootstrapper.Initialized += new EventHandler<Telerik.Sitefinity.Data.ExecutedEventArgs>(Bootstrapper_Initialized);
        }

        private void Bootstrapper_Initialized(object sender, Telerik.Sitefinity.Data.ExecutedEventArgs e)
        {
            if (e.CommandName == "Bootstrapped")
            {
                Config.RegisterSection<GoogleAuth>();             
                EcommerceEvents.OrderPlaced += new EcommerceEvents.OnOrderPlaced(EcommerceEvents_OrderPlaced);
            }
        }

        private void EcommerceEvents_OrderPlaced(Guid orderId)
        {
            //Your custom logic
        }
    }
}