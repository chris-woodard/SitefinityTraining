using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.RelatedData;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Frontend.Mvc.Models;
using Telerik.Sitefinity.Libraries.Model;
using Telerik.Sitefinity.Modules.Libraries;

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

        public static string GetRelatedMediaUrl(DynamicContent item, string fieldName)
        {
            var relatedItem = item.GetRelatedItems(fieldName).FirstOrDefault();

            if (relatedItem != null)
            {
                var imageId = relatedItem.Id;
                LibrariesManager manager = LibrariesManager.GetManager();
                Telerik.Sitefinity.Libraries.Model.Image image = manager.GetImage(imageId);
                if (image != null)
                    return image.MediaUrl;
            }

            return null;
        }
    }
}