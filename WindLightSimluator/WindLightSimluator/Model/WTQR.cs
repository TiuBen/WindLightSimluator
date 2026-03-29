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
using WindLightSimluator.Model;

namespace WindLightSimluator.Model
{
    public class UI_WQTR
    {
        public DateTime Time { get; init; }

        public float Temperature { get; set; }
        public float Duepoint { get; set; }
        public float SurfaceTemperature { get; set; }

        public string Qnh { get; set; }
        public string Qfe { get; set; }

        public string RvrStart { get; set; }
        public string RvrMiddle { get; set; }
        public string RvrEnd { get; set; }

        public string Visibility { get; set; }
        public string CeilingBase { get; set; }

        public string CurrentWindSpeed { get; set; }
        public string CurrentWindDir { get; set; }

        public string Min2WindSpeed { get; set; }
        public string Max2WindSpeed { get; set; }
        public string Avg2WindSpeed { get; set; }

        public string Min2WindDir { get; set; }
        public string Max2WindDir { get; set; }
        public string Avg2WindDir { get; set; }

        public string Min2HeadWindSpeed { get; set; }
        public string Max2HeadWindSpeed { get; set; }
        public string Avg2HeadWindSpeed { get; set; }

        public string Min5WindDir { get; set; }
        public string Max5WindDir { get; set; }

        public Wind Avg2Wind { get; set; }
        public Wind Min2Wind { get; set; }
        public Wind Max2Wind { get; set; }
    }

    public class WTQR
    {
        public DateTime Time { get; set; }

        public float Qnh { get; set; }
        public float Qfe { get; set; }

        public Wind? RwyStartWind { get; set; }
        public Wind? RwyMiddleWind { get; set; }
        public Wind? RwyEndWind { get; set; }

        public RvrVis? RwyStartRvrVis { get; set; }
        public RvrVis? RwyMiddleRvrVis { get; set; }
        public RvrVis? RwyEndRvrVis { get; set; }

        public WeatherCondition? RwyStartWeatherCondition { get; set; }
        public WeatherCondition? RwyMiddleWeatherCondition { get; set; }
        public WeatherCondition? RwyEndWeatherCondition { get; set; }

        public int ManualPV { get; set; }
        public int ManualPW { get; set; }
    }
}

/// <summary> 最终给 UI / 外部用的结构</summary>
//public class WQTStatisticSnapshot
//{
//    /// <summary>实时 QNH 温度 风向风速</summary>
//    public WTQR CurrentWQT { get; set; }

//    public WindViewModel Avg2Wind { get; set; }
//    public WindViewModel Min2Wind { get; set; }
//    public WindViewModel Max2Wind { get; set; }

//    public DateTime Time { get; set; }
//}



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


static public class WindTempQnhStatisticBuffer
{



    //private readonly TimeSpan _window5Min = TimeSpan.FromMinutes(5);
    //private readonly TimeSpan _window2Min = TimeSpan.FromMinutes(2);

    //private readonly List<WTQR> _samples = new();

    //public void AddSample(WTQR sample)
    //{
    //    _samples.Add(sample);

    //    var cutoff = DateTime.Now - _window5Min;
    //    _samples.RemoveAll(s => s.Time < cutoff);
    //}

    //public WQTStatisticSnapshot BuildSnapshot()
    //{
    //    var now = DateTime.Now;

    //    var samples2Min = _samples
    //        .Where(s => s.Time >= now - _window2Min)
    //        .ToList();

    //    var samples5Min = _samples
    //        .Where(s => s.Time >= now - _window5Min)
    //        .ToList();

    //    return new WQTStatisticSnapshot
    //    {
    //        Time = now,
    //        CurrentWQT = _samples.LastOrDefault(),

    //        //WindDirRange2Min = CalculateAngleRange(samples2Min),
    //        //WindDirRange5Min = CalculateAngleRange(samples5Min),

    //        //AvgWindSpeed2Min = CalculateAverageWindSpeed(samples2Min)
    //};
    //}


    //private static WindDirRange CalculateAngleRange(IEnumerable<WTQR> samples)
    //{
    //    if (samples == null)
    //        return new WindDirRange(0, 0);

    //    var angles = samples
    //        .Select(s => s.WindDir)
    //        .Distinct()
    //        .OrderBy(a => a)
    //        .ToList();

    //    if (angles.Count == 0)
    //        return new WindDirRange(0, 0);

    //    if (angles.Count == 1)
    //        return new WindDirRange(angles[0], angles[0]);

    //    // 最大空隙法（圆形角度）
    //    int count = angles.Count;
    //    int splitIndex = 0;
    //    int maxGap = -1;

    //    for (int i = 0; i < count; i++)
    //    {
    //        int current = angles[i];
    //        int next = angles[(i + 1) % count];

    //        int gap = (i == count - 1)
    //            ? (angles[0] + 360) - current
    //            : next - current;

    //        if (gap > maxGap)
    //        {
    //            maxGap = gap;
    //            splitIndex = (i + 1) % count;
    //        }
    //    }

    //    var start = angles[splitIndex];
    //    var end = angles[(splitIndex - 1 + count) % count];

    //    return new WindDirRange((short)start, (short)end);
    //}


    //private static float CalculateAverageWindSpeed(IEnumerable<WTQR> samples)
    //{
    //    var list = samples as IList<WTQR> ?? samples.ToList();

    //    if (list.Count == 0)
    //        return 0.8f;

    //    return (float)list.Average(s => s.WindSpeed);
    //}
}
