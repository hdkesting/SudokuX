using System.ComponentModel;
using System.Runtime.CompilerServices;
using SudokuX.UI.Annotations;

namespace SudokuX.UI.Common
{
    /// <summary>
    /// A <see cref="Count"/> of the number of cells yet to fill with this <see cref="Value"/>.
    /// </summary>
    public class ValueCount : INotifyPropertyChanged
    {
        private int _count;

        public string Value { get; private set; }

        public int Count
        {
            get { return _count; }
            set
            {
                if (value != _count)
                {
                    _count = value;
                    OnPropertyChanged();
                }

            }
        }

        public ValueCount(string value)
        {
            Value = value;
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
