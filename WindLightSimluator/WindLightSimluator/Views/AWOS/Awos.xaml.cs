using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WindLightSimluator.Views.AWOS
{
    public enum RunwayCommandType
    {
        SwapWestEast,
        SetIndex0,
        SetIndex1,
        SetIndex2
    }

    /// <summary>
    /// Awos.xaml 的交互逻辑
    /// </summary>
    public partial class Awos : UserControl
    {
        public Awos()
        {
            InitializeComponent();


            Switch_Menu_Bar.CommandRequested += OnCommandRequested;
         }

        private void OnCommandRequested(RunwayCommandType cmd)
        {
            switch (cmd)
            {
                case RunwayCommandType.SwapWestEast:
                    Runway_View.SwapWestEast();
                    break;

                case RunwayCommandType.SetIndex0:
                    Runway_View.ChangeRunwayIndex(0);
                    break;

                case RunwayCommandType.SetIndex1:
                    Runway_View.ChangeRunwayIndex(1);
                    break;
               
            }
        }
    }
}
