using System;
using System.Collections.Generic;
using System.Globalization;
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
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace WindLightSimluator.Components
{
    /// <summary>
    /// Runway.xaml 的交互逻辑
    /// </summary>
    public partial class Runway : UserControl
    {
        public Runway()
        {
            InitializeComponent();
        }

        // ======= 左侧 Label 文本 =======
        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register(
                nameof(LabelText),
                typeof(string),
                typeof(Runway),
                new PropertyMetadata("Label")
            );

        // ======= 右侧 TextBlock 文本 =======
        public string TextContent
        {
            get => (string)GetValue(TextContentProperty);
            set => SetValue(TextContentProperty, value);
        }

        public static readonly DependencyProperty TextContentProperty =
            DependencyProperty.Register(
                nameof(TextContent),
                typeof(string),
                typeof(Runway),
                new PropertyMetadata("NOT IN USE")      // 默认值 0
            );
    }
}
