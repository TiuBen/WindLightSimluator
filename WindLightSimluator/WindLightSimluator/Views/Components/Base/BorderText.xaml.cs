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

namespace WindLightSimluator.Views.Components.Base
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
            DependencyProperty.Register(nameof(MainText), typeof(string), typeof(BorderText), new PropertyMetadata("///"));

        // 主文字字体
        public double MainTextFontSize
        {
            get => (double)GetValue(MainTextFontSizeProperty);
            set => SetValue(MainTextFontSizeProperty, value);
        }
        public static readonly DependencyProperty MainTextFontSizeProperty =
            DependencyProperty.Register(nameof(MainTextFontSize), typeof(double), typeof(BorderText), new PropertyMetadata(20.0));


        public Brush MainTextColor
        {
            get => (Brush)GetValue(MainTextColorProperty);
            set => SetValue(MainTextColorProperty, value);
        }


        public static readonly DependencyProperty MainTextColorProperty =
             DependencyProperty.Register(nameof(MainTextColor), typeof(Brush), typeof(BorderText),
        new PropertyMetadata(Brushes.Black));

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
            DependencyProperty.Register(nameof(SubTextFontSize), typeof(double), typeof(BorderText), new PropertyMetadata(10.0));

        public Brush SubTextColor
        {
            get => (Brush)GetValue(SubTextColorProperty);
            set => SetValue(SubTextColorProperty, value);
        }

        public static readonly DependencyProperty SubTextColorProperty =
            DependencyProperty.Register(nameof(SubTextColor), typeof(Brush), typeof(BorderText), new PropertyMetadata(Brushes.Red));

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

        // 控制是否显示边框
        public bool IsBorderVisible
        {
            get => (bool)GetValue(IsBorderVisibleProperty);
            set => SetValue(IsBorderVisibleProperty, value);
        }
        public static readonly DependencyProperty IsBorderVisibleProperty =
            DependencyProperty.Register(nameof(IsBorderVisible), typeof(bool), typeof(BorderText), new PropertyMetadata(true, OnIsBorderVisibleChanged));

        // 边框圆角（胶囊效果）
        public CornerRadius BorderCornerRadius
        {
            get => (CornerRadius)GetValue(BorderCornerRadiusProperty);
            set => SetValue(BorderCornerRadiusProperty, value);
        }
        public static readonly DependencyProperty BorderCornerRadiusProperty =
            DependencyProperty.Register(nameof(BorderCornerRadius), typeof(CornerRadius), typeof(BorderText), new PropertyMetadata(new CornerRadius(20)));


        // 控制边框显示时的逻辑
        private static void OnIsBorderVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (BorderText)d;
            bool isVisible = (bool)e.NewValue;

            // 控制边框的显示与圆角
            if (isVisible)
            {
                control.BorderCornerRadius = new CornerRadius(20); // 胶囊形状
                control.mainBorder.Visibility = Visibility.Visible;
            }
            else
            {
                control.BorderCornerRadius = new CornerRadius(0); // 普通矩形
                control.BorderThicknessValue =new Thickness(0);
            }
        }




        // ===== 背景颜色 =====
        public Brush BackgroundColor
        {
            get => (Brush)GetValue(BackgroundColorProperty);


            set => SetValue(BackgroundColorProperty, value);
        }
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register(nameof(BackgroundColor), typeof(Brush), typeof(BorderText), new PropertyMetadata(Brushes.AliceBlue));


        public FontWeight MainTextFontWeight
        {
            get => (FontWeight)GetValue(MainTextFontWeightProperty);
            set => SetValue(MainTextFontWeightProperty, value);
        }

        public static readonly DependencyProperty MainTextFontWeightProperty =
            DependencyProperty.Register(
                nameof(MainTextFontWeight),
                typeof(FontWeight),
                typeof(BorderText),
                new PropertyMetadata(FontWeights.Normal));


    }
}
