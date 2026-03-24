using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WindLightSimluator.Views.Components;
using WindLightSimluator.ViewModels.vm;
using System.Data;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels
{

   

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
            => _canExecute?.Invoke() ?? true;

        public void Execute(object? parameter)
            => _execute();

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }


    public enum RunwayColumnState
    {
        Normal,
        Selected,
        Disabled
    }

    public enum RunwayPartEnum   
    {
        Start,
        Middle,
        End
    }

 
  

}
