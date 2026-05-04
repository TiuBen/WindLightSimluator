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

        private void ChangeThemeToDay_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ApplyTheme("LightTheme.xaml");
        }

        private void ChangeThemeToNight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ApplyTheme("DarkTheme.xaml");
        }


        private void ApplyTheme(string themeFile)
        {
            var dict = new ResourceDictionary
            {
                Source = new Uri(themeFile, UriKind.Relative)
            };

            var merged = Application.Current.Resources.MergedDictionaries;

            if (merged.Count > 0)
            {
                merged[0] = dict;   // ⭐核心：替换，不清空
            }
            else
            {
                merged.Add(dict);
            }
        }
    }
}
