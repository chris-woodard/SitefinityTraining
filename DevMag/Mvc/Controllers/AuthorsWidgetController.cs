using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;
using SitefinityWebApp.Mvc.Models;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.Utilities.TypeConverters;

namespace SitefinityWebApp.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "AuthorsWidget", Title = "Authors Widget", SectionName = "MvcWidgets")]
    public class AuthorsWidgetController : Controller
    {
        public bool EnableAvatar { get { return _enableAvatar; } set { _enableAvatar = value; } }
        private bool _enableAvatar = true;
        /// <summary>
        /// This is the default Action.
        /// </summary>
        public ActionResult Index()
        {
            var model = new AuthorsWidgetModel();
        
            //GET AUTHORS
            var providerName = String.Empty;
            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(providerName);
            Type authorType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.Authors.Author");

            // This is how we get the collection of Author items
            var myCollection = dynamicModuleManager.GetDataItems(authorType).Where(i => i.Status == Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Live).Take(4);
            // At this point myCollection contains the items from type authorType
            model.AvatarEnabled = this.EnableAvatar;
            model.Authors = myCollection.Select(i => AuthorViewModel.GetAuthorViewModel(i)).ToList();

            return View("Default", model);
        }
       
    }
}