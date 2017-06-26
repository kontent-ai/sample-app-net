using System.Linq;
using DancingGoat.Models.ContentTypes;
using KenticoCloud.Delivery.InlineContentItems;

namespace DancingGoat.InlineContentItemResolver
{
    public class HostedVideoResolver : IInlineContentItemsResolver<HostedVideo>
    {
        public string Resolve(ResolvedContentItemData<HostedVideo> data)
        {
            if (!data.Item.VideoHost.Any())
            {
                return string.Empty;
            }
            var selected = data.Item.VideoHost.First().Name;
            if (selected == "Vimeo")
            {
                return
                    $"<div><iframe  src=\"https://player.vimeo.com/video/{data.Item.VideoId}?title=0&byline=0&portrait=0\"  width=\"640\" height=\"360\" frameborder=\"0\" webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe><p></div>";
            }
            if (selected == "Youtube")
            {
                return $"<iframe width=\"560\" height=\"315\" class=\"hosted-video__wrapper\" src=\"https://www.youtube.com/embed/{data.Item.VideoId}\" frameborder=\"0\" allowfullscreen></iframe>";
            }
            return string.Empty;
        }
    }
}