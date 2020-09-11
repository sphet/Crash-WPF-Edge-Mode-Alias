using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CrashWPF
{
    class MyFrameworkElement : FrameworkElement
    {
        private Pen thePen;
        private bool _shouldAntiAlias = true;

        public MyFrameworkElement()
        {
            thePen = new Pen(Brushes.Red, 1.0);
            thePen.Freeze();
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            //if (shouldBeAntiAliased != isAntiAliased)
            //{
            //    isAntiAliased = shouldBeAntiAliased;
            //    if (isAntiAliased)
            //    {
            //        this.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            //    }
            //    else
            //    {
            //        this.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Unspecified);
            //    }
            //}

            //Rect rect = new Rect(0, 0, ActualWidth, ActualHeight);
            //drawingContext.DrawRectangle(Brushes.Pink, null, rect);

            base.OnRender(drawingContext);

            var x = DataSet.Get();

            StreamGeometry geo = new StreamGeometry();
            geo.FillRule = FillRule.Nonzero;

            // Open the context to use for drawing.
            using (StreamGeometryContext context = geo.Open())
            {
                Point p0 = x[0];

                int numSamples = x.Count;

                // Start at the first point.
                context.BeginFigure(p0, true, false);

                for (int i = 1; i < numSamples; i++)
                {
                    Point p1 = x[i];

                    // Add the points after the first one.
                    context.LineTo(p1, true, false); //, true, true);
                }

                context.Close();
            }

            var generatedGeometry = geo;

            generatedGeometry.Freeze();

            Rect bounds = new Rect(new Point(0, 0), RenderSize);
            //drawingContext.DrawRectangle(Brushes.Black, null, bounds);

            drawingContext.DrawGeometry(null, thePen, generatedGeometry);
        }


        public bool ShouldAntiAlias
        {
            get => _shouldAntiAlias;
            set
            {
                _shouldAntiAlias = value;

                if (_shouldAntiAlias)
                {
                    this.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Unspecified);
                }
                else
                {
                    this.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
                }
            }

        }
    }
}
