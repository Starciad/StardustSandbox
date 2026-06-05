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

using StardustSandbox.Core.Interfaces.Serialization.Worlds;

namespace StardustSandbox.Core.Serialization.Worlds.Migrations
{
    internal sealed class MigrationRegistry
    {
        private readonly IMigration[] actorMigrations =
        [
        
        ];

        private readonly IMigration[] contentMigrations =
        [
        
        ];

        private readonly IMigration[] environmentMigrations =
        [
        
        ];

        private readonly IMigration[] manifestMigrations =
        [
        
        ];

        private readonly IMigration[] propertyMigrations =
        [
        
        ];

        private readonly IMigration[] slotMigrations =
        [
        
        ];

        private readonly IMigration[] slotLayerMigrations =
        [
        
        ];

        private readonly IMigration[] texture2DMigrations =
        [
        
        ];

        // ====================== //

        private static int GetIndexFromVersion(int value)
        {
            return value - 1;
        }

        internal IMigration GetActorMigration(int version)
        {
            return this.actorMigrations[GetIndexFromVersion(version)];
        }

        internal IMigration GetContentMigration(int version)
        {
            return this.contentMigrations[GetIndexFromVersion(version)];
        }

        internal IMigration GetEnvironmentMigration(int version)
        {
            return this.environmentMigrations[GetIndexFromVersion(version)];
        }

        internal IMigration GetManifestMigration(int version)
        {
            return this.manifestMigrations[GetIndexFromVersion(version)];
        }

        internal IMigration GetPropertyMigration(int version)
        {
            return this.propertyMigrations[GetIndexFromVersion(version)];
        }

        internal IMigration GetSlotMigration(int version)
        {
            return this.slotMigrations[GetIndexFromVersion(version)];
        }

        internal IMigration GetSlotLayerMigration(int version)
        {
            return this.slotLayerMigrations[GetIndexFromVersion(version)];
        }

        internal IMigration GetTexture2DMigration(int version)
        {
            return this.texture2DMigrations[GetIndexFromVersion(version)];
        }
    }
}
