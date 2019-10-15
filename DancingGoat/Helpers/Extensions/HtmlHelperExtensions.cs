using System;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using DancingGoat.Areas.Admin;
using DancingGoat.Models;

using KenticoCloud.ContentManagement.Helpers.Models;
using Kentico.Kontent.Delivery;
using Kentico.Kontent.Delivery.ImageTransformation;

namespace DancingGoat.Helpers.Extensions
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Generates an IMG tag for an image file.
        /// </summary>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="asset">Asset</param>
        /// <param name="title">Title</param>
        /// <param name="cssClass">CSS class</param>
        /// <param name="width">Optional width size</param>
        /// <param name="height">Optional height size</param>
        /// <param name="sizes">Media conditions mapping screen width to image size</param>
        public static MvcHtmlString AssetImage(this HtmlHelper htmlHelper, Asset asset, string title = null, string cssClass = "", int? width = null, int? height = null, ResponsiveImageSizes sizes = null)
        {
            if (asset == null)
            {
                return MvcHtmlString.Empty;
            }

            var imageUrlBuilder = new ImageUrlBuilder(asset.Url);
            var image = new TagBuilder("img");

            if (width.HasValue)
            {
                image.MergeAttribute("width", width.ToString());
                imageUrlBuilder = imageUrlBuilder.WithWidth(Convert.ToDouble(width));
            }

            if (height.HasValue)
            {
                image.MergeAttribute("height", height.ToString());
                imageUrlBuilder = imageUrlBuilder.WithHeight(Convert.ToDouble(height));
            }

            if (AppSettingProvider.ResponsiveImagesEnabled && !width.HasValue && !height.HasValue)
            {
                image.MergeAttribute("srcset", GenerateSrcsetValue(asset.Url));

                if (sizes != null)
                {
                    image.MergeAttribute("sizes", sizes.GenerateSizesValue());
                }
            }

            image.MergeAttribute("src", $"{imageUrlBuilder.Url}");
            image.AddCssClass(cssClass);
            string titleToUse = title ?? asset.Description ?? string.Empty;
            image.MergeAttribute("alt", titleToUse);
            image.MergeAttribute("title", titleToUse);

            return MvcHtmlString.Create(image.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Generates an IMG tag for an inline image.
        /// </summary>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="image">Inline image.</param>
        /// <param name="sizes">Media conditions mapping screen width to image size</param>
        public static MvcHtmlString InlineImage(this HtmlHelper htmlHelper, IInlineImage image, ResponsiveImageSizes sizes = null)
        {
            if (image == null)
            {
                return MvcHtmlString.Empty;
            }

            var imageTag = new TagBuilder("img");

            if (AppSettingProvider.ResponsiveImagesEnabled)
            {
                imageTag.MergeAttribute("srcset", GenerateSrcsetValue(image.Src));

                if (sizes != null)
                {
                    imageTag.MergeAttribute("sizes", sizes.GenerateSizesValue());
                }
            }

            imageTag.MergeAttribute("src", image.Src);
            imageTag.MergeAttribute("alt", image.AltText);

            return MvcHtmlString.Create(imageTag.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Displays a <see cref="DateTime"/> in a formatted manner.
        /// </summary>
        /// <param name="htmlHelper">HTML helper</param>
        /// <param name="expression">The expression of the model property</param>
        /// <param name="format">The formatting character</param>
        /// <remarks>The TValue generic parameter is chosen instead of DateTime just to save views from falling to exceptions. With TValue, the views will get rendered, only this helper method will return an empty <see cref="MvcHtmlString"/>.</remarks>
        public static MvcHtmlString DateTimeFormattedFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, DateTime?>> expression, string format)
        {
            return htmlHelper.DisplayFor(expression, "DateTime", new DateTimeFormatterParameters { FormatCharacter = format });
        }

        /// <summary>
        /// Returns an HTML input element with a label and validation fields for each property in the object that is represented by the <see cref="Expression"/> expression.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the displayed properties.</param>
        /// <param name="explanationText">An explanation text describing usage of the rendered field.</param>
        public static MvcHtmlString ValidatedEditorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string explanationText = "", string id = "")
        {
            string label;
            string editor;

            if (!string.IsNullOrEmpty(id))
            {
                label = html.LabelFor(expression, new { @for = id }).ToString();
                editor = html.EditorFor(expression, new { id }).ToString();
            }
            else
            {
                editor = html.EditorFor(expression).ToString();
                label = html.LabelFor(expression).ToString();
            }

            var message = html.ValidationMessageFor(expression).ToString();
            var explanationTextHtml = "";

            if (!string.IsNullOrEmpty(explanationText))
            {
                explanationTextHtml = "<div class=\"explanation-text\">" + explanationText + "</div>";
            }

            var generatedHtml = $@"
<div class=""form-group"">
    <div class=""form-group-label"">{label}</div>
    <div class=""form-group-input"">{editor}
       {explanationTextHtml}
    </div>
    <div class=""message-validation"">{message}</div>
</div>";

            return MvcHtmlString.Create(generatedHtml);
        }

        public static MvcHtmlString StyledCheckBoxFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, bool>> expression, object htmlAttributes, string labelText)
        {
            var checkBox = html.CheckBoxFor(expression, htmlAttributes).ToString();
            var label = html.LabelFor(expression, labelText);
            var generatedHtml = $@"
<div class=""styled-checkbox"">
    {checkBox}
    <span></span>
    {label}
</div>";

            return MvcHtmlString.Create(generatedHtml);
        }

        public static MvcHtmlString StyledRadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, object value, object htmlAttributes, string labelText)
        {
            var radioButton = html.RadioButtonFor(expression, value, htmlAttributes).ToString();
            var label = html.LabelFor(expression, labelText, new { @class = "visible" });
            var generatedHtml = $@"
<div class=""styled-radio"">
    {radioButton}
    <span></span>
    {label}
</div>";

            return MvcHtmlString.Create(generatedHtml);
        }

        /// <summary>
        /// Returns a navigation button linked to Kentico Kontent's item suitable for block elements.
        /// </summary>
        /// <param name="htmlHelper">HTML helper</param>
        /// <param name="language">Codename of language variant.</param>
        /// <param name="elementIdentifiers">Identifiers of hierarchy of content item.</param>
        public static MvcHtmlString BlockElementEditLink(
            this HtmlHelper htmlHelper,
            string language,
            params ElementIdentifier[] elementIdentifiers
            )
        {
            var itemUrl = GetItemElementUrl(language, elementIdentifiers);

            var generatedHtml = $@"
<a target=""_blank"" class=""edit-link__overlay--block"" href=""{itemUrl}"" >
  <span>
      <i aria-hidden=""true"" class=""edit-link__button-icon edit-link__button-icon--block""></i>
  </span>
</a>";

            return MvcHtmlString.Create(generatedHtml);
        }

        /// <summary>
        /// Returns a navigation button linked to Kentico Kontent's item suitable for inline elements.
        /// </summary>
        /// <param name="htmlHelper">HTML helper</param>
        /// <param name="language">Codename of language variant.</param>
        /// <param name="elementIdentifiers">Identifiers of hierarchy of content item.</param>
        public static MvcHtmlString InlineElementEditLink(
            this HtmlHelper htmlHelper, 
            string language, 
            params ElementIdentifier[] elementIdentifiers
            )
        {
            var itemUrl = GetItemElementUrl(language, elementIdentifiers);

            var generatedHtml = $@"
<a target=""_blank"" class=""edit-link__overlay--inline"" href=""{itemUrl}"">
    <i aria-hidden=""true"" class=""edit-link__button-icon edit-link__button-icon--inline""></i>
</a>";

            return MvcHtmlString.Create(generatedHtml);
        }


        /// <summary>
        /// Displays Edit Mode Panel while using preview api.
        /// </summary>
        /// <param name="htmlHelper">HTML helper</param>
        /// <param name="itemId">Id (guid) of content item identifier</param>
        /// <param name="language">Codename of language variant</param>
        public static void EditPanel(this HtmlHelper htmlHelper, string itemId, string language)
        {
            bool.TryParse(ConfigurationManager.AppSettings["UsePreviewApi"], out var isPreview);

            if (isPreview)
            {
                var itemUrl = GetItemUrl(language, itemId);
                var editPanelViewModel = new EditPanelViewModel() { ItemUrl = itemUrl };
                htmlHelper.RenderPartial("EditModePanel", editPanelViewModel);
            }
        }

        private static string GetItemUrl(string language, string itemId)
        {
            return EditLinkHelper.Instance.Builder.BuildEditItemUrl(language, itemId);
        }

        private static string GetItemElementUrl(string language, params ElementIdentifier[] elementIdentifiers)
        {
            return EditLinkHelper.Instance.Builder.BuildEditItemUrl(language, elementIdentifiers);
        }

        private static string GenerateSrcsetValue(string imageUrl)
        {
            var imageUrlBuilder = new ImageUrlBuilder(imageUrl);

            return string.Join(",", AppSettingProvider.ResponsiveWidths.Select(w
                =>$"{imageUrlBuilder.WithWidth(Convert.ToDouble(w)).Url} {w}w"));
        }
    }
}