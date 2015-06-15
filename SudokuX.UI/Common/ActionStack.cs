using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace SudokuX.UI.Common
{
    /// <summary>
    /// A stack of <see cref="PerformedAction"/>s.
    /// </summary>
    internal class ActionStack
    {
        private readonly Stack<PerformedAction> _actions = new Stack<PerformedAction>();

        /// <summary>
        /// Gets a value indicating whether this stack has items.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this stack has items; otherwise, <c>false</c>.
        /// </value>
        public bool HasItems { get { return _actions.Count > 0; } }

        /// <summary>
        /// Gets the most recent action. Throws an error if there is none, so check <see cref="HasItems"/> first.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Stack is empty.</exception>
        public PerformedAction PopAction()
        {
            if (HasItems)
            {
                return _actions.Pop();
            }

            throw new InvalidOperationException("Stack is empty.");
        }

        /// <summary>
        /// Peeks at the most recent action.
        /// </summary>
        /// <returns></returns>
        private PerformedAction PeekAction()
        {
            return _actions.Peek();
        }

        /// <summary>
        /// Pushes the action, unless it undoes the previous one (in which case that one is removed instead).
        /// </summary>
        /// <param name="action">The action.</param>
        /// <exception cref="System.ArgumentNullException">action</exception>
        public void PushAction([NotNull] PerformedAction action)
        {
            if (action == null) throw new ArgumentNullException("action");

            if (HasItems)
            {
                // Did the user just undo his/her previous action?
                var top = PeekAction();
                if (top.Cell == action.Cell && top.IsValueSet != action.IsValueSet && top.IsRealValue == action.IsRealValue && top.IntValue == action.IntValue)
                {
                    PopAction(); // returns and discards "top"
                    return;
                }
            }

            _actions.Push(action);
        }
    }
}
