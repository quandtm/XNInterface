using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace XNInterfaceImporters
{
    public static partial class Loaders
    {
        public static string BuildFontExternal(ContentProcessorContext context, string fontPath)
        {
            var dir = Path.GetDirectoryName(fontPath);
            return Path.Combine(dir, Path.GetFileNameWithoutExtension(context.BuildAsset<FontDescription, SpriteFontContent>(new ExternalReference<FontDescription>(fontPath), "FontDescriptionProcessor").Filename));
        }

        public static string BuildTextureExternal(ContentProcessorContext context, string texPath)
        {
            var dir = Path.GetDirectoryName(texPath);
            return Path.Combine(dir, Path.GetFileNameWithoutExtension(context.BuildAsset<TextureContent, TextureContent>(new ExternalReference<TextureContent>(texPath), "TextureProcessor").Filename));
        }
    }
}
