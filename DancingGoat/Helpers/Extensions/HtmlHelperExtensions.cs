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

        /// <summary>
        /// Returns an HTML input element with a label and validation fields for each property in the object that is represented by the <see cref="Expression"/> expression.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the displayed properties.</param>
        /// <param name="explanationText">An explanation text describing usage of the rendered field.</param>
        public static IHtmlContent ValidatedEditorFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string explanationText = "", string id = "")
        {
            IHtmlContent label;
            IHtmlContent editor;

            if (!string.IsNullOrEmpty(id))
            {
                label = html.LabelFor(expression, new { @for = id });
                editor = html.EditorFor(expression, new { id });
            }
            else
            {
                label = html.LabelFor(expression);
                editor = html.EditorFor(expression);
            }

            var explanationTextHtml = "";

            if (!string.IsNullOrEmpty(explanationText))
            {
                explanationTextHtml = "<div class=\"explanation-text\">" + explanationText + "</div>";
            }

            var builder = new HtmlContentBuilder();
            builder.AppendHtml(@"<div class=""form-group""><div class=""form-group-label"">");
            builder.AppendHtml(label);
            builder.AppendHtml(@"</div><div class=""form-group-input"">");
            builder.AppendHtml(editor);
            builder.AppendHtml(explanationTextHtml);
            builder.AppendHtml(@"</div><div class=""message-validation"">");
            builder.AppendHtml(html.ValidationMessageFor(expression));
            builder.AppendHtml(@"</div></div>");

            return builder;
        }
    }
}
