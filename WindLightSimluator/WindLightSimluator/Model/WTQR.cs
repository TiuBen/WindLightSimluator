using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WindLightSimluator.Converters;
using WindLightSimluator.ViewModels.Base;
using WindLightSimluator.ViewModels;

namespace WindLightSimluator.Model
{
    public class Wind
    {
        
        private float _windSpeed;
        private short _windDir;
        private short _runwayDir;



        public float WindSpeed
        {
            get => (float)Math.Round(_windSpeed, 1);
            set => _windSpeed = value >= 0 ? value : throw new ArgumentException("风速不能为负数");
        }

        public short WindDir
        {
            get => (short)(Math.Round(_windDir / 5.0) * 5);
            set => _windDir = value >= 0 && value <= 360 ? value : throw new ArgumentException("风向必须在0-360度之间");
        }

        public Wind(float speed, short dir, short rwyDir)
        {
            ValidateRunwayDir(_runwayDir);
            WindSpeed = speed;
            WindDir = dir;
            _runwayDir = rwyDir;
        }


        private static float NormalizeAngle(float angle)
        {
            angle %= 360;
            if (angle > 180) angle -= 360;
            if (angle < -180) angle += 360;
            return angle;
        }


        public string HeadWindSpeed
        {
            get {
                float delta = NormalizeAngle(WindDir - _runwayDir);

                // 顶风分量 = V * cos(θ)
                // 正：顶风，负：顺风
                if (delta < 0)
                {

                    return $"-{Math.Round(WindSpeed * Math.Cos(delta * Math.PI / 180.0), 1)}";
                }
                return $"{Math.Round(WindSpeed * Math.Cos(delta * Math.PI / 180.0), 1)}";
            }
        }



        // 计算侧风分量（需要传入跑道方向）
        public string CrossWindSpeed
        {
            get {
                float delta = NormalizeAngle(WindDir - _runwayDir);

                // 侧风分量 = V * sin(θ)
                // 正：右侧风，负：左侧风
                double cw = WindSpeed * Math.Sin(delta * Math.PI / 180.0);
                string side = cw switch
                {
                    > 0 => "R", // Right
                    < 0 => "L", // Left
                    _ => "CALM"
                };

                return $"{side}{Math.Abs(cw).ToString()}";
            }

        }

        // 重写ToString，显示基本信息
        public override string ToString()
        {
            return $"Wind: {_windSpeed:F1}kt from {_windDir}°";
        }
        private void ValidateRunwayDir(short runwayDir)
        {
            if (runwayDir < 0 || runwayDir > 360)
                throw new ArgumentException("跑道方向必须在0-360度之间");
        }

    }

    public class RvrVis
    {
        private int _rvr = 2000;
        public string RvrValue
        {
            get
            {
                if (_rvr >= 2000)
                {
                    return $"P2000";
                }
                else
                {
                    return _rvr.ToString();
                }

            }
            set
            {   // 支持int赋值（通过字符串）
                if (int.TryParse(value, out int intValue))
                {
                    _rvr = intValue >= 0 ? intValue : 0;
                }
                else if (value?.StartsWith("P", StringComparison.OrdinalIgnoreCase) == true)
                {
                    _rvr = 2000;
                }
                else
                {
                    _rvr = 0;
                }
            }
        }

        private int _vis = 2000;
        public int VisValue
        {
            get { return _vis; }
            set { _vis = value; }
        }

    }

    public class Tempture
    {
        public float Temperature { get; init; }
        public float Duepoint { get; init; }
        public float SurfaceTemperature { get; init; }
    }

    public class WeatherCondition 
    {

        public string CloudFirstLayer { get; set; } = "NCD";

        public string Temperature { get; set; } = "14.4";

        public string VVIS { get; set; } = "";

        public string DewPoint { get; set; } = "1.4";

        public string Rain1h { get; set; } = "0.0";

        public string RelativeHumidity { get; set; } = "32";

        public string Rain24h { get; set; } = "0.0";

        public string QFE { get; set; } = "1017.2";

        public string STEMP { get; set; } = "22.2";

        public string Status { get; set; } = "Dry";

    }
    public class WTQR
    {
        public DateTime Time { get; init; }

        public float Qnh { get; init; }
        public float Qfe { get; init; }

        public Wind RvrStartWind { get; init; }
        public Wind RvrMiddleWind { get; init; }
        public Wind RvrEndWind { get; init; }

        public RvrVis RvrStartRvrVis { get; init; }
        public RvrVis RvrMiddleRvrVis { get; init; }
        public RvrVis RvrEndRvrVis { get; init; }

        public Tempture RvrStartTempture { get; init; }
        public Tempture RvrMiddleTempture { get; init; }
        public Tempture RvrEndRvrTempture { get; init; }

        public WeatherCondition RvrStartWeatherCondition { get; init; }
        public WeatherCondition RvrMiddleWeatherCondition { get; init; }
        public WeatherCondition RvrEndWeatherCondition { get; init; }


        public int visibility { get; init; }
        public int ceilingBase { get; init; }

      

    }


    /// <summary> 最终给 UI / 外部用的结构</summary>
    public class WQTStatisticSnapshot
    {
        /// <summary>实时 QNH 温度 风向风速</summary>
        public WTQR CurrentWQT { get; init; }

        public Wind Avg2Wind { get; init; }
        public Wind Min2Wind { get; init; }
        public Wind Max2Wind { get; init; }
        
        public DateTime Time { get; init; }
    }
}



//public float CalcHeadWind(short runwayDir)
//{
//    // 风向相对跑道的夹角
//    double delta = NormalizeAngle(WindDir - runwayDir);

//    // 顶风分量 = V * cos(θ)
//    // 正：顶风，负：顺风
//    return (float)(WindSpeed * Math.Cos(delta * Math.PI / 180.0));
//}
//public float CalcCrossWind(short runwayDir)
//{
//    double delta = NormalizeAngle(WindDir - runwayDir);

//    // 侧风分量 = V * sin(θ)
//    // 正：右侧风，负：左侧风
//    return (float)(WindSpeed * Math.Sin(delta * Math.PI / 180.0));
//}

//public (float value, string side) CalcCrossWindWithSide(short runwayDir)
//{
//    float cw = CalcCrossWind(runwayDir);

//    string side = cw switch
//    {
//        > 0 => "R", // Right
//        < 0 => "L", // Left
//        _ => "CALM"
//    };

//    return (Math.Abs(cw), side);
//}

//// 计算顶风分量（需要传入跑道方向）
//public double HeadWindSpeed()
//{

//    // 风向相对跑道的夹角
//    float delta = NormalizeAngle(WindDir - _runwayDir);

//    // 顶风分量 = V * cos(θ)
//    // 正：顶风，负：顺风
//    return Math.Round(WindSpeed * Math.Cos(delta * Math.PI / 180.0), 1);
//}