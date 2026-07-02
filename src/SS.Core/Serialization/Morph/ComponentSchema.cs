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

using StardustSandbox.Core.Interfaces.Serialization.Morph;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.Serialization.Morph
{
    internal sealed class ComponentSchema
    {
        internal string Identifier => this.identifier;
        internal IMapper Mapper => this.mapper;

        private readonly string identifier;
        private readonly IMapper mapper;
        private readonly IMigration[] migrations;
        private readonly Type[] versionTypes;

        internal ComponentSchema(string identifier, IMapper mapper, IMigration[] migrations, Type[] versionTypes)
        {
            ArgumentNullException.ThrowIfNull(mapper);
            ArgumentNullException.ThrowIfNull(versionTypes);
            ArgumentNullException.ThrowIfNull(migrations);

            if (versionTypes.Length == 0)
            {
                throw new InvalidOperationException("At least one version type must be provided.");
            }

            if (migrations.Length != versionTypes.Length - 1)
            {
                throw new InvalidOperationException($"The number of migrations ({migrations.Length}) must be one less than the number of version types ({versionTypes.Length}).");
            }

            this.identifier = identifier;
            this.mapper = mapper;
            this.migrations = migrations;
            this.versionTypes = versionTypes;
        }

        internal Type GetVersionType(int version)
        {
            return this.versionTypes[version - 1];
        }

        internal IMigration GetMigration(int version)
        {
            return this.migrations[version - 1];
        }

        internal IEnumerable<IMigration> GetMigrations(int sourceVersion, int targetVersion)
        {
            if (sourceVersion >= targetVersion)
            {
                yield break;
            }

            for (int version = sourceVersion; version < targetVersion; version++)
            {
                yield return GetMigration(version);
            }
        }
    }
}
