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

    public enum RunwayPart   
    {
        Start,
        Middle,
        End
    }

    public class RunwayColumnViewModel:ViewModelBase
    {
        public RunwayPart RunwayPart { get; }

        public RunwayStatusViewModel Status { get; } 
        public WindPanelViewModel Wind { get; } 
        public HeadCrossWindViewModel HeadCrossWind { get; } 
        public RvrVisViewModel RvrVis { get; } 
        public WeatherConditionViewModel Weather { get; }

        //public ICommand SelectCommand { get; }

        //private RunwayColumnState _state;
        //public RunwayColumnState State
        //{
        //    get { return _state; }
        //    set { _state = value; 
        //        OnPropertyChanged();
        //        OnPropertyChanged(nameof(IsActive));
        //        Status.IsActive = IsActive; // ⭐ 关键
        //    }

        //}

        //public bool IsActive => State == RunwayColumnState.Selected;



        //private readonly RunwayViewModel _parent;
        //public bool IsSelectable { get; }

        //public string RunwayNumber
        //{
        //    get => Status.RunwayNumber;
        //    set => Status.RunwayNumber = value;
        //}

        private bool? _isActive;
        public bool? IsActive
        {
            get => _isActive;
            set {
                _isActive = value;
                OnPropertyChanged();
            }
        }

        private RunwayViewModel _runway;
        public RunwayViewModel Runway
        {
            get { return _runway; }
            set {
                _runway = value;
            }
        }


        public RunwayColumnViewModel(RunwayViewModel parent ,RunwayPart part)
        {
            _runway = parent;
            RunwayPart = part;


            Status=new RunwayStatusViewModel(this,"rets");
            Wind = new WindPanelViewModel(this);
            HeadCrossWind = new HeadCrossWindViewModel(this);
            RvrVis = new RvrVisViewModel(this);
            Weather = new WeatherConditionViewModel(this);


            //IsSelectable = part != RunwayPart.Middle;
            //State = RunwayColumnState.Normal;

            //SelectCommand = new RelayCommand(
            //    () => _parent.OnColumnSelected(this),
            //    () => IsSelectable);
        }


    }
}
