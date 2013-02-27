using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace RoDuino.SMS.Components.RoApplicationMenu.ViewStates
{
    public class CoverFlow : ViewStateBase
    {
        private double VisibleChildrenCount = 60;
        private int howMany = 3;

        protected override Motion GetPreviousMotion(int index)
        {
            Motion m = new Motion();

            double angle = Math.PI / 2 + UnitAngle * (index - Owner.SelectedIndex);
            m.X = Radius * Math.Cos(angle);
            m.Z = -1 * Owner.FrontItemGap + Radius * Math.Sin(angle);
            m.Y = 0;
            //            angle = -1 * (Owner.SelectedIndex - index) * UnitAngle;
            m.Angle = 180 / Math.PI * (Math.PI / 2 - angle);// Owner.TiltAngle;
            m.Axis = new Vector3D(0, 1, 0);

            return m;
        }

        protected override Motion GetNextMotion(int index)
        {
            Motion m = new Motion();


            double angle = Math.PI / 2 + UnitAngle * (index - Owner.SelectedIndex);
            m.X = Radius * Math.Cos(angle);
            m.Z = -1 * Owner.FrontItemGap + Radius * Math.Sin(angle);
            m.Y = 0;
            //            angle = -1 * (Owner.SelectedIndex - index) * UnitAngle;
            m.Angle = 180 / Math.PI * (Math.PI / 2 - angle);// Owner.TiltAngle;
            m.Axis = new Vector3D(0, 1, 0);


            return m;
        }

        protected override Motion GetSelectionMotion(int index)
        {
            Motion m = new Motion();


            double angle = Math.PI / 2;
            m.X = Radius * Math.Cos(angle);
            m.Z = Owner.PopoutDistance + Radius * Math.Sin(angle);
            m.Y = 0;
            m.Axis = new Vector3D(0, 1, 0);
            return m;
        }


        private double UnitAngle
        {
            get
            {
                return 2 * Math.PI / VisibleChildrenCount;
                //                return 2 * Math.PI / Owner.VisibleChildrenCount;
            }
        }

        private double Radius
        {
            get
            {
                return VisibleChildrenCount * Owner.ItemGap / (2 * Math.PI);
                //                return Owner.VisibleChildrenCount * Owner.ItemGap / (2 * Math.PI);
            }
        }
    }
}
