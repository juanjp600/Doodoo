using DoodooCoreCsharp;

namespace FZH.Queries;

abstract record TypenameMatchFragment : QueryFragment {
    public override ValueString ToGraphQlQuery(ValueString selfFieldName) {
        var lines = new List<ValueString>();
        
        var firstLine = $"__typename ... on {((ValueString)GetType().Name).Replace(oldStr: "Fragment", newStr: "")} {{";
        lines.Add(firstLine);
        AddFields(lines);
        lines.Add("}");

        return string.Join(separator: "\n", values: lines);
    }
}
