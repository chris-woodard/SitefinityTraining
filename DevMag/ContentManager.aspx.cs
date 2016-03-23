using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Telerik.OpenAccess;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Data.Linq.Dynamic;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Libraries.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Libraries;
using Telerik.Sitefinity.Modules.News;
using Telerik.Sitefinity.News.Model;
using Telerik.Sitefinity.RelatedData;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Taxonomies;
using Telerik.Sitefinity.Taxonomies.Model;
using Telerik.Sitefinity.Utilities.TypeConverters;
using Telerik.Sitefinity.Workflow;
namespace SitefinityWebApp
{
    public partial class ContentManager : System.Web.UI.Page
    {
        public string ServerPath { get { return Server.MapPath("~/App_Data/Content"); } }
        string imageTitle = "kitten";
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void NewsExport_Click(object sender, EventArgs e)
        {
            StringBuilder newsItems = new StringBuilder();
            NewsManager man = NewsManager.GetManager();
            var tagString = string.Empty;
            List<string> tags = null;
            TaxonomyManager tMan = TaxonomyManager.GetManager();
            foreach (var newsItem in man.GetNewsItems().Where(i => i.Status == Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Master))
            {
                
                tags = newsItem.GetValue<TrackedList<Guid>>("Tags").ToList().Select(i => tMan.GetTaxon(i).Title.Value).ToList();

                newsItems.AppendLine(String.Format("{0}|{1}|{2}", newsItem.Title, String.Join(",", tags), newsItem.Content.Value.Replace("\n","")));
            }
            var file = new StreamWriter(ServerPath + "\\news.txt");
            file.Write(newsItems);
            file.Close();
        }
        protected void NewsImport_Click(object sender, EventArgs e)
        {
            string line;
            var newsText = new StreamReader(ServerPath + "\\news.txt");
            while ((line = newsText.ReadLine()) != null)
            {
                CreateNews(line.Split('|'));
            }
        }
        protected void AuthorImport_Click(object sender, EventArgs e)
        {
            string line;
            var authorSheet = new StreamReader(ServerPath + "\\Authors_items.csv");
            while ((line = authorSheet.ReadLine()) != null)
            {
                CreateAuthor(line.Split(','));
            }
        }
        private void CreateAuthor(string[] values)
        {
            // Set the provider name for the DynamicModuleManager here. All available providers are listed in
            // Administration -> Settings -> Advanced -> DynamicModules -> Providers
            var providerName = String.Empty;

            string name = values[6],
                bio = values[8],
                jobTitle = values[7];
            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(providerName);

            //Suppress permission checks to ensure code runs even by unauthorized users
            dynamicModuleManager.Provider.SuppressSecurityChecks = true;

            Type authorType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.Authors.Author");
            DynamicContent authorItem = dynamicModuleManager.CreateDataItem(authorType);

            // This is how values for the properties are set
            authorItem.SetValue("Name", name);
            authorItem.SetValue("Bio", bio);
            authorItem.SetValue("JobTitle", jobTitle);

            LibrariesManager avatarManager = LibrariesManager.GetManager();
            var avatarItem = avatarManager.GetImages().FirstOrDefault(i => i.Title == imageTitle && i.Status == Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Master);
            if (avatarItem == null)
            {
                CreateImage();
                avatarItem = avatarManager.GetImages().FirstOrDefault(i => i.Title == imageTitle && i.Status == Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Master);
            }
            // This is how we relate an item
            authorItem.CreateRelation(avatarItem, "Avatar");

            authorItem.SetString("UrlName", "SomeUrlName");
            authorItem.SetValue("Owner", SecurityManager.GetCurrentUserId());
            authorItem.SetValue("PublicationDate", DateTime.Now);
            authorItem.SetWorkflowStatus(dynamicModuleManager.Provider.ApplicationName, "Draft");

            // You need to call SaveChanges() in order for the items to be actually persisted to data store
            dynamicModuleManager.SaveChanges();
        }
        private void CreateImage()
        {
            LibrariesManager librariesManager = LibrariesManager.GetManager();

            //Suppress permission checks to ensure code runs even by unauthorized users
            librariesManager.Provider.SuppressSecurityChecks = true;

            Image image = librariesManager.GetImages().Where(i => i.Title == imageTitle && i.Status == Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Master).FirstOrDefault();
            FileStream imageStream = new FileStream(ServerPath + "\\author.jpg", FileMode.Open);
            if (image == null)
            {
                //The album post is created as master. The masterImageId is assigned to the master version.
                image = librariesManager.CreateImage();
                var imageGuid = image.Id;
                //Set the parent album.
                Album album = librariesManager.GetAlbums().Where(i => i.Title == "Default Library").SingleOrDefault();
                image.Parent = album;

                //Set the properties of the album post.
                image.Title = imageTitle;
                image.DateCreated = DateTime.UtcNow;
                image.PublicationDate = DateTime.UtcNow;
                image.LastModified = DateTime.UtcNow;
                image.UrlName = Regex.Replace(imageTitle.ToLower(), @"[^\w\-\!\$\'\(\)\=\@\d_]+", "-");

                //Upload the image file.
                librariesManager.Upload(image, imageStream, ".jpg");

                //Save the changes.
                librariesManager.SaveChanges();

                //Publish the Albums item. The live version acquires new ID.
                var bag = new Dictionary<string, string>();
                bag.Add("ContentType", typeof(Image).FullName);
                WorkflowManager.MessageWorkflow(imageGuid, typeof(Image), null, "Publish", false, bag);
            }
        }
        private void CreateNews(string[] values)
        {
            NewsManager man = NewsManager.GetManager();
            man.Provider.SuppressSecurityChecks = true;
            TaxonomyManager tMan = TaxonomyManager.GetManager();
            tMan.Provider.SuppressSecurityChecks = true;
            string title = values[0],
                tags = values[1],
                content = values[2];
            var newsItem = man.GetNewsItems().FirstOrDefault(i => i.Title == title);
            if (newsItem != null) return;
            newsItem = man.CreateNewsItem();
            var newsId = newsItem.Id;
            newsItem.Title = title;
            newsItem.Content = content;
            var tag = tMan.GetTaxa<FlatTaxon>().FirstOrDefault(i => i.Title == tags);
            var taxa = tMan.GetTaxonomy<FlatTaxonomy>(TaxonomyManager.TagsTaxonomyId);
            if (tag == null)
            {
                tag = tMan.CreateTaxon<FlatTaxon>(Guid.NewGuid());
                tag.Title = tags;
                tag.Name = tags;
                tag.Description = "This tag categorizes the Breakfast";
                tag.UrlName = new Lstring(Regex.Replace(tags, @"[^\w\-\!\$\'\(\)\=\@\d_]+", "-").ToLower());
                taxa.Taxa.Add(tag);
                tMan.SaveChanges();
            }

            newsItem.Organizer.AddTaxa("Tags", tag.Id);

            newsItem.DateCreated = DateTime.UtcNow;
            newsItem.PublicationDate = DateTime.UtcNow;
            newsItem.LastModified = DateTime.UtcNow;
            newsItem.UrlName = Regex.Replace(title.ToLower(), @"[^\w\-\!\$\'\(\)\=\@\d_]+", "-");

            //Recompiles and validates the url of the news item.
            man.RecompileAndValidateUrls(newsItem);

            //Save the changes.
            man.SaveChanges();

            //Publish the news item. The published version acquires new ID.
            var bag = new Dictionary<string, string>();
            bag.Add("ContentType", typeof(NewsItem).FullName);
            WorkflowManager.MessageWorkflow(newsId, typeof(NewsItem), null, "Publish", false, bag);
            man.Lifecycle.Publish(newsItem);
            man.SaveChanges();
        }
    }
}