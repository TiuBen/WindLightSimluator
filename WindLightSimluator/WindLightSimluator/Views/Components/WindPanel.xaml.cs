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
using WindLightSimluator.Views.Components.Base;

namespace WindLightSimluator.Views.Components
{
    /// <summary>
    /// WindPanel.xaml 的交互逻辑
    /// </summary>
    public partial class WindPanel : UserControl,IStateAware
    {
        public WindPanel()
        {
            InitializeComponent();
        }

        //整个风盘的背景色
        public Brush WindPanelBackground
        {
            get => (Brush)GetValue(WindPanelBackgroundProperty);
            set => SetValue(WindPanelBackgroundProperty, value);
        }

        public static readonly DependencyProperty WindPanelBackgroundProperty =
            DependencyProperty.Register(
                nameof(WindPanelBackground),
                typeof(Brush),
                typeof(WindPanel),
                new PropertyMetadata(Brushes.Transparent));

        //风盘上文字的颜色
        public Brush WindPanelTextColor
        {
            get => (Brush)GetValue(WindPanelTextColorProperty);
            set => SetValue(WindPanelTextColorProperty, value);
        }

        public static readonly DependencyProperty WindPanelTextColorProperty =
            DependencyProperty.Register(
                nameof(WindPanelTextColor),
                typeof(Brush),
                typeof(WindPanel),
                new PropertyMetadata(Brushes.Red));



        #region MyRegion

        //变色部分逻辑
        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(WindPanel),
                new PropertyMetadata(true, OnStateChanged));

        public string Theme
        {
            get => (string)GetValue(ThemeProperty);
            set => SetValue(ThemeProperty, value);
        }
        public static readonly DependencyProperty ThemeProperty =
            DependencyProperty.Register(nameof(Theme), typeof(string), typeof(WindPanel),
                new PropertyMetadata("Light", OnStateChanged));

        public string Mode
        {
            get => (string)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register(nameof(Mode), typeof(string), typeof(WindPanel),
                new PropertyMetadata("Normal", OnStateChanged));

        private static void OnStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if (d is BorderText bt)
            //{
            //    bt.ApplyState();
            //}
        }

        #endregion



    }
}
