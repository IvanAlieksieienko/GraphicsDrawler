using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicsDrawlerLibrary.Models
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point() => this.X = this.Y = 0;

        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
