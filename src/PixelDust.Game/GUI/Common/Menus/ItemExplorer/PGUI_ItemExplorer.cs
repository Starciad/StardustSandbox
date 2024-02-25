using Microsoft.Xna.Framework.Graphics;

using PixelDust.Game.Attributes.GameContent;
using PixelDust.Game.Attributes.GUI;
using PixelDust.Game.Constants.GUI;
using PixelDust.Game.Items;

namespace PixelDust.Game.GUI.Common.Menus.ItemExplorer
{
    [PGameContent]
    [PGUIRegister]
    public sealed partial class PGUI_ItemExplorer : PGUISystem
    {
        private Texture2D particleTexture;
        private Texture2D guiBackgroundTexture;
        private Texture2D squareShapeTexture;

        private string selectedCategoryName;
        private int selectedPageIndex;
        private PItem[] selectedItems;

        protected override void OnAwake()
        {
            base.OnAwake();

            this.Name = PGUIConstants.ELEMENT_EXPLORER_NAME;

            this.particleTexture = this.Game.AssetDatabase.GetTexture("particle_1");
            this.guiBackgroundTexture = this.Game.AssetDatabase.GetTexture("gui_background_1");
            this.squareShapeTexture = this.Game.AssetDatabase.GetTexture("shape_square_1");
        }

        protected override void OnLoad()
        {
            this.Game.GameInputManager.CanModifyEnvironment = false;
        }

        protected override void OnUnload()
        {
            this.Game.GameInputManager.CanModifyEnvironment = true;
        }
    }
}
