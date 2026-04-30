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
    /// RVRSwitch.xaml 的交互逻辑
    /// </summary>
    public partial class RVRSwitch : UserControl
    {
        public RVRSwitch()
        {
            InitializeComponent();
        }


        public string SelectedRunwayPartName
        {
            get => (string)GetValue(SelectedRunwayPartNameProperty);
            set => SetValue(SelectedRunwayPartNameProperty, value);
        }

        public static readonly DependencyProperty SelectedRunwayPartNameProperty =
            DependencyProperty.Register(nameof(SelectedRunwayPartName), typeof(string), typeof(RVRSwitch));



        public string StartPartRunwayNumber
        {
            get => (string)GetValue(StartPartRunwayNumberProperty);
            set => SetValue(StartPartRunwayNumberProperty, value);
        }

        public static readonly DependencyProperty StartPartRunwayNumberProperty =
            DependencyProperty.Register(nameof(StartPartRunwayNumber), typeof(string), typeof(RVRSwitch));



        public string EndPartRunwayNumber
        {
            get => (string)GetValue(EndPartRunwayNumberProperty);
            set => SetValue(EndPartRunwayNumberProperty, value);
        }

        public static readonly DependencyProperty EndPartRunwayNumberProperty =
            DependencyProperty.Register(nameof(EndPartRunwayNumber), typeof(string), typeof(RVRSwitch));



        public int SelectedLightDegree
        {
            get => (int)GetValue(SelectedLightDegreeProperty);
            set => SetValue(SelectedLightDegreeProperty, value);
        }

        public static readonly DependencyProperty SelectedLightDegreeProperty =
            DependencyProperty.Register(nameof(SelectedLightDegree), typeof(int), typeof(RVRSwitch));

    }
}
