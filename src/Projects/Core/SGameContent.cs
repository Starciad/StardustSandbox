using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using StardustSandbox.Core.Ambient.Background;
using StardustSandbox.Core.Catalog;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Constants.GUISystem;
using StardustSandbox.Core.Constants.IO;
using StardustSandbox.Core.Elements.Common.Energies;
using StardustSandbox.Core.Elements.Common.Gases;
using StardustSandbox.Core.Elements.Common.Liquids;
using StardustSandbox.Core.Elements.Common.Solids.Immovables;
using StardustSandbox.Core.Elements.Common.Solids.Movables;
using StardustSandbox.Core.Entities.Common.Specials;
using StardustSandbox.Core.Enums.Items;
using StardustSandbox.Core.GUISystem.Common.Elements.Informational;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Localization.Catalog;
using StardustSandbox.Core.Localization.Elements;
using StardustSandbox.Core.Localization.Tools;
using StardustSandbox.Core.Tools.Common;
using StardustSandbox.Core.GUISystem.GUIs.Hud;
using StardustSandbox.Core.GUISystem.GUIs.Hud.Complements.EnvironmentSettings;
using StardustSandbox.Core.GUISystem.GUIs.Hud.Complements.Information;
using StardustSandbox.Core.GUISystem.GUIs.Hud.Complements.ItemExplorer;
using StardustSandbox.Core.GUISystem.GUIs.Hud.Complements.Pause;
using StardustSandbox.Core.GUISystem.GUIs.Hud.Complements.PenSettings;
using StardustSandbox.Core.GUISystem.GUIs.Hud.Complements.SaveSettings;
using StardustSandbox.Core.GUISystem.GUIs.Hud.Complements.WorldSettings;
using StardustSandbox.Core.GUISystem.GUIs.Menus.Credits;
using StardustSandbox.Core.GUISystem.GUIs.Menus.Main;
using StardustSandbox.Core.GUISystem.GUIs.Menus.Options;
using StardustSandbox.Core.GUISystem.GUIs.Menus.Play;
using StardustSandbox.Core.GUISystem.GUIs.Menus.WorldExplorer;
using StardustSandbox.Core.GUISystem.GUIs.Menus.WorldExplorer.Complements;
using StardustSandbox.Core.GUISystem.GUIs.Tools.ColorPicker;
using StardustSandbox.Core.GUISystem.GUIs.Tools.Confirm;
using StardustSandbox.Core.GUISystem.GUIs.Tools.Message;
using StardustSandbox.Core.GUISystem.GUIs.Tools.TextInput;

using System;
using System.IO;

namespace StardustSandbox.Core
{
    internal static class SGameContent
    {
        private enum SAssetType
        {
            Texture,
            Font,
            Song,
            SoundEffect,
            Effect
        }

        internal static void Initialize(ISGame game, ContentManager contentManager)
        {
            OnRegisterAssets(contentManager, game.AssetDatabase);
            OnRegisterElements(game, game.ElementDatabase);
            OnRegisterCatalog(game, game.CatalogDatabase);
            OnRegisterGUIs(game, game.GUIDatabase);
            OnRegisterBackgrounds(game, game.BackgroundDatabase);
            OnRegisterEntities(game.EntityDatabase);
            OnRegisterTools(game.ToolDatabase);
        }

        private static void OnRegisterAssets(ContentManager contentManager, ISAssetDatabase assetDatabase)
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

