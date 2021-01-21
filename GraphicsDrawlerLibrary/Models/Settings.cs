using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace GraphicsDrawlerLibrary.Models
{
    public class Settings
    {
        public static class DefaultColors
        {
            public static SolidColorBrush DefaultBoundariesColor = Brushes.Red;
            public static SolidColorBrush DefaultGridLinesColor = Brushes.LightGray;
            public static SolidColorBrush DefaultAxisesColor = Brushes.Black;
            public static SolidColorBrush DefaultLinesColor = Brushes.Black;
            public static SolidColorBrush DefaultPointsColor = Brushes.Black;
        }

        public int DefaultOffset { get; set; } = 45;

        public int AxisLabelsFontSize { get; set; } = 14;

        public int XAxisValuesMarginRight { get; set; } = 20;

        public int YAxisValuesMarginBottom { get; set; } = 15;

        public bool DrawGrid { get; set; } = true;

        public (int MinX, int MaxX, int numberOfPoints) XAxisSettings { get; set; } = (0, 1000, 10);
        public (int MinY, int MaxY, int NumberOfPoints) YAxisSettings { get; set; } = (0, 1000, 10);
    }
}
