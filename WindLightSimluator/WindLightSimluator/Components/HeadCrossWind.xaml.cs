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
using WindLightSimluator.Components.Base;

namespace WindLightSimluator.Components
{
    public partial class HeadCrossWind : UserControl,IStateAware
    {
        public HeadCrossWind()
        {
            InitializeComponent();
        }

        // 依赖属性示例
        public static readonly DependencyProperty HeadWindAVG2TextProperty =
            DependencyProperty.Register(nameof(HeadWindAVG2Text), typeof(string), typeof(HeadCrossWind), new PropertyMetadata("///"));

        public string HeadWindAVG2Text
        {
            get => (string)GetValue(HeadWindAVG2TextProperty);
            set => SetValue(HeadWindAVG2TextProperty, value);
        }

        public static readonly DependencyProperty CrossWindAVG2TextProperty =
            DependencyProperty.Register(nameof(CrossWindAVG2Text), typeof(string), typeof(HeadCrossWind), new PropertyMetadata("///"));

        public string CrossWindAVG2Text
        {
            get => (string)GetValue(CrossWindAVG2TextProperty);
            set => SetValue(CrossWindAVG2TextProperty, value);
        }

        //wd mnm2
        public static readonly DependencyProperty WD_NNM2TextProperty =
            DependencyProperty.Register(nameof(WD_NNM2Text), typeof(string), typeof(HeadCrossWind), new PropertyMetadata("///"));

        public string WD_NNM2Text
        {
            get => (string)GetValue(WD_NNM2TextProperty);
            set => SetValue(WD_NNM2TextProperty, value);
        }
        //wd mnm2


        public static readonly DependencyProperty WS_NNM2TextProperty =
            DependencyProperty.Register(nameof(WS_NNM2Text), typeof(string), typeof(HeadCrossWind), new PropertyMetadata("///"));

        public string WS_NNM2Text
        {
            get => (string)GetValue(WS_NNM2TextProperty);
            set => SetValue(WS_NNM2TextProperty, value);
        }

        public static readonly DependencyProperty WD_AVG2TextProperty =
            DependencyProperty.Register(nameof(WD_AVG2Text), typeof(string), typeof(HeadCrossWind), new PropertyMetadata("///"));

        public string WD_AVG2Text
        {
            get => (string)GetValue(WD_AVG2TextProperty);
            set => SetValue(WD_AVG2TextProperty, value);
        }

        public static readonly DependencyProperty WS_AVG2TextProperty =
            DependencyProperty.Register(nameof(WS_AVG2Text), typeof(string), typeof(HeadCrossWind), new PropertyMetadata("///"));

        public string WS_AVG2Text
        {
            get => (string)GetValue(WS_AVG2TextProperty);
            set => SetValue(WS_AVG2TextProperty, value);
        }

        public static readonly DependencyProperty WD_MAX2TextProperty =
            DependencyProperty.Register(nameof(WD_MAX2Text), typeof(string), typeof(HeadCrossWind), new PropertyMetadata("///"));

        public string WD_MAX2Text
        {
            get => (string)GetValue(WD_MAX2TextProperty);
            set => SetValue(WD_MAX2TextProperty, value);
        }

        public static readonly DependencyProperty WS_MAX2TextProperty =
            DependencyProperty.Register(nameof(WS_MAX2Text), typeof(string), typeof(HeadCrossWind), new PropertyMetadata("///"));

        public string WS_MAX2Text
        {
            get => (string)GetValue(WS_MAX2TextProperty);
            set => SetValue(WS_MAX2TextProperty, value);
        }

        // 你也可以加方法快速设置
        public void SetHeadWindAVG2(string value) => HeadWindAVG2Text = value;
        public void SetCrossWindAVG2(string value) => CrossWindAVG2Text = value;
        // 其他类似





        //变色部分逻辑
        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(HeadCrossWind),
                new PropertyMetadata(true, OnStateChanged));

        public string Theme
        {
            get => (string)GetValue(ThemeProperty);
            set => SetValue(ThemeProperty, value);
        }
        public static readonly DependencyProperty ThemeProperty =
            DependencyProperty.Register(nameof(Theme), typeof(string), typeof(HeadCrossWind),
                new PropertyMetadata("Day", OnStateChanged));

        public string Mode
        {
            get => (string)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register(nameof(Mode), typeof(string), typeof(HeadCrossWind),
                new PropertyMetadata("Normal", OnStateChanged));

        private static void OnStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if (d is BorderText bt)
            //{
            //    bt.ApplyState();
            //}
        }



    }
}
