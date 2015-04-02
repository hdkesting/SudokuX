using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
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
        private bool _isHighlightd;
        private Color _backColor = Colors.Transparent;
        private bool _showPencilMarks;
        private bool _hasValue;


        readonly ObservableCollection<string> _possibleValuesValue;

        public Cell(ValueTranslator translator)
        {
            _translator = translator;
            _maxval = _translator.MaxValue;
            _possibleValuesValue = new ObservableCollection<string>();

            _pencilRows = new List<List<PencilValue>>();
            FillPencilValues();
        }

        private void FillPencilValues()
        {
            int w;
            int h;

            switch (_translator.MaxValue)
            {
                case 3: // 4x4
                    w = h = 2;
                    break;
                case 5: // 6x6
                    w = 3;
                    h = 2;
                    break;
                case 8: // 9x9
                    w = h = 3;
                    break;
                case 11: // 12x12
                    w = 4;
                    h = 3;
                    break;
                case 15: // 16x16
                    w = h = 4;
                    break;
                default:
                    throw new InvalidOperationException("Unknown max value: " + _translator.MaxValue);
            }

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

            Action<string> add = v =>
                {
                    //if (!_possibleValuesValue.Contains(v))
                    _possibleValuesValue.Add(v);
                };

            add(String.Empty);

            for (int i = 0; i <= _maxval; i++)
            {
                add(_translator.ToChar(i));
            }
        }

        public bool ReadOnly
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
                        _valueValue = null;
                        HasValue = false;
                    }
                    else
                    {
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
            get { return _isHighlightd; }
            set
            {
                if (_isHighlightd != value)
                {
                    _isHighlightd = value;
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
                    value.Visibility = PossibleValues.Contains(value.Value)
                        ? Visibility.Visible
                        : Visibility.Hidden;
                }
            }

        }
    }
}
