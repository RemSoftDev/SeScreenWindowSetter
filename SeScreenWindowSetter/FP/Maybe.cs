using System;

namespace SeScreenWindowSetter.FP
{
    public interface Maybe<T>
    {
        Maybe<TRes> Bind<TRes>(Func<T, Maybe<TRes>> f);
    }

    public class Nothing<T> : Maybe<T>
    {
        public override string ToString()
        {
            return "Nothing";
        }

        public Maybe<TRes> Bind<TRes>(Func<T, Maybe<TRes>> func)
        {
            return new Nothing<TRes>();
        }
    }

    public class Just<T> : Maybe<T>
    {
        private T Value { get; set; }
        public Just(T value)
        {
            Value = value;
        }

        public Maybe<TRes> Bind<TRes>(Func<T, Maybe<TRes>> f)
        {
            return Value != null ? f(Value) : new Nothing<TRes>();
        }
    }

    public static class MaybeExtensions
    {
        public static Maybe<T> ReturnMaybe<T>(this T value)
        {
            Maybe<T> res = new Nothing<T>();

            if (value != null)
            {
                res = new Just<T>(value);
            }

            return res;
        }

    }
}
