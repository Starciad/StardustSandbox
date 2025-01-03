using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.GUISystem.Specials.General;
using StardustSandbox.ContentBundle.GUISystem.Specials.Interactive;
using StardustSandbox.ContentBundle.Localization.GUIs;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.GUISystem.GUIs.Hud;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.World;

using System;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Hud.Complements
{
    internal sealed partial class SGUI_EnvironmentSettings : SGUISystem
    {
        private readonly Texture2D particleTexture;
        private readonly Texture2D guiBackgroundTexture;
        private readonly Texture2D guiButton1Texture;
        private readonly Texture2D[] iconTextures;
        private readonly SpriteFont bigApple3PMSpriteFont;

        private readonly SButton[] menuButtons;

        private readonly SButton[] sunlightBasedLife;
        private readonly SButton[] timeStateButtons;
        private readonly SButton[] timeButtons;
        private readonly SButton[] weatherStateButtons;
        private readonly SButton[] weatherButtons;
        private readonly SButton[] temperatureButtons;

        private readonly ISWorld world;

        internal SGUI_EnvironmentSettings(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.particleTexture = gameInstance.AssetDatabase.GetTexture("particle_1");
            this.guiBackgroundTexture = gameInstance.AssetDatabase.GetTexture("gui_background_1");
            this.guiButton1Texture = gameInstance.AssetDatabase.GetTexture("gui_button_1");
            this.bigApple3PMSpriteFont = gameInstance.AssetDatabase.GetSpriteFont("font_2");

            this.world = gameInstance.World;

            this.iconTextures = [
                gameInstance.AssetDatabase.GetTexture("icon_gui_16"),
            ];

            this.menuButtons = [
                new(this.iconTextures[0], SLocalization_GUIs.Button_Exit_Name, SLocalization_GUIs.Button_Exit_Description, ExitButtonAction),
            ];

            this.sunlightBasedLife = [
                new SButton(null, "Disable Life Simulation", "Stop simulating plant growth and sunlight-dependent processes.", () => { }),
                new SButton(null, "Enable Life Simulation", "Enable natural growth processes based on sunlight.", () => { }),
            ];

            this.timeStateButtons = [
                new SButton(null, "Disable", "Disable the time progression, freezing the current time.", () => { }),
                new SButton(null, "Enable", "Enable the time progression, resuming the natural time flow.", () => { }),
            ];

            this.weatherStateButtons = [
                new SButton(null, "Disable", "Disable dynamic weather changes, keeping the current weather.", () => { }),
                new SButton(null, "Enable", "Enable dynamic weather changes, allowing the weather to shift naturally.", () => { }),
            ];

            this.timeButtons = [
                new SButton(null, "Midnight", "Jump to midnight (00:00).", () => SetTimeButtonAction(new TimeSpan(0, 0, 0))),
                new SButton(null, "Dawn", "Jump to dawn (6:00 AM).", () => SetTimeButtonAction(new TimeSpan(6, 0, 0))),
                new SButton(null, "Morning", "Jump to morning (9:00 AM).", () => SetTimeButtonAction(new TimeSpan(9, 0, 0))),
                new SButton(null, "Noon", "Jump to noon (12:00 PM).", () => SetTimeButtonAction(new TimeSpan(12, 0, 0))),
                new SButton(null, "Afternoon", "Jump to afternoon (3:00 PM).", () => SetTimeButtonAction(new TimeSpan(15, 0, 0))),
                new SButton(null, "Dusk", "Jump to dusk (6:00 PM).", () => SetTimeButtonAction(new TimeSpan(18, 0, 0))),
                new SButton(null, "Evening", "Jump to evening (9:00 PM).", () => SetTimeButtonAction(new TimeSpan(21, 0, 0))),
            ];

            this.temperatureButtons = [
                new SButton(null, "Very Cold", "Set the environment to a very cold state (-10°C).", () => { }),
                new SButton(null, "Cold", "Set the environment to a cold state (0°C).", () => { }),
                new SButton(null, "Normal", "Set the environment to a normal temperature (20°C).", () => { }),
                new SButton(null, "Warm", "Set the environment to a warm state (30°C).", () => { }),
                new SButton(null, "Very Warm", "Set the environment to a very warm state (40°C).", () => { }),
            ];

            this.weatherButtons = [
                new SButton(null, "Clear", "Set the weather to a clear state with no precipitation.", () => { }),
                new SButton(null, "Rain", "Set the weather to rain, adding light to moderate precipitation.", () => { }),
                new SButton(null, "Thunder", "Set the weather to thunderstorms, with heavy rain and lightning.", () => { }),
            ];

            this.menuButtonSlots = new SSlot[this.menuButtons.Length];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateMenuButtons();
        }

        private void UpdateMenuButtons()
        {
            for (int i = 0; i < this.menuButtonSlots.Length; i++)
            {
                SSlot slot = this.menuButtonSlots[i];

                if (this.GUIEvents.OnMouseClick(slot.BackgroundElement.Position, new(SGUI_HUDConstants.SLOT_SIZE)))
                {
                    this.menuButtons[i].ClickAction.Invoke();
                }

                slot.BackgroundElement.Color = this.GUIEvents.OnMouseOver(slot.BackgroundElement.Position, new(SGUI_HUDConstants.SLOT_SIZE)) ? SColorPalette.HoverColor : SColorPalette.White;
            }
        }
    }
}
