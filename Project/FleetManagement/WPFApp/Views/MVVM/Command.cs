using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

<<<<<<< HEAD
// In de plaats van RelayCommand, bevat geen predicate en kan altijd uitvoeren
// Voor gebruik in wpf app is het wenselijk wel een predicate te hebben
=======
// In de plaats van RelayCommand, bevat geen predicate en kan altijd uitvoeren (L18)
// Voor gebruik in wpf app is het wenselijk wel een predicate te hebben (zie RelayCommand)
>>>>>>> parent of 87a59f3 (Fix requestDTOnaarDomein enum parsing, verplaatsen interface, RRNValideerder soft error, extra check bestuurdermgr, overbodige vpp files weg)
namespace WPFApp.Views.MVVM {
    internal sealed class Command : ICommand {
        private readonly Action<object> _action;

        public Command(Action<object> action) => _action = action;

        public void Execute(object parameter) => _action(parameter);

        public bool CanExecute(object parameter) => true;

        public event EventHandler CanExecuteChanged {
            add { }
            remove { }
        }
    }
}
