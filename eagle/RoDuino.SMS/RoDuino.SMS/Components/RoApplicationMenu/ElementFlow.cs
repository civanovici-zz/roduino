using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using RoDuino.SMS.Components.RoApplicationMenu.ViewStates;

namespace RoDuino.SMS.Components.RoApplicationMenu
{
    public abstract partial class ElementFlow : Panel
    {
        #region Fields

        protected internal ContainerUIElement3D _modelContainer;
        private Viewport3D _viewport;

        #endregion

        #region Properties

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public double TiltAngle
        {
            get { return (double)GetValue(TiltAngleProperty); }
            set { SetValue(TiltAngleProperty, value); }
        }

        public double ItemGap
        {
            get { return (double)GetValue(ItemGapProperty); }
            set { SetValue(ItemGapProperty, value); }
        }

        public double FrontItemGap
        {
            get { return (double)GetValue(FrontItemGapProperty); }
            set { SetValue(FrontItemGapProperty, value); }
        }

        public double PopoutDistance
        {
            get { return (double)GetValue(PopoutDistanceProperty); }
            set { SetValue(PopoutDistanceProperty, value); }
        }

        public ViewStateBase CurrentView
        {
            get { return (ViewStateBase)GetValue(CurrentViewProperty); }
            set { SetValue(CurrentViewProperty, value); }
        }

        public double ElementWidth
        {
            get { return (double)GetValue(ElementWidthProperty); }
            set { SetValue(ElementWidthProperty, value); }
        }

        public double ElementHeight
        {
            get { return (double)GetValue(ElementHeightProperty); }
            set { SetValue(ElementHeightProperty, value); }
        }

        public PerspectiveCamera Camera
        {
            get { return (PerspectiveCamera)GetValue(CameraProperty); }
            set { SetValue(CameraProperty, value); }
        }


        public double XOffset
        {
            get { return (double)GetValue(XOffsetProperty); }
            set { SetValue(XOffsetProperty, value); }
        }

        public double YOffset
        {
            get { return (double)GetValue(YOffsetProperty); }
            set { SetValue(YOffsetProperty, value); }
        }

        public double ZOffset
        {
            get { return (double)GetValue(ZOffsetProperty); }
            set { SetValue(ZOffsetProperty, value); }
        }

        public double MeshHeight
        {
            get { return (double)GetValue(MeshHeightProperty); }
            set { SetValue(MeshHeightProperty, value); }
        }

        public double MeshWidth
        {
            get { return (double)GetValue(MeshWidthProperty); }
            set { SetValue(MeshWidthProperty, value); }
        }

        private ResourceDictionary InternalResources { get; set; }

        /* This gives an accurate count of the number of visible children. 
         * Panel.Children is not always accurate and is generally off-by-one.
         */

        internal int VisibleChildrenCount
        {
            get
            {
                //                return 1;
                return _modelContainer.Children.Count;
            }
        }

        public bool HasReflection { get; set; }

        #endregion

        #region Events

        public event EventHandler SelectedIndexChanged;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty CameraProperty = DependencyProperty.Register(
            "Camera", typeof(PerspectiveCamera), typeof(ElementFlow),
            new PropertyMetadata(null, OnCameraChanged));

        public static readonly DependencyProperty CurrentViewProperty =
            DependencyProperty.Register("CurrentView", typeof(ViewStateBase), typeof(ElementFlow),
                                        new FrameworkPropertyMetadata(null, OnCurrentViewChanged));

        public static readonly DependencyProperty ElementHeightProperty =
            DependencyProperty.Register("ElementHeight", typeof(double), typeof(ElementFlow),
                                        new FrameworkPropertyMetadata(300.0));

        public static readonly DependencyProperty ElementWidthProperty =
            DependencyProperty.Register("ElementWidth", typeof(double), typeof(ElementFlow),
                                        new FrameworkPropertyMetadata(400.0));

        public static readonly DependencyProperty FrontItemGapProperty =
            DependencyProperty.Register("FrontItemGap", typeof(double), typeof(ElementFlow),
                                        new PropertyMetadata(0.65, OnFrontItemGapChanged));

