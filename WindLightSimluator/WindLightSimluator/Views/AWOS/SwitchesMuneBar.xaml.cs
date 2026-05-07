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

namespace WindLightSimluator.Views.AWOS
{
    /// <summary>
    /// SwitchesMuneBar.xaml 的交互逻辑
    /// </summary>
    public partial class SwitchesMuneBar : UserControl
    {

        public SwitchesMuneBar()
        {
            InitializeComponent();
        }

        public event Action<RunwayCommandType> CommandRequested;

        private void SwapWestEastButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CommandRequested?.Invoke(RunwayCommandType.SwapWestEast);
        }

        private void ChangeRunwayIndex0(object sender, MouseButtonEventArgs e)
        {
            CommandRequested?.Invoke(RunwayCommandType.SetIndex0);
        }
        private void ChangeRunwayIndex1(object sender, MouseButtonEventArgs e)
        {
            CommandRequested?.Invoke(RunwayCommandType.SetIndex1);
        }
    }
}
