using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.Converters;

namespace WindLightSimluator.Model
{
    public class Wind
    {
        public float WindSpeed { get; set; }
        public Int16 WindDir { get; set; }

        private static double NormalizeAngle(double angle)
        {
            angle %= 360;
            if (angle > 180) angle -= 360;
            if (angle < -180) angle += 360;
            return angle;
        }
        public float CalcHeadWind(short runwayDir)
        {
            // 风向相对跑道的夹角
            double delta = NormalizeAngle(WindDir - runwayDir);

            // 顶风分量 = V * cos(θ)
            // 正：顶风，负：顺风
            return (float)(WindSpeed * Math.Cos(delta * Math.PI / 180.0));
        }
        public float CalcCrossWind(short runwayDir)
        {
            double delta = NormalizeAngle(WindDir - runwayDir);

            // 侧风分量 = V * sin(θ)
            // 正：右侧风，负：左侧风
            return (float)(WindSpeed * Math.Sin(delta * Math.PI / 180.0));
        }

        public (float value, string side) CalcCrossWindWithSide(short runwayDir)
        {
            float cw = CalcCrossWind(runwayDir);

            string side = cw switch
            {
                > 0 => "R", // Right
                < 0 => "L", // Left
                _ => "CALM"
            };

            return (Math.Abs(cw), side);
        }

    }

    public class WTQ
    {
        public DateTime Time { get; init; }

        public float Qnh { get; init; }
        
        public float Temperature { get; init; }

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
        public WTQ CurrentWQT { get; init; }



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
