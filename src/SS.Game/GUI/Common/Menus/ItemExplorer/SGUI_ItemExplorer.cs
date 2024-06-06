using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Game.Attributes.GameContent;
using StardustSandbox.Game.Attributes.GUI;
using StardustSandbox.Game.Constants.GUI;
using StardustSandbox.Game.Items;

namespace StardustSandbox.Game.GUI.Common.Menus.ItemExplorer
{
    [SGameContent]
    [SGUIRegister]
    public sealed partial class SGUI_ItemExplorer : SGUISystem
    {
        private Texture2D particleTexture;
        private Texture2D guiBackgroundTexture;
        private Texture2D squareShapeTexture;

        private string[] categories;
        private string selectedCategoryName;
        private int selectedPageIndex;
        private SItem[] selectedItems;

        protected override void OnAwake()
        {
            base.OnAwake();

            this.Name = SGUIConstants.ELEMENT_EXPLORER_NAME;

            this.particleTexture = this.Game.AssetDatabase.GetTexture("particle_1");
            this.guiBackgroundTexture = this.Game.AssetDatabase.GetTexture("gui_background_1");
            this.squareShapeTexture = this.Game.AssetDatabase.GetTexture("shape_square_1");

            this.categories = this.Game.ItemDatabase.Categories;
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
