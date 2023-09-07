using System.Diagnostics.CodeAnalysis;

namespace ValiantResults;

public class Option<T>
{
    private T _value;
    private bool _hasValue;

    public Option(T value)
    {
        _value = value;
        _hasValue = true;
    }
    
    public Option()
    {
        _hasValue = false;
        _value = default!;
    }
    
    public bool HasValue() => _hasValue;
    public bool HasNoValue() => !_hasValue;
    
    public bool HasValue([MaybeNullWhen(false)] out T value)
    {
        value = _value;
        return _hasValue;
    }
    
    public static implicit operator Option<T>(T value) => new(value);
    public static implicit operator Option<T>(Option _) => new();

    public static explicit operator T(Option<T> value)
    {
        if (value._hasValue)
            return value._value;

        throw new InvalidCastException("Cannot cast Option.None to T");
    }
    
    public static bool operator true(Option<T> option) => option._hasValue;
    public static bool operator false(Option<T> option) => !option._hasValue;
    
    public static Option<T> operator |(Option<T> left, Option<T> right)
    {
        return left._hasValue ? left : right;
    }
    
    public static Option<T> operator &(Option<T> left, Option<T> right)
    {
        return left._hasValue ? right : left;
    }
    
    public static Option<T> None => new();

    public static Option<T> Cast<TFrom>(Option<TFrom> option) where TFrom : T
    {
        return option.HasValue(out var value) ? value : None;
    }
}

public class Option
{
    public static Option None => new();
}