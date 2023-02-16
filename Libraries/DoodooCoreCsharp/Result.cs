using System.Diagnostics.CodeAnalysis;

namespace DoodooCoreCsharp;

public readonly struct Result<TSuccess, TFailure>
    where TSuccess : notnull
    where TFailure : notnull
{
    private readonly Option<TSuccess> success;
    private readonly Option<TFailure> failure;

    public Result(Option<TSuccess> success, Option<TFailure> failure)
    {
        this.success = success;
        this.failure = failure;
    }

    public static Result<TSuccess, TFailure> Success(TSuccess value)
        => new Result<TSuccess, TFailure>(Option.Some(value), Option.None);
    
    public static Result<TSuccess, TFailure> Failure(TFailure value)
        => new Result<TSuccess, TFailure>(Option.None, Option.Some(value));

    public bool IsSuccess => success.IsSome;

    public bool IsFailure => failure.IsSome;

    public bool TryUnwrapSuccess([NotNullWhen(returnValue: true)] out TSuccess? value)
        => success.TryUnwrap(out value);
    
    public bool TryUnwrapFailure([NotNullWhen(returnValue: true)] out TFailure? value)
        => failure.TryUnwrap(out value);

    public static implicit operator Result<TSuccess, TFailure>(Result.UnspecifiedSuccess<TSuccess> unspecifiedSuccess)
        => Success(unspecifiedSuccess.Value);

    public static implicit operator Result<TSuccess, TFailure>(Result.UnspecifiedFailure<TFailure> unspecifiedFailure)
        => Failure(unspecifiedFailure.Value);
}

public static class Result
{
    public readonly ref struct UnspecifiedSuccess<TSuccess>
        where TSuccess : notnull
    {
        internal readonly TSuccess Value;
        internal UnspecifiedSuccess(TSuccess value) { Value = value; }
    }
        
    public readonly ref struct UnspecifiedFailure<TFailure>
        where TFailure : notnull
    {
        internal readonly TFailure Value;
        internal UnspecifiedFailure(TFailure value) { Value = value; }
    }

    public static UnspecifiedSuccess<TSuccess> Success<TSuccess>(TSuccess value) where TSuccess : notnull
        => new UnspecifiedSuccess<TSuccess>(value);

    public static UnspecifiedFailure<TFailure> Failure<TFailure>(TFailure value) where TFailure : notnull
        => new UnspecifiedFailure<TFailure>(value);
}