using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Mathematics.Primitives;

using System.Text;

namespace StardustSandbox.ContentBundle.GUISystem.Elements.Textual
{
    public abstract class SGUITextualElement : SGUIElement
    {
        public SpriteFont SpriteFont { get; private set; }
        public string Content => this.contentStringBuilder.ToString();
        protected StringBuilder ContentStringBuilder => this.contentStringBuilder;

        private readonly StringBuilder contentStringBuilder = new();

        public SGUITextualElement(ISGame gameInstance) : base(gameInstance)
        {
            this.IsVisible = true;
            this.ShouldUpdate = false;
        }

        public void SetSpriteFont(string spriteFontName)
        {
            this.SpriteFont = this.SGameInstance.AssetDatabase.GetSpriteFont(spriteFontName);
        }

        public virtual void SetTextualContent(string value)
        {
            ClearTextualContent();
            _ = this.contentStringBuilder.Append(value);
        }

        public virtual void ClearTextualContent()
        {
            _ = this.contentStringBuilder.Clear();
        }

        public SSize2F GetStringSize()
        {
            Vector2 measureString = this.SpriteFont.MeasureString(this.Content) * this.Scale / 2f;
            return new SSize2F(measureString.X, measureString.Y);
        }
    }
}
