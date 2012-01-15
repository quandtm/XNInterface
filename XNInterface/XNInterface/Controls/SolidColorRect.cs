using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XNInterface.Attributes;

namespace XNInterface.Controls
{
    [XNIControl("SolidColorRect", 0)]
    public class SolidColorRect : BaseControl
    {
        [XNIParam(Optional = false)]
        public Color Color;

        [ContentSerializerIgnore]
        private Texture2D _tex;

        public override void LoadGraphics(GraphicsDevice device, ContentManager content)
        {
            _tex = new Texture2D(device, 1, 1);
            _tex.SetData<Color>(new[] { Color });
        }

        public override void Draw(GraphicsDevice device, SpriteBatch sb, double elapsedSeconds)
        {
            if (_tex != null && IsVisible)
            {
                var rect = new Rectangle((int)Position.Final.X, (int)Position.Final.Y, (int)Size.Final.X, (int)Size.Final.Y);
                sb.Draw(_tex, rect, Color.White);
            }
        }
    }
}
