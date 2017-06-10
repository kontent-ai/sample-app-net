using System.Web.Mvc;

using Microsoft.Owin;
using Owin;

using KenticoCloud.Compose.RichText;
using KenticoCloud.Compose.RichText.Models;
using KenticoCloud.Compose.RichText.Resolvers;

using DancingGoat.Models;

[assembly: OwinStartupAttribute(typeof(DancingGoat.Startup))]

namespace DancingGoat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            RichTextProcessor<HtmlHelper>.Default.RegisterTypeResolver(new PartialViewResolver<Article>("InlineContent/Article"));
            RichTextProcessor<HtmlHelper>.Default.ImageResolver = new PartialViewResolver<IInlineImage>("InlineContent/Image");
        }
    }
}