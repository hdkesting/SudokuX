using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using SudokuX.UI.Annotations;

namespace SudokuX.UI.Common
{
    public class PencilValue : INotifyPropertyChanged
    {
        private Visibility _visibility;
        public string Value { get; private set; }

        public Visibility Visibility
        {
            get { return _visibility; }
            set
            {
                if (value != _visibility)
                {
                    _visibility = value;
                    OnPropertyChanged();
                }
            }
        }

        public PencilValue(string value)
        {
            Value = value;
            Visibility = Visibility.Visible;
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
