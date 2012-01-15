using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XNInterface.Attributes;
using XNInterface.Helpers;

namespace XNInterface.Controls
{
    public class FormatItem
    {
        public enum FormatItemType
        {
            Literal,
            Text,
            ParentText,
            Value,
            ParentValue
        }

        public FormatItemType ItemType;
        public string LiteralValue;

        public void BuildStringPart(ref StringBuilder sb, string text, string parentText, float value, float parentValue)
        {
            switch (ItemType)
            {
                case FormatItemType.Literal:
                    sb.AppendFormat("{0} ", LiteralValue);
                    break;

                case FormatItemType.Text:
                    sb.AppendFormat("{0} ", text);
                    break;

                case FormatItemType.ParentText:
                    sb.AppendFormat("{0} ", parentText);
                    break;

                case FormatItemType.Value:
                    sb.AppendFormat("{0} ", value);
                    break;

                case FormatItemType.ParentValue:
                    sb.AppendFormat("{0} ", parentValue);
                    break;
            }
        }

#if WINDOWS
        public static List<FormatItem> ParseFormatString(string formatString)
        {
            var l = new List<FormatItem>();
            var split = formatString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in split)
            {
                FormatItem fi = new FormatItem();
                var test = item.Replace("{", "").Replace("}", "");
                if (test.Equals("literal", StringComparison.InvariantCultureIgnoreCase) || !Enum.TryParse<FormatItemType>(test, true, out fi.ItemType))
                {
                    fi.ItemType = FormatItemType.Literal;
                    fi.LiteralValue = item;
                }
                l.Add(fi);
            }
            return l;
        }
#endif
    }

    [XNIControl("TextBlock", 0)]
    public class TextBlock : BaseControl
    {
        [ContentSerializerIgnore]
        private StringBuilder _renderText = new StringBuilder();

        [XNIParam("Font", false, "Font")]
        public string FontPath;

        [ContentSerializerIgnore]
        private SpriteFont _font;

        [XNIParam("Format", "FormatItems")]
        public List<FormatItem> FormatItems;

        [XNIParam("Color")]
        public Color TextColor;

        public override void Initialise(BaseControl parent)
        {
            base.Initialise(parent);
            if (FormatItems == null)
            {
                FormatItems = new List<FormatItem>();
                var fi = new FormatItem();
                fi.ItemType = FormatItem.FormatItemType.Text;
                FormatItems.Add(fi);
            }
        }

        public override void LoadGraphics(GraphicsDevice device, ContentManager content)
        {
            if (!string.IsNullOrEmpty(FontPath))
                _font = content.Load<SpriteFont>(FontPath);
        }

        public override void Draw(GraphicsDevice device, SpriteBatch sb, double elapsedSeconds)
        {
            if (_font == null || !IsVisible)
                return;
            sb.DrawString(_font, _renderText, Position.Final, TextColor);
        }

        public override void DetermineSize(float parentWidth, float parentHeight)
        {
            if (_font == null)
                return;

            LayoutText();

            if (Size.SpecialType != SpecialVector2Type.Inherit)
                base.DetermineSize(parentWidth, parentHeight);

            if (Size.SpecialType == SpecialVector2Type.Inherit)
                Size.Final = _font.MeasureString(_renderText);
        }

        private void LayoutText()
        {
            // Format and then wrap text (if not inheriting)
            ClearRenderText();
            for (int i = 0; i < FormatItems.Count; i++)
                FormatItems[i].BuildStringPart(ref _renderText, Text, Parent != null ? Parent.Text : "", Value, Parent != null ? Parent.Value : 0);

            if (Size.SpecialType != SpecialVector2Type.Inherit)
            {
                // Todo: Wrapping
            }
        }

        private void ClearRenderText()
        {
#if WINDOWS
            _renderText.Clear();
#else
            _renderText.Remove(0, _renderText.Length);
#endif
        }
    }
}
