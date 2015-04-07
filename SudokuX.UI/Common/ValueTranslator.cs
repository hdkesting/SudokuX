using System;
using System.ComponentModel;
using SudokuX.Solver.Support.Enums;

namespace SudokuX.UI.Common
{
    /// <summary>
    /// Translates between internal integer values and external visible string values for cells. 
    /// </summary>
    public class ValueTranslator
    {
        private readonly string _chars;
        private readonly int _max;

        public ValueTranslator(BoardSize size)
        {
            BoardSize = size;

            switch (size)
            {
                case BoardSize.Board4:
                    _chars = "♠♣♥♦";
                    _max = 3;
                    break;
                case BoardSize.Board6:
                case BoardSize.Board6Irregular:
                    _chars = "123456";
                    _max = 5;
                    break;
                case BoardSize.Board9:
                case BoardSize.Board9Irregular:
                case BoardSize.Board9X:
                    _chars = "123456789";
                    _max = 8;
                    break;
                case BoardSize.Board12:
                    _chars = "123456789ABC";
                    _max = 11;
                    break;
                case BoardSize.Board16:
                    _chars = "0123456789ABCDEF";
                    _max = 15;
                    break;
                default:
                    throw new InvalidEnumArgumentException("size", (int)size, typeof(BoardSize));
            }
        }

        public int MaxValue { get { return _max; } }

        public BoardSize BoardSize { get; private set; }

        /// <summary>
        /// Convert the 0-based value to a character.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public string ToChar(int? value)
        {
            if (!value.HasValue)
                return " ";

            if (value < 0 || value > _max)
                throw new ArgumentException(String.Format("Use a value between 0 and {0}, not {1}", _max, value), "value");
            return _chars[value.Value].ToString();
        }

        /// <summary>
        /// Converts the character value to a 0-based integer.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <returns>-1 if is was not a correct character</returns>
        public int ToInt(string character)
        {
            if (String.IsNullOrEmpty(character))
                return -1;

            return _chars.IndexOf(character, StringComparison.InvariantCulture);
        }
    }
}