        public static readonly DependencyProperty HasReflectionProperty =
            DependencyProperty.Register("HasReflection", typeof(bool), typeof(ElementFlow),
                                        new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty ItemGapProperty =
            DependencyProperty.Register("ItemGap", typeof(double), typeof(ElementFlow),
                                        new PropertyMetadata(0.25, OnItemGapChanged));

        protected static readonly DependencyProperty LinkedElementProperty =
            DependencyProperty.Register("LinkedElement", typeof(UIElement), typeof(ElementFlow));

        //        private static readonly DependencyProperty LinkedModelProperty =
        //            DependencyProperty.Register("LinkedModel", typeof(ModelUIElement3D), typeof(ElementFlow));
        protected static readonly DependencyProperty LinkedModelProperty =
            DependencyProperty.Register("LinkedModel", typeof(Viewport2DVisual3D), typeof(ElementFlow));

        public static readonly DependencyProperty MeshHeightProperty =
            DependencyProperty.Register("MeshHeight", typeof(double), typeof(ElementFlow),
                                        new FrameworkPropertyMetadata(1.2));

        public static readonly DependencyProperty MeshWidthProperty =
            DependencyProperty.Register("MeshWidth", typeof(double), typeof(ElementFlow),
                                        new FrameworkPropertyMetadata(0.9));

        public static readonly DependencyProperty PopoutDistanceProperty =
            DependencyProperty.Register("PopoutDistance", typeof(double), typeof(ElementFlow),
                                        new PropertyMetadata(1.0, OnPopoutDistanceChanged));

        public static readonly DependencyProperty SelectedIndexProperty =
            Selector.SelectedIndexProperty.AddOwner(typeof(ElementFlow),
                                                    new FrameworkPropertyMetadata(-1,
                                                                                  FrameworkPropertyMetadataOptions.
                                                                                      Inherits |
                                                                                  FrameworkPropertyMetadataOptions.
                                                                                      BindsTwoWayByDefault,
                                                                                  OnSelectedIndexChanged));

        public static readonly DependencyProperty TiltAngleProperty =
            DependencyProperty.Register("TiltAngle", typeof(double), typeof(ElementFlow),
                                        new PropertyMetadata(45.0, OnTiltAngleChanged));

        public static readonly DependencyProperty XOffsetProperty =
            DependencyProperty.Register("XOffset", typeof(double), typeof(ElementFlow),
                                        new FrameworkPropertyMetadata(0.0));

        public static readonly DependencyProperty YOffsetProperty =
            DependencyProperty.Register("YOffset", typeof(double), typeof(ElementFlow),
                                        new FrameworkPropertyMetadata(-0.6));

        public static readonly DependencyProperty ZOffsetProperty =
            DependencyProperty.Register("ZOffset", typeof(double), typeof(ElementFlow),
                                        new FrameworkPropertyMetadata(0.0));

        #endregion

        #region Initialization

        public ElementFlow()
        {
            LoadViewport();
            SetupEventHandlers();

            CurrentView = new CoverFlow();
        }

        protected void SetupEventHandlers()
        {
            _modelContainer.MouseLeftButtonDown += OnContainerLeftButtonDown;
            //            _modelContainer.DragEnter += _modelContainer_DragEnter;
            //            _modelContainer.Drop += _modelContainer_Drop;

            Loaded += ElementFlow_Loaded;
        }

        protected void LoadViewport()
        {
            Uri uri = new Uri("/RoDuino.SMS;component/Components/RoApplicationMenu/Viewport3D.xaml", UriKind.Relative);
            _viewport = Application.LoadComponent(uri) as Viewport3D;
            InternalResources = _viewport.Resources;

            // Container for containing the mesh models of elements
            _modelContainer = _viewport.FindName("ModelContainer") as ContainerUIElement3D;
        }

        #endregion

        #region DependencyProperty PropertyChange Callbacks

        private static void OnTiltAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ElementFlow cf = d as ElementFlow;
            cf.ReflowItems();
        }

        private static void OnItemGapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ElementFlow ef = d as ElementFlow;
            ef.ReflowItems();
        }

