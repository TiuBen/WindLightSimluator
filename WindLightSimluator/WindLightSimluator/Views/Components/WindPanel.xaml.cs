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
using WindLightSimluator.Views.Components.Base;

namespace WindLightSimluator.Views.Components
{
    /// <summary>
    /// WindPanel.xaml 的交互逻辑
    /// </summary>
    /// 

    public enum RunwayDisplayMode
    {
        None,           // 都不显示
        Both,           // 两头都显示
        Head,           // 只显示头（跑道起点）
        Tail            // 只显示尾（跑道终点）
    }

    public partial class WindPanel : UserControl
    {
        public WindPanel()
        {
            InitializeComponent();
        }



    }
}
