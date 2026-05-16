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

namespace WindLightSimluator.Views.AWOS.Components
{
    /// <summary>
    /// WindPanel.xaml 的交互逻辑
    /// </summary>
    public partial class WindPanel : UserControl
    {
        public WindPanel()
        {
            InitializeComponent();
        }

        public Visibility StartPartThresholdVisibility
        {
            get => (Visibility)GetValue(StartPartThresholdVisibilityProperty);
            set => SetValue(StartPartThresholdVisibilityProperty, value);
        }

        public static readonly DependencyProperty StartPartThresholdVisibilityProperty =
            DependencyProperty.Register(
                nameof(StartPartThresholdVisibility),
                typeof(Visibility),
                typeof(RunwayStatus),
                new PropertyMetadata(Visibility.Visible));

        public Visibility EndPartThresholdVisibility
        {
            get => (Visibility)GetValue(EndPartThresholdVisibilityProperty);
            set => SetValue(EndPartThresholdVisibilityProperty, value);
        }

        public static readonly DependencyProperty EndPartThresholdVisibilityProperty =
            DependencyProperty.Register(
                nameof(EndPartThresholdVisibility),
                typeof(Visibility),
                typeof(RunwayStatus),
                new PropertyMetadata(Visibility.Visible));


    }
}
