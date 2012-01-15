using System;
using Microsoft.Xna.Framework.Input;
using XNInterface.Controls;
using XNInterface.Helpers;

namespace XNInterface.Input
{
    public class MouseInput
    {
        public BaseControl RootControl;
        private MouseState _prevState;

        public event Action<BaseControl> LeftDown;
        public event Action<BaseControl> LeftUp;

        public MouseInput(BaseControl rootControl)
        {
            RootControl = rootControl;
        }

        public void HandleMouse(MouseState mouse)
        {
            HandleMouseControl(mouse, RootControl);
            _prevState = mouse;
        }

        protected void HandleMouseControl(MouseState current, BaseControl control)
        {
            var r = new RectangleF(control.Position, control.Size);
            if (r.Contains(current.X, current.Y))
            {
                if (IsJustPressed(_prevState.LeftButton, current.LeftButton))
                {
                    if (LeftDown != null)
                        LeftDown(control);
                }
                else if (IsJustReleased(_prevState.LeftButton, current.LeftButton))
                {
                    if (LeftUp != null)
                        LeftUp(control);
                    control.Trigger();
                }

                for (int i = 0; i < control.Children.Count; i++)
                    HandleMouseControl(current, control.Children[i]);
            }
        }

        protected bool IsJustPressed(ButtonState prev, ButtonState current)
        {
            return (prev == ButtonState.Released) && (current == ButtonState.Pressed);
        }

        protected bool IsJustReleased(ButtonState prev, ButtonState current)
        {
            return (prev == ButtonState.Pressed) && (current == ButtonState.Released);
        }
    }
}
