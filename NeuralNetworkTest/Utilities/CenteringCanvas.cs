using System.Windows;
using System.Windows.Controls;

namespace NeuralNetworkTest.Utilities
{
    public class CenteringCanvas : Canvas
    {
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            foreach (UIElement element in base.InternalChildren)
            {
                if (element == null)
                {
                    continue;
                }
                double x = 0.0;
                double y = 0.0;
                double left = GetLeft(element);
                if (!double.IsNaN(left))
                {
                    x = left - element.DesiredSize.Width / 2;
                }

                double top = GetTop(element);
                if (!double.IsNaN(top))
                {
                    y = top - element.DesiredSize.Height / 2;
                }

                element.Arrange(new Rect(new Point(x, y), element.DesiredSize));
            }
            return arrangeSize;
        }
    }
}