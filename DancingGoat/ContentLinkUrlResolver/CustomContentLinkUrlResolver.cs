using KenticoCloud.Delivery;

namespace DancingGoat
{
    public class CustomContentLinkUrlResolver : IContentLinkUrlResolver
    {
        public string ResolveLinkUrl(ContentLink link)
        {
            switch (link.ContentTypeCodename)
            {
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
                case "home":
                    return $"/";
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