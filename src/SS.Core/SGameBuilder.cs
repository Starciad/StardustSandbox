using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using StardustSandbox.Game.Background;
using StardustSandbox.Game.Elements;
using StardustSandbox.Game.GUISystem;
using StardustSandbox.Game.Items;

using System.Collections.Generic;

namespace StardustSandbox.Core
{
    public abstract class SGameBuilder
    {
        private readonly List<SElement> elements = [];
        private readonly List<SItemCategory> categories = [];
        private readonly List<SItem> items = [];
        private readonly List<SGUISystem> guis = [];
        private readonly Dictionary<string, SBackground> backgrounds = [];

        // Assets
        private readonly Dictionary<string, Texture2D> textures = [];
        private readonly Dictionary<string, SpriteFont> fonts = [];
        private readonly Dictionary<string, Song> songs = [];
        private readonly Dictionary<string, SoundEffect> sounds = [];
        private readonly Dictionary<string, Effect> shaders = [];
    }
}
