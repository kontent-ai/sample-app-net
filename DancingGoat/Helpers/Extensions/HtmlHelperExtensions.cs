using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using KenticoCloud.Delivery;
using System.Linq.Expressions;

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
            string titleToUse = title ?? asset.Name ?? string.Empty;
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
        /// <param name="dateTime">The <see cref="DateTime"/> to format</param>
        /// <param name="format">The formatting character</param>
        /// <remarks>The TValue generic parameter is chosen instead of DateTime just to save views from falling to exceptions. With TValue, the views will get rendered, only this helper method will return an empty <see cref="MvcHtmlString"/>.</remarks>
        public static MvcHtmlString DateTimeFormattedFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, string format)
        {
            if (expression.ReturnType != typeof(DateTime?) && expression.ReturnType != typeof(DateTime))
            {
                return MvcHtmlString.Empty;
            }

            return htmlHelper.DisplayFor(expression, "DateTime", new DateTimeFormatterParameters { FormatCharacter = format });
        }
    }
}