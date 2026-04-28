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

namespace WindLightSimluator.Views.RVRSwitch
{
    /// <summary>
    /// RunwayLamp.xaml 的交互逻辑
    /// </summary>
    public partial class RunwayLamp : UserControl
    {
        public RunwayLamp()
        {
            InitializeComponent();
        }

        // =======================================================
        // 1. 是否点亮 (IsActive) - 依赖属性
        // =======================================================
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(RunwayLamp), new PropertyMetadata(false));


        // =======================================================
        // 2. 灯下文本 (LampText) - 依赖属性
        // =======================================================
        public string LampText
        {
            get { return (string)GetValue(LampTextProperty); }
            set { SetValue(LampTextProperty, value); }
        }

        public static readonly DependencyProperty LampTextProperty =
            DependencyProperty.Register("LampText", typeof(string), typeof(RunwayLamp), new PropertyMetadata("00X"));
    }
}

