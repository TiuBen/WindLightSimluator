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
    /// RunwayStatus.xaml 的交互逻辑
    /// </summary>
    public partial class RunwayStatus : UserControl
    {
        public RunwayStatus()
        {
            InitializeComponent();
        }

        public Visibility CapsuleVisibility
        {
            get => (Visibility)GetValue(CapsuleVisibilityProperty);
            set => SetValue(CapsuleVisibilityProperty, value);
        }

        public static readonly DependencyProperty CapsuleVisibilityProperty =
            DependencyProperty.Register(
                nameof(CapsuleVisibility),
                typeof(Visibility),
                typeof(RunwayStatus),
                new PropertyMetadata(Visibility.Visible));

    }
}
