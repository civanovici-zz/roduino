using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;

namespace RoDuino.SMS.Components.RoApplicationMenu.ViewStates
{
    public class VForm : ViewStateBase
    {
        private double y = -0.6;

        protected override Motion GetPreviousMotion(int index)
        {
            Motion m = new Motion();

            m.Axis = new Vector3D(0, 1, 0);
            m.X = -1 * (Owner.FrontItemGap + Owner.ItemGap * (Owner.SelectedIndex - index)) + Owner.XOffset;
            m.Z = -1 * Owner.ItemGap * (Owner.SelectedIndex - index) + Owner.ZOffset;
            m.Y = Owner.YOffset;
            ChangeStateForElement(index, Visibility.Collapsed);
            return m;
        }

        protected override Motion GetNextMotion(int index)
        {
            Motion m = new Motion();

            m.Axis = new Vector3D(0, 1, 0);
            m.X = Owner.FrontItemGap + Owner.ItemGap * (index - Owner.SelectedIndex) + Owner.XOffset;
            m.Z = -1 * Owner.ItemGap * (index - Owner.SelectedIndex) + Owner.ZOffset;
            m.Y = Owner.YOffset;
            ChangeStateForElement(index, Visibility.Collapsed);
            return m;
        }

        protected override Motion GetSelectionMotion(int index)
        {
            Motion m = new Motion();

            m.Axis = new Vector3D(0, 1, 0);
            m.Z = Owner.PopoutDistance + Owner.ZOffset;
            m.Y = Owner.YOffset;
            m.X = Owner.XOffset;
            ChangeStateForElement(index, Visibility.Visible);

            Viewport2DVisual3D model = (Viewport2DVisual3D)Owner._modelContainer.Children[index];
            LoginGrid loginGrid = (LoginGrid)model.Visual;

            if (Owner._modelContainer.Children.Count <= 1)
            {
                loginGrid.imgMoveToRight.Visibility = Visibility.Collapsed;
                loginGrid.imgMoveToLeft.Visibility = Visibility.Collapsed;
            }
            else
            {
                loginGrid.imgMoveToLeft.Visibility = Owner.SelectedIndex < Owner._modelContainer.Children.Count - 1
                                                         ? Visibility.Visible
                                                         : Visibility.Collapsed;
                loginGrid.imgMoveToRight.Visibility = Owner.SelectedIndex > 0
                                                          ? Visibility.Visible
                                                          : Visibility.Collapsed;
            }

            return m;
        }

        public LoginGrid LoginGrid
        {
            get
            {
                Viewport2DVisual3D model = (Viewport2DVisual3D)Owner._modelContainer.Children[0];
                return (LoginGrid)model.Visual;
            }
        }

        private void ChangeStateForElement(int index, Visibility visibility)
        {
            Storyboard sbOpacity = null;
            Storyboard sbGrayScale = null;
            Storyboard sbBlur = null;
            Viewport2DVisual3D model = (Viewport2DVisual3D)Owner._modelContainer.Children[index];
            LoginGrid loginGrid = (LoginGrid)model.Visual;

            loginGrid.imgMoveToRight.Visibility = Visibility.Collapsed;
            loginGrid.imgMoveToLeft.Visibility = Visibility.Collapsed;

            if (visibility == Visibility.Visible)
            {
                sbOpacity = (Storyboard)(loginGrid).FindResource("sbFadein");
                sbGrayScale = (Storyboard)(loginGrid).FindResource("sbGrayEffectOut");
                //                sbBlur = (Storyboard)((LoginGrid)model.Visual).FindResource("sbBlurEffectIn");
                ((BlurEffect)(loginGrid).Effect).Radius = 0;
            }
            else
            {
                sbOpacity = (Storyboard)(loginGrid).FindResource("sbFadeout");
                sbGrayScale = (Storyboard)(loginGrid).FindResource("sbGrayEffectIn");
                //                sbBlur = (Storyboard)((LoginGrid)model.Visual).FindResource("sbBlurEffectOut");
                ((BlurEffect)(loginGrid).Effect).Radius = 7;
            }
            sbOpacity.Begin((loginGrid));
            sbGrayScale.Begin((loginGrid));
            //            sbBlur.Begin(((LoginGrid)model.Visual));
        }
    }
}
