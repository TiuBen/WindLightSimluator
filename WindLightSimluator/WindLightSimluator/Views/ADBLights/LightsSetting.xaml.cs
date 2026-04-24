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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WindLightSimluator.Views.Lights
{
    /// <summary>
    /// LightsSetting.xaml 的交互逻辑
    /// </summary>
    public partial class LightsSetting : UserControl
    {
        public LightsSetting()
        {
            InitializeComponent();
        }

        private void OpenDialogButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. 对主内容容器应用模糊效果
            MainContent.Effect = new BlurEffect { Radius = 3 };
            // 2. 显示弹窗
            DialogPopup.IsOpen = true;
        }

        private void CloseDialogButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. 移除模糊效果
            MainContent.Effect = null;
            // 2. 关闭弹窗
            DialogPopup.IsOpen = false;
        }
    }
}
