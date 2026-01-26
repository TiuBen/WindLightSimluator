using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WindLightSimluator.Converters;

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

    public class WTQR
    {
        public DateTime Time { get; init; }

        public float Qnh { get; init; }

        public float Qfe { get; init; }

        public int RvrStart { get; init; }
        public int RvrMiddle { get; init; }
        public int RvrEnd { get; init; }

        public int visibility { get; init; }
        public int ceilingBase { get; init; }

        public float Temperature { get; init; }
        public float Duepoint { get; init; }
        public float SurfaceTemperature { get; init; }

        public Wind Wind { get; init; }
    }

    public readonly struct WindDirRange
    {
        public Int16 Start { get; }
        public Int16 End { get; }

        /// <summary>
        /// 是否跨 0°（例如 350°–10°）
        /// </summary>
        public bool IsWrapped => Start > End;

        public WindDirRange(Int16 start, Int16 end)
        {
            Start = start;
            End = end;
        }

        public override string ToString()
            => IsWrapped
                ? $"{Start:0}° → 360° → {End:0}°"
                : $"{Start:0}° → {End:0}°";
    }




    /// <summary> 最终给 UI / 外部用的结构</summary>
    public class WQTStatisticSnapshot
    {
        /// <summary>实时 QNH 温度 风向风速</summary>
        public WTQR CurrentWQT { get; init; }

        public Wind Avg2Wind { get; init; }
        public Wind Min2Wind { get; init; }
        public Wind Max2Wind { get; init; }



        /// <summary>2 分钟风向范围（最小角 - 最大角）</summary>
        public WindDirRange WindDirRange2Min { get; init; }
        public int AvgWindDir2Min { get; }

        /// <summary>5 分钟风向范围,用在风盘上</summary>
        public WindDirRange WindDirRange5Min { get; init; }

        /// <summary>
        /// 两分钟内的最小平均风速
        /// </summary>
        public float MinAvgWindSpeed2Min { get; }
        /// <summary>
        /// 两分钟内的最大平均风速
        /// </summary>
        public float MaxAvgWindSpeed2Min { get; }

        /// <summary>2 分钟平均风速</summary>
        public float AvgWindSpeed2Min { get; init; }

        /// <summary>生成时间</summary>
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