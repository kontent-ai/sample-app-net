using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq.Expressions;
using System.Configuration;

using DancingGoat.Areas.Admin;
using DancingGoat.Models;

using KenticoCloud.ContentManagement.Helpers.Configuration;
using KenticoCloud.ContentManagement.Helpers;
using KenticoCloud.Delivery;
using KenticoCloud.ContentManagement.Helpers.Models;

namespace DancingGoat.Helpers.Extensions
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Generates an IMG tag for an image file.
        /// </summary>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="asset">The asset sequence to display the asset from</param>
        /// <param name="index">Index position of the asset</param>
        /// <param name="title">Title</param>
        /// <param name="cssClass">CSS class</param>
        /// <param name="width">Optional width size</param>
        /// <param name="height">Optional height size</param>
        public static MvcHtmlString AssetImage(this HtmlHelper htmlHelper, Asset asset, string title = null, string cssClass = "", int? width = null, int? height = null)
        {
            if (asset == null)
            {
                return MvcHtmlString.Empty;
            }

            var image = new TagBuilder("img");
            image.MergeAttribute("src", asset.Url);
            image.AddCssClass(cssClass);
            string titleToUse = title ?? asset.Description ?? string.Empty;
            image.MergeAttribute("alt", titleToUse);
            image.MergeAttribute("title", titleToUse);

            if (width.HasValue)
            {
                image.MergeAttribute("width", width.ToString());
            }

            if (height.HasValue)
            {
                image.MergeAttribute("height", height.ToString());
            }

            return MvcHtmlString.Create(image.ToString(TagRenderMode.SelfClosing));
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
                editor = html.EditorFor(expression, new { id = id }).ToString();
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

            var generatedHtml = string.Format(@"
<div class=""form-group"">
    <div class=""form-group-label"">{0}</div>
    <div class=""form-group-input"">{1}
       {2}
    </div>
    <div class=""message-validation"">{3}</div>
</div>", label, editor, explanationTextHtml, message);

            return MvcHtmlString.Create(generatedHtml);
        }

        public static MvcHtmlString StyledCheckBoxFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, bool>> expression, object htmlAttributes, string labelText)
        {
            var checkBox = html.CheckBoxFor(expression, htmlAttributes).ToString();
            var label = html.LabelFor(expression, labelText);
            var generatedHtml = string.Format(@"
<div class=""styled-checkbox"">
    {0}
    <span></span>
    {1}
</div>", checkBox, label);

            return MvcHtmlString.Create(generatedHtml);
        }

        public static MvcHtmlString StyledRadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, object value, object htmlAttributes, string labelText)
        {
            var radioButton = html.RadioButtonFor(expression, value, htmlAttributes).ToString();
            var label = html.LabelFor(expression, labelText, new { @class = "visible" });
            var generatedHtml = string.Format(@"
<div class=""styled-radio"">
    {0}
    <span></span>
    {1}
</div>", radioButton, label);

            return MvcHtmlString.Create(generatedHtml);
        }

        /// <summary>
        /// Returns a navigation button linked to Kentico Cloud's item suitable for block elements.
        /// </summary>
        /// <param name="language">Codename of language variant.</param>
        /// <param name="parentItemElementIdentifier">Identifier of parent item's element.</param>
        /// <param name="itemId">Target item's Id.</param>
        /// <param name="elementCodename">Target item elemnt's codename.</param>
        public static MvcHtmlString BlockEditLink(
            this HtmlHelper htmlHelper,
            string language,
            ElementIdentifier parentItemElementIdentifier,
            string itemId,
            string elementCodename
            )
        {
            var targetElementIdentifier = new ElementIdentifier(itemId, elementCodename);
            var itemUrl = GetItemElementUrl(language, parentItemElementIdentifier, targetElementIdentifier);

            var generatedHtml = string.Format(@"
<a target=""_blank"" class=""navigate-to-kc__overlay--block"" href=""{0}"" >
  <span class=""navigate-to-kc__button-link--block"">
      <i aria-hidden=""true"" class=""navigate-to-kc__button-icon--block""></i>
  </span>
</a>", itemUrl);

            return MvcHtmlString.Create(generatedHtml);
        }

        /// <summary>
        /// Returns a navigation button linked to Kentico Cloud's item suitable for inline elements.
        /// </summary>
        /// <param name="language">Codename of language variant.</param>
        /// <param name="parentItemElementIdentifier">Identifier of parent item's element.</param>
        /// <param name="itemId">Target item's Id.</param>
        /// <param name="elementCodename">Target item elemnt's codename.</param>
        public static MvcHtmlString InlineEditLink(
            this HtmlHelper htmlHelper, 
            string language, 
            ElementIdentifier parentItemElementIdentifier, 
            string itemId, 
            string elementCodename
            )
        {
            var targetElementIdentifier = new ElementIdentifier(itemId, elementCodename);
            var itemUrl = GetItemElementUrl(language, parentItemElementIdentifier, targetElementIdentifier);

            var generatedHtml = string.Format(@"
<a target=""_blank"" class=""navigate-to-kc__overlay--inline"" href=""{0}"">
        <i aria-hidden=""true"" class=""navigate-to-kc__button-icon--inline""></i>
</a>", itemUrl);

            return MvcHtmlString.Create(generatedHtml);
        }


        /// <summary>
        /// Displays Edit Mode Panel while using preview api.
        /// </summary>
        /// <param name="itemId">Id (guid) of content item identifier</param>
        /// <param name="language">Codename of language variant</param>
        public static MvcHtmlString EditPanel(this HtmlHelper htmlHelper, string itemId, string language)
        {
            bool isPreview = false;
            bool.TryParse(ConfigurationManager.AppSettings["UsePreviewApi"], out isPreview);

            if (isPreview)
            {
                var itemUrl = GetItemUrl(language, itemId);
                var editPanelViewModel = new EditPanelViewModel() { ItemUrl = itemUrl };
                htmlHelper.RenderPartial("EditModePanel", editPanelViewModel);
            }

            return MvcHtmlString.Create(string.Empty);
        }

        private static string GetItemUrl(string language, string itemId)
        {
            var projectId = AppSettingProvider.ProjectId.ToString();
            var linkBuilderOptions = new ContentManagementHelpersOptions() { ProjectId = projectId };
            var linkBuilder = new EditLinkBuilder(linkBuilderOptions);
            var editUrl = linkBuilder.BuildEditItemUrl(language, itemId);

            return editUrl;
        }

        private static string GetItemElementUrl(string language, params ElementIdentifier[] elementIdentifiers)
        {
            var projectId = AppSettingProvider.ProjectId.ToString();
            var linkBuilderOptions = new ContentManagementHelpersOptions() { ProjectId = projectId };
            var linkBuilder = new EditLinkBuilder(linkBuilderOptions);
            var editUrl = linkBuilder.BuildEditItemUrl(language, elementIdentifiers);

            return editUrl;
        }
    }
}