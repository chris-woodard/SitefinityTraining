using SitefinityWebApp.Mvc.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Frontend.Mvc.Models;
using Telerik.Sitefinity.News.Model;
using Telerik.Sitefinity.Model;

namespace SitefinityWebApp.Mvc.Models
{
    public class AuthorViewModel : ItemViewModel
    {
        public AuthorViewModel(DynamicContent author)
            : base(author)
        {
        }

        #region Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets the avatar URL.
        /// </summary>
        /// <value>
        /// The avatar URL.
        /// </value>
        public string Avatar
        {
            get
            {
                return this.avatar;
            }
            set
            {
                this.avatar = value;
            }
        }

        /// <summary>
        /// Gets or sets the job title.
        /// </summary>
        /// <value>
        /// The job title.
        /// </value>
        public string JobTitle
        {
            get
            {
                return this.jobTitle;
            }
            set
            {
                this.jobTitle = value;
            }
        }

        /// <summary>
        /// Gets or sets the bio.
        /// </summary>
        /// <value>
        /// The bio.
        /// </value>
        public string Bio
        {
            get
            {
                return this.bio;
            }
            set
            {
                this.bio = value;
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets the author view model.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="currentNewsItem">The current news item.</param>
        /// <returns>Author View Moddel</returns>
        public static AuthorViewModel GetAuthorViewModel(DynamicContent obj)
        {
            return new AuthorViewModel(obj)
            {
                Name = obj.GetString("Name"),
                JobTitle = obj.GetString("JobTitle"),
                Bio = obj.GetString("Bio"),
                Avatar = WidgetExtensions.GetRelatedMediaUrl(obj, "Avatar")
            };
        }

        #endregion

        #region Private fields

        private string name;
        private string avatar;
        private string jobTitle;
        private string bio;

        #endregion
    }
}