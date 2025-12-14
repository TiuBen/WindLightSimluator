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

        // ===== 背景颜色 =====
        public Brush BackgroundColor
        {
            get => (Brush)GetValue(BackgroundColorProperty);


            set => SetValue(BackgroundColorProperty, value);
        }
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register(nameof(BackgroundColor), typeof(Brush), typeof(BorderText), new PropertyMetadata(Brushes.AliceBlue));



        #region 
        //变色部分逻辑
        //public bool IsActive
        //{
        //    get => (bool)GetValue(IsActiveProperty);
        //    set => SetValue(IsActiveProperty, value);
        //}
        //public static readonly DependencyProperty IsActiveProperty =
        //    DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(BorderText),
        //        new PropertyMetadata(true, OnStateChanged));

        //public string Theme
        //{
        //    get => (string)GetValue(ThemeProperty);
        //    set => SetValue(ThemeProperty, value);
        //}
        //public static readonly DependencyProperty ThemeProperty =
        //    DependencyProperty.Register(nameof(Theme), typeof(string), typeof(BorderText),
        //        new PropertyMetadata("Day", OnStateChanged));

        //public string Mode
        //{
        //    get => (string)GetValue(ModeProperty);
        //    set => SetValue(ModeProperty, value);
        //}
        //public static readonly DependencyProperty ModeProperty =
        //    DependencyProperty.Register(nameof(Mode), typeof(string), typeof(BorderText),
        //        new PropertyMetadata("Normal", OnStateChanged));

        private static void OnStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if (d is BorderText bt)
            //{
            //    bt.ApplyState();
            //}
        }

        //private void ApplyState()
        //{
        //    // 例如："Day_Normal_Bg"
        //    string bgKey = $"{Theme}_{Mode}_Bg";
        //    string borderKey = $"{Theme}_{Mode}_Border";
        //    string textKey = $"{Theme}_{Mode}_Text";

        //    //string mainTextColor = "{Theme}_{Mode}_mainTextColor";



        //    BackgroundColor = TryFindResource(bgKey) as Brush ?? Brushes.Yellow;
        //    BorderColor = TryFindResource(borderKey) as Brush ?? Brushes.Red;

        //    // 主文字颜色
        //    // 你要不要也绑定文字颜色？下面是示例
        //    MainTextColor = TryFindResource(textKey) as Brush ?? Brushes.Blue;
        //    SubTextColor = TryFindResource(textKey) as Brush ?? Brushes.Blue;
        //    // 如果不 active，则淡化
        //    if (!IsActive)
        //    {
        //        BackgroundColor = Brushes.Gray;
        //        BorderColor = Brushes.DarkGray;
        //        MainTextColor = Brushes.LightGray;
        //        SubTextColor = Brushes.LightGray;

        //    }
        //}

        #endregion

    }
}
