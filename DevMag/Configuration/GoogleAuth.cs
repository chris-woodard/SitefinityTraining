using System;
using System.Configuration;
using System.Linq;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Localization;

namespace SitefinityWebApp.Configuration
{
    /// <summary>
    /// Sitefinity configuration section.
    /// </summary>
    /// <remarks>
    /// If this is a Sitefinity module's configuration,
    /// you need to add this to the module's Initialize method:
    /// App.WorkWith()
    ///     .Module(ModuleName)
    ///     .Initialize()
    ///         .Configuration<GoogleAuth>();
    /// 
    /// You also need to add this to the module:
    /// protected override ConfigSection GetModuleConfig()
    /// {
    ///     return Config.Get<GoogleAuth>();
    /// }
    /// </remarks>
    /// <see cref="http://www.sitefinity.com/documentation/documentationarticles/developers-guide/deep-dive/configuration/creating-configuration-classes"/>
    [ObjectInfo(Title = "Google Authorization", Description = "Google Authorization Description")]
    public class GoogleAuth : ConfigSection
    {
        [ObjectInfo(Title = "AuthKey", Description = "This is a sample string field.")]
        [ConfigurationProperty("AuthKey", DefaultValue = "")]
        public string AuthKey
        {
            get
            {
                return (string)this["AuthKey"];
            }
            set
            {
                this["AuthKey"] = value;
            }
        }
    }
}