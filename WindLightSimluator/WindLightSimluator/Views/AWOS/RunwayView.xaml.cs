using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
using WindLightSimluator.ViewModels;

namespace WindLightSimluator.Views.AWOS
{
   /// <summary>
    /// RunwayView.xaml 的交互逻辑
    /// </summary>
    public partial class RunwayView : UserControl
    {
        public RunwayView()
        {
            InitializeComponent();
        }


        public void SwapWestEast()
        {
            int col1 = Grid.GetColumn(Col1);
            int col3 = Grid.GetColumn(Col3);

            Grid.SetColumn(Col1, col3);
            Grid.SetColumn(Col3, col1);

        }

        public void ChangeRunwayIndex(int index)
        {
            Debug.WriteLine("ChangeRunwayIndexChangeRunwayIndexChangeRunwayIndexChangeRunwayIndexChangeRunwayIndex");
            var vm = DataContext as AirportVM;
            if (vm is not AirportVM ) return;

            vm.SelectedRunwayVM = vm.Runways[index];
        }

    }
}
