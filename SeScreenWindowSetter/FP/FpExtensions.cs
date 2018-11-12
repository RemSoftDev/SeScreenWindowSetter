using System;
using System.Collections.Generic;

namespace FP.SeScreenWindowSetter
{
    public static class FpCoomonExtensions
    {
        public static TRes
        PipeForward<T1, TRes>(this T1 a1, Func<T1, TRes> f) => f(a1);
        public static void
        PipeForward<T1>(this T1 a1, Action<T1> f) => f(a1);

        // Curry 2
        public static Func<T1, Func<T2, TRes>>
        Curry<T1, T2, TRes>(this Func<T1, T2, TRes> f) =>
           a1 => a2 => f(a1, a2);

        // Curry 3
        public static Func<T1, Func<T2, Func<T3, TRes>>>
        Curry<T1, T2, T3, TRes>(this Func<T1, T2, T3, TRes> f) =>
           a1 => a2 => a3 => f(a1, a2, a3);

        // Curry 4
        public static Func<T1, Func<T2, Func<T3, Func<T4, TRes>>>>
        Curry<T1, T2, T3, T4, TRes>(this Func<T1, T2, T3, T4, TRes> f) =>
           a1 => a2 => a3 => a4 => f(a1, a2, a3, a4);

        // Curry 5
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, TRes>>>>>
        Curry<T1, T2, T3, T4, T5, TRes>(this Func<T1, T2, T3, T4, T5, TRes> f) =>
           a1 => a2 => a3 => a4 => a5 => f(a1, a2, a3, a4, a5);

        public static IEnumerable<T>
        With<T>(this IEnumerable<T> collection, IEnumerable<T> withCollection)
        {
            var res = new List<T>(collection);
            res.AddRange(withCollection);
            return res;
        }
    }
}
