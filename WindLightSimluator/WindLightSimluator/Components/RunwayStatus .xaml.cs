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
            set => SetValue(StatusTextProperty, value);
        }
        public static readonly DependencyProperty StatusTextProperty =
            DependencyProperty.Register(nameof(StatusText), typeof(string), typeof(RunwayStatus), new PropertyMetadata("LANDING/TAKE OFF"));

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
