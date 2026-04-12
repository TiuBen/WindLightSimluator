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
using WindLightSimluator.Views.Components;
using WindLightSimluator.Views.Components.Base;

namespace WindLightSimluator.Views.Components
{

    /// <summary>
    /// RunwayNumber.xaml 的交互逻辑
    /// </summary>
    public partial class RunwayStatus : UserControl
    {
        public RunwayStatus()
        {
            InitializeComponent();

        }
 

        //// ===============================
        //// 状态属性：Active / InActive
        //// 是否激活（外部列选中）
        //// ===============================
        //public bool? IsActive
        //{
        //    get => (bool?)GetValue(IsActiveProperty);
        //    set => SetValue(IsActiveProperty, value);
        //}

        //public static readonly DependencyProperty IsActiveProperty =
        //    DependencyProperty.Register(
        //        nameof(IsActive),
        //        typeof(bool?),
        //        typeof(RunwayStatus),
        //        new PropertyMetadata(null));
  

        //#region
        //// 暴露颜色 
        //// 容器背景
        //public Brush ContainerBackgroundColor
        //{
        //    get => (Brush)GetValue(ContainerBackgroundColorProperty);
        //    set => SetValue(ContainerBackgroundColorProperty, value);
        //}

        //public static readonly DependencyProperty ContainerBackgroundColorProperty =
        //    DependencyProperty.Register(
        //        nameof(ContainerBackgroundColor),
        //        typeof(Brush),
        //        typeof(RunwayStatus),
        //        new PropertyMetadata(Brushes.Red));


        //// Label 背景
        //public Brush LabelBackgroundColor
        //{
        //    get => (Brush)GetValue(LabelBackgroundColorProperty);
        //    set => SetValue(LabelBackgroundColorProperty, value);
        //}

        //public static readonly DependencyProperty LabelBackgroundColorProperty =
        //    DependencyProperty.Register(
        //        nameof(LabelBackgroundColor),
        //        typeof(Brush),
        //        typeof(RunwayStatus),
        //        new PropertyMetadata(Brushes.Yellow));


        //// Label 文本颜色
        //public Brush LabelTextColor
        //{
        //    get => (Brush)GetValue(LabelTextColorProperty);
        //    set => SetValue(LabelTextColorProperty, value);
        //}

        //public static readonly DependencyProperty LabelTextColorProperty =
        //    DependencyProperty.Register(
        //        nameof(LabelTextColor),
        //        typeof(Brush),
        //        typeof(RunwayStatus),
        //        new PropertyMetadata(Brushes.Green));


        //// Value 文本背景
        //public Brush ValueTextBackgroundColor
        //{
        //    get => (Brush)GetValue(ValueTextBackgroundColorProperty);
        //    set => SetValue(ValueTextBackgroundColorProperty, value);
        //}

        //public static readonly DependencyProperty ValueTextBackgroundColorProperty =
        //    DependencyProperty.Register(
        //        nameof(ValueTextBackgroundColor),
        //        typeof(Brush),
        //        typeof(RunwayStatus),
        //        new PropertyMetadata(Brushes.Transparent));


        //// Value 文本颜色
        //public Brush ValueTextColor
        //{
        //    get => (Brush)GetValue(ValueTextColorProperty);
        //    set => SetValue(ValueTextColorProperty, value);
        //}

        //public static readonly DependencyProperty ValueTextColorProperty =
        //    DependencyProperty.Register(
        //        nameof(ValueTextColor),
        //        typeof(Brush),
        //        typeof(RunwayStatus),
        //        new PropertyMetadata(Brushes.Pink));



        //#endregion




        //#region MyRegion

        ////变色部分逻辑
     

        //public string Theme
        //{
        //    get => (string)GetValue(ThemeProperty);
        //    set => SetValue(ThemeProperty, value);
        //}
        //public static readonly DependencyProperty ThemeProperty =
        //    DependencyProperty.Register(nameof(Theme), typeof(string), typeof(RunwayStatus),
        //        new PropertyMetadata("Light", OnStateChanged));

        //public string Mode
        //{
        //    get => (string)GetValue(ModeProperty);
        //    set => SetValue(ModeProperty, value);
        //}
        //public static readonly DependencyProperty ModeProperty =
        //    DependencyProperty.Register(nameof(Mode), typeof(string), typeof(RunwayStatus),
        //        new PropertyMetadata("Normal", OnStateChanged));

        //private static void OnStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    //if (d is BorderText bt)
        //    //{
        //    //    bt.ApplyState();
        //    //}
        //}

        //#endregion






    }






}














//// ===============================
//// 内部业务状态（不暴露、不绑定）
//// ===============================
//private RwyState _state = RwyState.Empty;
//// ===============================
//// UI 用状态文本（只读）
//// ===============================
//public string RunwayStatusText
//{
//    get => (string)GetValue(RunwayStatusTextProperty);
//    private set => SetValue(RunwayStatusTextPropertyKey, value);
//}

//private static readonly DependencyPropertyKey RunwayStatusTextPropertyKey =
//    DependencyProperty.RegisterReadOnly(
//        nameof(RunwayStatusText),
//        typeof(string),
//        typeof(RunwayStatus),
//        new PropertyMetadata(""));

//public static readonly DependencyProperty RunwayStatusTextProperty =
//    RunwayStatusTextPropertyKey.DependencyProperty;

//private void UpdateRunwayStatusText()
//{
//    //bool isActive = false;

//    //// 自动读取父 Grid 的 StateBehavior.IsActive
//    //var grid = FindAncestor<Grid>(this);
//    //if (grid != null)
//    //{
//    //    isActive = StateBehavior.GetIsActive(grid);
//    //}

//    //RunwayStatusText = isActive
//    //    ? "NOT IN USE"
//    //    : "LANDING / TAKE OFF";
//}



//public enum RwyState
//{
//    Empty = 0,
//    InUse = 1,
//    NotInUse = 2
//}
//public static class RwyStateExtensions
//{
//    public static string ToStatusText(this RwyState state)
//    {
//        return state switch
//        {
//            RwyState.InUse => "LANDING/TAKE OFF",
//            RwyState.NotInUse => "NOT IN USE",
//            RwyState.Empty => "",
//            _ => ""
//        };
//    }
//}



//// 跑道号
//public string RunwayNumber
//{
//    get => (string)GetValue(RunwayNumberProperty);
//    set => SetValue(RunwayNumberProperty, value);
//}
//public static readonly DependencyProperty RunwayNumberProperty = DependencyProperty.Register(nameof(RunwayNumber), typeof(string), typeof(RunwayStatus), new PropertyMetadata("19R"));

