using System;
using System.ComponentModel;
using SudokuX.Solver.Support.Enums;

namespace SudokuX.UI.Common
{
    /// <summary>
    /// Translates between internal integer values (0-based) and external visible string values for cells. 
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
                    //_chars = "♠♣♥♦";
                    //_chars = "♈♋♉♌";
                    _chars = "🍔🍨🍰🍇";
                    _max = 3;
                    break;
                case BoardSize.Board6:
                    //_chars = "123456";
                    _chars = "🚀✈⛅⛄🐞🐘";
                    _max = 5;
                    break;
                case BoardSize.Irregular6:
                    _chars = "123456";
                    _max = 5;
                    break;
                case BoardSize.Board9:
                case BoardSize.Irregular9:
                case BoardSize.Board9X:
                case BoardSize.Hyper9:
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

            if (UnicodeLength(_chars) != _max + 1)
            {
                throw new InvalidOperationException("List of display characters not correct");
            }
        }

        public int MaxValue { get { return _max; } }

        public BoardSize BoardSize { get; private set; }

        private static int UnicodeLength(string value)
        {
            int idx = 0;
            int cnt = 0;

            while (idx < value.Length) // .Length is length in 16-bit chars
            {
                var len = Char.IsSurrogatePair(value, idx) ? 2 : 1;
                cnt += 1;
                idx += len;
            }

            return cnt;
        }

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

            // a simple string index will fail for UTF-32 codepoints.
            int idx = 0;
            int cnt = 0;
            while (idx < _chars.Length)
            {
                var len = Char.IsSurrogatePair(_chars, idx) ? 2 : 1;
                if (cnt == value.Value)
                {
                    return _chars.Substring(idx, len);
                }
                idx += len;
                cnt += 1;
            }

            throw new InvalidOperationException("Value not found");
        }

        /// <summary>
        /// Converts the character value to a 0-based integer.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <returns>-1 if is was not a correct character</returns>
        public int ToInt(string character)
        {
            if (String.IsNullOrWhiteSpace(character))
                return -1;

            int idx = 0;
            int cnt = 0;
            while (idx < _chars.Length)
            {
                var len = Char.IsSurrogatePair(_chars, idx) ? 2 : 1;
                var sub = _chars.Substring(idx, len);
                if (sub == character)
                {
                    return cnt;
                }
                idx += len;
                cnt += 1;
            }

            throw new InvalidOperationException("Character not found.");
        }
    }
}
