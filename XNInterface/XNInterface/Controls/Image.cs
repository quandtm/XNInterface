using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XNInterface.Attributes;

namespace XNInterface.Controls
{
    [XNIControl("Image", 0)]
    public class Image : BaseControl
    {
        [ContentSerializerIgnore]
        private Texture2D _tex;

        [XNIParam("src", "Texture2D")]
        public string TexturePath;

        public override void LoadGraphics(GraphicsDevice device, ContentManager content)
        {
            if (!string.IsNullOrEmpty(TexturePath))
                _tex = content.Load<Texture2D>(TexturePath);

            // If no size specified (default) then default to 100%
            if (Size.SpecialType == Helpers.SpecialVector2Type.Inherit && _tex != null)
            {
                Size.SpecialType = Helpers.SpecialVector2Type.Absolute;
                Size.InputX = _tex.Width;
                Size.InputY = _tex.Height;
            }
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
