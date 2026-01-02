using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WindLightSimluator.Components.Base
{
    public class CustomGrid : Grid
    {
        public static readonly DependencyProperty ShowGridLinesProperty =
            DependencyProperty.Register(nameof(ShowGridLines), typeof(bool), typeof(CustomGrid),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

        public bool ShowGridLines
        {
            get => (bool)GetValue(ShowGridLinesProperty);
            set => SetValue(ShowGridLinesProperty, value);
        }

        public static readonly DependencyProperty GridLineBrushProperty =
            DependencyProperty.Register(nameof(GridLineBrush), typeof(Brush), typeof(CustomGrid),
                new FrameworkPropertyMetadata(Brushes.Gray, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush GridLineBrush
        {
            get => (Brush)GetValue(GridLineBrushProperty);
            set => SetValue(GridLineBrushProperty, value);
        }

        public static readonly DependencyProperty GridLineThicknessProperty =
            DependencyProperty.Register(nameof(GridLineThickness), typeof(double), typeof(CustomGrid),
                new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double GridLineThickness
        {
            get => (double)GetValue(GridLineThicknessProperty);
            set => SetValue(GridLineThicknessProperty, value);
        }

        public CustomGrid()
        {
            SizeChanged += (s, e) => InvalidateVisual();
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (!ShowGridLines) return;

            var pen = new Pen(GridLineBrush, GridLineThickness);

            double x = 0;
            foreach (var col in ColumnDefinitions)
            {
                dc.DrawLine(pen, new Point(x, 0), new Point(x, RenderSize.Height));
                x += col.ActualWidth;
            }
            dc.DrawLine(pen, new Point(x, 0), new Point(x, RenderSize.Height));

            double y = 0;
            foreach (var row in RowDefinitions)
            {
                dc.DrawLine(pen, new Point(0, y), new Point(RenderSize.Width, y));
                y += row.ActualHeight;
            }
            dc.DrawLine(pen, new Point(0, y), new Point(RenderSize.Width, y));
        }
    }
}
