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

using MessagePack;
using MessagePack.Resolvers;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces.Serialization.Migrations;
using StardustSandbox.Core.Serialization.Data.Progress.Formats.V1;

using System;
using System.Collections.Generic;
using System.IO;

namespace StardustSandbox.Core.Serialization
{
    internal sealed partial class ProgressSerializer
    {
        private readonly MessagePackSerializerOptions options = MessagePackSerializerOptions.Standard
            .WithResolver(StandardResolver.Instance)
            .WithSecurity(MessagePackSecurity.UntrustedData)
            .WithCompression(MessagePackCompression.Lz4BlockArray);

        private readonly Dictionary<Type, IProgressDescriptor> descriptors;

        internal ProgressSerializer()
        {
            this.descriptors = new()
            {
                [typeof(AchievementsData)] = new ProgressDescriptor<AchievementsData>(IOConstants.ACHIEVEMENT_PROGRESS_FILE, this.options),
            };

            _ = Directory.CreateDirectory(IO.Directory.Progress);

            foreach (IProgressDescriptor descriptor in this.descriptors.Values)
            {
                descriptor.Load();
            }
        }

        public T Load<T>() where T : IStorageModel, new()
        {
            return GetDescriptor<T>().Value;
        }

        public void Save<T>(T value) where T : IStorageModel, new()
        {
            GetDescriptor<T>().Save(value);
        }

        private ProgressDescriptor<T> GetDescriptor<T>() where T : IStorageModel, new()
        {
            return !this.descriptors.TryGetValue(typeof(T), out IProgressDescriptor raw) ? null : (ProgressDescriptor<T>)raw;
        }
    }
}
