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
using WindLightSimluator.ViewModels;

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




        private void CloseDialogButton_Click(object sender, RoutedEventArgs e)
        {
            DialogPopup.Visibility = Visibility.Collapsed;
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {

            var radioButton = sender as RadioButton;
            string status = radioButton.Tag?.ToString();

            // 获取 ViewModel
            var vm = this.DataContext as ADBLightsVM; // 替换成你的 ViewModel 类型
                                                      // 或者使用 SelectedLightVM
            if (vm?.SelectedLightVM != null)
            {
                switch (status)
                {
                    case "Landing":
                        vm.SelectedLightVM.LightStatus = LightStatus.Landing;
                        break;
                    case "TakeOff":
                        vm.SelectedLightVM.LightStatus = LightStatus.TakeOff;
                        break;
                    case "Closed":
                        vm.SelectedLightVM.LightStatus = LightStatus.Closed;
                        break;
                }
            }

        }

        private void LightsPartButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as LightsPartButton;
            var tag = button.Tag;  // 获取传进来的参数

            // 如果是绑定 LightItemVM
            if (int.TryParse(tag.ToString(), out int selectedLightIndex))
            {
                var vm = this.DataContext as ADBLightsVM; // 替换成你的 ViewModel 类型
                vm.SelectedLightIndex = selectedLightIndex;

                vm.SelectedLightVM = vm.Lights[selectedLightIndex].Clone();

                DialogPopup.Visibility = Visibility.Visible;
            }



        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {

            var vm = this.DataContext as ADBLightsVM; // 替换成你的 ViewModel 类型
            vm.Lights[vm.SelectedLightIndex].ApplyFrom(vm.SelectedLightVM);


            DialogPopup.Visibility = Visibility.Collapsed;

        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void ChangeCat2(object sender, MouseButtonEventArgs e)
        {
            var vm = this.DataContext as ADBLightsVM;
            vm.IsCat2Enabled = !vm.IsCat2Enabled;
        }
    }
}
