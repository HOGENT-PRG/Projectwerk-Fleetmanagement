using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFApp.Views.MVVM {
    public class RelayCommand : ICommand {

        readonly Action<object> _uittevoeren;
        readonly Predicate<object> _kanUitvoeren;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute) {
            if (execute == null)
                throw new ArgumentNullException("Uit te voeren functie kan niet null zijn");

            _uittevoeren = execute;
            _kanUitvoeren = canExecute;
        }


        [DebuggerStepThrough]
        public bool CanExecute(object parameters) {
            return _kanUitvoeren == null ? true : _kanUitvoeren(parameters);
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameters) {
            _uittevoeren(parameters);
        }

    }
}
