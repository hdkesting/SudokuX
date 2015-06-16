using System.ComponentModel;
using System.Runtime.CompilerServices;
using SudokuX.UI.Annotations;

namespace SudokuX.UI.Common
{
    /// <summary>
    /// A "possible value" to show on screen.
    /// </summary>
    public class PencilValue : INotifyPropertyChanged
    {
        private bool _visible;
        private bool? _explicitlyVisible;

        public string Value { get; private set; }

        public bool Visible
        {
            get { return ExplicitlyVisible ?? _visible; }
            set
            {
                if (value != _visible)
                {
                    _visible = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool? ExplicitlyVisible
        {
            get { return _explicitlyVisible; }
            set
            {
                _explicitlyVisible = value;

                // always act as if Visible has changed to the correct value
                OnPropertyChanged("Visible");
            }
        }

        public PencilValue(string value)
        {
            Value = value;
            _visible = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
