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
    /// <summary>
    /// RvrVis.xaml 的交互逻辑
    /// </summary>
    public partial class RvrVis : UserControl, IStateAware
    {
        public RvrVis()
        {
            InitializeComponent();
        }

        public int RvrValue
        {
            get => (int)GetValue(RvrValueProperty);
            set => SetValue(RvrValueProperty, value);
        }
        public static readonly DependencyProperty RvrValueProperty =
            DependencyProperty.Register(nameof(RvrValue), typeof(int), typeof(RvrVis), new PropertyMetadata(550));

        public int VisValue
        {
            get => (int)GetValue(VisValueProperty);
            set => SetValue(VisValueProperty, value);
        }
        public static readonly DependencyProperty VisValueProperty =
            DependencyProperty.Register(nameof(VisValue), typeof(int), typeof(RvrVis), new PropertyMetadata(1000));


        #region 
        //变色部分逻辑
        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(BorderText),
                new PropertyMetadata(true, OnStateChanged));

        public string Theme
        {
            get => (string)GetValue(ThemeProperty);
            set => SetValue(ThemeProperty, value);
        }
        public static readonly DependencyProperty ThemeProperty =
            DependencyProperty.Register(nameof(Theme), typeof(string), typeof(BorderText),
                new PropertyMetadata("Day", OnStateChanged));

        public string Mode
        {
            get => (string)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register(nameof(Mode), typeof(string), typeof(BorderText),
                new PropertyMetadata("Normal", OnStateChanged));

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
