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

        protected override void OnInitialize()
        {
            this.Name = SGUIConstants.ELEMENT_EXPLORER_NAME;

            this.particleTexture = this.SGameInstance.AssetDatabase.GetTexture("particle_1");
            this.guiBackgroundTexture = this.SGameInstance.AssetDatabase.GetTexture("gui_background_1");
            this.squareShapeTexture = this.SGameInstance.AssetDatabase.GetTexture("shape_square_1");

            this.categories = this.SGameInstance.ItemDatabase.Categories;
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
