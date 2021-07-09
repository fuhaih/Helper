using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class Point
    {
        private double x;
        public double X { get { return x; } }
        private double y;
        public double Y { get { return y; } }
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
