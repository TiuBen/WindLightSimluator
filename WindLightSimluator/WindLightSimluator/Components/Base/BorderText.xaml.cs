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

namespace WindLightSimluator.Components.Base
{
    /// <summary>
    /// BorderText.xaml 的交互逻辑
    /// </summary>
    public partial class BorderText : UserControl
    {
        public BorderText()
        {
            InitializeComponent();
        }

        // ===== 主文字 =====
        public string MainText
        {
            get => (string)GetValue(MainTextProperty);
            set => SetValue(MainTextProperty, value);
        }
        public static readonly DependencyProperty MainTextProperty =
            DependencyProperty.Register(nameof(MainText), typeof(string), typeof(BorderText));

        // 主文字字体
        public double MainTextFontSize
        {
            get => (double)GetValue(MainTextFontSizeProperty);
            set => SetValue(MainTextFontSizeProperty, value);
        }
        public static readonly DependencyProperty MainTextFontSizeProperty =
            DependencyProperty.Register(nameof(MainTextFontSize), typeof(double), typeof(BorderText), new PropertyMetadata(20.0));

        // ===== 小字 =====
        public string SubText
        {
            get => (string)GetValue(SubTextProperty);
            set => SetValue(SubTextProperty, value);
        }
        public static readonly DependencyProperty SubTextProperty =
            DependencyProperty.Register(nameof(SubText), typeof(string), typeof(BorderText));

        // 小字体大小
        public double SubTextFontSize
        {
            get => (double)GetValue(SubTextFontSizeProperty);
            set => SetValue(SubTextFontSizeProperty, value);
        }
        public static readonly DependencyProperty SubTextFontSizeProperty =
            DependencyProperty.Register(nameof(SubTextFontSize), typeof(double), typeof(BorderText), new PropertyMetadata(12.0));

        // ===== 边框颜色 =====
        public Brush BorderColor
        {
            get => (Brush)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }
        public static readonly DependencyProperty BorderColorProperty =
            DependencyProperty.Register(nameof(BorderColor), typeof(Brush), typeof(BorderText), new PropertyMetadata(Brushes.Gray));

        // ===== 边框粗细 =====
        public Thickness BorderThicknessValue
        {
            get => (Thickness)GetValue(BorderThicknessValueProperty);
            set => SetValue(BorderThicknessValueProperty, value);
        }
        public static readonly DependencyProperty BorderThicknessValueProperty =
            DependencyProperty.Register(nameof(BorderThicknessValue), typeof(Thickness), typeof(BorderText), new PropertyMetadata(new Thickness(4)));

        // ===== 背景颜色 =====
        public Brush BackgroundColor
        {
            get => (Brush)GetValue(BackgroundColorProperty);


            set => SetValue(BackgroundColorProperty, value);
        }
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register(nameof(BackgroundColor), typeof(Brush), typeof(BorderText), new PropertyMetadata(Brushes.AliceBlue));

    }
}
