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

        // Wordt geretourneerd bij het getten van een ICommand gedefinieerd in een ViewModel
        // We retourneren een RelayCommand om controle te hebben in het ViewModel over hetgeen
        // uitgevoerd wordt en onder welke voorwaarde dat pas mag.
        // Args:
        // -Functie pointer naar het "te uitvoerene", doorgaans een functie binnen het ViewModel
        // -een predicaat welke aangeeft wat de conditie is waaraan voldaan dient te
        //  worden vooraleer de meegegeven functie uitgevoerd mag worden
        public RelayCommand(Action<object> execute, Predicate<object> canExecute) {
            if (execute == null) {
                throw new ArgumentNullException("Uit te voeren functie kan niet null zijn");
            }

            _uittevoeren = execute;
            _kanUitvoeren = canExecute;
        }

        // Wordt gebruikt door de XAML om te bepalen of het command uitgevoerd mag worden.
        [DebuggerStepThrough]
        public bool CanExecute(object parameters) {
            return _kanUitvoeren == null ? true : _kanUitvoeren(parameters);
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        // Indien het uitgevoerd mag worden zal de XAML deze oproepen dmv onderstaande.
        public void Execute(object parameters) {
            _uittevoeren(parameters);
        }

    }
}
