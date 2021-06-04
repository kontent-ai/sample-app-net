using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq.Expressions;

using IHtmlContent = Microsoft.AspNetCore.Html.IHtmlContent;

namespace DancingGoat.Helpers.Extensions
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Displays a <see cref="DateTime"/> in a formatted manner.
        /// </summary>
        /// <param name="htmlHelper">HTML helper</param>
        /// <param name="expression">The expression of the model property</param>
        /// <param name="format">The formatting character</param>
        /// <remarks>The TValue generic parameter is chosen instead of DateTime just to save views from falling to exceptions. With TValue, the views will get rendered, only this helper method will return an empty <see cref="MvcHtmlString"/>.</remarks>
        public static IHtmlContent DateTimeFormattedFor<TModel>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, DateTime?>> expression, string format)
        {
            return htmlHelper.DisplayFor(expression, "DateTime", new DateTimeFormatterParameters { FormatCharacter = format });
        }
    }
}