        private static void OnRegisterBackgrounds(ISGame game, ISBackgroundDatabase backgroundDatabase)
        {
            backgroundDatabase.RegisterBackground("main_menu", game.AssetDatabase.GetTexture("texture_background_1"), new Action<SBackground>((background) =>
            {
                // Settings
                background.IsAffectedByLighting = true;

                // Layers
                background.AddLayer(new Point(0, 0), new(2f, 0f), new(-16f, 0f), false, true);
            }));

            backgroundDatabase.RegisterBackground("ocean_1", game.AssetDatabase.GetTexture("texture_background_1"), new Action<SBackground>((background) =>
            {
                // Settings
                background.IsAffectedByLighting = true;

                // Layers
                background.AddLayer(new Point(0, 0), new(2f, 0f), Vector2.Zero, false, true);
            }));

            backgroundDatabase.RegisterBackground("credits", game.AssetDatabase.GetTexture("texture_background_3"), new Action<SBackground>((background) =>
            {
                // Layers
                background.AddLayer(new Point(0, 0), new(0f, 0f), new(-32f), false, false);
            }));
        }

        private static void OnRegisterCatalog(ISGame game, ISCatalogDatabase catalogDatabase)
        {
            #region Categories
            SCategory elementCategory = new(
                "elements",
                SLocalization_Catalog.Category_Elements_Name,
                SLocalization_Catalog.Category_Elements_Description,
                game.AssetDatabase.GetTexture("texture_icon_element_1")
            );

            SCategory toolCategory = new(
                "tools",
                SLocalization_Catalog.Category_Tools_Name,
                SLocalization_Catalog.Category_Tools_Description,
                game.AssetDatabase.GetTexture("texture_icon_gui_53")
            );

            catalogDatabase.RegisterCategory(elementCategory);
            catalogDatabase.RegisterCategory(toolCategory);
            #endregion

            #region Subcategories

            #region Elements
            SSubcategory elementPowderSubcategory = new(
                parent: elementCategory,
                identifier: "powders",
                name: SLocalization_Catalog.Subcategory_Elements_Powders_Name,
                description: SLocalization_Catalog.Subcategory_Elements_Powders_Description,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_1")
            );

            SSubcategory elementLiquidSubcategory = new(
                parent: elementCategory,
                identifier: "liquids",
                name: SLocalization_Catalog.Subcategory_Elements_Liquids_Name,
                description: SLocalization_Catalog.Subcategory_Elements_Liquids_Description,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_3")
            );

            SSubcategory elementGasSubcategory = new(
                parent: elementCategory,
                identifier: "gases",
                name: SLocalization_Catalog.Subcategory_Elements_Gases_Name,
                description: SLocalization_Catalog.Subcategory_Elements_Gases_Description,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_20")
            );

            SSubcategory elementSolidSubcategory = new(
                parent: elementCategory,
                identifier: "solids",
                name: SLocalization_Catalog.Subcategory_Elements_Solids_Name,
                description: SLocalization_Catalog.Subcategory_Elements_Solids_Description,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_15")
            );

            SSubcategory elementEnergySubcategory = new(
                parent: elementCategory,
                identifier: "energies",
                name: SLocalization_Catalog.Subcategory_Elements_Energies_Name,
                description: SLocalization_Catalog.Subcategory_Elements_Energies_Description,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_24")
            );

            SSubcategory elementExplosiveSubcategory = new(
                parent: elementCategory,
                identifier: "explosives",
                name: SLocalization_Catalog.Subcategory_Elements_Explosives_Name,
                description: SLocalization_Catalog.Subcategory_Elements_Explosives_Description,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_33")
            );

            SSubcategory elementTechnologySubcategory = new(
                parent: elementCategory,
                identifier: "technologies",
                name: SLocalization_Catalog.Subcategory_Elements_Technologies_Name,
                description: SLocalization_Catalog.Subcategory_Elements_Technologies_Description,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_37")
            );

            SSubcategory elementSpecialSubcategory = new(
                parent: elementCategory,
                identifier: "specials",
                name: SLocalization_Catalog.Subcategory_Elements_Specials_Name,
                description: SLocalization_Catalog.Subcategory_Elements_Specials_Description,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_26")
            );

            elementCategory.AddSubcategory(elementPowderSubcategory);
            elementCategory.AddSubcategory(elementLiquidSubcategory);
            elementCategory.AddSubcategory(elementGasSubcategory);
            elementCategory.AddSubcategory(elementSolidSubcategory);
            elementCategory.AddSubcategory(elementEnergySubcategory);
            elementCategory.AddSubcategory(elementExplosiveSubcategory);
            elementCategory.AddSubcategory(elementTechnologySubcategory);
            elementCategory.AddSubcategory(elementSpecialSubcategory);
            #endregion

            #region Tools
            SSubcategory toolEnvironmentSubcategory = new(
                parent: toolCategory,
                identifier: "environment",
                name: SLocalization_Catalog.Subcategory_Tools_Environment_Name,
                description: SLocalization_Catalog.Subcategory_Tools_Environment_Description,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_gui_54")
            );

            toolCategory.AddSubcategory(toolEnvironmentSubcategory);
            #endregion

            #endregion

            #region Items

            #region Elements
            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.DIRT_IDENTIFIER,
                name: SLocalization_Elements.Solid_Movable_Dirt_Name,
                description: SLocalization_Elements.Solid_Movable_Dirt_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_1")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.MUD_IDENTIFIER,
                name: SLocalization_Elements.Solid_Movable_Mud_Name,
                description: SLocalization_Elements.Solid_Movable_Mud_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_2")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.WATER_IDENTIFIER,
                name: SLocalization_Elements.Liquid_Water_Name,
                description: SLocalization_Elements.Liquid_Water_Description,
                contentType: SItemContentType.Element,
                subcategory: elementLiquidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_3")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.STONE_IDENTIFIER,
                name: SLocalization_Elements.Solid_Movable_Stone_Name,
                description: SLocalization_Elements.Solid_Movable_Stone_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_4")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.GRASS_IDENTIFIER,
                name: SLocalization_Elements.Solid_Movable_Grass_Name,
                description: SLocalization_Elements.Solid_Movable_Grass_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_5")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.ICE_IDENTIFIER,
                name: SLocalization_Elements.Solid_Movable_Ice_Name,
                description: SLocalization_Elements.Solid_Movable_Ice_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_6")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.SAND_IDENTIFIER,
                name: SLocalization_Elements.Solid_Movable_Sand_Name,
                description: SLocalization_Elements.Solid_Movable_Sand_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_7")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.SNOW_IDENTIFIER,
                name: SLocalization_Elements.Solid_Movable_Snow_Name,
                description: SLocalization_Elements.Solid_Movable_Snow_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_8")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.MOVABLE_CORRUPTION_IDENTIFIER,
                name: SLocalization_Elements.Solid_Movable_Corruption_Name,
                description: SLocalization_Elements.Solid_Movable_Corruption_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_9")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.LAVA_IDENTIFIER,
                name: SLocalization_Elements.Liquid_Lava_Name,
                description: SLocalization_Elements.Liquid_Lava_Description,
                contentType: SItemContentType.Element,
                subcategory: elementLiquidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_10")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.ACID_IDENTIFIER,
                name: SLocalization_Elements.Liquid_Acid_Name,
                description: SLocalization_Elements.Liquid_Acid_Description,
                contentType: SItemContentType.Element,
                subcategory: elementLiquidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_11")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.GLASS_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_Glass_Name,
                description: SLocalization_Elements.Solid_Immovable_Glass_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_12")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IRON_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_Iron_Name,
                description: SLocalization_Elements.Solid_Immovable_Iron_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_13")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.WALL_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_Wall_Name,
                description: SLocalization_Elements.Solid_Immovable_Wall_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_14")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.WOOD_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_Wood_Name,
                description: SLocalization_Elements.Solid_Immovable_Wood_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_15")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.GAS_CORRUPTION_IDENTIFIER,
                name: SLocalization_Elements.Gas_Corruption_Name,
                description: SLocalization_Elements.Gas_Corruption_Description,
                contentType: SItemContentType.Element,
                subcategory: elementGasSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_16")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.LIQUID_CORRUPTION_IDENTIFIER,
                name: SLocalization_Elements.Liquid_Corruption_Name,
                description: SLocalization_Elements.Liquid_Corruption_Description,
                contentType: SItemContentType.Element,
                subcategory: elementLiquidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_17")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.IMMOVABLE_CORRUPTION_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_Corruption_Name,
                description: SLocalization_Elements.Solid_Immovable_Corruption_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_18")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.STEAM_IDENTIFIER,
                name: SLocalization_Elements.Gas_Steam_Name,
                description: SLocalization_Elements.Gas_Steam_Description,
                contentType: SItemContentType.Element,
                subcategory: elementGasSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_19")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.SMOKE_IDENTIFIER,
                name: SLocalization_Elements.Gas_Smoke_Name,
                description: SLocalization_Elements.Gas_Smoke_Description,
                contentType: SItemContentType.Element,
                subcategory: elementGasSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_20")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.RED_BRICK_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_RedBrick_Name,
                description: SLocalization_Elements.Solid_Immovable_RedBrick_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_21")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.TREE_LEAF_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_TreeLeaf_Name,
                description: SLocalization_Elements.Solid_Immovable_TreeLeaf_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_22")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.MOUNTING_BLOCK_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_MountingBlock_Name,
                description: SLocalization_Elements.Solid_Immovable_MountingBlock_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_23")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.FIRE_IDENTIFIER,
                name: SLocalization_Elements.Energy_Fire_Name,
                description: SLocalization_Elements.Energy_Fire_Description,
                contentType: SItemContentType.Element,
                subcategory: elementEnergySubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_24")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.LAMP_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_Lamp_Name,
                description: SLocalization_Elements.Solid_Immovable_Lamp_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_25")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.VOID_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_Void_Name,
                description: SLocalization_Elements.Solid_Immovable_Void_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSpecialSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_26")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.CLONE_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_Clone_Name,
                description: SLocalization_Elements.Solid_Immovable_Clone_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSpecialSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_27")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.OIL_IDENTIFIER,
                name: SLocalization_Elements.Liquid_Oil_Name,
                description: SLocalization_Elements.Liquid_Oil_Description,
                contentType: SItemContentType.Element,
                subcategory: elementLiquidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_28")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.SALT_IDENTIFIER,
                name: SLocalization_Elements.Solid_Movable_Salt_Name,
                description: SLocalization_Elements.Solid_Movable_Salt_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_29")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.SALTWATER_IDENTIFIER,
                name: SLocalization_Elements.Liquid_Saltwater_Name,
                description: SLocalization_Elements.Liquid_Saltwater_Description,
                contentType: SItemContentType.Element,
                subcategory: elementLiquidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_30")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.BOMB_IDENTIFIER,
                name: SLocalization_Elements.Solid_Movable_Bomb_Name,
                description: SLocalization_Elements.Solid_Movable_Bomb_Description,
                contentType: SItemContentType.Element,
                subcategory: elementExplosiveSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_31")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.DYNAMITE_IDENTIFIER,
                name: SLocalization_Elements.Solid_Movable_Dynamite_Name,
                description: SLocalization_Elements.Solid_Movable_Dynamite_Description,
                contentType: SItemContentType.Element,
                subcategory: elementExplosiveSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_32")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.TNT_IDENTIFIER,
                name: SLocalization_Elements.Solid_Movable_TNT_Name,
                description: SLocalization_Elements.Solid_Movable_TNT_Description,
                contentType: SItemContentType.Element,
                subcategory: elementExplosiveSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_33")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.DRY_SPONGE_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_DrySponge_Name,
                description: SLocalization_Elements.Solid_Immovable_DrySponge_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_34")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.WET_SPONGE_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_WetSponge_Name,
                description: SLocalization_Elements.Solid_Immovable_WetSponge_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_35")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.GOLD_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_Gold_Name,
                description: SLocalization_Elements.Solid_Immovable_Gold_Description,
                contentType: SItemContentType.Element,
                subcategory: elementSolidSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_36")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.HEATER_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_Heater_Name,
                description: SLocalization_Elements.Solid_Immovable_Heater_Description,
                contentType: SItemContentType.Element,
                subcategory: elementTechnologySubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_37")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.FREEZER_IDENTIFIER,
                name: SLocalization_Elements.Solid_Immovable_Freezer_Name,
                description: SLocalization_Elements.Solid_Immovable_Freezer_Description,
                contentType: SItemContentType.Element,
                subcategory: elementTechnologySubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_38")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SElementConstants.ASH_IDENTIFIER,
                name: SLocalization_Elements.Solid_Movable_Ash_Name,
                description: SLocalization_Elements.Solid_Movable_Ash_Description,
                contentType: SItemContentType.Element,
                subcategory: elementPowderSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_element_39")
            ));
            #endregion

