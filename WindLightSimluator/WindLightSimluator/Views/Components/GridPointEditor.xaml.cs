using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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



namespace WindLightSimluator.Views.Components
{
    /// <summary>
    /// GridPointEditor.xaml 的交互逻辑
    /// </summary>
    public partial class GridPointEditor : UserControl
    {
       

        public ObservableCollection<EditableWeatherElement> Points
        {
            get => (ObservableCollection<EditableWeatherElement>)GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }

        public static readonly DependencyProperty PointsProperty =DependencyProperty.Register(nameof(Points), typeof(ObservableCollection<EditableWeatherElement>),typeof(GridPointEditor), new PropertyMetadata(null, OnDataChanged));


        public string SelectedField
        {
            get => (string)GetValue(SelectedFieldProperty);
            set => SetValue(SelectedFieldProperty, value);
        }
        public static readonly DependencyProperty SelectedFieldProperty =DependencyProperty.Register(nameof(SelectedField), typeof(string),typeof(GridPointEditor), new PropertyMetadata("Temperature", OnDataChanged));


        public DateTime StartTime
        {
            get => (DateTime)GetValue(StartTimeProperty);
            set => SetValue(StartTimeProperty, value);
        }

        public static readonly DependencyProperty StartTimeProperty = DependencyProperty.Register(nameof(StartTime), typeof(DateTime),typeof(GridPointEditor), new PropertyMetadata(DateTime.Now));

        public DateTime EndTime
        {
            get => (DateTime)GetValue(EndTimeProperty);
            set => SetValue(EndTimeProperty, value);
        }

        public static readonly DependencyProperty EndTimeProperty =DependencyProperty.Register(nameof(EndTime), typeof(DateTime), typeof(GridPointEditor), new PropertyMetadata(DateTime.Now));

        public double MinValue
        {
            get => (double)GetValue(MinValueProperty);
            set => SetValue(MinValueProperty, value);
        }

        public static readonly DependencyProperty MinValueProperty =DependencyProperty.Register(nameof(MinValue), typeof(double),typeof(GridPointEditor), new PropertyMetadata(0.0));

        public double MaxValue
        {
            get => (double)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        public static readonly DependencyProperty MaxValueProperty =DependencyProperty.Register(nameof(MaxValue), typeof(double),typeof(GridPointEditor), new PropertyMetadata(100.0));

        private EditableWeatherElement? selectedEditableWeatherElement;
        private bool isDragging = false;


        public GridPointEditor()
        {
            InitializeComponent();
            Loaded += (_, _) => Redraw();
            SizeChanged += (_, _) => Redraw();
        }

        static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as GridPointEditor)?.Redraw();
        }

        private void Redraw()
        {
            var canvas = GridPointEditorCanvas;
            canvas.Children.Clear();

            double w = ActualWidth;
            double h = ActualHeight;

            DrawGrid(canvas, w, h);
            DrawPoints(canvas, w, h);
        }

        private void DrawGrid(Canvas canvas, double w, double h)
        {
            double size = 10;

            for (double x = 0; x < w; x += size)
            {
                canvas.Children.Add(new Line
                {
                    X1 = x,
                    X2 = x,
                    Y1 = 0,
                    Y2 = h,
                    Stroke = Brushes.Gray,
                    StrokeThickness = 0.3
                });
            }

            for (double y = 0; y < h; y += size)
            {
                canvas.Children.Add(new Line
                {
                    X1 = 0,
                    X2 = w,
                    Y1 = y,
                    Y2 = y,
                    Stroke = Brushes.Gray,
                    StrokeThickness = 0.3
                });
            }
        }

        private void DrawPoints(Canvas canvas, double w, double h)
        {
            if (Points == null) return;

            foreach (var p in Points)
            {
                var val = GetValue(p);
                if (val == null) continue;

                double x = GetX(p.Time, w);
                double y = GetY(val.Value, h);

                var e = new Ellipse
                {
                    Width = 8,
                    Height = 8,
                    Fill = p == selectedEditableWeatherElement ? Brushes.Red : Brushes.Orange
                };

                Canvas.SetLeft(e, x - 4);
                Canvas.SetTop(e, y - 4);

                e.Tag = p;

                canvas.Children.Add(e);
            }
        }
        private double GetX(DateTime time, double width)
        {
            double total = (EndTime - StartTime).TotalMinutes;
            double cur = (time - StartTime).TotalMinutes;
            return cur / total * width;
        }

        private double GetY(double value, double height)
        {
            double percent = (value - MinValue) / (MaxValue - MinValue);
            return height * (1 - percent);
        }

        private double GetValue(EditableWeatherElement p)
        {
            return SelectedField switch
            {
                "Temperature" => p.Temperature ?? 0,
                "WindSpeed" => p.WindSpeed ?? 0,
                "WindDirection" => p.WindDirection ?? 0,
                "QNH" => p.QNH ?? 0,
                "QFE" => p.QFE ?? 0,
                _ => 0
            };
        }

        private void SetValue(EditableWeatherElement p, double value)
        {
            switch (SelectedField)
            {
                case "Temperature": p.Temperature = value; break;
                case "WindSpeed": p.WindSpeed = value; break;
                case "WindDirection": p.WindDirection = value; break;
                case "QNH": p.QNH = value; break;
                case "QFE": p.QFE = value; break;
            }
        }


      

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging || selectedEditableWeatherElement == null) return;

            var pos = e.GetPosition(GridPointEditorCanvas);

            double snapY = Math.Round(pos.Y / 10) * 10;

            double percent = 1 - snapY / ActualHeight;
            double value = MinValue + percent * (MaxValue - MinValue);

            SetValue(selectedEditableWeatherElement, value);

            Redraw();
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
        }

        private void GridPointEditorCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(GridPointEditorCanvas);

            foreach (UIElement child in GridPointEditorCanvas.Children)
            {
                if (child is Ellipse el)
                {
                    double x = Canvas.GetLeft(el) + 4;
                    double y = Canvas.GetTop(el) + 4;

                    if (Math.Abs(pos.X - x) < 6 && Math.Abs(pos.Y - y) < 6)
                    {
                        selectedEditableWeatherElement = (EditableWeatherElement)el.Tag;
                        isDragging = true;
                        break;
                    }
                }
            }

            Redraw();
        }
    }
}
