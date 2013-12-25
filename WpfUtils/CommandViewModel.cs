using System;
using System.Windows.Input;

namespace WpfUtils
{
    /// <summary>
    /// Represents an actionable item displayed by a View.
    /// </summary>
    public class CommandViewModel : ViewModelBase
    {
        public CommandViewModel(string displayName, ICommand command)
        {
            _displayName = displayName;

            if (command == null)
                throw new ArgumentNullException("command");
            Command = command;
        }

        private readonly string _displayName;
        public string DisplayName
        {
            get { return _displayName; }
        }

        public ICommand Command { get; private set; }
    }
}