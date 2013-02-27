using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace RoDuino.SMS.Views
{
    public class RoDuinoPopup : View
    {
        private Popup _parentPopup;

        static RoDuinoPopup()
        {
            //This OverrideMetadata call tells the system that this element 
            //wants to provide a style that is different than its base class.
            //This style is defined in themes\generic.xaml
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RoDuinoPopup),
                                                     new FrameworkPropertyMetadata(typeof(RoDuinoPopup)));
        }

        public RoDuinoPopup()
        {
        }

        #region Properties to implement Popup Behavior

        //Placement
        public static readonly DependencyProperty HorizontalOffsetProperty =
            Popup.HorizontalOffsetProperty.AddOwner(typeof(RoDuinoPopup));

        public static readonly DependencyProperty IsOpenProperty =
            Popup.IsOpenProperty.AddOwner(
                typeof(RoDuinoPopup),
                new FrameworkPropertyMetadata(
                    false,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnIsOpenChanged));

        public static readonly DependencyProperty PlacementProperty =
            Popup.PlacementProperty.AddOwner(typeof(RoDuinoPopup));

        public static readonly DependencyProperty PlacementRectangleProperty =
            Popup.PlacementRectangleProperty.AddOwner(typeof(RoDuinoPopup));

        //PlacementTarget
        public static readonly DependencyProperty PlacementTargetProperty =
            Popup.PlacementTargetProperty.AddOwner(typeof(RoDuinoPopup));

        public static readonly DependencyProperty VerticalOffsetProperty =
            Popup.VerticalOffsetProperty.AddOwner(typeof(RoDuinoPopup));

        public PlacementMode Placement
        {
            get { return (PlacementMode)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }

        public UIElement PlacementTarget
        {
            get { return (UIElement)GetValue(PlacementTargetProperty); }
            set { SetValue(PlacementTargetProperty, value); }
        }

        //PlacementRectangle

        public Rect PlacementRectangle
        {
            get { return (Rect)GetValue(PlacementRectangleProperty); }
            set { SetValue(PlacementRectangleProperty, value); }
        }

        //HorizontalOffset

        public double HorizontalOffset
        {
            get { return (double)GetValue(HorizontalOffsetProperty); }
            set { SetValue(HorizontalOffsetProperty, value); }
        }

        //VerticalOffset

        public double VerticalOffset
        {
            get { return (double)GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }


        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RoDuinoPopup ctrl = (RoDuinoPopup)d;

            if ((bool)e.NewValue)
            {
                if (ctrl._parentPopup == null)
                {
                    ctrl.HookupParentPopup();
                }
            }
        }

        //Create the Popup and attach the CustomControl to it.
        private void HookupParentPopup()
        {
            _parentPopup = new Popup();

            _parentPopup.AllowsTransparency = true;

            Popup.CreateRootPopup(_parentPopup, this);
        }

        #endregion
    }
}
