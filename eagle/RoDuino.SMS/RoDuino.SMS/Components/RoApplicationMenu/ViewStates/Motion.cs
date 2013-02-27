using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace RoDuino.SMS.Components.RoApplicationMenu.ViewStates
{
    public struct Motion
    {
        public Vector3D Axis { get; set; }
        public double Angle { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}
