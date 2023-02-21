using System.Collections;

namespace DoodooCoreCsharp;

public readonly struct ValueString : IReadOnlyList<char>
{
    private readonly string? valuePrivate;
    private ValueString(string? value)
    {
        this.valuePrivate = value;
    }

    public static implicit operator string(ValueString valueStr)
        => valueStr.Value;

    public static implicit operator ValueString(string? str)
        => new ValueString(str ?? "");

    public string Value
        => valuePrivate ?? "";

    public int Length
        => Value.Length;

    public IEnumerator<char> GetEnumerator()
        => Value.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    int IReadOnlyCollection<char>.Count
        => Length;

    public char this[int index]
        => Value[index];

    public ValueString this[Range range]
        => Value[range];

    public static ValueString operator+(ValueString a, ValueString b)
        => a.Value + b.Value;

    public override string ToString()
        => Value;

    public override bool Equals(object? obj)
        => obj switch
        {
            string str => Value.Equals(value: str, comparisonType: StringComparison.Ordinal),
            ValueString vStr => Value.Equals(value: vStr.Value, comparisonType: StringComparison.Ordinal),
            _ => false
        };

    public override int GetHashCode()
        => Value.GetHashCode();

    public ValueString Replace(ValueString oldStr, ValueString newStr)
        => Value.Replace(oldValue: oldStr, newValue: newStr);

    public ValueString LowerCamelCase()
        => Value.Length > 0
            ? char.ToLowerInvariant(Value[0]) + Value[1..]
            : "";
}