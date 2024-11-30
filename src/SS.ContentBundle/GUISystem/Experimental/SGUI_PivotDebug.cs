using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.GUISystem.Elements.Graphics;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.GUI;

namespace StardustSandbox.ContentBundle.GUISystem.Experimental
{
    internal sealed class SGUI_PivotDebug(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : SGUISystem(gameInstance, identifier, guiEvents)
    {
        private SGUILabelElement[] labels;
        private SGUIImageElement[] images;

        private readonly Texture2D squareTexture = gameInstance.AssetDatabase.GetTexture("shape_square_1");
        private readonly Texture2D particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");

        protected override void OnBuild(ISGUILayoutBuilder layout)
        {
            this.labels = [
                new(this.SGameInstance),
                new(this.SGameInstance),
                new(this.SGameInstance),
                new(this.SGameInstance),
                new(this.SGameInstance),
                new(this.SGameInstance),
                new(this.SGameInstance),
                new(this.SGameInstance),
                new(this.SGameInstance),
            ];

            this.images = [
                new(this.SGameInstance),
                new(this.SGameInstance),
                new(this.SGameInstance),
                new(this.SGameInstance),
                new(this.SGameInstance),
                new(this.SGameInstance),
                new(this.SGameInstance),
                new(this.SGameInstance),
                new(this.SGameInstance),
            ];

            Vector2 labelsMargin = new(-320, 128);
            Vector2 imagesMargin = new(320, 128);

            for (int i = 0; i < this.labels.Length; i++)
            {
                this.labels[i].SetTextContent(((SCardinalDirection)i).ToString());
                this.labels[i].SetScale(new Vector2(0.1f));
                this.labels[i].SetMargin(labelsMargin);
                this.labels[i].SetColor(Color.White);
                this.labels[i].SetFontFamily(SFontFamilyConstants.BIG_APPLE_3PM);
                this.labels[i].SetBorders(true);
                this.labels[i].SetBordersColor(Color.Black);
                this.labels[i].SetBorderOffset(new Vector2(4.5f));
                this.labels[i].SetOriginPivot((SCardinalDirection)i);
                this.labels[i].SetPositionAnchor(SCardinalDirection.North);
                this.labels[i].PositionRelativeToScreen();

                layout.AddElement(this.labels[i]);

                labelsMargin.Y += 64;
            }

            for (int i = 0; i < this.images.Length; i++)
            {
                this.images[i].SetTexture(this.squareTexture);
                this.images[i].SetScale(new Vector2(1f));
                this.images[i].SetMargin(imagesMargin);
                this.images[i].SetColor(Color.White);
                this.images[i].SetOriginPivot((SCardinalDirection)i);
                this.images[i].SetPositionAnchor(SCardinalDirection.North);
                this.images[i].PositionRelativeToScreen();

                layout.AddElement(this.images[i]);

                imagesMargin.Y += 64;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            for (int i = 0; i < this.labels.Length; i++)
            {
                spriteBatch.Draw(this.particleTexture, this.labels[i].Position, null, Color.Red, 0f, Vector2.Zero, new Vector2(1), SpriteEffects.None, 0f);
            }

            for (int i = 0; i < this.images.Length; i++)
            {
                spriteBatch.Draw(this.particleTexture, this.images[i].Position, null, Color.Red, 0f, Vector2.Zero, new Vector2(1), SpriteEffects.None, 0f);
            }
        }
    }
}