        private static void OnFrontItemGapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ElementFlow ef = d as ElementFlow;
            ef.ReflowItems();
        }

        private static void OnPopoutDistanceChanged(DependencyObject d,
                                                    DependencyPropertyChangedEventArgs e)
        {
            ElementFlow ef = d as ElementFlow;
            ef.ReflowItems();
        }

        private static void OnSelectedIndexChanged(DependencyObject d,
                                                   DependencyPropertyChangedEventArgs e)
        {
            ElementFlow ef = d as ElementFlow;
            if (ef.IsLoaded == false)
            {
                return;
            }

            int oldIndex = (int)e.OldValue;
            int newIndex = (int)e.NewValue;
            //            Console.WriteLine("old:{0} new:{1} selected:{2}" , oldIndex, newIndex,ef.SelectedIndex);

            ef.SelectItemCore(newIndex);
            if (oldIndex != newIndex && ef.SelectedIndexChanged != null)
            {
                ef.SelectedIndexChanged(ef, new EventArgs());
            }
        }

        private static void OnCurrentViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ElementFlow ef = d as ElementFlow;
            ViewStateBase newView = e.NewValue as ViewStateBase;
            if (newView == null)
            {
                throw new ArgumentNullException("e", "The CurrentView cannot be null");
            }
            newView.SetOwner(ef);
            ef.ReflowItems();
        }

        private static void OnCameraChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ElementFlow ef = d as ElementFlow;

            PerspectiveCamera camera = e.NewValue as PerspectiveCamera;
            if (camera == null)
            {
                throw new ArgumentNullException("e", "The Camera cannot be null");
            }

            ef._viewport.Camera = camera;
        }

        #endregion

        #region Event Handlers

        protected override void OnInitialized(EventArgs e)
        {
            AddVisualChild(_viewport);
        }

        private void ElementFlow_Loaded(object sender, RoutedEventArgs e)
        {
            SelectItemCore(SelectedIndex);
        }

        public void ChangeIndex(int index)
        {
            SelectedIndex = index;
        }

        protected virtual void OnContainerLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //should be implemented in inherited object
            Focus();
            Viewport2DVisual3D model = e.Source as Viewport2DVisual3D;
            if (model != null)
            {
                SelectedIndex = _modelContainer.Children.IndexOf(model);
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            //            Focus();

            if (e.Delta < 0)
            {
                SelectAdjacentItem(false);
            }
            else if (e.Delta > 0)
            {
                SelectAdjacentItem(true);
            }
        }

        //        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        //        {
        //            base.OnPreviewMouseWheel(e);
        //            RaiseCloseButtonHideEvent(null);
        //        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                SelectAdjacentItem(false);
            }
            else if (e.Key == Key.Right)
            {
                SelectAdjacentItem(true);
            }
        }

        #endregion

        #region Item Selection

        private void SelectItemCore(int index)
        {
            if (index >= 0 && index < VisibleChildrenCount)
            {
                CurrentView.SelectElement(index);
            }
        }

        internal Storyboard PrepareTemplateStoryboard(int index)
        {
            // Initialize storyboard
            Storyboard sb = (InternalResources["ElementAnimator"] as Storyboard).Clone();
            Rotation3DAnimation rotAnim = sb.Children[0] as Rotation3DAnimation;
            Storyboard.SetTargetProperty(rotAnim, BuildTargetPropertyPath(index, "Rotation"));

            DoubleAnimation xAnim = sb.Children[1] as DoubleAnimation;
            Storyboard.SetTargetProperty(xAnim, BuildTargetPropertyPath(index, "Translation-X"));

            DoubleAnimation yAnim = sb.Children[2] as DoubleAnimation;
            Storyboard.SetTargetProperty(yAnim, BuildTargetPropertyPath(index, "Translation-Y"));

            DoubleAnimation zAnim = sb.Children[3] as DoubleAnimation;
            Storyboard.SetTargetProperty(zAnim, BuildTargetPropertyPath(index, "Translation-Z"));

            return sb;
        }

        private PropertyPath BuildTargetPropertyPath(int index, string animType)
        {
            PropertyDescriptor childDesc = TypeDescriptor.GetProperties(_modelContainer).Find("Children", true);
            string pathString = string.Empty;
            if (animType == "Rotation")
            {
                pathString = "(0)[0].(1)[" + index + "].(2).(3).[0].(4)";
            }
            else if (animType == "Translation-X")
            {
                pathString = "(0)[0].(1)[" + index + "].(2).(3)[1].(5)";
            }
            else if (animType == "Translation-Y")
            {
                pathString = "(0)[0].(1)[" + index + "].(2).(3)[1].(6)";
            }
            else if (animType == "Translation-Z")
            {
                pathString = "(0)[0].(1)[" + index + "].(2).(3)[1].(7)";
            }

            PropertyPath x = new PropertyPath(pathString,
                                              Viewport3D.ChildrenProperty,
                                              childDesc,
                                              Viewport2DVisual3D.TransformProperty,
                                              Transform3DGroup.ChildrenProperty,
                                              RotateTransform3D.RotationProperty,
                                              TranslateTransform3D.OffsetXProperty,
                                              TranslateTransform3D.OffsetYProperty,
                                              TranslateTransform3D.OffsetZProperty);
            return x;
        }

        internal void AnimateElement(Storyboard sb)
        {
            sb.Begin(_viewport);
        }

        #endregion

        #region Layout overrides

        protected override int VisualChildrenCount
        {
            get
            {
                int count = base.VisualChildrenCount;
                count = (count == 0) ? 0 : 1;
                return count;
            }
        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            Size eltSize = new Size(ElementWidth, ElementHeight);
            // Arrange children so that their visualbrush has some width/height
            foreach (UIElement child in Children)
            {
                child.Arrange(new Rect(new Point(), eltSize));
            }

            _viewport.Arrange(new Rect(new Point(), finalSize));

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size eltSize = new Size(ElementWidth, ElementHeight);
            foreach (UIElement child in Children)
            {
                child.Measure(eltSize);
            }

            _viewport.Measure(availableSize);

            return availableSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index == 0)
            {
                return _viewport;
            }

            return null;
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded,
                                                        DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);

            if (visualAdded != null)
            {
                OnVisualAdded(visualAdded as UIElement);
            }

            if (visualRemoved != null)
            {
                OnVisualRemoved(visualRemoved as UIElement);
            }
        }

        #endregion

        #region Utility functions

        protected void ReflowItems()
        {
            SelectItemCore(SelectedIndex);
        }

        private void SelectAdjacentItem(bool isNext)
        {
            int index = -1;
            if (isNext == false) // Select previous
            {
                index = Math.Max(-1, SelectedIndex - 1);
            }
            else // Select next
            {
                index = Math.Min(VisibleChildrenCount - 1, SelectedIndex + 1);
            }

            if (index != -1)
            {
                SelectedIndex = index;
            }
        }

        protected abstract void OnVisualRemoved(UIElement elt);
        //        {
        //            Viewport2DVisual3D model = elt.GetValue(LinkedModelProperty) as Viewport2DVisual3D;
        //            _modelContainer.Children.Remove(model);
        //
        //            model.ClearValue(LinkedElementProperty);
        //            elt.ClearValue(LinkedModelProperty);
        //            SelectedIndex = 0;
        //            // Update SelectedIndex if needed
        //            if (SelectedIndex >= 0 && SelectedIndex < VisibleChildrenCount)
        //            {
        //                ReflowItems();
        //            }
        //            else
        //            {
        //                SelectedIndex = Math.Max(0, Math.Min(SelectedIndex, VisibleChildrenCount - 1));
        //            }
        //        }

        protected abstract void OnVisualAdded(UIElement elt);
        //        {
        //implement in inherited member

        //            if (elt is Viewport3D) return;
        //
        //            int index = Children.IndexOf(elt);
        //
        //            Viewport2DVisual3D model = CreateApplicationViewPort(elt);
        //
        //            _modelContainer.Children.Insert(index, model);
        //            model.SetValue(LinkedElementProperty, elt);
        //            elt.SetValue(LinkedModelProperty, model);
        //            if (IsLoaded)
        //            {
        //                ReflowItems();
        //            }
        //            SelectedIndex = Children.Count-1;
        //        }

        #endregion
    }
}
