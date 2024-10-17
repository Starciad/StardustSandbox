using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Constants.GUI;
using StardustSandbox.Game.GUI;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.GameContent.GUI.Content.Menus.ItemExplorer
{
    public sealed partial class SGUI_ItemExplorer : SGUISystem
    {
        private Texture2D particleTexture;
        private Texture2D guiBackgroundTexture;
        private Texture2D squareShapeTexture;

        private string selectedCategoryName;
        private int selectedPageIndex;
        private SItem[] selectedItems;

        public SGUI_ItemExplorer(SGame gameInstance) : base(gameInstance)
        {
            this.Name = SGUIConstants.ELEMENT_EXPLORER_NAME;

            this.particleTexture = this.SGameInstance.AssetDatabase.GetTexture("particle_1");
            this.guiBackgroundTexture = this.SGameInstance.AssetDatabase.GetTexture("gui_background_1");
            this.squareShapeTexture = this.SGameInstance.AssetDatabase.GetTexture("shape_square_1");
        }

        protected override void OnLoad()
        {
            this.SGameInstance.GameInputManager.CanModifyEnvironment = false;
        }

        protected override void OnUnload()
        {
            this.SGameInstance.GameInputManager.CanModifyEnvironment = true;
        }
    }
}
