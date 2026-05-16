using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using WindLightSimluator.Model;
using WindLightSimluator.Service;
using WindLightSimluator.utils;
using WindLightSimluator.ViewModels;
using WindLightSimluator.ViewModels.Base;
using WindLightSimluator.ViewModels.vm;

namespace WindLightSimluator.ViewModels
{
    public partial class AirportVM : ViewModelBase
    {
        //public ICommand StartSimulationCommand { get; }

        //public ICommand StopSimulationCommand { get; }

        public void start()
        {
            Debug.WriteLine(" 开始 开始 开始 练习");

            StopRandomSimulation();
            //var data = LoadDataFromTable(SelectedTableName);
            StartDatabaseSimulation(4);


        }
        public void stop()
        {
            Debug.WriteLine(" 停止 停止 停止 停止 停止");
            //PauseDatabaseSimulation();
            StopRandomSimulation();
        }


        private RelayCommand<object>? _startSimulationCommand;

        public RelayCommand<object> StartSimulationCommand
            => _startSimulationCommand ??=
                new RelayCommand<object>(
                    execute: _ =>
                    {
                        StartDatabaseSimulation();
                    },
                    canExecute: _ =>
                        CanStartSimulation);

        private RelayCommand<object>? _pauseSimulationCommand;

        public RelayCommand<object> PauseSimulationCommand
            => _pauseSimulationCommand ??=
                new RelayCommand<object>(
                    _ => PauseDatabaseSimulation(),
                    _ => CanPauseSimulation);

        private RelayCommand<object>? _resumeSimulationCommand;

        public RelayCommand<object> ResumeSimulationCommand
            => _resumeSimulationCommand ??=
                new RelayCommand<object>(
                    _ => ResumeDatabaseSimulation(),
                    _ => CanResumeSimulation);


        private RelayCommand _stopSimulationCommand;

        public RelayCommand StopSimulationCommand
            => _stopSimulationCommand ??=
                new RelayCommand(
                    _ => StopDatabaseSimulation(),
                    _ => CanStopSimulation);

        private void RefreshSimulationCommands()
        {
            _startSimulationCommand
                ?.RaiseCanExecuteChanged();

            _pauseSimulationCommand
                ?.RaiseCanExecuteChanged();

            _resumeSimulationCommand
                ?.RaiseCanExecuteChanged();

            _stopSimulationCommand
                ?.RaiseCanExecuteChanged();
        }



    }

}

//RunwayVM
//    3*WeatherConditionVM
//        CloudFirstLayer
//        Temperature********************
//        Duepoint
//        VVIS
//        Rain1h
//        RelativeHumidity
//        SurfaceTemperature
//        Rain24h
//        QFE
//        Status
//    3*RvrVisVM
//        RvrValue***********************                  
//        VisValue***********************
//    3*WindVM
//        WindSpeed**********************
//        WindDir************************
//    3*WindStatisticsVM
//        Avg2WindSpeed
//        Avg2WindDir
//        Max2WindSpeed
//        Min2WindSpeed
//        Avg2HeadWindSpeed
//        Avg2CrossWindSpeed