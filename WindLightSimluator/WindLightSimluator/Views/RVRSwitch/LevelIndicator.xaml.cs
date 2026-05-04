using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WindLightSimluator.Views.RVRSwitch
{
    /// <summary>
    /// LevelIndicator.xaml 的交互逻辑
    /// </summary>
    public partial class LevelIndicator : UserControl
    {
        public LevelIndicator()
        {
            InitializeComponent();
        }

        public int Level
        {
            get => (int)GetValue(LevelProperty);
            set => SetValue(LevelProperty, value);
        }

        public static readonly DependencyProperty LevelProperty =
                DependencyProperty.Register(
                    nameof(Level),
                    typeof(int),
                    typeof(LevelIndicator),
                    new PropertyMetadata(0, OnLevelChanged));

        // =========================
        // Level变化回调
        // =========================
        private static void OnLevelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (LevelIndicator)d;
            int level = (int)e.NewValue;

            ctrl.UpdateLamps(level);
        }

        // =========================
        // 核心：控制灯亮
        // =========================
        private void UpdateLamps(int level)
        {
            SetLamp(Lamp1, level == 1);
            SetLamp(Lamp2, level == 2);
            SetLamp(Lamp3, level == 3);
            SetLamp(Lamp4, level == 4);
            SetLamp(Lamp5, level == 5);
        }

        // =========================
        // 灯状态统一控制
        // =========================
        private void SetLamp(UIElement lamp, bool isOn)
        {
            if (lamp is System.Windows.Shapes.Ellipse el)
            {
                if (isOn)
                {
                    el.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 80)); // 亮绿
                    el.Opacity = 1.0;
                }
                else
                {
                    el.Fill = new SolidColorBrush(Color.FromRgb(34, 34, 34)); // 灰
                    el.Opacity = 0.4;
                }
            }
        }
    }
}
