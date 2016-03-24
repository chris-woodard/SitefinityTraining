using System;
using System.Linq;
using System.Collections.Generic;
namespace SitefinityWebApp.Mvc.Models
{
    public class AuthorsWidgetModel
    {
        public Boolean AvatarEnabled { get; set; }
        public List<AuthorViewModel> Authors { get; set; }
    }
}