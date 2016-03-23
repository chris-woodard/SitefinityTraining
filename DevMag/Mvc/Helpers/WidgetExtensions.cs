using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Frontend.Mvc.Models;
using Telerik.Sitefinity.Libraries.Model;

namespace SitefinityWebApp.Mvc.Helpers
{
    public static class WidgetExtensions
    {
        public static IHtmlString RenderImage(this HtmlHelper helper, Image image, string className = "", string height = "", string width = "")
        {
            if (image == null)
            {
                return new HtmlString("");
            }

            return new HtmlString(image.MediaUrl);
        }

        public static T GetDataItem<T>(this ItemViewModel item) where T : class
        {
            return item.DataItem as T;
        }
    }
}