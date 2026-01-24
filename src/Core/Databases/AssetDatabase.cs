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
    internal static class AssetDatabase
    {
        private static bool isLoaded = false;

        private static Texture2D pixelTexture;

        private static Texture2D[] textures;
        private static SpriteFont[] fonts;
        private static Song[] songs;
        private static SoundEffect[] soundEffects;
        private static Effect[] effects;

        internal static void Load(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            if (isLoaded)
            {
                throw new InvalidOperationException($"{nameof(AssetDatabase)} has already been loaded.");
            }

            pixelTexture = new(graphicsDevice, 1, 1);
            pixelTexture.SetData([Color.White]);

            effects = [
                contentManager.Load<Effect>(Path.Combine("effects", "gradient_transition")),
            ];

            fonts = [
                contentManager.Load<SpriteFont>(Path.Combine("fonts", "arial")),
                contentManager.Load<SpriteFont>(Path.Combine("fonts", "big_apple_3pm")),
                contentManager.Load<SpriteFont>(Path.Combine("fonts", "comic_sans_ms")),
                contentManager.Load<SpriteFont>(Path.Combine("fonts", "cooper_bits")),
                contentManager.Load<SpriteFont>(Path.Combine("fonts", "de_pixel_breit")),
                contentManager.Load<SpriteFont>(Path.Combine("fonts", "digital_disco")),
                contentManager.Load<SpriteFont>(Path.Combine("fonts", "pixel_operator")),
                contentManager.Load<SpriteFont>(Path.Combine("fonts", "vcr_osd_mono_1.001")),
                contentManager.Load<SpriteFont>(Path.Combine("fonts", "windows_command_prompt")),
            ];

            songs = [
                contentManager.Load<Song>(Path.Combine("songs", "volume_01", "track_01")),
                contentManager.Load<Song>(Path.Combine("songs", "volume_01", "track_02")),
                contentManager.Load<Song>(Path.Combine("songs", "volume_01", "track_03")),
                contentManager.Load<Song>(Path.Combine("songs", "volume_01", "track_04")),
                contentManager.Load<Song>(Path.Combine("songs", "volume_01", "track_05")),
                contentManager.Load<Song>(Path.Combine("songs", "volume_01", "track_06")),
            ];

            soundEffects = [
                contentManager.Load<SoundEffect>(Path.Combine("sounds", "guis", "accepted")),
                contentManager.Load<SoundEffect>(Path.Combine("sounds", "guis", "click")),
                contentManager.Load<SoundEffect>(Path.Combine("sounds", "guis", "error")),
                contentManager.Load<SoundEffect>(Path.Combine("sounds", "guis", "hover")),
                contentManager.Load<SoundEffect>(Path.Combine("sounds", "guis", "message")),
                contentManager.Load<SoundEffect>(Path.Combine("sounds", "guis", "pause_ended")),
                contentManager.Load<SoundEffect>(Path.Combine("sounds", "guis", "pause_started")),
                contentManager.Load<SoundEffect>(Path.Combine("sounds", "guis", "rejected")),
                contentManager.Load<SoundEffect>(Path.Combine("sounds", "guis", "returning")),
                contentManager.Load<SoundEffect>(Path.Combine("sounds", "guis", "typing_1")),
                contentManager.Load<SoundEffect>(Path.Combine("sounds", "guis", "typing_2")),
                contentManager.Load<SoundEffect>(Path.Combine("sounds", "guis", "typing_3")),
                contentManager.Load<SoundEffect>(Path.Combine("sounds", "guis", "typing_4")),
                contentManager.Load<SoundEffect>(Path.Combine("sounds", "guis", "typing_5")),
                contentManager.Load<SoundEffect>(Path.Combine("sounds", "guis", "world_loaded")),
                contentManager.Load<SoundEffect>(Path.Combine("sounds", "guis", "world_saved")),
            ];

            textures = [
                pixelTexture,
                contentManager.Load<Texture2D>(Path.Combine("textures", "achievements")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "actors")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "cursors")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "elements")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "backgrounds", "ocean")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "bgos", "celestial_bodies")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "bgos", "clouds")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "characters", "starciad")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "game", "title")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "gui", "backgrounds", "environment_settings")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "gui", "backgrounds", "generator_settings")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "gui", "backgrounds", "hud_horizontal_toolbar")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "gui", "backgrounds", "hud_vertical_toolbar")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "gui", "backgrounds", "information")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "gui", "backgrounds", "item_explorer")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "gui", "backgrounds", "options")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "gui", "backgrounds", "pause")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "gui", "backgrounds", "pen_settings")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "gui", "backgrounds", "save_settings")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "gui", "backgrounds", "temperature_settings")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "gui", "backgrounds", "world_settings")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "gui", "buttons")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "gui", "size_slider")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "gui", "slider_input_ornament")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "gui", "text_input_ornament")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "icons", "actors")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "icons", "elements")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "icons", "keys")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "icons", "tools")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "icons", "ui")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "miscellaneous", "theatrical_curtains")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "patterns", "diamonds")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "shapes", "squares")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "third_parties", "monogame")),
                contentManager.Load<Texture2D>(Path.Combine("textures", "third_parties", "xna")),
            ];

            isLoaded = true;
        }

        internal static void Unload()
        {
            pixelTexture.Dispose();
        }

        internal static Texture2D GetTexture(in TextureIndex index)
        {
            return textures[(int)index];
        }

        internal static SpriteFont GetSpriteFont(in SpriteFontIndex index)
        {
            return fonts[(int)index];
        }

        internal static Song GetSong(in SongIndex index)
        {
            return songs[(int)index];
        }

        internal static Effect[] GetEffects()
        {
            return effects;
        }

        internal static Effect GetEffect(in EffectIndex index)
        {
            return effects[(int)index];
        }

        internal static SoundEffect GetSoundEffect(SoundEffectIndex index)
        {
            return soundEffects[(int)index];
        }
    }
}
