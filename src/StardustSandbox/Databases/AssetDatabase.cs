using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using StardustSandbox.Constants;

using System.Collections.Generic;
using System.IO;

namespace StardustSandbox.Databases
{
    internal static class AssetDatabase
    {
        private enum AssetType : byte
        {
            Texture = 0,
            Font = 1,
            Song = 2,
            Effect = 3
        }

        private static readonly Dictionary<string, Texture2D> textures = [];
        private static readonly Dictionary<string, SpriteFont> fonts = [];
        private static readonly Dictionary<string, Song> songs = [];
        private static readonly Dictionary<string, Effect> effects = [];

        private static void RegisterTexture(string identifier, Texture2D value)
        {
            textures.Add(identifier, value);
        }

        private static void RegisterSpriteFont(string identifier, SpriteFont value)
        {
            fonts.Add(identifier, value);
        }

        private static void RegisterSong(string identifier, Song value)
        {
            songs.Add(identifier, value);
        }

        private static void RegisterEffect(string identifier, Effect value)
        {
            effects.Add(identifier, value);
        }

        internal static Texture2D GetTexture(string name)
        {
            return textures[name];
        }

        internal static SpriteFont GetSpriteFont(string name)
        {
            return fonts[name];
        }

        internal static Song GetSong(string name)
        {
            return songs[name];
        }

        internal static Effect GetEffect(string name)
        {
            return effects[name];
        }

        // =============================================================== //

        internal static Effect[] GetAllEffects()
        {
            return [.. effects.Values];
        }

        // =============================================================== //

        internal static void Load(ContentManager contentManager)
        {
            LoadEffects(contentManager);
            LoadFonts(contentManager);
            LoadGraphics(contentManager);
            LoadSongs(contentManager);
        }

        private static void LoadEffects(ContentManager contentManager)
        {
            AssetLoader(contentManager, AssetType.Effect, AssetConstants.EFFECTS_LENGTH, "effect_", IOConstants.ASSETS_EFFECTS_DIRECTORY);
        }

        private static void LoadFonts(ContentManager contentManager)
        {
            AssetLoader(contentManager, AssetType.Font, AssetConstants.FONTS_LENGTH, "font_", IOConstants.ASSETS_FONTS_DIRECTORY);
        }

