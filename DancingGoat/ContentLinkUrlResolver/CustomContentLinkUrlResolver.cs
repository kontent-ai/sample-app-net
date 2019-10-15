using Kentico.Kontent.Delivery;
using System.Globalization;

namespace DancingGoat
{
    public class CustomContentLinkUrlResolver : IContentLinkUrlResolver
    {
        protected string CurrentCulture => CultureInfo.CurrentUICulture.Name;

        public string ResolveLinkUrl(ContentLink link)
        {
            switch (link.ContentTypeCodename)
            {
                case "about_us":
                case "fact_about_us":
                    return $"/{CurrentCulture}/about";
                case "article":
                    return $"/{CurrentCulture}/articles/{link.UrlSlug}";
                case "brewer":
                    return $"/{CurrentCulture}/product/detail/{link.UrlSlug}";
                case "cafe":
                    return $"/{CurrentCulture}/cafes";
                case "coffee":
                    return $"/{CurrentCulture}/product/detail/{link.UrlSlug}";
                case "home":
                    return $"/{CurrentCulture}/";
                default:
                    return $"/not_found";
            }
        }

        public string ResolveBrokenLinkUrl()
        {
            return $"/broken_link";
        }
    }
}