using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.Model;

namespace WindLightSimluator.Model
{
   

    public class WindTempQnhStatisticBuffer
    {

        private readonly TimeSpan _window5Min = TimeSpan.FromMinutes(5);
        private readonly TimeSpan _window2Min = TimeSpan.FromMinutes(2);

        private readonly List<WTQR> _samples = new();

        public void AddSample(WTQR sample)
        {
            _samples.Add(sample);

            var cutoff = DateTime.Now - _window5Min;
            _samples.RemoveAll(s => s.Time < cutoff);
        }

        public WQTStatisticSnapshot BuildSnapshot()
        {
            var now = DateTime.Now;

            var samples2Min = _samples
                .Where(s => s.Time >= now - _window2Min)
                .ToList();

            var samples5Min = _samples
                .Where(s => s.Time >= now - _window5Min)
                .ToList();

            return new WQTStatisticSnapshot
            {
                Time = now,
                CurrentWQT = _samples.LastOrDefault(),

                //WindDirRange2Min = CalculateAngleRange(samples2Min),
                //WindDirRange5Min = CalculateAngleRange(samples5Min),

                //AvgWindSpeed2Min = CalculateAverageWindSpeed(samples2Min)
        };
        }


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

  
}
