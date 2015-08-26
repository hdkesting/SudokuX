using System;
using System.Collections.Generic;

namespace SudokuX.Solver.Support
{
    /// <summary>
    /// Extension methods for <see cref="Stack{T}"/>
    /// </summary>
    public static class StackExtensions
    {
        /// <summary>
        /// Tries to pop the top items of the stack. Returns <c>default(T)</c> if empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stack">The stack.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">stack</exception>
        public static T TryPop<T>(this Stack<T> stack)
        {
            if (stack == null) throw new ArgumentNullException("stack");
            if (stack.Count == 0)
                return default(T);

            return stack.Pop();
        }
    }
}
