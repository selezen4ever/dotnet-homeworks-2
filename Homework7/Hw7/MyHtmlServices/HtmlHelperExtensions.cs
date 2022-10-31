using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hw7.MyHtmlServices;

public static class HtmlHelperExtensions
{
    public static IHtmlContent MyEditorForModel(this IHtmlHelper helper)
    {
        var modelType = helper.ViewData.ModelMetadata.ModelType;
        var form = modelType.GetProperties().Select(p => p.GetHtmlContentFromProperty(helper.ViewData.Model!));
        IHtmlContentBuilder view = new HtmlContentBuilder();
        return form.Aggregate(view, (builder, content) => builder.AppendHtml(content));
    }

    private static IHtmlContent GetHtmlContentFromProperty(this PropertyInfo info, object model)
    {
        var tag = new TagBuilder("div");
        tag.Attributes.Add("class", "form-group m-1");
        tag.InnerHtml.AppendHtml(GetPropertyLabel(info));
        tag.InnerHtml.AppendHtml(GetPropertyInput(info, model));
        tag.InnerHtml.AppendHtml(ValidateProperty(info, model)!);

        return tag;
    }
    
    private static IHtmlContent GetPropertyLabel(MemberInfo info)
    {
        var tag = new TagBuilder("label")
        {
            Attributes =
            {
                { "class", "form-label" },
                { "for", info.Name }
            }
        };
        tag.InnerHtml.AppendHtmlLine(GetPropertyName(info));
        return tag;
    }
    
    private static IHtmlContent GetPropertyInput(this PropertyInfo info, object model) =>
        info.PropertyType.IsEnum ? GetSelect(info, model) : GetInput(info, model);
    
    private static IHtmlContent GetInput(PropertyInfo info, object? model)
    {
        var tag = new TagBuilder("input")
        {
            Attributes =
            {
                { "class", "form-control" },
                { "name", info.Name },
                {"id", info.Name},
                { "type", info.PropertyType == typeof(int) ? "number" : "text" },
                { "value", model is not null ? info.GetValue(model)?.ToString() ?? "" : "" }
            }
        };
        return tag;
    }
    
    private static IHtmlContent GetSelect(PropertyInfo info, object? model)
    {
        var tag = new TagBuilder("select")
        {
            Attributes =
            {
                { "class", "form-control" },
                { "name", info.Name }
            }
        };
        var selectedValue = model is not null ? info.GetValue(model) : 0;
        var enumItems = info.PropertyType
            .GetFields(BindingFlags.Public | BindingFlags.Static);
        GetSelectOptionsList(enumItems, selectedValue);
        foreach (var option in GetSelectOptionsList(enumItems, selectedValue))
        {
            tag.InnerHtml.AppendHtml(option);
        }
        
        return tag;

    }
    
    private static List<IHtmlContent> GetSelectOptionsList(IEnumerable<FieldInfo> enumItems, object? selectedValue)
    {
        var tags = new List<IHtmlContent>();
        foreach (var item in enumItems)
        {
            var enumType = item.DeclaringType;
            var tag = new TagBuilder("option");
            tag.Attributes.Add("value", item.Name);
            if (item.GetValue(enumType)!.Equals(selectedValue))
                tag.Attributes.Add("selected", "true");
            tag.InnerHtml.AppendHtmlLine(GetPropertyName(item));
            tags.Add(tag);
        }

        return tags;
    }
    
    
    private static IHtmlContent? ValidateProperty(PropertyInfo info, object? model)
    {
        if (model == null)
            return null;

        var result = new List<ValidationResult>();
        var context = new ValidationContext(model)
        {
            DisplayName = GetPropertyName(info),
            MemberName = info.Name
        };
        return (Validator.TryValidateProperty(info.GetValue(model)!, context, result) || result.Count == 0)
            ? null
            : GetErrorSpan(result[0].ErrorMessage!, GetPropertyName(info));
    }
    
    private static IHtmlContent GetErrorSpan(string message, string propertyName)
    {
        var tag = new TagBuilder("span");
        tag.MergeAttributes(
            new Dictionary<string, string>
            {
                { "class", "text-danger col-form-label" },
                { "data-for", propertyName }
            }!
        );
        tag.InnerHtml.SetContent(message);
        return tag;
    }

    private static string GetPropertyName(MemberInfo propertyInfo)
    {
        return propertyInfo.GetCustomAttribute<DisplayAttribute>()?.Name ??
               Regex.Replace(propertyInfo.Name, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
    }
}