using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KenticoCloud.Delivery;

namespace DancingGoat.Helpers.Extensions
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Generates an IMG tag for an image file.
        /// </summary>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="assets">The asset sequence to display the asset from</param>
        /// <param name="index">Index position of the asset</param>
        /// <param name="title">Title</param>
        /// <param name="cssClass">CSS class</param>
        /// <param name="width">Optional width size</param>
        /// <param name="height">Optional height size</param>
        public static MvcHtmlString AssetImage(this HtmlHelper htmlHelper, IEnumerable<Asset> assets, int index, string title = null, string cssClass = "", int? width = null, int? height = null)
        {
            if (assets == null || !assets.Any() || assets.ElementAt(index) == null)
            {
                return MvcHtmlString.Empty;
            }

            var image = new TagBuilder("img");
            image.MergeAttribute("src", assets.ElementAt(index).Url);
            image.AddCssClass(cssClass);
            string titleToUse = title ?? assets.ElementAt(index).Name ?? string.Empty;
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
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="dateTime">The <see cref="DateTime"/> to format</param>
        /// <param name="format">The formatting character</param>
        public static MvcHtmlString DateTimeFormatted(this HtmlHelper htmlHelper, DateTime? dateTime, string format)
        {
            if (!dateTime.HasValue || string.IsNullOrEmpty(format))
            {
                return MvcHtmlString.Empty;
            }

            return MvcHtmlString.Create(dateTime.Value.ToString(format));
        }
    }
}