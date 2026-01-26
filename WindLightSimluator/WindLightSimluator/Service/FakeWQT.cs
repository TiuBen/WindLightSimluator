using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindLightSimluator.Model;


namespace WindLightSimluator.Service
{


    public static class FakeWQTProvider
    {
        public static ObservableCollection<WTQR> Generate(DateTime startTime, TimeSpan duration, int intervalSeconds)
        {
            var list = new ObservableCollection<WTQR>();
            var rand = new Random();

            int count = (int)(duration.TotalSeconds / intervalSeconds);

            /* ========= 初始状态 ========= */
            float windSpeed = 5f;
            short windDir = 200;

            float qnh = 1015.0f;
            float qfeOffset = 8.0f;   // 场高简化
            float temperature = 18f;
            float dewPoint = 14f;
            float surfaceTemp = 17f;

            int visibility = 8000;
            int ceiling = 1500;

            int rvrBase = 1800;

            for (int i = 0; i < count; i++)
            {
                var time = startTime.AddSeconds(i * intervalSeconds);

                /* ========= 风 ========= */
                windSpeed += (float)(rand.NextDouble() * 2 - 1);   // ±1 m/s
                windSpeed = Math.Clamp(windSpeed, 0, 30);

                windDir += (short)rand.Next(-6, 7);
                windDir = (short)((windDir + 360) % 360);

                // 瞬时阵风 / 风切变
                if (rand.NextDouble() < 0.03)
                {
                    windSpeed += rand.Next(5, 9);
                    windDir += (short)rand.Next(-90, 91);
                }

                windSpeed = Math.Clamp(windSpeed, 0, 35);
                windDir = (short)((windDir + 360) % 360);

                /* ========= 气压 ========= */
                qnh += (float)(rand.NextDouble() * 0.3 - 0.15);

                if (rand.NextDouble() < 0.02)
                    qnh += rand.Next(-2, 3);   // 跳变

                qnh = Math.Clamp(qnh, 980, 1040);
                float qfe = qnh - qfeOffset;

                /* ========= 温度 / 露点 ========= */
                temperature += (float)(rand.NextDouble() * 0.2 - 0.1);
                surfaceTemp += (float)(rand.NextDouble() * 0.2 - 0.1);

                // 露点缓慢跟随
                dewPoint += (float)(rand.NextDouble() * 0.15 - 0.05);
                dewPoint = Math.Min(dewPoint, temperature - 0.1f);

                /* ========= 能见度 & 云底 ========= */
                bool fogRisk = (temperature - dewPoint) < 2.0;

                if (fogRisk && rand.NextDouble() < 0.4)
                {
                    visibility -= rand.Next(300, 1200);
                    ceiling -= rand.Next(100, 300);
                }
                else
                {
                    visibility += rand.Next(-200, 400);
                    ceiling += rand.Next(-50, 150);
                }

                visibility = Math.Clamp(visibility, 200, 10000);
                ceiling = Math.Clamp(ceiling, 50, 3000);

                /* ========= RVR（三段） ========= */
                rvrBase = visibility - rand.Next(0, 800);

                if (fogRisk && rand.NextDouble() < 0.3)
                    rvrBase -= rand.Next(300, 800);

                rvrBase = Math.Clamp(rvrBase, 150, 3000);

                int rvrStart = rvrBase + rand.Next(0, 200);
                int rvrMiddle = rvrBase + rand.Next(-100, 100);
                int rvrEnd = rvrBase - rand.Next(0, 200);

                rvrStart = Math.Clamp(rvrStart, 150, 3000);
                rvrMiddle = Math.Clamp(rvrMiddle, 150, 3000);
                rvrEnd = Math.Clamp(rvrEnd, 150, 3000);

                list.Add(new WTQR
                {
                    Time = time,
                    Qnh = qnh,
                    Qfe = qfe,

                    RvrStart = rvrStart,
                    RvrMiddle = rvrMiddle,
                    RvrEnd = rvrEnd,

                    visibility = visibility,
                    ceilingBase = ceiling,

                    Temperature = temperature,
                    Duepoint = dewPoint,
                    SurfaceTemperature = surfaceTemp,

                    Wind = new Wind
                    {
                        WindSpeed = windSpeed,
                        WindDir = windDir
                    }
                });
            }

            return list;
        }
    }
}




