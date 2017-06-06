using DancingGoat.Models.ContentTypes;
using KenticoCloud.Delivery.InlineContentItems;

namespace DancingGoat.ModularContentInRichTextResolver
{
    public class YoutubeVideoResolver : IInlineContentItemsResolver<YoutubeVideo>
    {
        public string Resolve(ResolvedContentItemData<YoutubeVideo> data)
        {
            return
                $"<div><iframe type=\"text/html\" width=\"1000\" height=\"385\" style=\"display:block; margin: auto; margin-top:30px ; margin-bottom: 30px\" src=\"https://www.youtube.com/embed/{data.Item.VideoId}?autoplay=1\" frameborder=\"0\"></iframe></div>";
        }
    }
}