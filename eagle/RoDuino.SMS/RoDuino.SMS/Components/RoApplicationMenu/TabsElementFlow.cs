using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using RoDuino.SMS.Components.RoApplicationMenu.ViewStates;
using RoDuino.SMS.Controllers.Base;

namespace RoDuino.SMS.Components.RoApplicationMenu
{
    public class TabsElementFlow : ElementFlow
    {
        #region Delegates

        public delegate void CurrentElementMouseEnter(Tab tab);

        public delegate void CurrentElementMouseLeave(Tab tab);

        public delegate void TabClose(Tab tab);

        public delegate void TabLoad(Tab tab);

        #endregion

        public TabsElementFlow()
        {
            LoadViewport();
            SetupEventHandlers();

            CurrentView = new CoverFlow();
        }

        /// <summary>
        /// get tab from selected index element
        /// </summary>
        public Tab GetCurrentTab
        {
            get
            {
                Viewport2DVisual3D newSelectedModel = (Viewport2DVisual3D)_modelContainer.Children[SelectedIndex];
                ListBoxItem item = (ListBoxItem)newSelectedModel.GetValue(LinkedElementProperty);
                return (Tab)item.Content;
            }
        }

        public event TabLoad TabLoaded;
        public event TabClose TabClosed;
        public event CurrentElementMouseEnter CurrentElementMouseEntered;
        public event CurrentElementMouseLeave CurrentElementMouseLeaved;


        protected override void OnVisualRemoved(UIElement elt)
        {
            Viewport2DVisual3D model = elt.GetValue(LinkedModelProperty) as Viewport2DVisual3D;
            _modelContainer.Children.Remove(model);

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
            //load current tab
            Viewport2DVisual3D newSelectedModel = (Viewport2DVisual3D)_modelContainer.Children[SelectedIndex];
            ListBoxItem item = (ListBoxItem)newSelectedModel.GetValue(LinkedElementProperty);
            RaiseTabLoadedEvent((Tab)item.Content);
        }

        protected override void OnVisualAdded(UIElement elt)
        {
            if (elt is Viewport3D) return;

            int index = Children.IndexOf(elt);

            Viewport2DVisual3D model = CreateApplicationViewPort(elt);
            //set binding
            ApplicationGrid visual = (ApplicationGrid)model.Visual;
            visual.AllowDrop = true;
            visual.DataContext = elt;
            visual.DragEnter += TabsElementFlow_DragEnter;
            visual.MouseEnter += TabsElementFlow_MouseEnter;
            visual.MouseDown += TabsElementFlow_MouseDown;
            visual.CloseButton.Click += CloseButtonClicked;
            _modelContainer.Children.Insert(index, model);
            model.SetValue(LinkedElementProperty, elt);
            elt.SetValue(LinkedModelProperty, model);
            if (IsLoaded)
            {
                ReflowItems();
            }
            SelectedIndex = Children.Count - 1;
        }

        private void TabsElementFlow_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.Children.Count < 2)
                ((ApplicationGrid)e.Source).CloseButton.Visibility = Visibility.Collapsed;
            else
                ((ApplicationGrid)e.Source).CloseButton.Visibility =Visibility.Visible;
        }

        private void TabsElementFlow_DragEnter(object sender, DragEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)((FrameworkElement)e.Source).DataContext;
            RaiseTabLoadedEvent((Tab)item.Content);
        }


        private void TabsElementFlow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)((FrameworkElement)e.Source).DataContext;
            RaiseTabLoadedEvent((Tab)item.Content);
        }

        private void CloseButtonClicked(object sender, RoutedEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)((FrameworkElement)e.Source).DataContext;
            RaiseTabClosedEvent((Tab)item.Content);
        }

        protected override void OnContainerLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Viewport2DVisual3D model = e.Source as Viewport2DVisual3D;
            if (model != null)
            {
                SelectedIndex = _modelContainer.Children.IndexOf(model);

                //raise event to load 
                ListBoxItem item = (ListBoxItem)model.GetValue(LinkedElementProperty);
                RaiseTabLoadedEvent((Tab)item.Content);
            }
        }


        /// <summary>
        /// show current tab
        /// </summary>
        /// <param name="tab"></param>
        public void RaiseTabLoadedEvent(Tab tab)
        {
            if (TabLoaded != null)
            {
                if (tab != null)
                    TabLoaded(tab);
            }
        }

        /// <summary>
        /// raise event when mouse enter on current element 
        /// </summary>
        public void RaiseCurrentElementMouseEnterEvent(Tab tab)
        {
            if (CurrentElementMouseEntered != null)
                CurrentElementMouseEntered(tab);
        }

        /// <summary>
        /// raise event when mouse enter on current element 
        /// </summary>
        public void RaiseCloseButtonHideEvent(Tab tab)
        {
            if (CurrentElementMouseLeaved != null)
                CurrentElementMouseLeaved(tab);
        }

        /// <summary>
        /// raise event when close button pressed
        /// </summary>
        public void RaiseTabClosedEvent(Tab tab)
        {
            if (TabClosed != null)
                TabClosed(tab);
        }


        /// <summary>
        /// select UIElement by current tab
        /// </summary>
        /// <param name="current"></param>
        public void SelectIndexByTab(Tab current)
        {
            for (int i = 0; i < _modelContainer.Children.Count; i++)
            {
                Viewport2DVisual3D newSelectedModel = (Viewport2DVisual3D)_modelContainer.Children[i];
                ListBoxItem item = (ListBoxItem)newSelectedModel.GetValue(LinkedElementProperty);
                if (current.Id == ((Tab)item.Content).Id)
                {
                    SelectedIndex = i;
                    return;
                }
            }
        }


        private void model_MouseLeave(object sender, MouseEventArgs e)
        {
            if (SelectedIndex == _modelContainer.Children.IndexOf((Visual3D)sender))
            {
                ModelUIElement3D newSelectedModel = (ModelUIElement3D)_modelContainer.Children[SelectedIndex];
                ListBoxItem item = (ListBoxItem)newSelectedModel.GetValue(LinkedElementProperty);
                RaiseCloseButtonHideEvent((Tab)item.Content);
            }
        }

        private void model_MouseEnter(object sender, MouseEventArgs e)
        {
            if (SelectedIndex == _modelContainer.Children.IndexOf((Visual3D)sender))
            {
                ModelUIElement3D newSelectedModel = (ModelUIElement3D)_modelContainer.Children[SelectedIndex];
                ListBoxItem item = (ListBoxItem)newSelectedModel.GetValue(LinkedElementProperty);
                RaiseCurrentElementMouseEnterEvent((Tab)item.Content);
            }
        }
    }
}
