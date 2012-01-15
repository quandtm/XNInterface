#if WINDOWS_PHONE
using Microsoft.Xna.Framework.Input.Touch;
using XNInterface.Controls;
using XNInterface.Helpers;

namespace XNInterface.Input
{
    public class WP7Touch
    {
        public BaseControl RootControl;

        public WP7Touch(BaseControl root)
        {
            RootControl = root;
        }

        public void EnableTap()
        {
            TouchPanel.EnabledGestures |= GestureType.Tap;
        }

        public void EnableDoubleTap()
        {
            TouchPanel.EnabledGestures |= GestureType.DoubleTap;
        }

        public void ClearGestures()
        {
            TouchPanel.EnabledGestures = GestureType.None;
        }

        public void HandleGestures()
        {
            if (TouchPanel.EnabledGestures == GestureType.None)
                return;

            while (TouchPanel.IsGestureAvailable)
            {
                var gesture = TouchPanel.ReadGesture();
                HandleGesture(gesture, RootControl);
            }
        }

        public void HandleGesture(GestureSample gesture, BaseControl control)
        {
            var r = new RectangleF(control.Position, control.Size);
            if (r.Contains(gesture.Position))
            {
                switch (gesture.GestureType)
                {
                    case GestureType.Tap:
                        control.Trigger();
                        break;

                    case GestureType.DoubleTap:
                        control.Trigger();
                        break;
                }

                for (int i = 0; i < control.Children.Count; ++i)
                    HandleGesture(gesture, control.Children[i]);
            }
        }
    }
}
#endif