        private static void LoadGraphics(ContentManager contentManager)
        {
            AssetLoader(contentManager, AssetType.Texture, AssetConstants.TEXTURES_BACKGROUNDS_LENGTH, "texture_background_", Path.Combine(IOConstants.ASSETS_TEXTURES_DIRECTORY, IOConstants.ASSETS_TEXTURES_BACKGROUNDS_DIRECTORY));
            AssetLoader(contentManager, AssetType.Texture, AssetConstants.TEXTURES_BGOS_CELESTIAL_BODIES_LENGTH, "texture_bgo_celestial_body_", Path.Combine(IOConstants.ASSETS_TEXTURES_DIRECTORY, IOConstants.ASSETS_TEXTURES_BGOS_DIRECTORY, IOConstants.ASSETS_TEXTURES_BGOS_CELESTIAL_BODIES_DIRECTORY));
            AssetLoader(contentManager, AssetType.Texture, AssetConstants.TEXTURES_BGOS_CLOUDS_LENGTH, "texture_bgo_cloud_", Path.Combine(IOConstants.ASSETS_TEXTURES_DIRECTORY, IOConstants.ASSETS_TEXTURES_BGOS_DIRECTORY, IOConstants.ASSETS_TEXTURES_BGOS_CLOUDS_DIRECTORY));
            AssetLoader(contentManager, AssetType.Texture, AssetConstants.TEXTURES_CHARACTERS_LENGTH, "texture_character_", Path.Combine(IOConstants.ASSETS_TEXTURES_DIRECTORY, IOConstants.ASSETS_TEXTURES_CHARACTERS_DIRECTORY));
            AssetLoader(contentManager, AssetType.Texture, AssetConstants.TEXTURES_CURSORS_LENGTH, "texture_cursor_", Path.Combine(IOConstants.ASSETS_TEXTURES_DIRECTORY, IOConstants.ASSETS_TEXTURES_CURSORS_DIRECTORY));
            AssetLoader(contentManager, AssetType.Texture, AssetConstants.TEXTURES_EFFECTS_LENGTH, "texture_effect_", Path.Combine(IOConstants.ASSETS_TEXTURES_DIRECTORY, IOConstants.ASSETS_TEXTURES_EFFECTS_DIRECTORY));
            AssetLoader(contentManager, AssetType.Texture, AssetConstants.TEXTURES_ELEMENTS_LENGTH, "texture_element_", Path.Combine(IOConstants.ASSETS_TEXTURES_DIRECTORY, IOConstants.ASSETS_TEXTURES_ELEMENTS_DIRECTORY));
            AssetLoader(contentManager, AssetType.Texture, AssetConstants.TEXTURES_ENTITIES_LENGTH, "texture_entity_", Path.Combine(IOConstants.ASSETS_TEXTURES_DIRECTORY, IOConstants.ASSETS_TEXTURES_ENTITIES_DIRECTORY));
            AssetLoader(contentManager, AssetType.Texture, AssetConstants.TEXTURES_GAME_ICONS_LENGTH, "texture_game_icon_", Path.Combine(IOConstants.ASSETS_TEXTURES_DIRECTORY, IOConstants.ASSETS_TEXTURES_GAME_DIRECTORY, IOConstants.ASSETS_TEXTURES_GAME_ICONS_DIRECTORY));
            AssetLoader(contentManager, AssetType.Texture, AssetConstants.TEXTURES_GAME_TITLES_LENGTH, "texture_game_title_", Path.Combine(IOConstants.ASSETS_TEXTURES_DIRECTORY, IOConstants.ASSETS_TEXTURES_GAME_DIRECTORY, IOConstants.ASSETS_TEXTURES_GAME_TITLES_DIRECTORY));
            AssetLoader(contentManager, AssetType.Texture, AssetConstants.TEXTURES_GUI_BACKGROUNDS_LENGTH, "texture_gui_background_", Path.Combine(IOConstants.ASSETS_TEXTURES_DIRECTORY, IOConstants.ASSETS_TEXTURES_GUI_DIRECTORY, IOConstants.ASSETS_TEXTURES_GUI_BACKGROUNDS_DIRECTORY));
            AssetLoader(contentManager, AssetType.Texture, AssetConstants.TEXTURES_GUI_BUTTONS_LENGTH, "texture_gui_button_", Path.Combine(IOConstants.ASSETS_TEXTURES_DIRECTORY, IOConstants.ASSETS_TEXTURES_GUI_DIRECTORY, IOConstants.ASSETS_TEXTURES_GUI_BUTTONS_DIRECTORY));
            AssetLoader(contentManager, AssetType.Texture, AssetConstants.TEXTURES_GUI_SLIDERS_LENGTH, "texture_gui_slider_", Path.Combine(IOConstants.ASSETS_TEXTURES_DIRECTORY, IOConstants.ASSETS_TEXTURES_GUI_DIRECTORY, IOConstants.ASSETS_TEXTURES_GUI_SLIDERS_DIRECTORY));
            AssetLoader(contentManager, AssetType.Texture, AssetConstants.TEXTURES_GUI_FIELDS_LENGTH, "texture_gui_field_", Path.Combine(IOConstants.ASSETS_TEXTURES_DIRECTORY, IOConstants.ASSETS_TEXTURES_GUI_DIRECTORY, IOConstants.ASSETS_TEXTURES_GUI_FIELDS_DIRECTORY));
            AssetLoader(contentManager, AssetType.Texture, AssetConstants.TEXTURES_ICONS_ENTITIES_LENGTH, "texture_icon_entity_", Path.Combine(IOConstants.ASSETS_TEXTURES_DIRECTORY, IOConstants.ASSETS_TEXTURES_ICONS_DIRECTORY, IOConstants.ASSETS_TEXTURES_ICONS_ENTITIES_DIRECTORY));
            AssetLoader(contentManager, AssetType.Texture, AssetConstants.TEXTURES_ICONS_ELEMENTS_LENGTH, "texture_icon_element_", Path.Combine(IOConstants.ASSETS_TEXTURES_DIRECTORY, IOConstants.ASSETS_TEXTURES_ICONS_DIRECTORY, IOConstants.ASSETS_TEXTURES_ICONS_ELEMENTS_DIRECTORY));
            AssetLoader(contentManager, AssetType.Texture, AssetConstants.TEXTURES_ICONS_GUI_LENGTH, "texture_icon_gui_", Path.Combine(IOConstants.ASSETS_TEXTURES_DIRECTORY, IOConstants.ASSETS_TEXTURES_ICONS_DIRECTORY, IOConstants.ASSETS_TEXTURES_ICONS_GUI_DIRECTORY));
            AssetLoader(contentManager, AssetType.Texture, AssetConstants.TEXTURES_ICONS_CONTROLLERS_LENGTH, "texture_icon_controller_", Path.Combine(IOConstants.ASSETS_TEXTURES_DIRECTORY, IOConstants.ASSETS_TEXTURES_ICONS_DIRECTORY, IOConstants.ASSETS_TEXTURES_ICONS_CONTROLLERS_DIRECTORY));
            AssetLoader(contentManager, AssetType.Texture, AssetConstants.TEXTURES_ICONS_TOOLS_LENGTH, "texture_icon_tool_", Path.Combine(IOConstants.ASSETS_TEXTURES_DIRECTORY, IOConstants.ASSETS_TEXTURES_ICONS_DIRECTORY, IOConstants.ASSETS_TEXTURES_ICONS_TOOLS_DIRECTORY));
            AssetLoader(contentManager, AssetType.Texture, AssetConstants.TEXTURES_MISCELLANEOUS, "texture_miscellany_", Path.Combine(IOConstants.ASSETS_TEXTURES_DIRECTORY, IOConstants.ASSETS_TEXTURES_MISCELLANEOUS_DIRECTORY));
            AssetLoader(contentManager, AssetType.Texture, AssetConstants.TEXTURES_PARTICLES_LENGTH, "texture_particle_", Path.Combine(IOConstants.ASSETS_TEXTURES_DIRECTORY, IOConstants.ASSETS_TEXTURES_PARTICLES_DIRECTORY));
            AssetLoader(contentManager, AssetType.Texture, AssetConstants.TEXTURES_SHAPES_SQUARES_LENGTH, "texture_shape_square_", Path.Combine(IOConstants.ASSETS_TEXTURES_DIRECTORY, IOConstants.ASSETS_TEXTURES_SHAPES_DIRECTORY, IOConstants.ASSETS_TEXTURES_SHAPES_SQUARES_DIRECTORY));
            AssetLoader(contentManager, AssetType.Texture, AssetConstants.TEXTURES_SHAPES_THIRD_PARTIES_LENGTH, "texture_third_party_", Path.Combine(IOConstants.ASSETS_TEXTURES_DIRECTORY, IOConstants.ASSETS_TEXTURES_THIRD_PARTIES_DIRECTORY));
        }

        private static void LoadSongs(ContentManager contentManager)
        {
            AssetLoader(contentManager, AssetType.Song, AssetConstants.SONGS_LENGTH, "song_", IOConstants.ASSETS_SONGS_DIRECTORY);
        }

        private static void AssetLoader(ContentManager contentManager, AssetType assetType, int length, string prefix, string path)
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
                        RegisterTexture(targetName, contentManager.Load<Texture2D>(targetPath));
                        break;

                    case AssetType.Font:
                        RegisterSpriteFont(targetName, contentManager.Load<SpriteFont>(targetPath));
                        break;

                    case AssetType.Song:
                        RegisterSong(targetName, contentManager.Load<Song>(targetPath));
                        break;

                    case AssetType.Effect:
                        RegisterEffect(targetName, contentManager.Load<Effect>(targetPath));
                        break;

                    default:
                        break;
                }
            }
        }
    }
}