using KenticoCloud.Delivery;

namespace DancingGoat
{
    public class CustomContentLinkUrlResolver : IContentLinkUrlResolver
    {
        public string ResolveLinkUrl(ContentLink link)
        {
            switch (link.ContentTypeCodename)
            {
                case "fact_about_us":
                case "about_us":
                    return $"/about";
                case "article":
                    return $"/articles/{link.UrlSlug}";
                case "brewer":
                    return $"/products/{link.UrlSlug}";
                case "cafe":
                    return $"/cafes";
                case "coffee":
                    return $"/products/{link.UrlSlug}";
                case "hero_unit":
                case "home":
                    return $"/";
                case "office":
                case "accessory":
                case "grinder":
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