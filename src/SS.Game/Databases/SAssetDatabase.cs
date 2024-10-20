using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using StardustSandbox.Game.Constants;
using StardustSandbox.Game.Objects;

using System.Collections.Generic;
using System.IO;

namespace StardustSandbox.Game.Databases
{
    public sealed class SAssetDatabase(SGame gameInstance, ContentManager contentManager) : SGameObject(gameInstance)
    {
        private enum AssetType
        {
            Texture,
            Font,
            Song,
            Sound,
            Shader
        }

        public Texture2D[] Textures => [.. this.textures.Values];
        public SpriteFont[] Fonts => [.. this.fonts.Values];
        public Song[] Songs => [.. this.songs.Values];
        public SoundEffect[] Sounds => [.. this.sounds.Values];
        public Effect[] Shaders => [.. this.shaders.Values];

        private readonly ContentManager _contentManager = contentManager;

        private readonly Dictionary<string, Texture2D> textures = [];
        private readonly Dictionary<string, SpriteFont> fonts = [];
        private readonly Dictionary<string, Song> songs = [];
        private readonly Dictionary<string, SoundEffect> sounds = [];
        private readonly Dictionary<string, Effect> shaders = [];

        public override void Initialize()
        {
            LoadShaders();
            LoadFonts();
            LoadGraphics();
            LoadSounds();
            LoadSongs();
        }

        #region LOAD
        private void LoadShaders()
        {
            AssetLoader(AssetType.Shader, SAssetConstants.SHADERS_LENGTH, "shader_", SDirectoryConstants.ASSETS_SHADERS);
        }
        private void LoadFonts()
        {
            AssetLoader(AssetType.Font, SAssetConstants.FONTS_LENGTH, "font_", SDirectoryConstants.ASSETS_FONTS);
        }
        private void LoadGraphics()
        {
            // Backgrounds
            AssetLoader(AssetType.Texture, SAssetConstants.GRAPHICS_BACKGROUNDS_LENGTH, "background_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_BACKGROUNDS));

            // BGOs
            AssetLoader(AssetType.Texture, SAssetConstants.GRAPHICS_BGOS_CELESTIAL_BODIES_LENGTH, "bgo_celestial_body_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_BGOS, SDirectoryConstants.ASSETS_GRAPHICS_BGOS_CELESTIAL_BODIES));
            AssetLoader(AssetType.Texture, SAssetConstants.GRAPHICS_BGOS_CLOUDS_LENGTH, "bgo_cloud_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_BGOS, SDirectoryConstants.ASSETS_GRAPHICS_BGOS_CLOUDS));

            // Effects
            AssetLoader(AssetType.Texture, SAssetConstants.GRAPHICS_EFFECTS_LENGTH, "effect_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_EFFECTS));

            // Elements
            AssetLoader(AssetType.Texture, SAssetConstants.GRAPHICS_ELEMENTS_LENGTH, "element_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_ELEMENTS));

            // Particles
            AssetLoader(AssetType.Texture, SAssetConstants.GRAPHICS_PARTICLES_LENGTH, "particle_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_PARTICLES));

            // Cursors
            AssetLoader(AssetType.Texture, SAssetConstants.GRAPHICS_CURSORS_LENGTH, "cursor_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_CURSORS));

            // GUI
            AssetLoader(AssetType.Texture, SAssetConstants.GRAPHICS_GUI_BACKGROUNDS_LENGTH, "gui_background_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_GUI, SDirectoryConstants.ASSETS_GRAPHICS_GUI_BACKGROUNDS));

            // Icons
            AssetLoader(AssetType.Texture, SAssetConstants.GRAPHICS_ICONS_ELEMENTS_LENGTH, "icon_element_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_ICONS, SDirectoryConstants.ASSETS_GRAPHICS_ICONS_ELEMENTS));
            AssetLoader(AssetType.Texture, SAssetConstants.GRAPHICS_ICONS_GUI_LENGTH, "icon_gui_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_ICONS, SDirectoryConstants.ASSETS_GRAPHICS_ICONS_GUI));

            // Game
            AssetLoader(AssetType.Texture, SAssetConstants.GRAPHICS_GAME_ICONS_LENGTH, "game_icon_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_GAME, SDirectoryConstants.ASSETS_GRAPHICS_GAME_ICONS));
            AssetLoader(AssetType.Texture, SAssetConstants.GRAPHICS_GAME_TITLES_LENGTH, "game_title_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_GAME, SDirectoryConstants.ASSETS_GRAPHICS_GAME_TITLES));

            // Shapes
            AssetLoader(AssetType.Texture, SAssetConstants.GRAPHICS_SHAPES_SQUARES_LENGTH, "shape_square_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_SHAPES, SDirectoryConstants.ASSETS_GRAPHICS_SHAPES_SQUARES));
        }
        private void LoadSounds()
        {
            AssetLoader(AssetType.Sound, SAssetConstants.SOUNDS_LENGTH, "sound_", SDirectoryConstants.ASSETS_SOUNDS);
        }
        private void LoadSongs()
        {
            AssetLoader(AssetType.Song, SAssetConstants.SONGS_LENGTH, "song_", SDirectoryConstants.ASSETS_SONGS);
        }
        #endregion

        #region GET
        public Texture2D GetTexture(string name)
        {
            return this.textures[name];
        }
        public SpriteFont GetFont(string name)
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
        #endregion

        private void AssetLoader(AssetType assetType, int length, string prefix, string path)
        {
            int targetId;
            string targetName;
            string targetPath;

            for (int i = 0; i < length; i++)
            {
                targetId = i + 1;
                targetName = string.Concat(prefix, targetId);
                targetPath = Path.Combine(path, targetName);

                switch (assetType)
                {
                    case AssetType.Texture:
                        this.textures.Add(targetName, this._contentManager.Load<Texture2D>(targetPath));
                        break;

                    case AssetType.Font:
                        this.fonts.Add(targetName, this._contentManager.Load<SpriteFont>(targetPath));
                        break;

                    case AssetType.Song:
                        this.songs.Add(targetName, this._contentManager.Load<Song>(targetPath));
                        break;

                    case AssetType.Sound:
                        this.sounds.Add(targetName, this._contentManager.Load<SoundEffect>(targetPath));
                        break;

                    default:
                        return;
                }
            }
        }
    }
}