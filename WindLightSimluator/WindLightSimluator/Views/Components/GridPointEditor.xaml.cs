using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Printing.IndexedProperties;
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
using WindLightSimluator.Model;
using WindLightSimluator.ViewModels;

namespace WindLightSimluator.Views.Components
{
    /// <summary>
    /// GridPointEditor.xaml 的交互逻辑
    /// </summary>
    public partial class GridPointEditor : UserControl
    {
        public GridPointEditor()
        {
            InitializeComponent();
            Loaded += (_, _) => Redraw();
            SizeChanged += (_, _) => Redraw();

            // 添加键盘事件处理
            this.Focusable = true;
            this.KeyDown += OnKeyDown;
            this.MouseDown += OnUserControlMouseDown;
        }

        #region Points

        public ObservableCollection<double> Points
        {
            get => (ObservableCollection<double>)GetValue(PointsProperty);
            set {
                Debug.WriteLine("Points Points Points");
                SetValue(PointsProperty, value);
            
            }
        }

        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register(nameof(Points),
                typeof(ObservableCollection<double>),
                typeof(GridPointEditor),
                new PropertyMetadata(null, OnPointsChanged));

        private static void OnPointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (GridPointEditor)d;

            if (e.OldValue is ObservableCollection<double> oldCol)
                oldCol.CollectionChanged -= control.OnPointsCollectionChanged;

            if (e.NewValue is ObservableCollection<double> newCol)
                newCol.CollectionChanged += control.OnPointsCollectionChanged;

            control.Redraw();
        }

