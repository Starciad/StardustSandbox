using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.IO;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;

using System.IO;

namespace StardustSandbox.ContentBundle
{
    public sealed partial class SDefaultGameBundle
    {
        private enum SAssetType
        {
            Texture,
            Font,
            Song,
            Sound,
            Effect
        }

        protected override void OnRegisterAssets(ISGame game, ContentManager contentManager, ISAssetDatabase assetDatabase)
        {
            LoadEffects(contentManager, assetDatabase);
            LoadFonts(contentManager, assetDatabase);
            LoadGraphics(contentManager, assetDatabase);
            LoadSounds(contentManager, assetDatabase);
            LoadSongs(contentManager, assetDatabase);
        }

        private static void LoadEffects(ContentManager contentManager, ISAssetDatabase assetDatabase)
        {
            AssetLoader(contentManager, assetDatabase, SAssetType.Effect, SAssetConstants.EFFECTS_LENGTH, "effect_", SDirectoryConstants.ASSETS_EFFECTS);
        }

        private static void LoadFonts(ContentManager contentManager, ISAssetDatabase assetDatabase)
        {
            AssetLoader(contentManager, assetDatabase, SAssetType.Font, SAssetConstants.FONTS_LENGTH, "font_", SDirectoryConstants.ASSETS_FONTS);
        }

        private static void LoadGraphics(ContentManager contentManager, ISAssetDatabase assetDatabase)
        {
            // Backgrounds
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.GRAPHICS_BACKGROUNDS_LENGTH, "background_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_BACKGROUNDS));

            // BGOs
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.GRAPHICS_BGOS_CELESTIAL_BODIES_LENGTH, "bgo_celestial_body_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_BGOS, SDirectoryConstants.ASSETS_GRAPHICS_BGOS_CELESTIAL_BODIES));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.GRAPHICS_BGOS_CLOUDS_LENGTH, "bgo_cloud_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_BGOS, SDirectoryConstants.ASSETS_GRAPHICS_BGOS_CLOUDS));

            // Characters
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.GRAPHICS_CHARACTERS_LENGTH, "character_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_CHARACTERS));

            // Cursors
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.GRAPHICS_CURSORS_LENGTH, "cursor_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_CURSORS));

            // Effects
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.GRAPHICS_EFFECTS_LENGTH, "effect_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_EFFECTS));

            // Elements
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.GRAPHICS_ELEMENTS_LENGTH, "element_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_ELEMENTS));

            // Entities
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.GRAPHICS_ENTITIES_LENGTH, "entity_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_ENTITIES));

            // Game
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.GRAPHICS_GAME_ICONS_LENGTH, "game_icon_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_GAME, SDirectoryConstants.ASSETS_GRAPHICS_GAME_ICONS));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.GRAPHICS_GAME_TITLES_LENGTH, "game_title_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_GAME, SDirectoryConstants.ASSETS_GRAPHICS_GAME_TITLES));

            // GUI
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.GRAPHICS_GUI_BACKGROUNDS_LENGTH, "gui_background_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_GUI, SDirectoryConstants.ASSETS_GRAPHICS_GUI_BACKGROUNDS));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.GRAPHICS_GUI_BUTTONS_LENGTH, "gui_button_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_GUI, SDirectoryConstants.ASSETS_GRAPHICS_GUI_BUTTONS));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.GRAPHICS_GUI_SLIDERS_LENGTH, "gui_slider_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_GUI, SDirectoryConstants.ASSETS_GRAPHICS_GUI_SLIDERS));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.GRAPHICS_GUI_FIELDS_LENGTH, "gui_field_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_GUI, SDirectoryConstants.ASSETS_GRAPHICS_GUI_FIELDS));

            // Icons
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.GRAPHICS_ICONS_ENTITIES_LENGTH, "icon_entity_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_ICONS, SDirectoryConstants.ASSETS_GRAPHICS_ICONS_ENTITIES));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.GRAPHICS_ICONS_ELEMENTS_LENGTH, "icon_element_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_ICONS, SDirectoryConstants.ASSETS_GRAPHICS_ICONS_ELEMENTS));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.GRAPHICS_ICONS_GUI_LENGTH, "icon_gui_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_ICONS, SDirectoryConstants.ASSETS_GRAPHICS_ICONS_GUI));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.GRAPHICS_ICONS_CONTROLLERS_LENGTH, "icon_controller_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_ICONS, SDirectoryConstants.ASSETS_GRAPHICS_ICONS_CONTROLLERS));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.GRAPHICS_ICONS_TOOLS_LENGTH, "icon_tool_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_ICONS, SDirectoryConstants.ASSETS_GRAPHICS_ICONS_TOOLS));

            // Miscellaneous
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.GRAPHICS_MISCELLANEOUS, "miscellany_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_MISCELLANEOUS));

            // Particles
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.GRAPHICS_PARTICLES_LENGTH, "particle_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_PARTICLES));

            // Shapes
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.GRAPHICS_SHAPES_SQUARES_LENGTH, "shape_square_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_SHAPES, SDirectoryConstants.ASSETS_GRAPHICS_SHAPES_SQUARES));

            // Third Parties
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.GRAPHICS_SHAPES_THIRD_PARTIES_LENGTH, "third_party_", Path.Combine(SDirectoryConstants.ASSETS_GRAPHICS, SDirectoryConstants.ASSETS_GRAPHICS_THIRD_PARTIES));
        }

        private static void LoadSounds(ContentManager contentManager, ISAssetDatabase assetDatabase)
        {
            AssetLoader(contentManager, assetDatabase, SAssetType.Sound, SAssetConstants.SOUNDS_LENGTH, "sound_", SDirectoryConstants.ASSETS_SOUNDS);
        }

        private static void LoadSongs(ContentManager contentManager, ISAssetDatabase assetDatabase)
        {
            AssetLoader(contentManager, assetDatabase, SAssetType.Song, SAssetConstants.SONGS_LENGTH, "song_", SDirectoryConstants.ASSETS_SONGS);
        }

        private static void AssetLoader(ContentManager contentManager, ISAssetDatabase assetDatabase, SAssetType assetType, int length, string prefix, string path)
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
                    case SAssetType.Texture:
                        assetDatabase.RegisterTexture(targetName, contentManager.Load<Texture2D>(targetPath));
                        break;

                    case SAssetType.Font:
                        assetDatabase.RegisterSpriteFont(targetName, contentManager.Load<SpriteFont>(targetPath));
                        break;

                    case SAssetType.Song:
                        assetDatabase.RegisterSong(targetName, contentManager.Load<Song>(targetPath));
                        break;

                    case SAssetType.Sound:
                        assetDatabase.RegisterSoundEffect(targetName, contentManager.Load<SoundEffect>(targetPath));
                        break;

                    case SAssetType.Effect:
                        assetDatabase.RegisterEffect(targetName, contentManager.Load<Effect>(targetPath));
                        break;

                    default:
                        return;
                }
            }
        }
    }
}
