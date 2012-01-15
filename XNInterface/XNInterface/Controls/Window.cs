using XNInterface.Attributes;

namespace XNInterface.Controls
{
    [XNIControl("Window", -1)]
    public class Window : BaseControl
    {
        public void PerformLayout(float screenWidth, float screenHeight)
        {
            DetermineSize(screenWidth, screenHeight);
            DeterminePosition(0, 0, screenWidth, screenHeight);
        }
    }
}
