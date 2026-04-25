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
    /// RoundIndicator.xaml 的交互逻辑
    /// </summary>
    public partial class RoundIndicator : UserControl
    {
        public static readonly DependencyProperty IsOnProperty =
          DependencyProperty.Register(nameof(IsOn), typeof(bool), typeof(RoundIndicator),
              new PropertyMetadata(false, OnChanged));

        public bool IsOn
        {
            get => (bool)GetValue(IsOnProperty);
            set => SetValue(IsOnProperty, value);
        }

        public RoundIndicator()
        {
            InitializeComponent();
        }

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (RoundIndicator)d;
            ctrl.Inner.Fill = (bool)e.NewValue ? Brushes.LimeGreen : Brushes.Gray;
        }
    }
}
