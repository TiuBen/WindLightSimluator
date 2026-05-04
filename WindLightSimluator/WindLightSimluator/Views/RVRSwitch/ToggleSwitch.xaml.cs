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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WindLightSimluator.Views.RVRSwitch
{
    /// <summary>
    /// ToggleSwitch.xaml 的交互逻辑
    /// </summary>
    public partial class ToggleSwitch : UserControl
    {
        public ToggleSwitch()
        {
            InitializeComponent();
        }

        // IsChecked（UI状态）
        // =========================
        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(
                nameof(IsChecked),
                typeof(bool),
                typeof(ToggleSwitch),
                new PropertyMetadata(false, OnVisualChanged));

        // =========================
        // Command（MVVM核心）
        // =========================
        public ICommand ToggleCommand
        {
            get => (ICommand)GetValue(ToggleCommandProperty);
            set => SetValue(ToggleCommandProperty, value);
        }

        public static readonly DependencyProperty ToggleCommandProperty =
            DependencyProperty.Register(
                nameof(ToggleCommand),
                typeof(ICommand),
                typeof(ToggleSwitch),
                new PropertyMetadata(null));

        // =========================
        // ON / OFF Value（只用于显示）
        // =========================
        public string OnValue
        {
            get =>(string)GetValue(OnValueProperty);
            set => SetValue(OnValueProperty, value);
        }

        public static readonly DependencyProperty OnValueProperty =
            DependencyProperty.Register(nameof(OnValue), typeof(string), typeof(ToggleSwitch));

        public string OffValue
        {
            get => (string)GetValue(OffValueProperty);
            set => SetValue(OffValueProperty, value);
        }

        public static readonly DependencyProperty OffValueProperty =
            DependencyProperty.Register(nameof(OffValue), typeof(string), typeof(ToggleSwitch));



        // =========================
        // UI动画刷新
        // =========================
        private static void OnVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ToggleSwitch)d;

            double target = ctrl.IsChecked ? 35 : -35;

            var anim = new DoubleAnimation
            {
                To = target,
                Duration = TimeSpan.FromMilliseconds(150),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            ctrl.leverRotate.BeginAnimation(RotateTransform.AngleProperty, anim);
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            ToggleCommand?.Execute(true);
        }

        private void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            ToggleCommand?.Execute(false);
        }
    }
}
