using XNInterface.Attributes;

namespace XNInterface.Controls
{
    public enum StackPanelOrientation : short
    {
        Vertical,
        Horizontal
    }

    [XNIControl("StackPanel", -1)]
    public class StackPanel : BaseControl
    {
        [XNIParam]
        public StackPanelOrientation Orientation;

        public override void DeterminePosition(float parentX, float parentY, float parentWidth, float parentHeight)
        {
            Position.CalculatePosition(parentX, parentY, parentWidth, parentHeight, Size.Final.X, Size.Final.Y, Align, VAlign);
            float x = Position.Final.X;
            float y = Position.Final.Y;
            for (int i = 0; i < Children.Count; i++)
            {
                var child = Children[i];
                child.Position.Final.Y = y;
                child.Position.Final.X = x;
                if (Orientation == StackPanelOrientation.Horizontal)
                    x += child.Size.Final.X;
                else
                    y += child.Size.Final.Y;
                // Call DeterminePosition for the grandchildren of this control
                for (int j = 0; j < child.Children.Count; j++)
                    child.Children[j].DeterminePosition(child.Position.Final.X, child.Position.Final.Y, child.Size.Final.X, child.Size.Final.Y);
            }
        }
    }
}
