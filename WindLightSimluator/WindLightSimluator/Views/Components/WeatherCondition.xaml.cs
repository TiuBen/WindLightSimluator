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

namespace WindLightSimluator.Views.Components
{
    /// <summary>
    /// WeatherCondition.xaml 的交互逻辑
    /// </summary>
    public partial class WeatherCondition : UserControl,IStateAware
    {
        public WeatherCondition()
        {
            InitializeComponent();
        }


        #region
        // 暴露颜色 
        // 容器背景
        public Brush ContainerBackgroundColor
        {
            get => (Brush)GetValue(ContainerBackgroundColorProperty);
            set => SetValue(ContainerBackgroundColorProperty, value);
        }

        public static readonly DependencyProperty ContainerBackgroundColorProperty =
            DependencyProperty.Register(
                nameof(ContainerBackgroundColor),
                typeof(Brush),
                typeof(WeatherCondition),
                new PropertyMetadata(Brushes.Red));


        // Label 背景
        public Brush LabelBackgroundColor
        {
            get => (Brush)GetValue(LabelBackgroundColorProperty);
            set => SetValue(LabelBackgroundColorProperty, value);
        }

        public static readonly DependencyProperty LabelBackgroundColorProperty =
            DependencyProperty.Register(
                nameof(LabelBackgroundColor),
                typeof(Brush),
                typeof(WeatherCondition),
                new PropertyMetadata(Brushes.Yellow));


        // Label 文本颜色
        public Brush LabelTextColor
        {
            get => (Brush)GetValue(LabelTextColorProperty);
            set => SetValue(LabelTextColorProperty, value);
        }

        public static readonly DependencyProperty LabelTextColorProperty =
            DependencyProperty.Register(
                nameof(LabelTextColor),
                typeof(Brush),
                typeof(WeatherCondition),
                new PropertyMetadata(Brushes.Green));


        // Value 文本背景
        public Brush ValueTextBackgroundColor
        {
            get => (Brush)GetValue(ValueTextBackgroundColorProperty);
            set => SetValue(ValueTextBackgroundColorProperty, value);
        }

        public static readonly DependencyProperty ValueTextBackgroundColorProperty =
            DependencyProperty.Register(
                nameof(ValueTextBackgroundColor),
                typeof(Brush),
                typeof(WeatherCondition),
                new PropertyMetadata(Brushes.Blue));


        // Value 文本颜色
        public Brush ValueTextColor
        {
            get => (Brush)GetValue(ValueTextColorProperty);
            set => SetValue(ValueTextColorProperty, value);
        }

        public static readonly DependencyProperty ValueTextColorProperty =
            DependencyProperty.Register(
                nameof(ValueTextColor),
                typeof(Brush),
                typeof(WeatherCondition),
                new PropertyMetadata(Brushes.Pink));



        #endregion






        //变色部分逻辑
        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(WeatherCondition),
                new PropertyMetadata(true, OnStateChanged));

        public string Theme
        {
            get => (string)GetValue(ThemeProperty);
            set => SetValue(ThemeProperty, value);
        }
        public static readonly DependencyProperty ThemeProperty =
            DependencyProperty.Register(nameof(Theme), typeof(string), typeof(WeatherCondition),
                new PropertyMetadata("Day", OnStateChanged));

        public string Mode
        {
            get => (string)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register(nameof(Mode), typeof(string), typeof(WeatherCondition),
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
