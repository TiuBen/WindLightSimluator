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
    /// LevelIndicator.xaml 的交互逻辑
    /// </summary>
    public partial class LevelIndicator : UserControl
    {
        public static readonly DependencyProperty LevelProperty =
           DependencyProperty.Register(nameof(Level), typeof(int), typeof(LevelIndicator),
               new PropertyMetadata(0));

        public int Level
        {
            get => (int)GetValue(LevelProperty);
            set => SetValue(LevelProperty, value);
        }

        public LevelIndicator()
        {
            InitializeComponent();
        }
    }
}
