using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Core.Databases
{
    internal sealed class SAssetDatabase(ISGame gameInstance) : SGameObject(gameInstance), ISAssetDatabase
    {
        public Effect[] Shaders => [.. this.shaders.Values];

        private readonly Dictionary<string, Texture2D> textures = [];
        private readonly Dictionary<string, SpriteFont> fonts = [];
        private readonly Dictionary<string, Song> songs = [];
        private readonly Dictionary<string, SoundEffect> sounds = [];
        private readonly Dictionary<string, Effect> shaders = [];

        public void RegisterTexture(string identifier, Texture2D value)
        {
            this.textures.Add(identifier, value);
        }

        public void RegisterSpriteFont(string identifier, SpriteFont value)
        {
            this.fonts.Add(identifier, value);
        }

        public void RegisterSong(string identifier, Song value)
        {
            this.songs.Add(identifier, value);
        }

        public void RegisterSoundEffect(string identifier, SoundEffect value)
        {
            this.sounds.Add(identifier, value);
        }

        public void RegisterShader(string identifier, Effect value)
        {
            this.shaders.Add(identifier, value);
        }

        public Texture2D GetTexture(string name)
        {
            return this.textures[name];
        }

        public SpriteFont GetSpriteFont(string name)
        {
            return this.fonts[name];
        }

        public Song GetSong(string name)
        {
            return this.songs[name];
        }

        public SoundEffect GetSound(string name)
        {
            return this.sounds[name];
        }

        public Effect GetShader(string name)
        {
            return this.shaders[name];
        }
    }
}