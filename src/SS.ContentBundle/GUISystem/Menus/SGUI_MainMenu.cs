using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants.GUI;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Mathematics.Primitives;

using System.Windows.Forms;

namespace StardustSandbox.ContentBundle.GUISystem.Menus
{
    public sealed partial class SGUI_MainMenu : SGUISystem
    {
        private readonly Texture2D gameTitleTexture;
        private readonly Texture2D particleTexture;

        public SGUI_MainMenu(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.gameTitleTexture = gameInstance.AssetDatabase.GetTexture("game_title_1");
            this.particleTexture = this.SGameInstance.AssetDatabase.GetTexture("particle_1");
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            this.SGameInstance.GameInputManager.CanModifyEnvironment = false;

            this.SGameInstance.World.Resize(new SSize2(40, 23));
            this.SGameInstance.World.Reset();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateButtons();
        }

        private void UpdateButtons()
        {
            // Individually check all element slots present in the item catalog.
            for (int i = 0; i < this.menuButtons.Length; i++)
            {
                SGUILabelElement labelElement = this.menuButtons[i];

                // Check if the mouse clicked on the current slot.
                if (this.GUIEvents.OnMouseClick(labelElement.Position, labelElement.GetMeasureStringSize()))
                {
                    this.menuButtonActions[i].Invoke();
                }

                // Highlight when mouse is over slot.
                if (this.GUIEvents.OnMouseOver(labelElement.Position, labelElement.GetMeasureStringSize()))
                {
                    labelElement.SetColor(Color.Yellow);
                }
                // If none of the above events occur, the slot continues with its normal color.
                else
                {
                    labelElement.SetColor(Color.White);
                }
            }
        }

        // ================================= //
        // Actions

        private void CreateMenuButton()
        {
            this.SGameInstance.GUIManager.ShowGUI(SGUIConstants.HUD_IDENTIFIER);
            this.SGameInstance.GUIManager.CloseGUI(this.Identifier);

            this.SGameInstance.World.Resize(SWorldConstants.WORLD_SIZES_TEMPLATE[2]);
            this.SGameInstance.World.Reset();

            this.SGameInstance.CameraManager.Position = new(0f, -(this.SGameInstance.World.Infos.Size.Height * SWorldConstants.GRID_SCALE));
            
            this.SGameInstance.GameInputManager.CanModifyEnvironment = true;
        }

        private static void QuitMenuButton()
        {
            Application.Exit();
        }
    }
}
