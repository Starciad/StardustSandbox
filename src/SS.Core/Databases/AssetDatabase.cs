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

using StardustSandbox.Core.Enums.Indexers;

using System.IO;

namespace StardustSandbox.Core.Databases
{
    internal sealed class AssetDatabase
    {
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
            this.pixelTexture = new(this.graphicsDeviceManager.GraphicsDevice, 1, 1);
            this.pixelTexture.SetData([Color.White]);

            this.effects = [
                this.contentManager.Load<Effect>(Path.Combine("effects", "gradient_transition")),
            ];

            this.fonts = [

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
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "ui", "accepted")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "ui", "click")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "ui", "error")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "ui", "hover")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "ui", "message")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "ui", "pause_ended")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "ui", "pause_started")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "ui", "rejected")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "ui", "returning")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "ui", "typing_1")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "ui", "typing_2")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "ui", "typing_3")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "ui", "typing_4")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "ui", "typing_5")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "ui", "world_loaded")),
                this.contentManager.Load<SoundEffect>(Path.Combine("sounds", "ui", "world_saved")),
            ];

            this.textures = [
                this.pixelTexture,
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "ui", "elements")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "ui", "panels")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "ui", "tutorial")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "achievements")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "actors")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "backgrounds")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "bgos")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "cursors")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "elements")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "frames")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "icons")),
                this.contentManager.Load<Texture2D>(Path.Combine("textures", "patterns")),
            ];
        }

        internal void Unload()
        {
            this.pixelTexture.Dispose();
        }

        internal Texture2D GetTexture(TextureIndex index)
        {
            return index is TextureIndex.None ? null : this.textures[((byte)index) - 1];
        }

        internal SpriteFont GetSpriteFont(SpriteFontIndex index)
        {
            return index is SpriteFontIndex.None ? null : this.fonts[((byte)index) - 1];
        }

        internal Song GetSong(SongIndex index)
        {
            return index is SongIndex.None ? null : this.songs[((byte)index) - 1];
        }

        internal Effect[] GetEffects()
        {
            return this.effects;
        }

        internal Effect GetEffect(EffectIndex index)
        {
            return index is EffectIndex.None ? null : this.effects[((byte)index) - 1];
        }

        internal SoundEffect GetSoundEffect(SoundEffectIndex index)
        {
            return index is SoundEffectIndex.None ? null : this.soundEffects[((byte)index) - 1];
        }
    }
}