        private void OnPointsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Redraw();
        }


        #endregion



        // 选中的设置项 比如 风向 
        public string SelectedField
        {
            get => (string)GetValue(SelectedFieldProperty);
            set => SetValue(SelectedFieldProperty, value);
        }
        public static readonly DependencyProperty SelectedFieldProperty = DependencyProperty.Register(nameof(SelectedField), typeof(string), typeof(GridPointEditor), new PropertyMetadata("Temperature", OnDataChanged));

        // 选中的设置项 的 基础信息 比如最大值 最小值 
        public FieldConfig FieldConfig
        {
            get => (FieldConfig)GetValue(FieldConfigProperty);
            set => SetValue(FieldConfigProperty, value);
        }
        public static readonly DependencyProperty FieldConfigProperty = DependencyProperty.Register(nameof(FieldConfig), typeof(FieldConfig), typeof(GridPointEditor), new PropertyMetadata(new FieldConfig(), OnDataChanged));





        //有关draw的部分
        // 网格的默认设置
        //内边距
        private const double GRID_PADDING = 50; //边距 
        private const double GRID_SIZE = 20;
        private const double GRID_TAIL = 10;
        private const int POINT_COUNT = 120;

        // 定义样式
        private SolidColorBrush gridBrush = Brushes.Gray;// 浅灰色网格
        private SolidColorBrush xAxisColor = Brushes.White;
        private SolidColorBrush SubTextColor = Brushes.Blue;


        static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as GridPointEditor)?.Redraw();
        }

        public  void Redraw()
        {
            //Debug.WriteLine($"re drawed {SelectedField}");

            var canvas = GridPointEditorCanvas;
            canvas.Children.Clear();

            double drawWidth = ActualWidth - GRID_PADDING - GRID_PADDING;
            double drawHeight = ActualHeight - GRID_PADDING - GRID_PADDING;

            //DrawCrosshair(canvas, drawWidth, drawHeight);
            DrawCoordinateSystem(canvas, drawWidth, drawHeight);
            DrawPoints(canvas, drawWidth, drawHeight);

            // 如果有选中的点，显示数值提示
            if (selectedIndex >= 0 && Points != null && selectedIndex < Points.Count)
            {
                DrawSelectedPointTooltip(canvas, drawWidth, drawHeight);
            }
        }

        private void DrawCoordinateSystem(Canvas canvas, double w, double h)
        {
            if (FieldConfig == null || FieldConfig.Step <= 0) return;
            // 1. 基础参数计算
            double totalMinutes = POINT_COUNT;
            double range = FieldConfig.Max - FieldConfig.Min;
            if (range <= 0) range = 1; // 防止除零

            // --- 绘制 Y 轴逻辑 (数值 & 横向网格线) ---
            // 中心线（BaseValue）
            double centerY = GRID_PADDING + h / 2;

            for (int i = 0; i <= h / 2 / GRID_SIZE; i++)
            {

                // 绘制横向网格线
                // 向下 如果值小于最小值 就不画线了
                if (FieldConfig.BaseValue - i * FieldConfig.Step >= FieldConfig.Min)
                {
                    canvas.Children.Add(new Line
                    {
                        X1 = i % 5 == 0 ? (GRID_PADDING - GRID_TAIL * 3) : (GRID_PADDING - GRID_TAIL), //每5个格子 多出来一点
                        X2 = GRID_PADDING + w,
                        Y1 = centerY + i * GRID_SIZE,
                        Y2 = centerY + i * GRID_SIZE,
                        Stroke = i == 0 ? Brushes.Blue : (i % 5 == 0 ? Brushes.DarkBlue : gridBrush),// 如果是基准线则变蓝 每5个格子淡蓝色
                        StrokeThickness = 0.3
                    });

                    // 绘制 Y 轴数值
                    if (i % 5 == 0) // 每5个配个数值
                    {
                        var text = new TextBlock
                        {
                            Text = (FieldConfig.BaseValue - i * FieldConfig.Step).ToString(),
                            Foreground = SubTextColor,
                            FontSize = 10
                        };
                        Canvas.SetLeft(text, 5);
                        Canvas.SetTop(text, centerY + i * GRID_SIZE - 7);
                        canvas.Children.Add(text);


                    }
                }
                if (FieldConfig.BaseValue + i * FieldConfig.Step <= FieldConfig.Max)
                {
                    // 向上的横线
                    canvas.Children.Add(new Line
                    {
                        X1 = i % 5 == 0 ? GRID_PADDING - GRID_TAIL * 3 : GRID_PADDING + GRID_TAIL,
                        X2 = GRID_PADDING + w,
                        Y1 = centerY - i * GRID_SIZE,
                        Y2 = centerY - i * GRID_SIZE,
                        Stroke = i == 0 ? Brushes.Blue : (i % 5 == 0 ? Brushes.DarkBlue : gridBrush),// 如果是基准线则变蓝
                        StrokeThickness = 0.3
                    });

                    // 绘制 Y 轴数值
                    if (i % 5 == 0) // 每5个配个数值
                    {


                        var text2 = new TextBlock
                        {
                            Text = (FieldConfig.BaseValue + i * FieldConfig.Step).ToString(),
                            Foreground = SubTextColor,
                            FontSize = 10
                        };
                        Canvas.SetLeft(text2, 5);
                        Canvas.SetTop(text2, centerY - i * GRID_SIZE - 7);
                        canvas.Children.Add(text2);
                    }
                }







            }

            double pixelsPerUnit = GRID_SIZE / FieldConfig.Step;
            double yMax = centerY - (FieldConfig.Max - FieldConfig.BaseValue) * pixelsPerUnit;
            double yMin = centerY - (FieldConfig.Min - FieldConfig.BaseValue) * pixelsPerUnit;
            //Debug.WriteLine(pixelsPerUnit);
            //Debug.WriteLine(yMax);
            // --- 绘制 Max 红线 ---
            canvas.Children.Add(new Line
            {
                X1 = GRID_PADDING - GRID_TAIL * 3,
                X2 = GRID_PADDING + w,
                Y1 = yMax,
                Y2 = yMax,
                Stroke = Brushes.Red,// 如果是基准线则变蓝 每5个格子淡蓝色
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection { 2, 2 }
            });
            var textMax = new TextBlock
            {
                Text = FieldConfig.Max.ToString(),
                Foreground = Brushes.Red,
                FontSize = 10
            };
            Canvas.SetLeft(textMax, 5);
            Canvas.SetTop(textMax, yMax - 7);
            canvas.Children.Add(textMax);
            canvas.Children.Add(new Line
            {
                X1 = GRID_PADDING - GRID_TAIL * 3,
                X2 = GRID_PADDING + w,
                Y1 = yMin,
                Y2 = yMin,
                Stroke = Brushes.Red,// 如果是基准线则变蓝 每5个格子淡蓝色
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection { 2, 2 }
            });
            var textMin = new TextBlock
            {
                Text = FieldConfig.Min.ToString(),
                Foreground = Brushes.Red,
                FontSize = 10
            };
            Canvas.SetLeft(textMin, 5);
            Canvas.SetTop(textMin, yMin - 7);
            canvas.Children.Add(textMin);


            // --- 绘制 X 轴逻辑 (时间 & 纵向网格线) ---
            //// 假设每 5 分钟一个刻度，每 60 分钟一个大标签
            for (int m = 0; m <= totalMinutes; m += 1)
            {
                // 绘制纵向网格线
                canvas.Children.Add(new Line
                {
                    X1 = GRID_PADDING + m * GRID_SIZE,
                    X2 = GRID_PADDING + m * GRID_SIZE,
                    Y1 = GRID_PADDING - GRID_TAIL,
                    Y2 = GRID_PADDING + h + GRID_TAIL,
                    Stroke = m % 5 == 0 ? Brushes.Blue : gridBrush,
                    StrokeThickness = 0.3
                });

                // 绘制 X 轴时间文字
                if (m % 5 == 0) // 可以增加逻辑：如果是整点用不同颜色
                {
                    bool isHour = (m % 60 == 0);
                    var text = new TextBlock
                    {
                        Text = isHour ? $"{m / 60}h" : m.ToString(),
                        Foreground = isHour ? Brushes.Red : Brushes.Blue,
                        FontSize = 12
                    };

                    text.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    Canvas.SetLeft(text, GRID_PADDING + m * GRID_SIZE - text.DesiredSize.Width / 2);
                    Canvas.SetTop(text, GRID_PADDING + h + 10);
                    canvas.Children.Add(text);
                }
            }
        }




        private int selectedIndex = -1;
        private bool isDragging = false;
        private double dragStartY;
        private double dragStartValue;
        // 添加一个事件，当点被修改时触发
        public event EventHandler<PointValueChangedEventArgs> PointValueChanged;

        //中间基准线
        private void DrawPoints(Canvas canvas, double w, double h)
        {
            if (Points == null || FieldConfig == null) return;

            double centerY = GRID_PADDING + h / 2;

            for (int i = 0; i < Points.Count; i++)
            {

                double value = Points[i];
                double x = GRID_PADDING + i * GRID_SIZE;
                double y = centerY - (value - FieldConfig.BaseValue) * GRID_SIZE / FieldConfig.Step;

                // 确保点在画布范围内（但允许稍微超出，因为用户可能需要看到边界）
                y = Math.Max(GRID_PADDING - 10, Math.Min(GRID_PADDING + h + 10, y));


                var e = new Ellipse
                {
                    Width = 8,
                    Height = 8,
                    Fill = (i == selectedIndex) ? Brushes.Red : Brushes.Orange,
                    Stroke = (i == selectedIndex) ? Brushes.Yellow : Brushes.Transparent,
                    StrokeThickness = 2,
                    Tag = i,
                    //Cursor = Cursors.Hand
                };

                // 添加鼠标事件到每个点
                e.MouseDown += OnPointMouseDown;
                e.MouseMove += OnPointMouseMove;
                e.MouseUp += OnPointMouseUp;


                Canvas.SetLeft(e, x - 3);
                Canvas.SetTop(e, y - 3);

                canvas.Children.Add(e);
            }
        }

        private void DrawSelectedPointTooltip(Canvas canvas, double w, double h)
        {
            if (Points == null || selectedIndex < 0 || selectedIndex >= Points.Count) return;

            double centerY = GRID_PADDING + h / 2;
            double x = GRID_PADDING + selectedIndex * GRID_SIZE;
            double y = centerY - (Points[selectedIndex] - FieldConfig.BaseValue) * GRID_SIZE / FieldConfig.Step;

            // 创建提示框
            var tooltip = new Border
            {
                Background = new SolidColorBrush(Color.FromArgb(200, 50, 50, 50)),
                BorderBrush = Brushes.White,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(8, 4, 8, 4)
            };

            var text = new TextBlock
            {
                Text = $"索引: {selectedIndex}\n值: {Points[selectedIndex]:F2} {FieldConfig?.Unit ?? ""}",
                Foreground = Brushes.White,
                FontSize = 11,
                FontWeight = FontWeights.Bold
            };

            tooltip.Child = text;

            // 定位提示框
            double tooltipX = x + 10;
            double tooltipY = y - 30;

            // 确保提示框不超出画布
            tooltip.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            if (tooltipX + tooltip.DesiredSize.Width > ActualWidth - GRID_PADDING)
                tooltipX = x - tooltip.DesiredSize.Width - 10;
            if (tooltipY < GRID_PADDING)
                tooltipY = y + 10;

            Canvas.SetLeft(tooltip, tooltipX);
            Canvas.SetTop(tooltip, tooltipY);
            canvas.Children.Add(tooltip);

            // 绘制十字线
            var crosshairX = new Line
            {
                X1 = x,
                X2 = x,
                Y1 = GRID_PADDING,
                Y2 = GRID_PADDING + h,
                Stroke = Brushes.Yellow,
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection { 2, 2 }
            };

            var crosshairY = new Line
            {
                X1 = GRID_PADDING,
                X2 = GRID_PADDING + w,
                Y1 = y,
                Y2 = y,
                Stroke = Brushes.Yellow,
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection { 2, 2 }
            };

            canvas.Children.Add(crosshairX);
            canvas.Children.Add(crosshairY);
        }


        private double SnapToSubStep(double value)
        {
            if (FieldConfig == null || FieldConfig.SubStep <= 0) return value;
            return Math.Round(value / FieldConfig.SubStep) * FieldConfig.SubStep;
        }

        private void UpdatePointValue(int index, double newValue)
        {
            if (Points == null || index < 0 || index >= Points.Count) return;
            if (FieldConfig == null) return;

            // 吸附到步长
            newValue = SnapToSubStep(newValue);

            // 限制在最小最大值范围内
            newValue = Math.Max(FieldConfig.Min, Math.Min(FieldConfig.Max, newValue));

            if (Math.Abs(Points[index] - newValue) > 0.0001)
            {
                Points[index] = newValue;

                // 触发事件通知外部
                PointValueChanged?.Invoke(this, new PointValueChangedEventArgs(index, newValue));

                // 重新绘制
                Redraw();
            }
        }

        #region 鼠标交互

        private void OnPointMouseDown(object sender, MouseButtonEventArgs e)
        {
            var ellipse = sender as Ellipse;
            if (ellipse?.Tag is int index)
            {
                selectedIndex = index;
                isDragging = true;
                dragStartY = e.GetPosition(GridPointEditorCanvas).Y;
                dragStartValue = Points[index];

                ellipse.CaptureMouse();
                Redraw();
                e.Handled = true;

                // 确保控件获得焦点以接收键盘事件
                this.Focus();
            }
        }

        private void OnPointMouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging || selectedIndex < 0 || Points == null || FieldConfig == null) return;

            var ellipse = sender as Ellipse;
            if (ellipse == null) return;

            double currentY = e.GetPosition(GridPointEditorCanvas).Y;
            double deltaY = dragStartY - currentY;

            // 计算值的变化量
            double valueChange = deltaY * FieldConfig.Step / GRID_SIZE;
            double newValue = dragStartValue + valueChange;

            UpdatePointValue(selectedIndex, newValue);
        }

        private void OnPointMouseUp(object sender, MouseButtonEventArgs e)
        {
            var ellipse = sender as Ellipse;
            if (ellipse != null)
            {
                ellipse.ReleaseMouseCapture();
            }
            isDragging = false;
        }

        private void OnUserControlMouseDown(object sender, MouseButtonEventArgs e)
        {
            // 点击空白区域取消选择
            if (e.OriginalSource == GridPointEditorCanvas ||
                (e.OriginalSource is FrameworkElement element && element.Parent == GridPointEditorCanvas))
            {
                selectedIndex = -1;
                Redraw();
                this.Focus();
            }
        }

        #endregion

        #region 键盘交互

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (selectedIndex < 0 || Points == null || FieldConfig == null) return;

            double step = FieldConfig.SubStep > 0 ? FieldConfig.SubStep : FieldConfig.Step;

            switch (e.Key)
            {
                case Key.Up:
                    // 上键：增加数值
                    UpdatePointValue(selectedIndex, Points[selectedIndex] + step);
                    e.Handled = true;
                    break;

                case Key.Down:
                    // 下键：减少数值
                    UpdatePointValue(selectedIndex, Points[selectedIndex] - step);
                    e.Handled = true;
                    break;

                case Key.Left:
                    // 左键：选择上一个点
                    if (selectedIndex > 0)
                    {
                        selectedIndex--;
                        Redraw();
                    }
                    e.Handled = true;
                    break;

                case Key.Right:
                    // 右键：选择下一个点
                    if (selectedIndex < Points.Count - 1)
                    {
                        selectedIndex++;
                        Redraw();
                    }
                    e.Handled = true;
                    break;

                case Key.Home:
                    // Home键：选择第一个点
                    selectedIndex = 0;
                    Redraw();
                    e.Handled = true;
                    break;

                case Key.End:
                    // End键：选择最后一个点
                    selectedIndex = Points.Count - 1;
                    Redraw();
                    e.Handled = true;
                    break;

                case Key.PageUp:
                    // PageUp：增加较大数值
                    UpdatePointValue(selectedIndex, Points[selectedIndex] + step * 10);
                    e.Handled = true;
                    break;

                case Key.PageDown:
                    // PageDown：减少较大数值
                    UpdatePointValue(selectedIndex, Points[selectedIndex] - step * 10);
                    e.Handled = true;
                    break;

                case Key.Delete:
                    // Delete：重置为基准值
                    UpdatePointValue(selectedIndex, FieldConfig.BaseValue);
                    e.Handled = true;
                    break;
            }
        }

        #endregion

       
    }

    // 事件参数类
    public class PointValueChangedEventArgs : EventArgs
    {
        public int Index { get; }
        public double NewValue { get; }

        public PointValueChangedEventArgs(int index, double newValue)
        {
            Index = index;
            NewValue = newValue;
        }
    }
}

