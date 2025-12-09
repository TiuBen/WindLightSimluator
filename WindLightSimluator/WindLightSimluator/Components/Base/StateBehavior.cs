using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace WindLightSimluator.Components.Base
{
    public class StateBehavior
    {
        // Active
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.RegisterAttached(
            "IsActive", typeof(bool), typeof(StateBehavior),
            new PropertyMetadata(false, OnStateChanged));

        // Theme
        public static readonly DependencyProperty ThemeProperty =
            DependencyProperty.RegisterAttached(
            "Theme", typeof(string), typeof(StateBehavior),
            new PropertyMetadata("Day", OnStateChanged));

        // Mode
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.RegisterAttached(
            "Mode", typeof(string), typeof(StateBehavior),
            new PropertyMetadata("Normal", OnStateChanged));

        public static void SetIsActive(DependencyObject d, bool v) => d.SetValue(IsActiveProperty, v);
        public static bool GetIsActive(DependencyObject d) => (bool)d.GetValue(IsActiveProperty);

        public static void SetTheme(DependencyObject d, string v) => d.SetValue(ThemeProperty, v);
        public static string GetTheme(DependencyObject d) => (string)d.GetValue(ThemeProperty);

        public static void SetMode(DependencyObject d, string v) => d.SetValue(ModeProperty, v);
        public static string GetMode(DependencyObject d) => (string)d.GetValue(ModeProperty);

        private static void OnStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Panel panel)
            {
                bool active = GetIsActive(panel);
                string theme = GetTheme(panel);
                string mode = GetMode(panel);

                foreach (var child in panel.Children)
                {
                    if (child is IStateAware ctrl)
                    {
                        ctrl.IsActive = active;
                        ctrl.Theme = theme;
                        ctrl.Mode = mode;
                    }
                }
            }
        }
    }

}
