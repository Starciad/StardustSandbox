using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using StardustSandbox.ContentBundle.Entities.Specials;
using StardustSandbox.Core.Audio;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.GUI;
using StardustSandbox.Core.Entities;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Elements;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.World;

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StardustSandbox.ContentBundle.GUISystem.Menus
{
    public sealed partial class SGUI_MainMenu : SGUISystem
    {
        private Vector2 originalGameTitleElementPosition;

        private const float animationSpeed = 2f;
        private const float animationAmplitude = 10f;
        private const float ButtonAnimationSpeed = 1.5f;
        private const float ButtonAnimationAmplitude = 5f;

        private float animationTime = 0f;
        
        private Dictionary<SGUILabelElement, Vector2> buttonOriginalPositions;
        private float[] buttonAnimationOffsets;

        private readonly Texture2D gameTitleTexture;
        private readonly Texture2D particleTexture;
        private readonly Texture2D prosceniumCurtainTexture;

        private readonly SWorld world;

        private readonly Song mainMenuSong;

        public SGUI_MainMenu(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.gameTitleTexture = gameInstance.AssetDatabase.GetTexture("game_title_1");
            this.particleTexture = this.SGameInstance.AssetDatabase.GetTexture("particle_1");
            this.prosceniumCurtainTexture = this.SGameInstance.AssetDatabase.GetTexture("miscellany_1");

            this.mainMenuSong = this.SGameInstance.AssetDatabase.GetSong("song_1");

            this.world = gameInstance.World;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            DisableControls();
            LoadAnimationValues();
            LoadMainMenuWorld();
            LoadMagicCursor();

            SSongEngine.Play(this.mainMenuSong);
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            this.SGameInstance.EntityManager.RemoveAll();
            this.world.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateAnimations(gameTime);
            UpdateButtons();
        }

        // =========================================== //
        // Load

        private void DisableControls()
        {
            this.SGameInstance.GameInputManager.CanModifyEnvironment = false;
        }

        private void LoadAnimationValues()
        {
            // GameTitle
            this.originalGameTitleElementPosition = this.gameTitleElement.Position;

            // Buttons
            this.buttonOriginalPositions = [];
            this.buttonAnimationOffsets = new float[this.menuButtonElements.Length];

            for (int i = 0; i < this.menuButtonElements.Length; i++)
            {
                this.buttonOriginalPositions[this.menuButtonElements[i]] = this.menuButtonElements[i].Position;
                this.buttonAnimationOffsets[i] = (float)(new Random().NextDouble() * Math.PI * 2);
            }
        }

        private void LoadMainMenuWorld()
        {
            this.world.Resize(SWorldConstants.WORLD_SIZES_TEMPLATE[0]);
            this.world.Reset();
        }

        private void LoadMagicCursor()
        {
            _ = this.SGameInstance.EntityManager.Instantiate(this.SGameInstance.EntityDatabase.GetEntityDescriptor(typeof(SMagicCursorEntity)), null);
        }

        // =========================================== //
        // Update

        private void UpdateAnimations(GameTime gameTime)
        {
            UpdateGameTitleElementAnimation(gameTime);
            UpdateButtonElementsAnimation(gameTime);
        }

        private void UpdateGameTitleElementAnimation(GameTime gameTime)
        {
            this.animationTime += (float)gameTime.ElapsedGameTime.TotalSeconds * animationSpeed;

            float offsetY = (float)Math.Sin(this.animationTime) * animationAmplitude;
            Vector2 newPosition = new(this.originalGameTitleElementPosition.X, this.originalGameTitleElementPosition.Y + offsetY);

            this.gameTitleElement.SetPosition(newPosition);
        }

        private void UpdateButtonElementsAnimation(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = 0; i < this.menuButtonElements.Length; i++)
            {
                SGUILabelElement button = this.menuButtonElements[i];
                Vector2 originalPosition = this.buttonOriginalPositions[button];

                this.buttonAnimationOffsets[i] += elapsedTime * ButtonAnimationSpeed;
                float offsetY = (float)Math.Sin(this.buttonAnimationOffsets[i]) * ButtonAnimationAmplitude;

                button.SetPosition(new Vector2(originalPosition.X, originalPosition.Y + offsetY));
            }
        }

        private void UpdateButtons()
        {
            // Individually check all element slots present in the item catalog.
            for (int i = 0; i < this.menuButtonElements.Length; i++)
            {
                SGUILabelElement labelElement = this.menuButtonElements[i];

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
            SSongEngine.Stop();

            this.SGameInstance.GUIManager.CloseGUI(this.Identifier);
            this.SGameInstance.GUIManager.ShowGUI(SGUIConstants.HUD_IDENTIFIER);

            this.world.Reset();

            this.SGameInstance.CameraManager.Position = new(0f, -(this.world.Infos.Size.Height * SWorldConstants.GRID_SCALE));
            this.SGameInstance.GameInputManager.CanModifyEnvironment = true;
        }

        private static void QuitMenuButton()
        {
            Application.Exit();
        }
    }
}
