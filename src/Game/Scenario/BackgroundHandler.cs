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
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Backgrounds;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Backgrounds;

namespace StardustSandbox.Scenario
{
    internal sealed class BackgroundHandler
    {
        internal bool IsAffectedByLighting => this.selectedBackground.IsAffectedByLighting;

        private Background selectedBackground;

        internal void Update(GameTime gameTime)
        {
            this.selectedBackground.Update(gameTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            this.selectedBackground.Draw(spriteBatch);
        }

        internal void SetBackground(BackgroundIndex backgroundIndex)
        {
            this.selectedBackground = BackgroundDatabase.GetBackground(backgroundIndex);
        }
    }
}

