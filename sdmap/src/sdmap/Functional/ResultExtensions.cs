using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.Functional
{
    public static class ResultExtensions
    {
        public static Result<T> ToResult<T>(this Maybe<T> maybe, string errorMessage) where T : class
        {
            if (maybe.HasNoValue)
                return Result.Fail<T>(errorMessage);

            return Result.Ok(maybe.Value);
        }

        public static Result<K> OnSuccess<T, K>(this Result<T> result, Func<T, K> func)
        {
            if (result.IsFailure)
                return Result.Fail<K>(result.Error);

            return Result.Ok(func(result.Value));
        }

        public static Result<K> OnSuccess<T, K>(this Result<T> result, Func<K> func)
        {
            if (result.IsFailure)
                return Result.Fail<K>(result.Error);

            return Result.Ok(func());
        }

        public static Result<K> OnSuccess<T, K>(this Result<T> result, Func<T, Result<K>> func)
        {
            if (result.IsFailure)
                return Result.Fail<K>(result.Error);

            return func(result.Value);
        }

        public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, string errorMessage)
        {
            if (result.IsFailure)
                return result;

            if (!predicate(result.Value))
                return Result.Fail<T>(errorMessage);

            return result;
        }

        public static Result<K> Map<T, K>(this Result<T> result, Func<T, K> func)
        {
            if (result.IsFailure)
                return Result.Fail<K>(result.Error);

            return Result.Ok(func(result.Value));
        }

        public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> action)
        {
            if (result.IsSuccess)
            {
                action(result.Value);
            }

            return result;
        }

        public static Result<T> ExecWhen<T>(this Result<T> result, Func<T, bool> predicate, Action<T> action)
        {
            if (result.IsSuccess && predicate(result.Value))
            {
                action(result.Value);
            }

            return result;
        }

        public static T OnBoth<T>(this Result result, Func<Result, T> func)
        {
            return func(result);
        }

        public static Result OnSuccess(this Result result, Action action)
        {
            if (result.IsSuccess)
            {
                action();
            }

            return result;
        }

        public static Result<T> OnSuccess<T>(this Result result, Func<T> action)
        {
            if (result.IsSuccess)
            {
                return Result.Ok(action());
            }

            return Result.Fail<T>(result.Error);
        }

        public static Result<T> Unwrap<T>(this Result<Result<T>> result)
        {
            if (result.IsSuccess)
            {
                return result.Value;
            }

            return Result.Fail<T>(result.Error);
        }
    }
}
