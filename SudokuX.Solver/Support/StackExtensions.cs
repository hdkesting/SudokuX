using System;
using System.Collections.Generic;

namespace SudokuX.Solver.Support
{
    public static class StackExtensions
    {
        public static T TryPop<T>(this Stack<T> stack)
        {
            if (stack == null) throw new ArgumentNullException("stack");
            if (stack.Count == 0)
                return default(T);

            return stack.Pop();
        }
    }
}
