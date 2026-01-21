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

namespace StardustSandbox.Core.Enums.Assets
{
    internal enum TextureIndex : sbyte
    {
        None = -1,

        #region General

        Pixel,
        Actors,
        Cursors,
        Elements,

        #endregion

        #region Backgrounds

        BackgroundOcean,

        #endregion

        #region BGOs

        BgoCelestialBodies,
        BgoClouds,

        #endregion

        #region Characters

        CharacterStarciad,

        #endregion

        #region Game

        GameTitle,

        #endregion

        #region UI

        UIBackgroundEnvironmentSettings,
        UIBackgroundGeneratorSettings,
        UIBackgroundHudHorizontalToolbar,
        UIBackgroundHudVerticalToolbar,
        UIBackgroundInformation,
        UIBackgroundItemExplorer,
        UIBackgroundOptions,
        UIBackgroundPause,
        UIBackgroundPenSettings,
        UIBackgroundSave,
        UIBackgroundTemperatureSettings,
        UIBackgroundWorldSettings,
        UIButtons,
        UISizeSlider,
        UISliderInputOrnament,
        UITextInputOrnament,

        #endregion

        #region Icons

        IconActors,
        IconElements,
        IconKeys,
        IconTools,
        IconUI,

        #endregion

        #region Miscellaneous

        MiscellaneousTheatricalCurtains,

        #endregion

        #region Patterns

        PatternDiamonds,

        #endregion

        #region Shapes

        ShapeSquares,

        #endregion

        #region ThirdParty

        ThirdPartyMonogame,
        ThirdPartyXna,

        #endregion
    }
}

