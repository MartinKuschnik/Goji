namespace Goji.TestApp
{
    using System;
    using System.Windows.Input;

    public class RelayCommand : ICommand
    {

        #region Events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region Fields

        readonly Action<object> execute;
        readonly Predicate<object> canExecute;

        #endregion // Fields

        #region Constructors

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            this.execute = execute;
            this.canExecute = canExecute;
        }

        #endregion

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null) CanExecuteChanged(this, EventArgs.Empty);
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }

        #endregion
    }
}
