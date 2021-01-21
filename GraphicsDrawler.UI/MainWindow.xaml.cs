using System.Collections.Generic;
using System.Windows;
using GraphicsDrawlerLibrary;
using GraphicsDrawlerLibrary.Models;
using Point = GraphicsDrawlerLibrary.Models.Point;

namespace GraphicsDrawlerUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<Point> points = new List<Point>()
            {
                new Point(50, 60),
                new Point(76, 99),
                new Point(101, 105),
                new Point(128, 203),
                new Point(239, 197),
                new Point(438, 378),
                new Point(584, 230),
            };
            var drawler = new GraphicsDrawler(this.ChartExample);
            drawler.SetupDrawler();
            drawler.DrawPoints(points);
        }
    }
}
