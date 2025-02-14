using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.IO;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;

using System.IO;

namespace StardustSandbox.GameContent
{
    public sealed partial class SDefaultGameContent
    {
        private enum SAssetType
        {
            Texture,
            Font,
            Song,
            SoundEffect,
            Effect
        }

        protected override void OnRegisterAssets(ISGame game, ContentManager contentManager, ISAssetDatabase assetDatabase)
        {
            LoadEffects(contentManager, assetDatabase);
            LoadFonts(contentManager, assetDatabase);
            LoadGraphics(contentManager, assetDatabase);
            LoadSoundEffects(contentManager, assetDatabase);
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
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.TEXTURES_BACKGROUNDS_LENGTH, "texture_background_", Path.Combine(SDirectoryConstants.ASSETS_TEXTURES, SDirectoryConstants.ASSETS_TEXTURES_BACKGROUNDS));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.TEXTURES_BGOS_CELESTIAL_BODIES_LENGTH, "texture_bgo_celestial_body_", Path.Combine(SDirectoryConstants.ASSETS_TEXTURES, SDirectoryConstants.ASSETS_TEXTURES_BGOS, SDirectoryConstants.ASSETS_TEXTURES_BGOS_CELESTIAL_BODIES));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.TEXTURES_BGOS_CLOUDS_LENGTH, "texture_bgo_cloud_", Path.Combine(SDirectoryConstants.ASSETS_TEXTURES, SDirectoryConstants.ASSETS_TEXTURES_BGOS, SDirectoryConstants.ASSETS_TEXTURES_BGOS_CLOUDS));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.TEXTURES_CHARACTERS_LENGTH, "texture_character_", Path.Combine(SDirectoryConstants.ASSETS_TEXTURES, SDirectoryConstants.ASSETS_TEXTURES_CHARACTERS));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.TEXTURES_CURSORS_LENGTH, "texture_cursor_", Path.Combine(SDirectoryConstants.ASSETS_TEXTURES, SDirectoryConstants.ASSETS_TEXTURES_CURSORS));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.TEXTURES_EFFECTS_LENGTH, "texture_effect_", Path.Combine(SDirectoryConstants.ASSETS_TEXTURES, SDirectoryConstants.ASSETS_TEXTURES_EFFECTS));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.TEXTURES_ELEMENTS_LENGTH, "texture_element_", Path.Combine(SDirectoryConstants.ASSETS_TEXTURES, SDirectoryConstants.ASSETS_TEXTURES_ELEMENTS));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.TEXTURES_ENTITIES_LENGTH, "texture_entity_", Path.Combine(SDirectoryConstants.ASSETS_TEXTURES, SDirectoryConstants.ASSETS_TEXTURES_ENTITIES));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.TEXTURES_GAME_ICONS_LENGTH, "texture_game_icon_", Path.Combine(SDirectoryConstants.ASSETS_TEXTURES, SDirectoryConstants.ASSETS_TEXTURES_GAME, SDirectoryConstants.ASSETS_TEXTURES_GAME_ICONS));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.TEXTURES_GAME_TITLES_LENGTH, "texture_game_title_", Path.Combine(SDirectoryConstants.ASSETS_TEXTURES, SDirectoryConstants.ASSETS_TEXTURES_GAME, SDirectoryConstants.ASSETS_TEXTURES_GAME_TITLES));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.TEXTURES_GUI_BACKGROUNDS_LENGTH, "texture_gui_background_", Path.Combine(SDirectoryConstants.ASSETS_TEXTURES, SDirectoryConstants.ASSETS_TEXTURES_GUI, SDirectoryConstants.ASSETS_TEXTURES_GUI_BACKGROUNDS));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.TEXTURES_GUI_BUTTONS_LENGTH, "texture_gui_button_", Path.Combine(SDirectoryConstants.ASSETS_TEXTURES, SDirectoryConstants.ASSETS_TEXTURES_GUI, SDirectoryConstants.ASSETS_TEXTURES_GUI_BUTTONS));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.TEXTURES_GUI_SLIDERS_LENGTH, "texture_gui_slider_", Path.Combine(SDirectoryConstants.ASSETS_TEXTURES, SDirectoryConstants.ASSETS_TEXTURES_GUI, SDirectoryConstants.ASSETS_TEXTURES_GUI_SLIDERS));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.TEXTURES_GUI_FIELDS_LENGTH, "texture_gui_field_", Path.Combine(SDirectoryConstants.ASSETS_TEXTURES, SDirectoryConstants.ASSETS_TEXTURES_GUI, SDirectoryConstants.ASSETS_TEXTURES_GUI_FIELDS));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.TEXTURES_ICONS_ENTITIES_LENGTH, "texture_icon_entity_", Path.Combine(SDirectoryConstants.ASSETS_TEXTURES, SDirectoryConstants.ASSETS_TEXTURES_ICONS, SDirectoryConstants.ASSETS_TEXTURES_ICONS_ENTITIES));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.TEXTURES_ICONS_ELEMENTS_LENGTH, "texture_icon_element_", Path.Combine(SDirectoryConstants.ASSETS_TEXTURES, SDirectoryConstants.ASSETS_TEXTURES_ICONS, SDirectoryConstants.ASSETS_TEXTURES_ICONS_ELEMENTS));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.TEXTURES_ICONS_GUI_LENGTH, "texture_icon_gui_", Path.Combine(SDirectoryConstants.ASSETS_TEXTURES, SDirectoryConstants.ASSETS_TEXTURES_ICONS, SDirectoryConstants.ASSETS_TEXTURES_ICONS_GUI));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.TEXTURES_ICONS_CONTROLLERS_LENGTH, "texture_icon_controller_", Path.Combine(SDirectoryConstants.ASSETS_TEXTURES, SDirectoryConstants.ASSETS_TEXTURES_ICONS, SDirectoryConstants.ASSETS_TEXTURES_ICONS_CONTROLLERS));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.TEXTURES_ICONS_TOOLS_LENGTH, "texture_icon_tool_", Path.Combine(SDirectoryConstants.ASSETS_TEXTURES, SDirectoryConstants.ASSETS_TEXTURES_ICONS, SDirectoryConstants.ASSETS_TEXTURES_ICONS_TOOLS));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.TEXTURES_MISCELLANEOUS, "texture_miscellany_", Path.Combine(SDirectoryConstants.ASSETS_TEXTURES, SDirectoryConstants.ASSETS_TEXTURES_MISCELLANEOUS));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.TEXTURES_PARTICLES_LENGTH, "texture_particle_", Path.Combine(SDirectoryConstants.ASSETS_TEXTURES, SDirectoryConstants.ASSETS_TEXTURES_PARTICLES));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.TEXTURES_SHAPES_SQUARES_LENGTH, "texture_shape_square_", Path.Combine(SDirectoryConstants.ASSETS_TEXTURES, SDirectoryConstants.ASSETS_TEXTURES_SHAPES, SDirectoryConstants.ASSETS_TEXTURES_SHAPES_SQUARES));
            AssetLoader(contentManager, assetDatabase, SAssetType.Texture, SAssetConstants.TEXTURES_SHAPES_THIRD_PARTIES_LENGTH, "texture_third_party_", Path.Combine(SDirectoryConstants.ASSETS_TEXTURES, SDirectoryConstants.ASSETS_TEXTURES_THIRD_PARTIES));
        }

        private static void LoadSoundEffects(ContentManager contentManager, ISAssetDatabase assetDatabase)
        {
            AssetLoader(contentManager, assetDatabase, SAssetType.SoundEffect, SAssetConstants.SOUNDS_EXPLOSIONS_LENGTH, "sound_explosion_", Path.Combine(SDirectoryConstants.ASSETS_SOUNDS, SDirectoryConstants.ASSETS_SOUNDS_EXPLOSIONS));
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

                    case SAssetType.SoundEffect:
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
