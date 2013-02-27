using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace RoDuino.SMS.Components.RoApplicationMenu
{
    public partial class ElementFlow
    {
        #region Mesh Creation

        private ModelUIElement3D CreateMeshModel(Visual visual)
        {
            GeometryModel3D model3d = (InternalResources["ElementModel"] as GeometryModel3D).Clone();
            VisualBrush brush = new VisualBrush(visual);
            RenderOptions.SetCachingHint(brush, CachingHint.Cache);
            (model3d.Material as DiffuseMaterial).Brush = brush;
            (model3d.Geometry as MeshGeometry3D).Positions = CreateMeshPositions();

            ModelUIElement3D model = new ModelUIElement3D();
            model.Model = model3d;

            return model;
        }

        private Geometry3D CreateGeometry(Visual visual)
        {
            GeometryModel3D model3d = (InternalResources["ElementModel"] as GeometryModel3D).Clone();
            VisualBrush brush = new VisualBrush(visual);
            (model3d.Material as DiffuseMaterial).Brush = brush;
            (model3d.Geometry as MeshGeometry3D).Positions = CreateMeshPositions();
            return model3d.Geometry;
        }

        private Point3DCollection CreateMeshPositions()
        {
            double eltHeight = HasReflection ? ElementHeight / 2 : ElementHeight;
            double aspect = ElementWidth / eltHeight;
            double reflectionFactor = HasReflection ? 1.0 : 0.5;

            Point3DCollection positions = new Point3DCollection();
            positions.Add(new Point3D(-aspect / 2, 1 * reflectionFactor, 0));
            positions.Add(new Point3D(aspect / 2, 1 * reflectionFactor, 0));
            positions.Add(new Point3D(aspect / 2, -1 * reflectionFactor, 0));
            positions.Add(new Point3D(-aspect / 2, -1 * reflectionFactor, 0));
            //            double x = 0.6;
            //            double y = 1.2;
            //            positions.Add(new Point3D(-x, y, 0));
            //            positions.Add(new Point3D(x, y, 0));
            //            positions.Add(new Point3D(x, -y, 0));
            //            positions.Add(new Point3D(-x, -y, 0));
            return positions;
        }

        #endregion

        /// <summary>
        /// create login element
        /// </summary>
        /// <param name="visualBrush"></param>
        /// <returns></returns>
        protected Viewport2DVisual3D CreateLoginViewPort(UIElement visualBrush)
        {
            VisualBrush brush = new VisualBrush(visualBrush);
            Viewport2DVisual3D view =
                Application.LoadComponent(
                    new Uri("/RoDuino.SMS;component/Components/RoApplicationMenu/Viewport2DVisual3D.xaml",
                            UriKind.Relative)) as
                Viewport2DVisual3D;
            view.Visual = new LoginGrid();

            ((MeshGeometry3D)view.Geometry).Positions = CreateMeshPositionsForLogin();
            return view;
        }

        private Point3DCollection CreateMeshPositionsForLogin()
        {
            double eltHeight = HasReflection ? ElementHeight / 2 : ElementHeight;
            double aspect = ElementWidth / eltHeight;
            double reflectionFactor = HasReflection ? 1.0 : 0.5;

            Point3DCollection positions = new Point3DCollection();
            double x = 0.6; //MeshWidth; //
            double y = 1; // MeshHeight;//
            positions.Add(new Point3D(-x, y, 0));
            positions.Add(new Point3D(x, y, 0));
            positions.Add(new Point3D(x, -y, 0));
            positions.Add(new Point3D(-x, -y, 0));
            return positions;
        }

        /// <summary>
        /// create application element
        /// </summary>
        /// <param name="visualBrush"></param>
        /// <returns></returns>
        protected Viewport2DVisual3D CreateApplicationViewPort(UIElement visualBrush)
        {
            VisualBrush brush = new VisualBrush(visualBrush);
            Viewport2DVisual3D view =
                Application.LoadComponent(
                    new Uri("/RoDuino.SMS;component/Components/RoApplicationMenu/Viewport2DVisual3D.xaml",
                            UriKind.Relative)) as
                Viewport2DVisual3D;
            ((MeshGeometry3D)view.Geometry).Positions = CreateMeshPositions();
            ApplicationGrid visual = new ApplicationGrid();
            visual.borderBrush.Background = brush;

            view.Visual = visual;

            return view;
        }
    }
}
