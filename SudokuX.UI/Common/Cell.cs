using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using SudokuX.Solver.Support.Enums;
using SudokuX.UI.Annotations;

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
        private bool _isHighlighted;
        private bool _isSelected;
        private Color _backColor = Colors.Transparent;
        private bool _showPencilMarks;
        private bool _hasValue;


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
                        IsHighlighted = false;
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

        public string StringValue
        {
            get { return _translator.ToChar(IntValue); }
            set { IntValue = _translator.ToInt(value); }
        }

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

        public bool IsHighlighted
        {
            get { return _isHighlighted; }
            set
            {
                if (_isHighlighted != value)
                {
                    _isHighlighted = value;
                    OnPropertyChanged();
                }
            }
        }

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

        public Board Board { get; set; }

        private bool _shouldShowPencilMarks;
        public bool ShouldShowPencilMarks
        {
            get { return _shouldShowPencilMarks; }
            set
            {
                _shouldShowPencilMarks = value;
                ShowPencilMarks = value && !HasValue;
            }
        }

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

        public void UpdatePencilmarkStatus()
        {
            foreach (var row in PencilRows)
            {
                foreach (var value in row)
                {
                    value.Visibility = !HasValue && PossibleValues.Contains(value.Value)
                        ? Visibility.Visible
                        : Visibility.Hidden;
                }
            }

        }
    }
}
