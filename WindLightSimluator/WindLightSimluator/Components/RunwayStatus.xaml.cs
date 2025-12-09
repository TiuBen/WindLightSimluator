using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace WindLightSimluator.Components
{
    public enum RwyState
    {
        InUse,
        NotInUse,
        Empty
    }
    public static class RwyStateExtensions
    {
        public static string ToStatusText(this RwyState state)
        {
            return state switch
            {
                RwyState.InUse => "LANDING/TAKE OFF",
                RwyState.NotInUse => "NOT IN USE",
                RwyState.Empty => "",
                _ => ""
            };
        }
    }

    /// <summary>
    /// RunwayNumber.xaml 的交互逻辑
    /// </summary>
    public partial class RunwayStatus : UserControl
    {
        public RunwayStatus()
        {
            InitializeComponent();
            this.DataContext = this;

            this.Height = RunwayFontSize * 2;
            UpdateStateColors();
            UpdateStatusText();

        }
        // 跑道号
        public string RunwayNumber
        {
            get => (string)GetValue(RunwayNumberProperty);
            set => SetValue(RunwayNumberProperty, value);
        }
        public static readonly DependencyProperty RunwayNumberProperty =
            DependencyProperty.Register(nameof(RunwayNumber), typeof(string), typeof(RunwayStatus), new PropertyMetadata("18R"));

        // 状态文本
        public string StatusText
        {
            get => (string)GetValue(StatusTextProperty);
            private set => SetValue(StatusTextPropertyKey, value);
        }
        private static readonly DependencyPropertyKey StatusTextPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(StatusText), typeof(string), typeof(RunwayStatus), new PropertyMetadata(""));

        public static readonly DependencyProperty StatusTextProperty =
            StatusTextPropertyKey.DependencyProperty;

        // 根据枚举更新 StatusText
        private void UpdateStatusText()
        {
            StatusText = RunwayState.ToStatusText();
        }


        // 跑道号字体大小（用于计算控件高度 = 字体高 * 2）
        public double RunwayFontSize
        {
            get => (double)GetValue(RunwayFontSizeProperty);
            set => SetValue(RunwayFontSizeProperty, value);
        }
        public static readonly DependencyProperty RunwayFontSizeProperty =
            DependencyProperty.Register(nameof(RunwayFontSize), typeof(double), typeof(RunwayStatus), new PropertyMetadata(24.0, OnRunwayFontSizeChanged));

        private static void OnRunwayFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RunwayStatus ctrl)
            {
                // 当字体大小变化时，自动把 Height 设置为 RunwayFontSize * 2（额外保留最小值）
                var size = (double)e.NewValue;
                ctrl.Height = Math.Max(ctrl.MinHeight, size * 1.5);
            }
        }

        // ===============================
        // 状态属性：Active / InActive
        // ===============================
        public RwyState RunwayState
        {
            get => (RwyState)GetValue(RunwayStateProperty);
            set => SetValue(RunwayStateProperty, value);
        }

        public static readonly DependencyProperty RunwayStateProperty =
            DependencyProperty.Register(nameof(RunwayState), typeof(RwyState), typeof(RunwayStatus),
                new PropertyMetadata(RwyState.InUse, OnRwyStateChanged));

        private static void OnRwyStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (RunwayStatus)d;
            ctrl.UpdateStateColors();
            ctrl.UpdateStatusText();
        }

        // ===============================
        // 更新颜色
        // ===============================
        public Brush RwyTextBg
        {
            get => (Brush)GetValue(RwyTextBgProperty);
            set => SetValue(RwyTextBgProperty, value);
        }
        public static readonly DependencyProperty RwyTextBgProperty =
            DependencyProperty.Register(nameof(RwyTextBg), typeof(Brush), typeof(RunwayStatus), new PropertyMetadata(null));

        public Brush RwyTextColor
        {
            get => (Brush)GetValue(RwyTextColorProperty);
            set => SetValue(RwyTextColorProperty, value);
        }
        public static readonly DependencyProperty RwyTextColorProperty =
            DependencyProperty.Register(nameof(RwyTextColor), typeof(Brush), typeof(RunwayStatus), new PropertyMetadata(null));


        public Brush RwyCapsuleBg
        {
            get => (Brush)GetValue(RwyCapsuleBgProperty);
            set => SetValue(RwyCapsuleBgProperty, value);
        }
        public static readonly DependencyProperty RwyCapsuleBgProperty =
            DependencyProperty.Register(nameof(RwyCapsuleBg), typeof(Brush), typeof(RunwayStatus), new PropertyMetadata(null));

        // ===============================
        // 根据状态读取 ResourceDictionary 中的颜色
        // ===============================
        private void UpdateStateColors()
        {
            string state = RunwayState == RwyState.InUse ? "InUse" : "NotInUse";



            string textBgKey = "RwyTextBg_" + state;
            string textColorKey = "RwyTextColor_" + state;
            string capsuleBgKey = "RwyCapsuleBg_" + state;

            RwyTextBg = (Brush)TryFindResource(textBgKey) ?? Brushes.Yellow;
            RwyTextColor = (Brush)TryFindResource(textColorKey) ?? Brushes.Blue;
            RwyCapsuleBg = (Brush)TryFindResource(capsuleBgKey) ?? Brushes.Green;
        }








    }

    // Converter：把输入乘以 Factor（可设置）
    public class MultiplyConverter : IValueConverter
    {
        public double Factor { get; set; } = 1.0;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
            {
                double factor = Factor;
                // 若传入 ConverterParameter（尝试解析为 double），支持覆盖或复杂表达
                if (parameter is string pStr && double.TryParse(pStr, out var pVal))
                    factor = pVal;
                return d * factor;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }


    public class EmptyToCollapsedConverter : IValueConverter
    {
        public object Convert(object value, Type t, object p, CultureInfo c)
        {
            string str = value as string;
            return string.IsNullOrWhiteSpace(str) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type t, object p, CultureInfo c) =>
            throw new NotImplementedException();
    }

    // HalfConverter：把输入 / 2（这里只实现为 IMultiValueConverter 以便 XAML 中 MultiBinding 调用）
    public class HalfConverter : IMultiValueConverter
    {
        // 取第一个 numeric 输入并除以 2
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length > 0 && values[0] is double d)
            {
                // CornerRadius 需要四个同值，这里返回一个 CornerRadius 实例
                var rad = d / 2.0;
                return new CornerRadius(rad);
            }
            return new CornerRadius(0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
