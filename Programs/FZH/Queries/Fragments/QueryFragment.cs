using DoodooCoreCsharp;
using System.Reflection;

namespace FZH.Queries;

abstract record QueryFragment {
    public virtual ValueString ToGraphQlQuery(ValueString selfFieldName) {
        var lines = new List<ValueString>();

        var firstLine = selfFieldName;
        //var constructor = GetType().GetConstructors().MaxBy(c => c.GetParameters().Length);
        //var consParams = constructor?.GetParameters() ?? throw new Exception($"Malformed query");
        

        ValueString propToStr(PropertyInfo prop) {
            var value = prop.GetValue(this);
            return $"{((ValueString)prop.Name).LowerCamelCase()}: "
                + (value is string or ValueString
                    ? $"\"{value}\""
                    : value?.ToString() ?? "");
        }

        var t = GetType();
        
        var props = GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var propValues = props
            .Select(propToStr)
            .ToArray();
        if (propValues.Length > 0) {
            firstLine += $"({string.Join(separator: ", ", values: propValues)})";
        }
        firstLine += "{";
        lines.Add(firstLine);

        AddFields(lines);
        
        lines.Add("}");
        return string.Join(separator: "\n", values: lines);
    }

    protected void AddFields(List<ValueString> lines) {
        foreach (var field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance)) {
            var fieldValue = field.GetValue(this);
            if (fieldValue is QueryFragment subFragment) {
                string newLine = $"  ";
                newLine += subFragment.ToGraphQlQuery(((ValueString)field.Name).LowerCamelCase())
                    .Replace(oldStr: "\n", newStr: "\n  ");
                lines.Add(newLine);
            } else if (field.FieldType.IsArray
                       && field.FieldType.GetElementType()!.IsSubclassOf(typeof(QueryFragment))) {
                var temp = Activator.CreateInstance(field.FieldType.GetElementType()!) is QueryFragment newFragment
                    ? newFragment
                    : throw new Exception("Malformed query");
                string newLine = $"  ";
                newLine += temp.ToGraphQlQuery(((ValueString)field.Name).LowerCamelCase())
                    .Replace(oldStr: "\n", newStr: "\n  ");
                lines.Add(newLine);
            } else {
                lines.Add($"  {((ValueString)field.Name).LowerCamelCase()}");
            }
        }
    }
}
