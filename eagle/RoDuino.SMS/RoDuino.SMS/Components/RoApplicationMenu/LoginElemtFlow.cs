using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using RoDuino.SMS.Components.RoApplicationMenu.ViewStates;

namespace RoDuino.SMS.Components.RoApplicationMenu
{
    public class LoginElemtFlow : ElementFlow
    {
        public LoginElemtFlow()
        {
            LoadViewport();
            SetupEventHandlers();

            CurrentView = new VForm();
        }

        protected override void OnVisualRemoved(UIElement elt)
        {
            Viewport2DVisual3D model = elt.GetValue(LinkedModelProperty) as Viewport2DVisual3D;
            _modelContainer.Children.Remove(model);
            LoginGrid loginGrid = (LoginGrid)model.Visual;
            loginGrid.imgMoveToLeft.MouseDown -= MoveToLeft;
            loginGrid.imgMoveToRight.MouseDown -= MoveToRight;

            model.ClearValue(LinkedElementProperty);
            elt.ClearValue(LinkedModelProperty);
            SelectedIndex = 0;
            // Update SelectedIndex if needed
            if (SelectedIndex >= 0 && SelectedIndex < VisibleChildrenCount)
            {
                ReflowItems();
            }
            else
            {
                SelectedIndex = Math.Max(0, Math.Min(SelectedIndex, VisibleChildrenCount - 1));
            }
        }

        protected override void OnVisualAdded(UIElement elt)
        {
            if (elt is Viewport3D) return;

            int index = Children.IndexOf(elt);

            Viewport2DVisual3D model = CreateLoginViewPort(elt);
            //set binding
            LoginGrid loginGrid = (LoginGrid)model.Visual;
            loginGrid.DataContext = elt;
            //                        loginGrid.txtConnectionName.Text = ((ListBoxItem)loginGrid.DataContext).Content.ToString();
            loginGrid.imgMoveToLeft.MouseDown += MoveToLeft;
            loginGrid.imgMoveToRight.MouseDown += MoveToRight;

            _modelContainer.Children.Insert(index, model);
            model.SetValue(LinkedElementProperty, elt);
            elt.SetValue(LinkedModelProperty, model);
            if (IsLoaded)
            {
                ReflowItems();
            }
            SelectedIndex = Children.Count - 1;
        }

        private void MoveToLeft(object sender, MouseButtonEventArgs e)
        {
            if (this.SelectedIndex < this.Children.Count)
                this.ChangeIndex(this.SelectedIndex + 1);
        }

        private void MoveToRight(object sender, MouseButtonEventArgs e)
        {
            if (this.SelectedIndex > 0)
                this.ChangeIndex(this.SelectedIndex - 1);
        }

        protected override void OnContainerLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Viewport2DVisual3D model = e.Source as Viewport2DVisual3D;
            if (model != null)
            {
                SelectedIndex = _modelContainer.Children.IndexOf(model);
            }
        }
    }
}
