using System;

namespace TreeLine.Sagas
{
    internal static class FuncExtensions
    {
        public static Func<T, TNextOut> Compose<T, TInitOut, TNextOut>(this Func<T, TInitOut> initial, Func<TInitOut, TNextOut> next)
        {
            return x => next(initial(x));
        }
    }
}