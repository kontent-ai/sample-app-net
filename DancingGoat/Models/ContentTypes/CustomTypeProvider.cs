using System;

using DancingGoat.Models.ContentTypes;

using KenticoCloud.Delivery;

namespace DancingGoat.Models
{
    public class CustomTypeProvider : ICodeFirstTypeProvider
    {
        public Type GetType(string contentType)
        {
            switch (contentType)
            {
                case "about_us":
                    return typeof(AboutUs);
                case "accessory":
                    return typeof(Accessory);
                case "article":
                    return typeof(Article);
                case "brewer":
                    return typeof(Brewer);
                case "cafe":
                    return typeof(Cafe);
                case "coffee":
                    return typeof(Coffee);
                case "fact_about_us":
                    return typeof(FactAboutUs);
                case "grinder":
                    return typeof(Grinder);
                case "hero_unit":
                    return typeof(HeroUnit);
                case "home":
                    return typeof(Home);
                case "office":
                    return typeof(Office);
                case "image":
                    return typeof(Image);
                case "youtube_video":
                    return typeof(YoutubeVideo);
                case "inline_image":
                    return typeof(InlineImage);
                default:
                    return null;
            }
        }
    }
}