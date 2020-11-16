using Kentico.Kontent.Management.Helpers;
using Kentico.Kontent.Management.Helpers.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DancingGoat.TagHelpers
{
    [HtmlTargetElement("edit-link", Attributes = "element-ids,language")]
    public class EditLinkTagHelper : TagHelper
    {
        IEditLinkBuilder EditLinkBuilder { get; set; }

        [HtmlAttributeName("element-ids")]
        public ElementIdentifier[] ElementIdentifiers { get; set; }

        [HtmlAttributeName("language")]
        public string Language { get; set; }

        [HtmlAttributeName("inline")]
        public bool Inline { get; set; }

        public EditLinkTagHelper(IEditLinkBuilder editLinkBuilder)
        {
            EditLinkBuilder = editLinkBuilder;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var itemUrl = EditLinkBuilder.BuildEditItemUrl(Language, ElementIdentifiers);

            var style = Inline ? "inline" : "block";

            output.TagName = "a";
            output.Attributes.SetAttribute("target", "_blank");
            output.Attributes.SetAttribute("rel", "noopener");
            output.Attributes.SetAttribute("href", itemUrl);
            output.Attributes.Add("class", $"edit-link__overlay--{style}");
            output.TagMode = TagMode.StartTagAndEndTag;

            var btn = $@"<i aria-hidden=""true"" class=""edit-link__button-icon edit-link__button-icon--{style}""></i>";

            if (!Inline)
            {
                btn = $@"<span>{btn}</span>";
            }
            output.Content.SetHtmlContent(btn);
        }
    }
}
