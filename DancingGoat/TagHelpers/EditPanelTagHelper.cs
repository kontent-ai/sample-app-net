using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Management.Helpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace DancingGoat.TagHelpers
{
    [HtmlTargetElement("edit-panel", Attributes = "content-item-id,language")]
    public class EditPanelTagHelper : TagHelper
    {
        IEditLinkBuilder EditLinkBuilder { get; set; }

        public IOptionsMonitor<DeliveryOptions> DeliveryOptions { get; }

        [HtmlAttributeName("content-item-id")]
        public string ContentItemId { get; set; }

        [HtmlAttributeName("language")]
        public string Language { get; set; }

        public bool Inline { get; set; }

        public EditPanelTagHelper(IEditLinkBuilder editLinkBuilder, IOptionsMonitor<DeliveryOptions> deliveryOptions)
        {
            EditLinkBuilder = editLinkBuilder;
            DeliveryOptions = deliveryOptions;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("class", "edit-mode-panel");
            output.TagMode = TagMode.StartTagAndEndTag;
            if (DeliveryOptions.CurrentValue.UsePreviewApi)
            {
                var itemUrl = EditLinkBuilder.BuildEditItemUrl(Language, ContentItemId);
                string html = $@"
    <div>
        <div class=""edit-mode-panel__toggle"">
            <label class=""switch"">
                <input id=""edit-mode-switch"" type=""checkbox"" onchange=""toggleEditMode(this.checked)"" autocomplete=""off"">
                <span class=""slider round""></span>
            </label>
        </div>
        <div class=""edit-mode-panel__caption"">
            <span class=""edit-mode-panel__title"">Edit mode</span>
        </div>
    </div>
    <div class=""edit-mode-panel__navigate-link"">
        <a target=""_blank"" rel=""noopener"" href=""{itemUrl}"">Open content item in Kontent</a>
    </div>";
                output.Content.SetHtmlContent(html);

                string after = $@"
<script type=""text/javascript"">
    function toggleEditMode(isChecked) {{
        const body = document.getElementsByTagName('body')[0];
        isChecked ? body.classList.add('edit-mode') : body.classList.remove('edit-mode');
    }}
</script>";
                output.PostContent.SetHtmlContent(after);
            }
            else
            {
                output.SuppressOutput();
            }
        }
    }
}
