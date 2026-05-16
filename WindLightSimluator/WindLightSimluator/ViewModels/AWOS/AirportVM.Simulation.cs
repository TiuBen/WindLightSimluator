using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using WindLightSimluator.Model;
using WindLightSimluator.ViewModels.Base;
using WindLightSimluator.ViewModels.vm;

namespace WindLightSimluator.ViewModels
{
    public partial class AirportVM : ViewModelBase
    {
        // ============================================
        // 模拟相关字段
        // ============================================
        private DispatcherTimer _simTimer;
        private int _minuteIndex = 0;
        private int _elapsedSeconds = 0;

        private readonly Random _random = new();


        /// <summary>
        /// 使用数据库数据启动模拟
        /// </summary>
        public void StartDatabaseSimulation(int intervalSeconds = 2)
        {
            Debug.WriteLine("使用数据库数据启动模拟");
            if (fakeData.Count == 0)
                return;

            if (_simTimer == null)
            {
                _simTimer = new DispatcherTimer
                {
                    Interval =
                        TimeSpan.FromSeconds(intervalSeconds)
                };

                _simTimer.Tick += Database_Timer_Tick;
            }

            _simTimer.Start();

            IsSimulationRunning = true;
            IsSimulationPaused = false;

            RefreshSimulationCommands();
        }

        private void Database_Timer_Tick(object? sender, EventArgs e)
        {
            if (fakeData.Count == 0)
                return;
            //

            // =====================================
            // 当前分钟基准数据
            // =====================================

            double baseDir = fakeData["WindDirection"][_minuteIndex];
            double baseSpeed = fakeData["WindSpeed"][_minuteIndex];

            // =====================================
            // 小范围随机扰动
            // =====================================
            var wind0 = CreateRandomWindData(baseDir, baseSpeed);
            var wind1 = CreateRandomWindData(baseDir, baseSpeed);
            var wind2 = CreateRandomWindData(baseDir, baseSpeed);
          
            // =====================================
            // 创建实时风
            // =====================================

            foreach (var runway in Runways)
            {
                // 更新实时风
                runway.startPart.Wind.WindDirValue = wind0.dir;
                runway.startPart.Wind.WindSpeedValue = wind0.speed;

                runway.middlePart.Wind.WindDirValue = wind1.dir;
                runway.middlePart.Wind.WindSpeedValue = wind1.speed;

                runway.endPart.Wind.WindDirValue = wind2.dir;
                runway.endPart.Wind.WindSpeedValue = wind2.speed;

                // 更新统计
                runway.startPart.Statistics.AddWindVM(new WindVM(wind0.speed, wind0.dir));
                runway.middlePart.Statistics.AddWindVM(new WindVM(wind1.speed, wind1.dir));
                runway.endPart.Statistics.AddWindVM(new WindVM(wind2.speed, wind2.dir));
            }

            if (_elapsedSeconds % 60 == 0)
            {
                double baseTemp = fakeData["Temperature"][_minuteIndex];
                double baseQnh = fakeData["QNH"][_minuteIndex];
                double baseRvr = fakeData["RVR"][_minuteIndex];
                double baseVis = fakeData["VIS"][_minuteIndex];

                //Debug.WriteLine("Temperature\t\t" + baseTemp);
                //Debug.WriteLine("QNH\t\t" + baseQnh);
                //Debug.WriteLine("RVR\t\t" + baseRvr);
                //Debug.WriteLine("VIS\t\t" + baseTemp);

                foreach (var runway in Runways)
                {
                    runway.startPart.Weather.Temperature = baseTemp;
                    runway.middlePart.Weather.Temperature = baseTemp;
                    runway.endPart.Weather.Temperature = baseTemp;

                    // RVR
                    runway.startPart.RvrVis.Rvr = (int)baseRvr;
                    runway.middlePart.RvrVis.Rvr = (int)baseRvr;
                    runway.endPart.RvrVis.Rvr = (int)baseRvr;
                    runway.startPart.RvrVis.Vis = (int)baseVis;
                    runway.middlePart.RvrVis.Vis = (int)baseVis;
                    runway.endPart.RvrVis.Vis = (int)baseVis;

                }
                Qnh = baseQnh;
            }





            // =====================================
            // 秒计数
            // =====================================

            _elapsedSeconds++;

            // 到下一分钟数据
            if (_elapsedSeconds >= 60)
            {
                _elapsedSeconds = 0;

                _minuteIndex++;

                // 循环播放
                if (_minuteIndex >= fakeData["WindDirection"].Count)
                {
                    _minuteIndex = 0;
                }
            }
        }


        private (double dir, double speed) CreateRandomWindData( double baseDir, double baseSpeed)
        {
            // 风向 ±4°
            double dir = baseDir + _random.NextDouble() * 20 - 10;

            // 风速 ±1m/s
            double speed = baseSpeed + _random.NextDouble() * 3 - 1.5;

            // 修正风向
            dir = (dir + 360) % 360;

            // 修正风速
            speed = Math.Max(0, speed);

            return (dir, speed);
        }



        public void StopDatabaseSimulation()
        {
            _simTimer?.Stop();

            _minuteIndex = 0;
            _elapsedSeconds = 0;

            IsSimulationRunning = false;
            IsSimulationPaused = false;

            RefreshSimulationCommands();
        }

        public void PauseDatabaseSimulation()
        {
            _simTimer?.Stop();

            IsSimulationPaused = true;

            RefreshSimulationCommands();
        }
        public void ResumeDatabaseSimulation()
        {
            _simTimer?.Start();

            IsSimulationPaused = false;

            RefreshSimulationCommands();
        }



        private bool _isSimulationRunning;

        public bool IsSimulationRunning
        {
            get => _isSimulationRunning;
            set {
                if (SetProperty(ref _isSimulationRunning, value))
                {
                    OnPropertyChanged(nameof(CanStartSimulation));
                    OnPropertyChanged(nameof(CanPauseSimulation));
                    OnPropertyChanged(nameof(CanResumeSimulation));
                    OnPropertyChanged(nameof(CanStopSimulation));
                }
            }
        }

        private bool _isSimulationPaused;

        public bool IsSimulationPaused
        {
            get => _isSimulationPaused;
            set {
                if (SetProperty(ref _isSimulationPaused, value))
                {
                    OnPropertyChanged(nameof(CanStartSimulation));
                    OnPropertyChanged(nameof(CanPauseSimulation));
                    OnPropertyChanged(nameof(CanResumeSimulation));
                    OnPropertyChanged(nameof(CanStopSimulation));
                }
            }
        }


        public bool CanStartSimulation => !string.IsNullOrWhiteSpace(SelectedTableName) && !IsSimulationRunning;
        public bool CanPauseSimulation => IsSimulationRunning && !IsSimulationPaused;
        public bool CanResumeSimulation => IsSimulationRunning && IsSimulationPaused;
        public bool CanStopSimulation => IsSimulationRunning;
    }
}
