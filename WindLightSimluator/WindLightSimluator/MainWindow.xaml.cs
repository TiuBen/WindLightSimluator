using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using WindLightSimluator.ViewModels;


namespace WindLightSimluator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private MainWindowViewModel _vm;
        public MainWindow()
        {
            InitializeComponent();
            //DrawArc(100, 100, 50, -20, 20); // 圆心(100,100)，半径50，从0度到180度
            //DrawArc(100, 100, 50, -20, 20); // 圆心(100,100)，半径50，从0度到180度
            Debug.WriteLine("dddddddddddddddddddddddddddddddddddddddddddddddddd");
            _vm = new MainWindowViewModel();
            DataContext = _vm;
        }


        private void SwitchTheme(bool isDark)
        {
            Application.Current.Resources.MergedDictionaries.Clear();

            if (isDark)
            {
                Application.Current.Resources.MergedDictionaries.Add(
                    new ResourceDictionary()
                    { Source = new Uri("DarkTheme.xaml", UriKind.Relative) }
                );
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Add(
                    new ResourceDictionary()
                    { Source = new Uri("LightTheme.xaml", UriKind.Relative) }
                );
            }
        }

        private void SwitchThemeButtton_Click(object sender, RoutedEventArgs e)
        {

        }


        public string Theme
        {
            get => (string)GetValue(ThemeProperty);
            set => SetValue(ThemeProperty, value);
        }

        public static readonly DependencyProperty ThemeProperty =
            DependencyProperty.Register(
                nameof(Theme),
                typeof(string),
                typeof(MainWindow),
                new PropertyMetadata("Light"));


    }
}