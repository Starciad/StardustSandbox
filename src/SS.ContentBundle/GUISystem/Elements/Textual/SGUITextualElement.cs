using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.Interfaces.General;

using System.Text;

namespace StardustSandbox.ContentBundle.GUISystem.Elements.Textual
{
    public abstract class SGUITextualElement : SGUIElement
    {
        public SpriteFont SpriteFont { get; private set; }

        public SGUITextualElement(ISGame gameInstance) : base(gameInstance)
        {
            this.IsVisible = true;
            this.ShouldUpdate = false;
        }

        public void SetSpriteFont(string spriteFontName)
        {
            this.SpriteFont = this.SGameInstance.AssetDatabase.GetSpriteFont(spriteFontName);
        }
    }
}
