using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace WindLightSimluator.Model
{
    public class WTQ
    {
        public DateTime Time { get; init; }

        public float Qnh { get; init; }

        private Int16 _angle;
        //风向规范通常是：0–359
        public Int16 WindDir
        {
            get => _angle;
            init => _angle = (short)((value % 360 + 360) % 360);
        }

        public float WindSpeed { get; init; }
        public float Temperature { get; init; }
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

        /// <summary>5 分钟风向范围</summary>
        public WindDirRange WindDirRange5Min { get; init; }

        /// <summary>2 分钟平均风速</summary>
        public float AvgWindSpeed2Min { get; init; }

        /// <summary>生成时间</summary>
        public DateTime Time { get; init; }
    }
}
