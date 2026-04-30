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
using WindLightSimluator.Views.Components.Base;
using System.Diagnostics;
using System.Windows.Threading;

namespace WindLightSimluator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public AirportVM AwosVM;
        public MainWindow()
        {
            InitializeComponent();
            //DrawArc(100, 100, 50, -20, 20); // 圆心(100,100)，半径50，从0度到180度
            //DrawArc(100, 100, 50, -20, 20); // 圆心(100,100)，半径50，从0度到180度
            Debug.WriteLine("dddddddddddddddddddddddddddddddddddddddddddddddddd");
            //AwosVM = new WindLightSimluator.ViewModels.AirportVM();
            //this.DataContext = _vm;
        }


        private bool isDark=false;
      
        private void SwitchThemeButtton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"Switch theme {isDark}");

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

           isDark= !isDark;
        }


     

    }
}