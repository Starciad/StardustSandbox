using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Interfaces.General;

using System.IO;

namespace StardustSandbox.ContentBundle
{
    public sealed partial class SContentBundleBuilder
    {
        private enum AssetType
        {
            Texture,
            Font,
            Song,
            Sound,
            Shader
        }

        protected override void OnRegisterAssets(ISGame game, ContentManager contentManager, SAssetDatabase assetDatabase)
        {
            LoadShaders(contentManager, assetDatabase);
            LoadFonts(contentManager, assetDatabase);
            LoadGraphics(contentManager, assetDatabase);
            LoadSounds(contentManager, assetDatabase);
            LoadSongs(contentManager, assetDatabase);
        }

        private static void LoadShaders(ContentManager contentManager, SAssetDatabase assetDatabase)
        {
            AssetLoader(contentManager, assetDatabase, AssetType.Shader, SAssetConstants.SHADERS_LENGTH, "shader_", SDirectoryConstants.ASSETS_SHADERS);
        }

        private static void LoadFonts(ContentManager contentManager, SAssetDatabase assetDatabase)
        {
            AssetLoader(contentManager, assetDatabase, AssetType.Font, SAssetConstants.FONTS_LENGTH, "font_", SDirectoryConstants.ASSETS_FONTS);
        }

        private static void LoadGraphics(ContentManager contentManager, SAssetDatabase assetDatabase)
        {
            // Backgrounds
            AssetLoader(contentManager, assetDatabase, AssetType.Texture, SAssetConstants.GRAPHICS_BACKGROUNDS_LENGTH, "background_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_BACKGROUNDS));

            // BGOs
            AssetLoader(contentManager, assetDatabase, AssetType.Texture, SAssetConstants.GRAPHICS_BGOS_CELESTIAL_BODIES_LENGTH, "bgo_celestial_body_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_BGOS, SDirectoryConstants.ASSETS_GRAPHICS_BGOS_CELESTIAL_BODIES));
            AssetLoader(contentManager, assetDatabase, AssetType.Texture, SAssetConstants.GRAPHICS_BGOS_CLOUDS_LENGTH, "bgo_cloud_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_BGOS, SDirectoryConstants.ASSETS_GRAPHICS_BGOS_CLOUDS));

            // Effects
            AssetLoader(contentManager, assetDatabase, AssetType.Texture, SAssetConstants.GRAPHICS_EFFECTS_LENGTH, "effect_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_EFFECTS));

            // Elements
            AssetLoader(contentManager, assetDatabase, AssetType.Texture, SAssetConstants.GRAPHICS_ELEMENTS_LENGTH, "element_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_ELEMENTS));

            // Particles
            AssetLoader(contentManager, assetDatabase, AssetType.Texture, SAssetConstants.GRAPHICS_PARTICLES_LENGTH, "particle_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_PARTICLES));

            // Cursors
            AssetLoader(contentManager, assetDatabase, AssetType.Texture, SAssetConstants.GRAPHICS_CURSORS_LENGTH, "cursor_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_CURSORS));

            // GUI
            AssetLoader(contentManager, assetDatabase, AssetType.Texture, SAssetConstants.GRAPHICS_GUI_BACKGROUNDS_LENGTH, "gui_background_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_GUI, SDirectoryConstants.ASSETS_GRAPHICS_GUI_BACKGROUNDS));

            // Icons
            AssetLoader(contentManager, assetDatabase, AssetType.Texture, SAssetConstants.GRAPHICS_ICONS_ELEMENTS_LENGTH, "icon_element_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_ICONS, SDirectoryConstants.ASSETS_GRAPHICS_ICONS_ELEMENTS));
            AssetLoader(contentManager, assetDatabase, AssetType.Texture, SAssetConstants.GRAPHICS_ICONS_GUI_LENGTH, "icon_gui_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_ICONS, SDirectoryConstants.ASSETS_GRAPHICS_ICONS_GUI));
            AssetLoader(contentManager, assetDatabase, AssetType.Texture, SAssetConstants.GRAPHICS_ICONS_CONTROLLERS_LENGTH, "icon_controller_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_ICONS, SDirectoryConstants.ASSETS_GRAPHICS_ICONS_CONTROLLERS));

            // Miscellaneous
            AssetLoader(contentManager, assetDatabase, AssetType.Texture, SAssetConstants.GRAPHICS_MISCELLANEOUS, "miscellany_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_MISCELLANEOUS));

            // Game
            AssetLoader(contentManager, assetDatabase, AssetType.Texture, SAssetConstants.GRAPHICS_GAME_ICONS_LENGTH, "game_icon_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_GAME, SDirectoryConstants.ASSETS_GRAPHICS_GAME_ICONS));
            AssetLoader(contentManager, assetDatabase, AssetType.Texture, SAssetConstants.GRAPHICS_GAME_TITLES_LENGTH, "game_title_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_GAME, SDirectoryConstants.ASSETS_GRAPHICS_GAME_TITLES));

            // Shapes
            AssetLoader(contentManager, assetDatabase, AssetType.Texture, SAssetConstants.GRAPHICS_SHAPES_SQUARES_LENGTH, "shape_square_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_SHAPES, SDirectoryConstants.ASSETS_GRAPHICS_SHAPES_SQUARES));
        }

        private static void LoadSounds(ContentManager contentManager, SAssetDatabase assetDatabase)
        {
            AssetLoader(contentManager, assetDatabase, AssetType.Sound, SAssetConstants.SOUNDS_LENGTH, "sound_", SDirectoryConstants.ASSETS_SOUNDS);
        }

        private static void LoadSongs(ContentManager contentManager, SAssetDatabase assetDatabase)
        {
            AssetLoader(contentManager, assetDatabase, AssetType.Song, SAssetConstants.SONGS_LENGTH, "song_", SDirectoryConstants.ASSETS_SONGS);
        }

        private static void AssetLoader(ContentManager contentManager, SAssetDatabase assetDatabase, AssetType assetType, int length, string prefix, string path)
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
                        assetDatabase.RegisterTexture(targetName, contentManager.Load<Texture2D>(targetPath));
                        break;

                    case AssetType.Font:
                        assetDatabase.RegisterSpriteFont(targetName, contentManager.Load<SpriteFont>(targetPath));
                        break;

                    case AssetType.Song:
                        assetDatabase.RegisterSong(targetName, contentManager.Load<Song>(targetPath));
                        break;

                    case AssetType.Sound:
                        assetDatabase.RegisterSoundEffect(targetName, contentManager.Load<SoundEffect>(targetPath));
                        break;

                    default:
                        return;
                }
            }
        }
    }
}
