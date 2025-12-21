using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WindLightSimluator.Components.Base
{
    public class CapsuleBorder : Border
    {
        static CapsuleBorder()
        {
            // 设置默认的样式
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CapsuleBorder),
                new FrameworkPropertyMetadata(typeof(CapsuleBorder)));
        }

        // 自定义 CornerRadius 属性，但限制为高度的一半（胶囊形状）
        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(double), typeof(CapsuleBorder),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        // 确保最小尺寸考虑胶囊形状
        protected override Size MeasureOverride(Size constraint)
        {
            // 调用基类测量
            Size measuredSize = base.MeasureOverride(constraint);

            // 胶囊形状需要最小宽度等于高度（当 CornerRadius 为自动时）
            if (double.IsNaN(CornerRadius))
            {
                // 如果有子元素，确保考虑子元素的尺寸
                UIElement child = Child;
                if (child != null)
                {
                    child.Measure(constraint);
                    Size childSize = child.DesiredSize;

                    // 胶囊形状的最小宽度至少是高度的2倍（两个半圆）
                    double minWidth = Math.Max(childSize.Width, childSize.Height);

                    // 加上内边距
                    Thickness padding = Padding;
                    minWidth += padding.Left + padding.Right;

                    // 加上边框厚度
                    Thickness borderThickness = BorderThickness;
                    minWidth += borderThickness.Left + borderThickness.Right;

                    measuredSize = new Size(
                        Math.Max(measuredSize.Width, minWidth),
                        measuredSize.Height);
                }
            }

            return measuredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            // 先调用基类的布局
            Size arrangedSize = base.ArrangeOverride(finalSize);

            // 对于胶囊形状，如果高度为0或很小，则使用宽度的一半作为半径
            if (finalSize.Height <= 0)
            {
                return arrangedSize;
            }

            // 如果有子元素，确保子元素在胶囊形状内正确布局
            UIElement child = Child;
            if (child != null)
            {
                // 计算内边距和边框的影响
                Thickness borderThickness = BorderThickness;
                Thickness padding = Padding;

                // 胶囊形状的内部空间是矩形减去两端的半圆
                double radius = Math.Min(finalSize.Height / 2, finalSize.Width / 2);
                if (!double.IsNaN(CornerRadius) && CornerRadius > 0)
                {
                    radius = Math.Min(CornerRadius, Math.Min(finalSize.Height / 2, finalSize.Width / 2));
                }

                // 子元素的可用宽度需要减去两侧的半圆曲边部分
                // 实际上，子元素可以放在整个矩形区域内，但内容应考虑曲线边界的视觉影响
                Rect childRect = new Rect(
                    borderThickness.Left + padding.Left,
                    borderThickness.Top + padding.Top,
                    Math.Max(0, finalSize.Width - borderThickness.Left - borderThickness.Right - padding.Left - padding.Right),
                    Math.Max(0, finalSize.Height - borderThickness.Top - borderThickness.Bottom - padding.Top - padding.Bottom));

                child.Arrange(childRect);
            }

            return arrangedSize;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            double width = ActualWidth;
            double height = ActualHeight;

            // 如果尺寸无效，不进行绘制
            if (width <= 0 || height <= 0)
                return;

            // 计算半径
            double radius = height / 2;

            // 如果设置了自定义的 CornerRadius，使用它（但不能超过高度的一半）
            if (!double.IsNaN(CornerRadius) && CornerRadius > 0)
            {
                radius = Math.Min(CornerRadius, height / 2);
            }

            // 确保半径不超过宽度的一半，防止绘制错误
            if (radius > width / 2)
            {
                radius = width / 2;

                // 如果是极端情况（宽度小于高度），调整半径使用宽度的一半
                // 这样可以保证胶囊形状始终有效
                if (height > width)
                {
                    // 使用椭圆形状而不是胶囊形状
                    radius = width / 2;
                }
            }

            // 创建胶囊形状的几何图形
            StreamGeometry geometry = new StreamGeometry();

            using (StreamGeometryContext ctx = geometry.Open())
            {
                // 从左侧开始（顶部中点）
                ctx.BeginFigure(new Point(radius, 0), true, true);

                // 顶部直线（右侧）
                if (width > 2 * radius)
                {
                    ctx.LineTo(new Point(width - radius, 0), true, false);
                }

                // 右侧半圆（上半部分到下半部分）
                ctx.ArcTo(
                    new Point(width - radius, height),  // 终点
                    new Size(radius, radius),           // 半径
                    0,                                  // 旋转角度
                    false,                              // 是否大于180度
                    SweepDirection.Clockwise,           // 方向
                    true,                               // 是否描画直线到起点
                    false);

                // 底部直线（左侧）
                if (width > 2 * radius)
                {
                    ctx.LineTo(new Point(radius, height), true, false);
                }

                // 左侧半圆（下半部分到上半部分）
                ctx.ArcTo(
                    new Point(radius, 0),               // 终点
                    new Size(radius, radius),           // 半径
                    0,                                  // 旋转角度
                    false,                              // 是否大于180度
                    SweepDirection.Clockwise,           // 方向
                    true,                               // 是否描画直线到起点
                    false);
            }

            geometry.Freeze();

            // 创建画笔
            Pen borderPen = null;
            if (BorderBrush != null && BorderThickness.Left > 0)
            {
                borderPen = new Pen(BorderBrush, BorderThickness.Left);
                borderPen.Freeze();
            }

            // 绘制胶囊形状
            drawingContext.DrawGeometry(Background, borderPen, geometry);

            // 注意：我们不调用 base.OnRender(drawingContext)，
            // 因为我们已经完全覆盖了绘制逻辑
        }
    }
}

