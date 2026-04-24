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

namespace WindLightSimluator.Views.Lights
{
    /// <summary>
    /// LightsPartButton.xaml 的交互逻辑
    /// </summary>
    public partial class LightsPartButton : UserControl
    {
        public LightsPartButton()
        {
            InitializeComponent();
        }

        // 对应 text1
        public string Text1
        {
            get { return (string)GetValue(Text1Property); }
            set { SetValue(Text1Property, value); }
        }
        public static readonly DependencyProperty Text1Property =
            DependencyProperty.Register("Text1", typeof(string), typeof(LightsPartButton), new PropertyMetadata("01L 关闭"));

        // 对应 text2
        public string Text2
        {
            get { return (string)GetValue(Text2Property); }
            set { SetValue(Text2Property, value); }
        }
        public static readonly DependencyProperty Text2Property =
            DependencyProperty.Register("Text2", typeof(string), typeof(LightsPartButton), new PropertyMetadata("夜晚"));

        // 对应 text3
        public string Text3
        {
            get { return (string)GetValue(Text3Property); }
            set { SetValue(Text3Property, value); }
        }
        public static readonly DependencyProperty Text3Property =
            DependencyProperty.Register("Text3", typeof(string), typeof(LightsPartButton), new PropertyMetadata("2档"));
    }
}
