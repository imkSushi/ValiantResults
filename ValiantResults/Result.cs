using System.Diagnostics.CodeAnalysis;

namespace ValiantResults;

public class Result<TValue, TError>
{
    private TValue _value;
    private TError _error;
    private bool _success;
    
    public Result(TValue value)
    {
        _value = value;
        _success = true;
        _error = default!;
    }
    
    public Result(TError error)
    {
        _error = error;
        _success = false;
        _value = default!;
    }
    
    public bool IsSuccess() => _success;
    
    public bool IsFailure() => !_success;

    public bool IsSuccess([MaybeNullWhen(false)] out TValue value)
    {
        value = _value;
        return _success;
    }
    
    public bool IsFailure([MaybeNullWhen(false)] out TError error)
    {
        error = _error;
        return !_success;
    }
    
    public bool Deconstruct([MaybeNullWhen(false)] out TValue value, [MaybeNullWhen(true)] out TError error)
    {
        value = _value;
        error = _error;
        return _success;
    }
    
    public static implicit operator Result<TValue, TError>(TValue value) => new(value);
    public static implicit operator Result<TValue, TError>(TError error) => new(error);
    
    public static bool operator true(Result<TValue, TError> result) => result._success;
    public static bool operator false(Result<TValue, TError> result) => !result._success;
    
    public static Result<TValue, TError> operator |(Result<TValue, TError> left, Result<TValue, TError> right)
    {
        return left._success ? left : right;
    }
    
    public static Result<TValue, TError> operator &(Result<TValue, TError> left, Result<TValue, TError> right)
    {
        return left._success ? right : left;
    }
    
    public static explicit operator TValue(Result<TValue, TError> result)
    {
        if (!result._success)
            throw new InvalidCastException("Cannot cast a failed result to a value.");
        
        return result._value;
    }

    public static explicit operator TError(Result<TValue, TError> result)
    {
        if (result._success)
            throw new InvalidCastException("Cannot cast a successful result to an error.");
        
        return result._error;
    }
    
    public static Result<TValue, TError> Cast<TFromValue, TFromError>(Result<TFromValue, TFromError> result) where TFromValue : TValue where TFromError : TError
    {
        return result.Deconstruct(out var value, out var error) ? value : error;
    }
}