            #region Tools
            catalogDatabase.RegisterItem(new(
                identifier: SToolConstants.HEAT_IDENTIFIER,
                name: SLocalization_Tools.Environment_Heat_Name,
                description: SLocalization_Tools.Environment_Heat_Description,
                contentType: SItemContentType.Tool,
                subcategory: toolEnvironmentSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_tool_1")
            ));

            catalogDatabase.RegisterItem(new(
                identifier: SToolConstants.FREEZE_IDENTIFIER,
                name: SLocalization_Tools.Environment_Freeze_Name,
                description: SLocalization_Tools.Environment_Freeze_Description,
                contentType: SItemContentType.Tool,
                subcategory: toolEnvironmentSubcategory,
                iconTexture: game.AssetDatabase.GetTexture("texture_icon_tool_2")
            ));
            #endregion

            #endregion
        }

        private static void OnRegisterElements(ISGame game, ISElementDatabase elementDatabase)
        {
            elementDatabase.RegisterElement(new SDirt(game, SElementConstants.DIRT_IDENTIFIER));
            elementDatabase.RegisterElement(new SMud(game, SElementConstants.MUD_IDENTIFIER));
            elementDatabase.RegisterElement(new SWater(game, SElementConstants.WATER_IDENTIFIER));
            elementDatabase.RegisterElement(new SStone(game, SElementConstants.STONE_IDENTIFIER));
            elementDatabase.RegisterElement(new SGrass(game, SElementConstants.GRASS_IDENTIFIER));
            elementDatabase.RegisterElement(new SIce(game, SElementConstants.ICE_IDENTIFIER));
            elementDatabase.RegisterElement(new SSand(game, SElementConstants.SAND_IDENTIFIER));
            elementDatabase.RegisterElement(new SSnow(game, SElementConstants.SNOW_IDENTIFIER));
            elementDatabase.RegisterElement(new SMCorruption(game, SElementConstants.MOVABLE_CORRUPTION_IDENTIFIER));
            elementDatabase.RegisterElement(new SLava(game, SElementConstants.LAVA_IDENTIFIER));
            elementDatabase.RegisterElement(new SAcid(game, SElementConstants.ACID_IDENTIFIER));
            elementDatabase.RegisterElement(new SGlass(game, SElementConstants.GLASS_IDENTIFIER));
            elementDatabase.RegisterElement(new SIron(game, SElementConstants.IRON_IDENTIFIER));
            elementDatabase.RegisterElement(new SWall(game, SElementConstants.WALL_IDENTIFIER));
            elementDatabase.RegisterElement(new SWood(game, SElementConstants.WOOD_IDENTIFIER));
            elementDatabase.RegisterElement(new SGCorruption(game, SElementConstants.GAS_CORRUPTION_IDENTIFIER));
            elementDatabase.RegisterElement(new SLCorruption(game, SElementConstants.LIQUID_CORRUPTION_IDENTIFIER));
            elementDatabase.RegisterElement(new SIMCorruption(game, SElementConstants.IMMOVABLE_CORRUPTION_IDENTIFIER));
            elementDatabase.RegisterElement(new SSteam(game, SElementConstants.STEAM_IDENTIFIER));
            elementDatabase.RegisterElement(new SSmoke(game, SElementConstants.SMOKE_IDENTIFIER));
            elementDatabase.RegisterElement(new SRedBrick(game, SElementConstants.RED_BRICK_IDENTIFIER));
            elementDatabase.RegisterElement(new STreeLeaf(game, SElementConstants.TREE_LEAF_IDENTIFIER));
            elementDatabase.RegisterElement(new SMountingBlock(game, SElementConstants.MOUNTING_BLOCK_IDENTIFIER));
            elementDatabase.RegisterElement(new SFire(game, SElementConstants.FIRE_IDENTIFIER));
            elementDatabase.RegisterElement(new SLamp(game, SElementConstants.LAMP_IDENTIFIER));
            elementDatabase.RegisterElement(new SVoid(game, SElementConstants.VOID_IDENTIFIER));
            elementDatabase.RegisterElement(new SClone(game, SElementConstants.CLONE_IDENTIFIER));
            elementDatabase.RegisterElement(new SOil(game, SElementConstants.OIL_IDENTIFIER));
            elementDatabase.RegisterElement(new SSalt(game, SElementConstants.SALT_IDENTIFIER));
            elementDatabase.RegisterElement(new SSaltwater(game, SElementConstants.SALTWATER_IDENTIFIER));
            elementDatabase.RegisterElement(new SBomb(game, SElementConstants.BOMB_IDENTIFIER));
            elementDatabase.RegisterElement(new SDynamite(game, SElementConstants.DYNAMITE_IDENTIFIER));
            elementDatabase.RegisterElement(new STnt(game, SElementConstants.TNT_IDENTIFIER));
            elementDatabase.RegisterElement(new SDrySponge(game, SElementConstants.DRY_SPONGE_IDENTIFIER));
            elementDatabase.RegisterElement(new SWetSponge(game, SElementConstants.WET_SPONGE_IDENTIFIER));
            elementDatabase.RegisterElement(new SGold(game, SElementConstants.GOLD_IDENTIFIER));
            elementDatabase.RegisterElement(new SHeater(game, SElementConstants.HEATER_IDENTIFIER));
            elementDatabase.RegisterElement(new SFreezer(game, SElementConstants.FREEZER_IDENTIFIER));
            elementDatabase.RegisterElement(new SAsh(game, SElementConstants.ASH_IDENTIFIER));
        }

        private static void OnRegisterEntities(ISEntityDatabase entityDatabase)
        {
            entityDatabase.RegisterEntityDescriptor(new SMagicCursorEntityDescriptor(SEntityConstants.MAGIC_CURSOR_IDENTIFIER));
        }

        private static void OnRegisterGUIs(ISGame game, ISGUIDatabase guiDatabase)
        {
            // =================================== //
            // Elements

            SGUITooltipBoxElement tooltipBoxElement = new(game)
            {
                MinimumSize = new(500f, 0f),
            };

            // =================================== //
            // Tools

            SGUI_Message message = new(game, SGUIConstants.MESSAGE_TOOL_IDENTIFIER, game.GUIManager.GUIEvents);
            SGUI_Confirm confirm = new(game, SGUIConstants.CONFIRM_TOOL_IDENTIFIER, game.GUIManager.GUIEvents);
            SGUI_ColorPicker colorPicker = new(game, SGUIConstants.COLOR_PICKER_TOOL_IDENTIFIER, game.GUIManager.GUIEvents, tooltipBoxElement);
            SGUI_TextInput input = new(game, SGUIConstants.INPUT_TOOL_IDENTIFIER, game.GUIManager.GUIEvents, message);

            // =================================== //
            // Build

            SGUI_MainMenu mainMenu = new(game, SGUIConstants.MAIN_MENU_IDENTIFIER, game.GUIManager.GUIEvents);
            SGUI_PlayMenu playMenu = new(game, SGUIConstants.PLAY_MENU_IDENTIFIER, game.GUIManager.GUIEvents);
            SGUI_OptionsMenu optionsMenu = new(game, SGUIConstants.OPTIONS_MENU_IDENTIFIER, game.GUIManager.GUIEvents, colorPicker, message, tooltipBoxElement);
            SGUI_CreditsMenu creditsMenu = new(game, SGUIConstants.CREDITS_MENU_IDENTIFIER, game.GUIManager.GUIEvents);

            SGUI_HUD hud = new(game, SGUIConstants.HUD_IDENTIFIER, game.GUIManager.GUIEvents, confirm, tooltipBoxElement);
            SGUI_Pause pause = new(game, SGUIConstants.HUD_PAUSE_IDENTIFIER, game.GUIManager.GUIEvents, confirm);
            SGUI_ItemExplorer itemExplorer = new(game, SGUIConstants.HUD_ITEM_EXPLORER_IDENTIFIER, game.GUIManager.GUIEvents, hud, tooltipBoxElement);
            SGUI_PenSettings penSettings = new(game, SGUIConstants.HUD_PEN_SETTINGS_IDENTIFIER, game.GUIManager.GUIEvents, hud, tooltipBoxElement);
            SGUI_EnvironmentSettings environmentSettings = new(game, SGUIConstants.HUD_ENVIRONMENT_SETTINGS_IDENTIFIER, game.GUIManager.GUIEvents, tooltipBoxElement);
            SGUI_SaveSettings saveSettings = new(game, SGUIConstants.HUD_SAVE_SETTINGS_IDENTIFIER, game.GUIManager.GUIEvents, input, tooltipBoxElement);
            SGUI_WorldSettings worldSettings = new(game, SGUIConstants.HUD_WORLD_SETTINGS_IDENTIFIER, game.GUIManager.GUIEvents, confirm, tooltipBoxElement);
            SGUI_Information information = new(game, SGUIConstants.HUD_INFORMATION_IDENTIFIER, game.GUIManager.GUIEvents);

            SGUI_WorldDetailsMenu detailsMenu = new(game, SGUIConstants.WORLD_DETAILS_IDENTIFIER, game.GUIManager.GUIEvents);
            SGUI_WorldExplorerMenu worldsExplorer = new(game, SGUIConstants.WORLDS_EXPLORER_IDENTIFIER, game.GUIManager.GUIEvents, detailsMenu);

            // =================================== //
            // Register

            guiDatabase.RegisterGUISystem(message);
            guiDatabase.RegisterGUISystem(confirm);
            guiDatabase.RegisterGUISystem(colorPicker);
            guiDatabase.RegisterGUISystem(input);

            guiDatabase.RegisterGUISystem(mainMenu);
            guiDatabase.RegisterGUISystem(playMenu);
            guiDatabase.RegisterGUISystem(optionsMenu);
            guiDatabase.RegisterGUISystem(creditsMenu);

            guiDatabase.RegisterGUISystem(hud);
            guiDatabase.RegisterGUISystem(pause);
            guiDatabase.RegisterGUISystem(itemExplorer);
            guiDatabase.RegisterGUISystem(penSettings);
            guiDatabase.RegisterGUISystem(environmentSettings);
            guiDatabase.RegisterGUISystem(saveSettings);
            guiDatabase.RegisterGUISystem(worldSettings);
            guiDatabase.RegisterGUISystem(information);

            guiDatabase.RegisterGUISystem(worldsExplorer);
            guiDatabase.RegisterGUISystem(detailsMenu);
        }

        private static void OnRegisterTools(ISToolDatabase toolDatabase)
        {
            toolDatabase.RegisterTool(new SHeatTool(SToolConstants.HEAT_IDENTIFIER));
            toolDatabase.RegisterTool(new SFreezeTool(SToolConstants.FREEZE_IDENTIFIER));
        }
    }
}
