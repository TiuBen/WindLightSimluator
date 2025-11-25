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

namespace WindLightSimluator.Components
{
    public partial class WindSpeedGrid : UserControl
    {
        public WindSpeedGrid()
        {
            InitializeComponent();
        }

        // 依赖属性示例
        public static readonly DependencyProperty HeadWindAVG2TextProperty =
            DependencyProperty.Register(nameof(HeadWindAVG2Text), typeof(string), typeof(WindSpeedGrid), new PropertyMetadata(string.Empty));

        public string HeadWindAVG2Text
        {
            get => (string)GetValue(HeadWindAVG2TextProperty);
            set => SetValue(HeadWindAVG2TextProperty, value);
        }

        public static readonly DependencyProperty CrossWindAVG2TextProperty =
            DependencyProperty.Register(nameof(CrossWindAVG2Text), typeof(string), typeof(WindSpeedGrid), new PropertyMetadata(string.Empty));

        public string CrossWindAVG2Text
        {
            get => (string)GetValue(CrossWindAVG2TextProperty);
            set => SetValue(CrossWindAVG2TextProperty, value);
        }

        public static readonly DependencyProperty WD_NNM2TextProperty =
            DependencyProperty.Register(nameof(WD_NNM2Text), typeof(string), typeof(WindSpeedGrid), new PropertyMetadata(string.Empty));

        public string WD_NNM2Text
        {
            get => (string)GetValue(WD_NNM2TextProperty);
            set => SetValue(WD_NNM2TextProperty, value);
        }

        public static readonly DependencyProperty WS_NNM2TextProperty =
            DependencyProperty.Register(nameof(WS_NNM2Text), typeof(string), typeof(WindSpeedGrid), new PropertyMetadata(string.Empty));

        public string WS_NNM2Text
        {
            get => (string)GetValue(WS_NNM2TextProperty);
            set => SetValue(WS_NNM2TextProperty, value);
        }

        public static readonly DependencyProperty WD_AVG2TextProperty =
            DependencyProperty.Register(nameof(WD_AVG2Text), typeof(string), typeof(WindSpeedGrid), new PropertyMetadata(string.Empty));

        public string WD_AVG2Text
        {
            get => (string)GetValue(WD_AVG2TextProperty);
            set => SetValue(WD_AVG2TextProperty, value);
        }

        public static readonly DependencyProperty WS_AVG2TextProperty =
            DependencyProperty.Register(nameof(WS_AVG2Text), typeof(string), typeof(WindSpeedGrid), new PropertyMetadata(string.Empty));

        public string WS_AVG2Text
        {
            get => (string)GetValue(WS_AVG2TextProperty);
            set => SetValue(WS_AVG2TextProperty, value);
        }

        public static readonly DependencyProperty WD_MAX2TextProperty =
            DependencyProperty.Register(nameof(WD_MAX2Text), typeof(string), typeof(WindSpeedGrid), new PropertyMetadata(string.Empty));

        public string WD_MAX2Text
        {
            get => (string)GetValue(WD_MAX2TextProperty);
            set => SetValue(WD_MAX2TextProperty, value);
        }

        public static readonly DependencyProperty WS_MAX2TextProperty =
            DependencyProperty.Register(nameof(WS_MAX2Text), typeof(string), typeof(WindSpeedGrid), new PropertyMetadata(string.Empty));

        public string WS_MAX2Text
        {
            get => (string)GetValue(WS_MAX2TextProperty);
            set => SetValue(WS_MAX2TextProperty, value);
        }

        // 你也可以加方法快速设置
        public void SetHeadWindAVG2(string value) => HeadWindAVG2Text = value;
        public void SetCrossWindAVG2(string value) => CrossWindAVG2Text = value;
        // 其他类似
    }
}
