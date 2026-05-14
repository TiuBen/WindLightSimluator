using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using WindLightSimluator.ViewModels.Base;

namespace WindLightSimluator.ViewModels
{
    public partial class AirportVM : ViewModelBase
    {
        // ============================================
        // 模拟相关字段
        // ============================================
        private DispatcherTimer _simTimer;
        private int _sensorTick = 0;
        private int _minuteIndex = 0;
        private int _elapsedSeconds = 0;

        private readonly Random _random = new();


        /// <summary>
        /// 使用数据库数据启动模拟
        /// </summary>
        public void StartDatabaseSimulation( int intervalSeconds = 5)
        {
            // 参数验证
            if (fakeData == null)
                throw new ArgumentNullException(nameof(fakeData));

            Random r = new();

            foreach (var runway in Runways)
            {
                runway.startPart.Offset = r.Next(0, 2);
                runway.middlePart.Offset = r.Next(2, 5);
                runway.endPart.Offset = r.Next(4, 6);

                InitPart(runway.startPart, r);
                InitPart(runway.middlePart, r);
                InitPart(runway.endPart, r);
            }

            _simTimer = new DispatcherTimer();
            _simTimer.Interval = TimeSpan.FromSeconds(intervalSeconds);

            _simTimer.Tick += (_, __) =>
            {
                UpdateAllRunways();

                _sensorTick++;
                //-----------------------------------
                // 时间累计
                //-----------------------------------

                _elapsedSeconds += 5;

                //-----------------------------------
                // 满60秒才切换数据库数据
                //-----------------------------------

                if (_elapsedSeconds >= 60)
                {
                    _elapsedSeconds = 0;

                    _minuteIndex++;

                    _minuteIndex %= 120;
                }
            };

            _simTimer.Start();


        }
        //====================================================
        // 初始化单个传感器
        //====================================================
        private void InitPart(RunwayPartVM part, Random r)
        {
            //// 风向扰动
            //part.WindDirNoise = r.NextDouble() * 4 - 2;

            //// 风速扰动
            //part.WindSpeedNoise = r.NextDouble() * 1.2 - 0.6;

            // 传感器延迟
            part.SensorDelay = r.Next(0, 3);

            // 不同步更新
            part.UpdatePhase = r.Next(0, 4);
        }
        // ============================================
        // 跑道更新
        // ============================================
        private void UpdateAllRunways()
        {
            foreach (var runway in Runways)
            {
                UpdatePart(runway.startPart );
                UpdatePart(runway.middlePart );
                UpdatePart(runway.endPart);
            }
        }
        //====================================================
        // 更新单个传感器
        //====================================================
        private void UpdatePart(RunwayPartVM part)
        {
            //-----------------------------------------
            // 不同步刷新
            //-----------------------------------------

            if ((_minuteIndex + part.UpdatePhase) % 2 != 0)
                return;

            // 原始数据
            //-----------------------------------------

            double baseDir = fakeData["WindDirection"][_minuteIndex];

            double baseSpeed = fakeData["WindSpeed"][_minuteIndex];

            //-----------------------------------------
            // 风向慢变化扰动
            //-----------------------------------------
            int baseAngleIndex = (int)(((baseDir + 5) % 360) / 10);
            // 在 ±1 个区间内轻微漂移
            int angleOffset = _random.Next(-1, 2);
            int finalIndex = (baseAngleIndex + angleOffset + 36) % 36;
            int finalDir = finalIndex * 10 + _random.Next(-3, 4);
            if (finalDir < 0) finalDir += 360;
            if (finalDir >= 360) finalDir -= 360;
            part.wind.WindDir = finalDir;
            //-----------------------------------------
            // 风速轻微波动（±1m/s）
            //-----------------------------------------
            double finalSpeed = baseSpeed + (_random.NextDouble() * 2 - 1);
            finalSpeed = Math.Max(0, finalSpeed);
            part.wind.WindSpeed = finalSpeed;

            //-----------------------------------------
            // 应用到UI
            //-----------------------------------------


            part.statistics.AddSample(finalSpeed, finalDir);

            // RVR
            part.rvrVis.Rvr = (int)fakeData["RVR"][_minuteIndex];

            // VIS
            part.rvrVis.VisValue = (int)fakeData["VIS"][_minuteIndex];

            // 温度
            part.weather.Temperature = fakeData["Temperature"][_minuteIndex];

            // QNH
            Qnh = (float)fakeData["QNH"][_minuteIndex];
        }



        public void PauseDatabaseSimulation() => _simTimer?.Stop();

        public void RestartDatabaseSimulation( int intervalSeconds = 5)
        {
            _simTimer?.Stop();
            _sensorTick = 0;
            _minuteIndex = 0;
            _elapsedSeconds = 0;
            StartDatabaseSimulation( intervalSeconds);
        }
    }
}
