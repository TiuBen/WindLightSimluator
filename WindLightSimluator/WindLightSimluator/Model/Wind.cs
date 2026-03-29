using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindLightSimluator.Model
{
    public class Wind
    {

        private float _windSpeed;
        private short _windDir;
        private short _runwayDir;

        public DateTime time;

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
}
