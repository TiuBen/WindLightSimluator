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
    /// KnobControl.xaml 的交互逻辑
    /// </summary>
    public partial class KnobControl : UserControl
    {
        public KnobControl()
        {
            InitializeComponent();
        }


        // =========================
        // Level 依赖属性
        // =========================
        public int Level
        {
            get => (int)GetValue(LevelProperty);
            set => SetValue(LevelProperty, value);
        }

        public static readonly DependencyProperty LevelProperty =
            DependencyProperty.Register(
                nameof(Level),
                typeof(int),
                typeof(KnobControl),
                new PropertyMetadata(0, OnLevelChanged));

        // =========================
        // Level变化
        // =========================
        private static void OnLevelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (KnobControl)d;
            int level = (int)e.NewValue;

            ctrl.UpdateKnob(level);
        }

        // =========================
        // 核心：控制旋钮角度
        // =========================
        private void UpdateKnob(int level)
        {
            double angle = level switch
            {
                0 => -90,
                1 => -54,
                2 => -23.336,
                3 => 29.138,
                4 => 54,
                5 => 90,
                _ => -90
            };

            KnobRotation.Angle = angle;
        }

        public ICommand ChangeLevelCommand
        {
            get => (ICommand)GetValue(ChangeLevelCommandProperty);
            set => SetValue(ChangeLevelCommandProperty, value);
        }

        public static readonly DependencyProperty ChangeLevelCommandProperty =
            DependencyProperty.Register(
                nameof(ChangeLevelCommand),
                typeof(ICommand),
                typeof(KnobControl),
                new PropertyMetadata(null));

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Level > 0)
                Level--;

            Apply();
        }

        private void Grid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Level < 5)
                Level++;

            Apply();
        }

        // =========================
        // 同步 UI + VM
        // =========================
        private void Apply()
        {
            UpdateKnob(Level);

            ChangeLevelCommand?.Execute(Level);
        }
    }
}
