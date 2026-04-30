using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Threading;

namespace WindLightSimluator.Views.AWOS
{

    public class TopBarViewModel : INotifyPropertyChanged
    {
        private readonly DispatcherTimer _timer;

        public DateTime BjtTime => DateTime.Now;
        public DateTime UtcTime => DateTime.UtcNow;

        public TopBarViewModel()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            _timer.Tick += (s, e) =>
            {
                OnPropertyChanged(nameof(BjtTime));
                OnPropertyChanged(nameof(UtcTime));
            };

            _timer.Start();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    /// <summary>
    /// TopBar.xaml 的交互逻辑
    /// </summary>
    public partial class TopBar : UserControl
    {
        public TopBar()
        {
            InitializeComponent();
            DataContext = new TopBarViewModel();

        }
        private bool isDark = false;

        private void ChangeTheme_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

            isDark = !isDark;
        }
    }
}
