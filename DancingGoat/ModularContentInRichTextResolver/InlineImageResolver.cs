using System.Linq;
using DancingGoat.Models.ContentTypes;
using KenticoCloud.Delivery.InlineContentItems;

namespace DancingGoat.ModularContentInRichTextResolver
{
    public class InlineImageResolver : IInlineContentItemsResolver<InlineImage>
    {
        public string Resolve(ResolvedContentItemData<InlineImage> data)
        {
            var image = data.Item.Image.First();
            var floating = string.Empty;
            if (data.Item.Flow.Any())
            {
                var option = data.Item.Flow.First().Name;
                if (option == "Float left")
                {
                    floating = "float: left;";
                }
                else if (option == "Float right")
                {
                    floating = "float: right;";
                }
            }
            return
                $"<div style=\"{floating}\">" +
                $"<figure style=\"height:300px; display: block;\">" +
                    $"<img style=\"max-width: 100%; max-height: 100%; display: block; margin: auto;\" src=\"{image.Url}\" />" + $"<figcaption style=\"text-align:center; font-weight:200; font-size:16px;\">{data.Item.Title}</figcaption>" +
                $"</figure>" + 
                "</div>";
        }
    }
}