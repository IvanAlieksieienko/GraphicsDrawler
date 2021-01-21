using GraphicsDrawlerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace GraphicsDrawlerLibrary
{
    public class GraphicsDrawler
    {
        private Grid surface;
        private Point zeroPoint;
        private double xAxisRealWidth => this.surface.Width - this.Settings.DefaultOffset - this.zeroPoint.X;
        private double yAxisRealWidth => this.surface.Height - this.Settings.DefaultOffset * 2;

        public Settings Settings { get; set; }

        public GraphicsDrawler()
        {
            this.Settings = new Settings();
        }

        public GraphicsDrawler(Grid surface)
        {
            this.surface = surface;
            this.Settings = new Settings();
        }

        public GraphicsDrawler(Grid surface, Settings settings)
        {
            this.surface = surface;
            this.Settings = new Settings();
        }

        public void SetupDrawler()
        {
            this.CalculateZeroPoint();
            this.DrawAxises();
            this.DrawScale();
            this.DrawBoundaries();
        }

        public void DrawLine(Point startPoint, Point endPoint, SolidColorBrush color = null, bool isMakePointsAppropriate = false, bool isDrawPoints = false)
        {
            if (isDrawPoints)
            {
                this.DrawPoint(startPoint);
                this.DrawPoint(endPoint);
            }
            if (isMakePointsAppropriate)
            {
                startPoint = this.GetPointAppropriateToScale(startPoint);
                startPoint.X += this.Settings.DefaultOffset;
                startPoint.Y = this.surface.Height - startPoint.Y - this.Settings.DefaultOffset;
                endPoint = this.GetPointAppropriateToScale(endPoint);
                endPoint.X += this.Settings.DefaultOffset;
                endPoint.Y = this.surface.Height - endPoint.Y - this.Settings.DefaultOffset;
            }
            Line newLine = new Line()
            {
                X1 = startPoint.X,
                Y1 = startPoint.Y,
                X2 = endPoint.X,
                Y2 = endPoint.Y,
                Stroke = color ?? Settings.DefaultColors.DefaultLinesColor
            };
            this.surface.Children.Add(newLine);
        }

        public void DrawPoints(List<Point> points)
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                this.DrawLine(points[i], points[i + 1], isMakePointsAppropriate: true, isDrawPoints: true);
            }
        }

        public void DrawAxises()
        {
            this.DrawLine(this.zeroPoint, new Point((int)this.surface.Width - this.Settings.DefaultOffset, this.zeroPoint.Y), color: Settings.DefaultColors.DefaultAxisesColor);
            this.DrawLine(this.zeroPoint, new Point(this.zeroPoint.X, this.Settings.DefaultOffset), color: Settings.DefaultColors.DefaultAxisesColor);
        }

        public void DrawScale()
        {
            this.PrintText(Settings.XAxisSettings.MinX.ToString(), new Point(this.zeroPoint.X - 10, this.zeroPoint.Y));
            double oneXPointWidth = this.xAxisRealWidth / Settings.XAxisSettings.numberOfPoints;
            var minMaxXDifference = Settings.XAxisSettings.MaxX - Settings.XAxisSettings.MinX; // entered x axis width
            double oneXPointValue = minMaxXDifference / (double)Settings.XAxisSettings.numberOfPoints;
            for (int i = 0; i < Settings.XAxisSettings.numberOfPoints; i++)
            {
                if (this.Settings.DrawGrid)
                {
                    this.DrawLine(new Point(this.zeroPoint.X + oneXPointWidth * (i + 1), this.zeroPoint.Y),
                                    new Point(this.zeroPoint.X + oneXPointWidth * (i + 1), this.Settings.DefaultOffset), Settings.DefaultColors.DefaultGridLinesColor);
                }
                this.DrawLine(new Point((int)(this.zeroPoint.X + oneXPointWidth * (i + 1)), this.zeroPoint.Y - 5),
                                new Point((int)(this.zeroPoint.X + oneXPointWidth * (i + 1)), this.zeroPoint.Y + 5));
                this.PrintText((Settings.XAxisSettings.MinX + oneXPointValue * (i + 1)).ToString(),
                    new Point((int)(this.zeroPoint.X - this.Settings.XAxisValuesMarginRight + oneXPointWidth * (i + 1)), this.zeroPoint.Y));
            }

            var oneYPointHeight = this.yAxisRealWidth / Settings.YAxisSettings.NumberOfPoints;
            var minMaxYDifference = Settings.YAxisSettings.MaxY - Settings.YAxisSettings.MinY;
            double oneYPointValue = (double)minMaxYDifference / (double)Settings.YAxisSettings.NumberOfPoints;
            for (int i = 0; i < Settings.YAxisSettings.NumberOfPoints; i++)
            {
                if (this.Settings.DrawGrid)
                {
                    this.DrawLine(new Point(this.zeroPoint.X, this.zeroPoint.Y - oneYPointHeight * (i + 1)),
                                    new Point(this.surface.Width - this.Settings.DefaultOffset, this.zeroPoint.Y - oneYPointHeight * (i + 1)), Settings.DefaultColors.DefaultGridLinesColor);
                }
                this.DrawLine(new Point(this.zeroPoint.X - 5, (int)(this.zeroPoint.Y - oneYPointHeight * (i + 1))),
                                new Point(this.zeroPoint.X + 5, (int)(this.zeroPoint.Y - oneYPointHeight * (i + 1))));
                this.PrintText((Settings.YAxisSettings.MinY + oneYPointValue * (i + 1)).ToString(), new Point(0, (int)(this.zeroPoint.Y - this.Settings.YAxisValuesMarginBottom - oneYPointHeight * (i + 1))));
            }
        }

        public void SetScalesSettings((int MinX, int MaxX, int numberOfPoints) xSettings, (int MinY, int MaxY, int NumberOfPoints) ySettings)
        {
            this.Settings.XAxisSettings = xSettings;
            this.Settings.YAxisSettings = ySettings;
        }

        public Point GetPointAppropriateToScale(Point point) =>
            new Point(point.X * xAxisRealWidth / (this.Settings.XAxisSettings.MaxX - this.Settings.XAxisSettings.MinX),
                point.Y * yAxisRealWidth / (this.Settings.YAxisSettings.MaxY - this.Settings.YAxisSettings.MinY));

        public void DrawBoundaries()
        {
            this.DrawLine(new Point(0, 0), new Point(0, (int)this.surface.Height), Settings.DefaultColors.DefaultBoundariesColor);
            this.DrawLine(new Point(0, (int)this.surface.Height), new Point((int)this.surface.Width, (int)this.surface.Height), Settings.DefaultColors.DefaultBoundariesColor);
            this.DrawLine(new Point((int)this.surface.Width, (int)this.surface.Height), new Point((int)this.surface.Width, 0), Settings.DefaultColors.DefaultBoundariesColor);
            this.DrawLine(new Point(0, 0), new Point((int)this.surface.Width, 0), Settings.DefaultColors.DefaultBoundariesColor);
        }

        public void DrawPoint(Point point, double radius = 5, SolidColorBrush color = null)
        {
            var appropriatePoint = this.GetPointAppropriateToScale(point);
            Ellipse dot = new Ellipse();
            dot.VerticalAlignment = VerticalAlignment.Bottom;
            dot.HorizontalAlignment = HorizontalAlignment.Left;
            dot.Width = dot.Height = radius;
            dot.Margin = new Thickness(this.zeroPoint.X + appropriatePoint.X - dot.Width / 2, 0, 0, this.Settings.DefaultOffset + appropriatePoint.Y - dot.Height / 2);
            dot.Fill = color ?? Settings.DefaultColors.DefaultPointsColor;
            this.surface.Children.Add(dot);
        }

        public void PrintText(string text, Point startPoint, double? fontSize = null)
        {

            Label newTextLabel = new Label();
            newTextLabel.Content = text;
            newTextLabel.Margin = new Thickness(startPoint.X, startPoint.Y, 0, 0);
            newTextLabel.FontSize = fontSize ?? this.Settings.AxisLabelsFontSize;
            this.surface.Children.Add(newTextLabel);
        }

        private void CalculateZeroPoint() => this.zeroPoint = new Point(this.Settings.DefaultOffset, (int)this.surface.Height - this.Settings.DefaultOffset);
    }
}
