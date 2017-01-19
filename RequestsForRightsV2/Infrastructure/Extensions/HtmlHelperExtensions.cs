using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace RequestsForRights.Infrastructure.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString RequiredLabelFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var labelText = metaData.DisplayName ?? metaData.PropertyName ?? htmlFieldName.Split('.').Last();

            if (metaData.IsRequired)
                labelText += "<span class=\"rr-required-field\" aria-hidden=\"true\"></span>";

            if (string.IsNullOrEmpty(labelText))
                return MvcHtmlString.Empty;

            var label = new TagBuilder("label");
            label.Attributes.Add("for", 
                helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));

            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(htmlAttributes))
            {
                var value = prop.GetValue(htmlAttributes);
                if (value != null)
                    label.MergeAttribute(prop.Name.Replace('_', '-'), 
                        value.ToString(), true);
            }

            label.InnerHtml = labelText;
            return MvcHtmlString.Create(label.ToString());
        }
    }
}