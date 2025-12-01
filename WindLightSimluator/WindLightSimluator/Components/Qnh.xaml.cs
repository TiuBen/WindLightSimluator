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

namespace WindLightSimluator.Components
{
    /// <summary>
    /// Qnh.xaml 的交互逻辑
    /// </summary>
    public partial class Qnh : UserControl
    {
        public Qnh()
        {
            InitializeComponent();
        }
    }



    //public class HeightToFontSizeConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value is double height && height > 0)
    //        {
    //            double multiplier = 0.8; // 默认倍数
    //            if (parameter != null && double.TryParse(parameter.ToString(), out double customMultiplier))
    //            {
    //                multiplier = customMultiplier;
    //            }

    //            // 根据Grid的实际高度计算字体大小，使用0.8倍
    //            return height * 0.6 * multiplier;
    //        }
    //        return 36.0; // 默认字体大小
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}


    public class HeightToFontSizeConverter : IValueConverter
    {
        // 添加静态实例属性
        public static HeightToFontSizeConverter Instance { get; } = new HeightToFontSizeConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double height && height > 0)
            {
                double multiplier = 0.8; // 默认倍数
                if (parameter != null && double.TryParse(parameter.ToString(), out double customMultiplier))
                {
                    multiplier = customMultiplier;
                }

                // 根据Grid的实际高度计算字体大小，使用0.8倍
                return height * 0.6 * multiplier;
            }
            return 36.0; // 默认字体大小
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
