using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Helpers;
using System.Web.Mvc;

namespace WebGridExtensions
{
    public static class WebGridExtensions
    {
        /// <summary>
        /// Create WebGridColumn from a single lambda expression
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="html"></param>
        /// <param name="exp">Lambda expression for column property.</param>
        /// <param name="format">Function used to format the data item that is associated with the WebGrid column.</param>
        /// <param name="style">The CSS class attribute that is rendered as part of the HTML table cells that are associated with the WebGrid column.</param>
        /// <param name="canSort">Indicates whether the column can be sorted.</param>
        /// <returns>Data column</returns>
        public static WebGridColumn GridColumnFor<TModel, TValue>(this HtmlHelper<IEnumerable<TModel>> html, Expression<Func<TModel, TValue>> exp, string header = null, Func<dynamic, object> format = null, string style = null, bool canSort = true)
        {
            // ModelMetadata from which other info is determined.
            var metadata = ModelMetadata.FromLambdaExpression(exp, new ViewDataDictionary<TModel>());
            // The full property being accessed by the lambda.
            var modelText = ExpressionHelper.GetExpressionText(exp);

            // No provided format but has DisplayFormatAttribute
            if (format == null && metadata.DisplayFormatString != null)
            {
                // Apply DisplayFormatAttribute formatting.
                format = (item) => string.Format(metadata.DisplayFormatString, item[modelText] ?? String.Empty);
            }

            return new WebGridColumn()
            {
                ColumnName = modelText,
                Header = header ?? metadata.DisplayName ?? metadata.PropertyName ?? modelText.Split('.').Last(),
                Style = style,
                CanSort = canSort,
                Format = format
            };
        }
    }
}
