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

namespace WindLightSimluator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //DrawArc(100, 100, 50, -20, 20); // 圆心(100,100)，半径50，从0度到180度
            //DrawArc(100, 100, 50, -20, 20); // 圆心(100,100)，半径50，从0度到180度

        }

        /// <summary>
        /// 绘制圆弧到 MainWindow 的 Canvas 上
        /// </summary>
        /// <param name="centerX">圆心 X</param>
        /// <param name="centerY">圆心 Y</param>
        /// <param name="radius">半径</param>
        /// <param name="startAngle">起始角度（度）</param>
        /// <param name="endAngle">结束角度（度）</param>
        private void DrawArc(double centerX, double centerY, double radius, double startAngle, double endAngle)
        {
            // ---- 0° 在上方：转换到 WPF 坐标系 ----
            double startRad = (90 - startAngle) * Math.PI / 180;
            double endRad = (90 - endAngle) * Math.PI / 180;

            // 起点
            Point startPoint = new Point(
                centerX + radius * Math.Cos(startRad),
                centerY - radius * Math.Sin(startRad)); // WPF Y轴向下，所以减

            // 终点
            Point endPoint = new Point(
                centerX + radius * Math.Cos(endRad),
                centerY - radius * Math.Sin(endRad));

            double delta = endAngle - startAngle;
            if (delta < 0) delta += 360; // 确保始终顺时针
            // 创建圆弧段
            ArcSegment arcSegment = new ArcSegment
            {
                Point = endPoint,
                Size = new Size(radius, radius),
                IsLargeArc = Math.Abs(endAngle - startAngle) > 180,
                SweepDirection = SweepDirection.Clockwise
            };

            // 创建路径图形
            PathFigure figure = new PathFigure
            {
                StartPoint = startPoint,
                Segments = new PathSegmentCollection { arcSegment }
            };

            PathGeometry geometry = new PathGeometry();
            geometry.Figures.Add(figure);

            Path path = new Path
            {
                Stroke = Brushes.Blue,
                StrokeThickness = 20,
                Data = geometry
            };

            // 将圆弧添加到窗口的 Canvas 上
            if (this.Content is Canvas canvas)
            {
                canvas.Children.Add(path);
            }
            else
            {
                // 如果窗口内容不是 Canvas，可以新建一个 Canvas
                Canvas newCanvas = new Canvas();
                newCanvas.Children.Add(path);
                this.Content = newCanvas;
            }
        }
    }
}