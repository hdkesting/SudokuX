using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using SudokuX.Solver.Support.Enums;
using SudokuX.UI.Annotations;
using SudokuX.UI.Common.Enums;

namespace SudokuX.UI.Common
{
    public class Cell : INotifyPropertyChanged
    {
        private readonly int _maxval;
        private readonly ValueTranslator _translator;
        private readonly List<List<PencilValue>> _pencilRows;
        public event PropertyChangedEventHandler PropertyChanged;

        bool _readOnlyValue;
        int? _valueValue;
        bool _isValidValue = true;
        private Highlight _highlighted;
        private bool _isSelected;
        private Color _backColor = Colors.Transparent;
        private bool _showPencilMarks;
        private bool _hasValue;
        private bool _shouldShowPencilMarks;


        readonly ObservableCollection<string> _possibleValuesValue;

        public Cell(ValueTranslator translator, string tag)
        {
            Tag = tag;
            _translator = translator;
            _maxval = _translator.MaxValue;
            _possibleValuesValue = new ObservableCollection<string>();

            _pencilRows = new List<List<PencilValue>>();
            FillPencilValues();
        }

        private void FillPencilValues()
        {
            int w = _translator.BoardSize.BlockWidth();
            int h = _translator.BoardSize.BlockHeight();

            for (int y = 0; y < h; y++)
            {
                var row = new List<PencilValue>();
                _pencilRows.Add(row);
                for (int x = 0; x < w; x++)
                {
                    var val = new PencilValue(_translator.ToChar(y * w + x));
                    row.Add(val);
                }
            }
        }

        public void ResetPossibleValues()
        {
            _possibleValuesValue.Clear();

            for (int i = 0; i <= _maxval; i++)
            {
                _possibleValuesValue.Add(_translator.ToChar(i));
            }
        }

        public string Tag { get; set; }

        public BorderType BorderNorth { get; set; }
        public BorderType BorderSouth { get; set; }
        public BorderType BorderWest { get; set; }
        public BorderType BorderEast { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this cell is read only (given by the challenge).
        /// </summary>
        /// <value>
        /// <c>true</c> if this cell is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get
            {
                return _readOnlyValue;
            }
            set
            {
                if (_readOnlyValue != value)
                {
                    _readOnlyValue = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the integer version of this cell's value. Range from 0 to "max".
        /// </summary>
        /// <value>
        /// The int value.
        /// </value>
        public int? IntValue
        {
            get
            {
                return _valueValue;
            }
            set
            {
                if ((_valueValue ?? -1) != (value ?? -1))
                {
                    if ((value ?? -1) < 0)
                    {
                        // clear user-value
                        _valueValue = null;
                        HasValue = false;
                        Highlighted = Highlight.None;
                        if (ShouldShowPencilMarks) ShowPencilMarks = true;
                    }
                    else
                    {
                        // set user-value
                        _valueValue = value;
                        HasValue = true;
                    }
                    OnPropertyChanged();
                    OnPropertyChanged("StringValue");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this cell has a value.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this cell has a value; otherwise, <c>false</c>.
        /// </value>
        public bool HasValue
        {
            get { return _hasValue; }
            set
            {
                if (value != _hasValue)
                {
                    _hasValue = value;
                    OnPropertyChanged();
                    if (_hasValue)
                    {
                        ShowPencilMarks = false;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the string (symbol) version of this cell's value. This may be a digit, letter or other symbol.
        /// </summary>
        /// <value>
        /// The string value.
        /// </value>
        public string StringValue
        {
            get { return _translator.ToChar(IntValue); }
            set { IntValue = _translator.ToInt(value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this cell is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this cell is valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid
        {
            get
            {
                return _isValidValue;
            }
            set
            {
                if (_isValidValue != value)
                {
                    _isValidValue = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this cell is highlighted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this cell is highlighted; otherwise, <c>false</c>.
        /// </value>
        public Highlight Highlighted
        {
            get { return _highlighted; }
            set
            {
                if (_highlighted != value)
                {
                    _highlighted = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this cell is selected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this cell is selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the list of remaining possible values, as strings.
        /// </summary>
        /// <value>
        /// The possible values.
        /// </value>
        public ObservableCollection<string> PossibleValues
        {
            get
            {
                return _possibleValuesValue;
            }
        }

        public List<List<PencilValue>> PencilRows
        {
            get { return _pencilRows; }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Gets or sets the color of the block this cell belongs to.
        /// </summary>
        /// <value>
        /// The color of the block.
        /// </value>
        public Color BlockColor
        {
            get { return _backColor; }
            set
            {
                if (_backColor != value)
                {
                    _backColor = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool BelongsToSpecialGroup { get; set; }
        public int BlockOrdinal { get; set; }

        public Board Board { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this cell <em>should</em> show pencil marks.
        /// </summary>
        /// <remarks>The pencilmarks will still be hidden when the cell has a value.</remarks>
        /// <value>
        /// <c>true</c> if this should show pencil marks; otherwise, <c>false</c>.
        /// </value>
        public bool ShouldShowPencilMarks
        {
            get { return _shouldShowPencilMarks; }
            set
            {
                _shouldShowPencilMarks = value;
                ShowPencilMarks = value && !HasValue;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to really show pencil marks.
        /// </summary>
        /// <value>
        ///   <c>true</c> if pencil marks should be shown; otherwise, <c>false</c>.
        /// </value>
        public bool ShowPencilMarks
        {
            get { return _showPencilMarks; }
            set
            {
                if (value != _showPencilMarks)
                {
                    _showPencilMarks = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Sets the visibility of all pencil marks. They may however still be hidden if the cell has a value.
        /// </summary>
        public void UpdatePencilmarkStatus()
        {
            foreach (var row in PencilRows)
            {
                foreach (var value in row)
                {
                    value.Visible = PossibleValues.Contains(value.Value);
                }
            }

        }

        public bool HasPencilMark(string value)
        {
            foreach (var row in _pencilRows)
            {
                foreach (var pencilValue in row)
                {
                    if (pencilValue.Value == value)
                    {
                        return pencilValue.Visible;
                    }
                }
            }

            throw new InvalidOperationException("Unknown input value");
        }

        public void SetPencilMark(string value, bool visible, bool isExplicit)
        {
            foreach (var row in _pencilRows)
            {
                foreach (var pencilValue in row)
                {
                    if (pencilValue.Value == value)
                    {
                        if (isExplicit)
                            pencilValue.ExplicitlyVisible = visible ? default(bool?) : false;
                        else
                            pencilValue.Visible = visible;
                    }
                }
            }
        }
    }
}
