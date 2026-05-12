/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using StardustSandbox.Core.Enums.Assets;

using System;
using System.IO;

namespace StardustSandbox.Core.Databases
{
    internal sealed class AssetDatabase
    {
        private bool isLoaded = false;

        private Texture2D pixelTexture;

        private Texture2D[] textures;
        private SpriteFont[] fonts;
        private Song[] songs;
        private SoundEffect[] soundEffects;
        private Effect[] effects;

        private readonly ContentManager contentManager;
        private readonly GraphicsDeviceManager graphicsDeviceManager;

        internal AssetDatabase(ContentManager contentManager, GraphicsDeviceManager graphicsDeviceManager)
        {
            this.contentManager = contentManager;
            this.graphicsDeviceManager = graphicsDeviceManager;
        }

        internal void Load()
        {
            if (this.isLoaded)
            {
                throw new InvalidOperationException($"{nameof(AssetDatabase)} has already been loaded.");
            }

            this.pixelTexture = new(this.graphicsDeviceManager.GraphicsDevice, 1, 1);
            this.pixelTexture.SetData([Color.White]);

            this.effects = [
                this.contentManager.Load<Effect>(Path.Combine("effects", "gradient_transition")),
            ];

            this.fonts = [
                this.contentManager.Load<SpriteFont>(Path.Combine("fonts", "arial")),
                this.contentManager.Load<SpriteFont>(Path.Combine("fonts", "big_apple_3pm")),
                this.contentManager.Load<SpriteFont>(Path.Combine("fonts", "comic_sans_ms")),
                this.contentManager.Load<SpriteFont>(Path.Combine("fonts", "cooper_bits")),
                this.contentManager.Load<SpriteFont>(Path.Combine("fonts", "de_pixel_breit")),
                this.contentManager.Load<SpriteFont>(Path.Combine("fonts", "digital_disco")),
                this.contentManager.Load<SpriteFont>(Path.Combine("fonts", "pixel_operator")),
                this.contentManager.Load<SpriteFont>(Path.Combine("fonts", "vcr_osd_mono_1.001")),
                this.contentManager.Load<SpriteFont>(Path.Combine("fonts", "windows_command_prompt")),
            ];

            this.songs = [
                this.contentManager.Load<Song>(Path.Combine("songs", "volume_01", "track_01")),
                this.contentManager.Load<Song>(Path.Combine("songs", "volume_01", "track_02")),
                this.contentManager.Load<Song>(Path.Combine("songs", "volume_01", "track_03")),
                this.contentManager.Load<Song>(Path.Combine("songs", "volume_01", "track_04")),
                this.contentManager.Load<Song>(Path.Combine("songs", "volume_01", "track_05")),
                this.contentManager.Load<Song>(Path.Combine("songs", "volume_01", "track_06")),
            ];

            this.soundEffects = [
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "uis", "accepted")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "uis", "click")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "uis", "error")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "uis", "hover")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "uis", "message")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "uis", "pause_ended")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "uis", "pause_started")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "uis", "rejected")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "uis", "returning")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "uis", "typing_1")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "uis", "typing_2")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "uis", "typing_3")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "uis", "typing_4")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "uis", "typing_5")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "uis", "world_loaded")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "uis", "world_saved")),
            ];

            this.textures = [
                this.pixelTexture,
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "achievements")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "actors")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "cursors")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "elements")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "frames")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "backgrounds", "clouds")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "backgrounds", "ocean")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "bgos", "celestial_bodies")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "bgos", "clouds")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "characters", "starciad")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "game", "title")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "icons", "actors")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "icons", "elements")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "icons", "keys")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "icons", "tools")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "icons", "ui")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "miscellaneous", "theatrical_curtains")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "patterns", "diamonds")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "shapes", "squares")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "third_parties", "monogame")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "third_parties", "xna")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "ui", "backgrounds", "achievements")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "ui", "backgrounds", "environment_settings")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "ui", "backgrounds", "generator_settings")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "ui", "backgrounds", "hud_horizontal_toolbar")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "ui", "backgrounds", "hud_vertical_toolbar")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "ui", "backgrounds", "information")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "ui", "backgrounds", "item_explorer")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "ui", "backgrounds", "item_search")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "ui", "backgrounds", "options")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "ui", "backgrounds", "pause")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "ui", "backgrounds", "pen_settings")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "ui", "backgrounds", "save_settings")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "ui", "backgrounds", "selector")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "ui", "backgrounds", "temperature_settings")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "ui", "backgrounds", "tutorial")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "ui", "backgrounds", "world_explorer")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "ui", "backgrounds", "world_settings")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "ui", "buttons")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "ui", "size_slider")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "ui", "slider_input_ornament")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "ui", "text_input_ornament")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "ui", "tutorial")),
            ];

            this.isLoaded = true;
        }

        internal void Unload()
        {
            this.pixelTexture.Dispose();
        }

        internal Texture2D GetTexture(TextureIndex index)
        {
            return this.textures[(int)index];
        }

        internal SpriteFont GetSpriteFont(SpriteFontIndex index)
        {
            return this.fonts[(int)index];
        }

        internal Song GetSong(SongIndex index)
        {
            return this.songs[(int)index];
        }

        internal Effect[] GetEffects()
        {
            return this.effects;
        }

        internal Effect GetEffect(EffectIndex index)
        {
            return this.effects[(int)index];
        }

        internal SoundEffect GetSoundEffect(SoundEffectIndex index)
        {
            return this.soundEffects[(int)index];
        }
    }
}
