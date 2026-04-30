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



        // 当前值（绑定 VM）
        public object IsOn
        {
            get => GetValue(IsOnProperty);
            set => SetValue(IsOnProperty, value);
        }

        public static readonly DependencyProperty IsOnProperty =
         DependencyProperty.Register(
             nameof(IsOn),
             typeof(object),
             typeof(ToggleSwitch),
             new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));





        public object OnValue
        {
            get => GetValue(OnValueProperty);
            set => SetValue(OnValueProperty, value);
        }

        public static readonly DependencyProperty OnValueProperty =
            DependencyProperty.Register(
                nameof(OnValue),
                typeof(object),
                typeof(ToggleSwitch),
                new PropertyMetadata(null, OnValueChanged));



        public object OffValue
        {
            get => GetValue(OffValueProperty);
            set => SetValue(OffValueProperty, value);
        }

        public static readonly DependencyProperty OffValueProperty =
            DependencyProperty.Register(
                nameof(OffValue),
                typeof(object),
                typeof(ToggleSwitch),
                new PropertyMetadata(null, OnValueChanged));



        // 👉 内部状态（bool）
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
                new PropertyMetadata(false));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ToggleSwitch)d;

            // 自动判断当前状态
            ctrl.IsChecked = Equals(ctrl.IsOn, ctrl.OnValue);

            if (Equals(ctrl.IsOn, ctrl.OnValue))
            {
                ctrl.leverRotate.Angle = 35;   // ON
            }
            else
            {
                ctrl.leverRotate.Angle = -35;  // OFF
            }

        }


        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            IsOn = OnValue; // 强制 ON
            double target = 35;
            var anim = new DoubleAnimation
            {
                To = target,
                Duration = TimeSpan.FromMilliseconds(150),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            leverRotate.BeginAnimation(RotateTransform.AngleProperty, anim);
        }

        private void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            IsOn = OffValue; // 强制 OFF
            double target = -35;
            var anim = new DoubleAnimation
            {
                To = target,
                Duration = TimeSpan.FromMilliseconds(150),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            leverRotate.BeginAnimation(RotateTransform.AngleProperty, anim);
        }
    }
}
