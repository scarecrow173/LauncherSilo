using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;

namespace LauncherSilo.Utility
{
    public class MaterialDesignIconSourceStorage
    {
        static private Dictionary<PackIconKind, ImageSource> IconImageDictionary = new Dictionary<PackIconKind, ImageSource>();

        public static ImageSource FindPackIconImage(PackIconKind value, Brush foregroundBrush, double penThickness)
        {
            if (IconImageDictionary.ContainsKey(value))
            {
                return IconImageDictionary[value];
            }
            var packIcon = new PackIcon { Kind = value };

            var geometryDrawing = new GeometryDrawing
            {
                Geometry = Geometry.Parse(packIcon.Data),
                Brush = foregroundBrush,
                Pen = new Pen(foregroundBrush, penThickness)
            };

            var drawingGroup = new DrawingGroup { Children = { geometryDrawing }, Transform = new ScaleTransform(1, -1) };

            return new DrawingImage { Drawing = drawingGroup };
        }
    }
}
