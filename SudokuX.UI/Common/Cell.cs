using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using SudokuX.UI.Annotations;

namespace SudokuX.UI.Common
{
    public class Cell : INotifyPropertyChanged
    {
        private readonly int _maxval;
        public event PropertyChangedEventHandler PropertyChanged;

        bool _readOnlyValue;
        int? _valueValue;
        bool _isValidValue = true;
        private bool _isHighlightd;
        private Color _backColor = Colors.DarkOliveGreen;
        private readonly ValueTranslator _translator;

        readonly ObservableCollection<string> _possibleValuesValue;

        public Cell(ValueTranslator translator)
        {
            _translator = translator;
            _maxval = _translator.MaxValue;
            _possibleValuesValue = new ObservableCollection<string>();
        }

        public void ResetPossibleValues()
        {
            _possibleValuesValue.Clear();

            Action<string> add = v =>
                {
                    if (!_possibleValuesValue.Contains(v))
                        _possibleValuesValue.Add(v);
                };

            add(String.Empty);
            //_possibleValuesValue.Add(String.Empty); // to clear the value
            for (int i = 0; i <= _maxval; i++)
            {
                //_possibleValuesValue.Add(_translator.ToChar(i));
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
                    }
                    else
                    {
                        _valueValue = value;
                    }
                    OnPropertyChanged();
                    OnPropertyChanged("StringValue");
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
    }
}
