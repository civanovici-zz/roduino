using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace RoDuino.SMS.Components.RoApplicationMenu.ViewStates
{
        public abstract class ViewStateBase
    {
        protected ElementFlow Owner { get; private set; }

        internal void SetOwner(ElementFlow owner)
        {
            Owner = owner;
        }

        public virtual void SelectElement(int index)
        {
            Storyboard sb = null;
            for (int leftItem = 0; leftItem < index; leftItem++)
            {
                sb = Owner.PrepareTemplateStoryboard(leftItem);
                PrepareStoryboard(sb, GetPreviousMotion(leftItem));
                Owner.AnimateElement(sb);
            }

            sb = Owner.PrepareTemplateStoryboard(index);
            PrepareStoryboard(sb, GetSelectionMotion(index));
            Owner.AnimateElement(sb);

            for (int rightItem = index + 1; rightItem < Owner.VisibleChildrenCount; rightItem++)
            {
                sb = Owner.PrepareTemplateStoryboard(rightItem);
                PrepareStoryboard(sb, GetNextMotion(rightItem));
                Owner.AnimateElement(sb);
            }
        }

        private void PrepareStoryboard(Storyboard sb, Motion motion)
        {
            // Child animations
            AxisAngleRotation3D rotation = (sb.Children[0] as Rotation3DAnimation).To as AxisAngleRotation3D;
            DoubleAnimation xAnim = sb.Children[1] as DoubleAnimation;
            DoubleAnimation yAnim = sb.Children[2] as DoubleAnimation;
            DoubleAnimation zAnim = sb.Children[3] as DoubleAnimation;

            rotation.Angle = motion.Angle;
            rotation.Axis = motion.Axis;
            xAnim.To = motion.X;
            yAnim.To = motion.Y;
            zAnim.To = motion.Z;
        }

        protected abstract Motion GetPreviousMotion(int index);
        protected abstract Motion GetSelectionMotion(int index);
        protected abstract Motion GetNextMotion(int index);
//        public abstract Visual GetVisual();
    }
}
