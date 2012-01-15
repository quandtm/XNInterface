using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XNInterface.Attributes;
using XNInterface.Helpers;

namespace XNInterface.Controls
{
    public enum ControlVisibility
    {
        Visible,
        Hidden,
        Enabled,
        Disabled,
        ParentEnabled,
        ParentDisabled
    }

    public abstract class BaseControl
    {
        public readonly List<BaseControl> Children = new List<BaseControl>();
        [ContentSerializerIgnore]
        public readonly Dictionary<string, BaseControl> NamedChildren = new Dictionary<string, BaseControl>();
        [ContentSerializerIgnore]
        public BaseControl Parent;

        [XNIParam]
        public string Name;

        [XNIParam]
        public SpecialVector2 Position;
        [XNIParam]
        public SpecialVector2 Size;

        [XNIParam]
        public HorizontalAlignment Align;
        [XNIParam]
        public VerticalAlignment VAlign;

        [XNIParam]
        public string Text;
        [XNIParam]
        public float Value;

        [XNIParam]
        public ControlVisibility Visibility;

        [XNIParam]
        public bool Enabled;

        public bool IsVisible
        {
            get
            {
                switch (Visibility)
                {
                    case ControlVisibility.Hidden:
                        return false;

                    case ControlVisibility.Enabled:
                        return Enabled;

                    case ControlVisibility.Disabled:
                        return !Enabled;

                    case ControlVisibility.ParentEnabled:
                        return Parent != null ? Parent.Enabled : true;

                    case ControlVisibility.ParentDisabled:
                        return Parent != null ? !Parent.Enabled : true;

                    default:
                        return true;
                }
            }
        }

        public event Action<BaseControl> Triggered;

        public virtual void Initialise(BaseControl parent)
        {
            if (Size == null)
                Size = new SpecialVector2();
            if (Position == null)
                Position = new SpecialVector2();
            Parent = parent;
            NamedChildren.Clear();
            foreach (var c in Children)
            {
                c.Initialise(this);
                foreach (var namedChild in c.NamedChildren)
                    NamedChildren.Add(namedChild.Key, namedChild.Value);
                if (!string.IsNullOrEmpty(c.Name))
                    NamedChildren.Add(c.Name, c);
            }
        }

        public void AddChild(BaseControl child)
        {
            Children.Add(child);
            child.Initialise(this);
            foreach (var namedChild in child.NamedChildren)
                NamedChildren.Add(namedChild.Key, namedChild.Value);
        }

        public virtual void DetermineSize(float parentWidth, float parentHeight)
        {
            if (Enabled)
            {
                if (Size.SpecialType == SpecialVector2Type.Inherit)
                {
                    float furthestX = 0, furthestY = 0;
                    for (int i = 0; i < Children.Count; i++)
                    {
                        Children[i].DetermineSize(0, 0);
                        var x = Children[i].Position.Final.X + Children[i].Size.Final.X;
                        var y = Children[i].Position.Final.Y + Children[i].Size.Final.Y;
                        furthestX = x > furthestX ? x : furthestX;
                        furthestY = y > furthestY ? y : furthestY;
                    }

                    Size.CalculateSize(furthestX - Position.Final.X, furthestY - Position.Final.Y);
                }
                else
                {
                    Size.CalculateSize(parentWidth, parentHeight);
                    for (int i = 0; i < Children.Count; i++)
                        Children[i].DetermineSize(Size.Final.X, Size.Final.Y);
                }
            }
        }

        public virtual void DeterminePosition(float parentX, float parentY, float parentWidth, float parentHeight)
        {
            if (Enabled)
            {
                Position.CalculatePosition(parentX, parentY, parentWidth, parentHeight, Size.Final.X, Size.Final.Y, Align, VAlign);
                for (int i = 0; i < Children.Count; i++)
                    Children[i].DeterminePosition(Position.Final.X, Position.Final.Y, Size.Final.X, Size.Final.Y);
            }
        }

        public virtual void LoadGraphics(GraphicsDevice device, ContentManager content)
        {
            for (int i = 0; i < Children.Count; i++)
                Children[i].LoadGraphics(device, content);
        }

        public virtual void Update(double elapsedSeconds)
        {
            if (Enabled)
            {
                for (int i = 0; i < Children.Count; i++)
                    Children[i].Update(elapsedSeconds);
            }
        }

        public virtual void Draw(GraphicsDevice device, SpriteBatch sb, double elapsedSeconds)
        {
            if (IsVisible)
            {
                for (int i = 0; i < Children.Count; i++)
                    Children[i].Draw(device, sb, elapsedSeconds);
            }
        }

        public void Trigger()
        {
            if (Enabled && Triggered != null)
                Triggered(this);
        }

        public T GetChild<T>(string name) where T : BaseControl
        {
            BaseControl child;
            if (NamedChildren.TryGetValue(name, out child))
                return (T)child;
            else
                return default(T);
        }
    }
}
