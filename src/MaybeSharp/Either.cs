using System;
using System.Collections.Generic;

namespace MaybeSharp;

public interface IEither<TLeft, out TRight>
{
    IEither<TLeft, TResult> Bind<TResult>(Func<TRight, IEither<TLeft, TResult>> func);
}

public static class Either
{
    public static IEither<TLeft, TRight> Left<TLeft, TRight>(TLeft left)
    {
        return new LeftImpl<TLeft, TRight>(left);
    }

    public static IEither<TLeft, TRight> Right<TLeft, TRight>(TRight right)
    {
        return new RightImpl<TLeft, TRight>(right);
    }

    private sealed record LeftImpl<TLeft, TRight> : IEither<TLeft, TRight>
    {
        private readonly TLeft _left;

        public LeftImpl(TLeft left)
        {
            _left = left;
        }

        public IEither<TLeft, TResult> Bind<TResult>(Func<TRight, IEither<TLeft, TResult>> func)
        {
            return new LeftImpl<TLeft, TResult>(_left);
        }
    }

    private sealed record RightImpl<TLeft, TRight> : IEither<TLeft, TRight>
    {
        private readonly TRight _right;

        public RightImpl(TRight right)
        {
            _right = right;
        }

        public IEither<TLeft, TResult> Bind<TResult>(Func<TRight, IEither<TLeft, TResult>> func)
        {
            return func(_right);
        }
    }
}

public class Result<T>
{
    private readonly IEither<Exception, T> _value;

    public Result(IEither<Exception, T> value)
    {
        _value = value;
    }

    public Result(T right)
    {
        _value = Either.Right<Exception, T>(right);
    }

    public Result(Exception left)
    {
        _value = Either.Left<Exception, T>(left);
    }

    public Result<TResult> Bind<TResult>(Func<T, TResult> func)
    {
        return new Result<TResult>(_value.Bind(x => Either.Right<Exception, TResult>(func(x))));
    }
}

public class TypedResult<TError>
{
    public static IEither<TError, T> Success<T>(T value)
    {
        return Either.Right<TError, T>(value);
    }

    public static IEither<TError, T> Failure<T>(TError err)
    {
        return Either.Left<TError, T>(err);
    }
}

public class ValidationResult : TypedResult<IList<string>> {}
