using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Constants.GUI.Common;
using PixelDust.Game.Elements;
using PixelDust.Game.GUI.Elements;

namespace PixelDust.Game.GUI.Common
{
    public sealed partial class PGUI_HUD : PGUISystem
    {
        private Texture2D particleTexture;
        private Texture2D squareShapeTexture;
        private Texture2D iconTexture;

        protected override void OnAwake()
        {
            this.particleTexture = this.Game.AssetDatabase.GetTexture("particle_1");
            this.squareShapeTexture = this.Game.AssetDatabase.GetTexture("shape_square_1");
            this.iconTexture = this.Game.AssetDatabase.GetTexture("icon_element_1");

            base.OnAwake();
        }
        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            for (int i = 0; i < PHUDConstants.HEADER_ELEMENT_SELECTION_SLOTS_LENGTH; i++)
            {
                PGUIElement slot = this.headerElementSlots[i];
            }
        }

        public Texture2D GetElementIconTexture(int id)
        {
            return this.Game.ElementDatabase.GetElementById((uint)id).IconTexture;
        }
    }
}
