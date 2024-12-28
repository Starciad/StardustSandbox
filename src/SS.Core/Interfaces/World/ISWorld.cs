using Microsoft.Xna.Framework;

using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.Entities;
using StardustSandbox.Core.Interfaces.System;
using StardustSandbox.Core.IO.Files.World;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.World.Data;

namespace StardustSandbox.Core.Interfaces.World
{
    public interface ISWorld : ISReset, ISElementHandler, ISEntityHandler, ISWorldChunking
    {
        SWorldInfo Infos { get; }

        bool IsActive { get; set; }
        bool IsVisible { get; set; }

        void StartNew(SSize2 size);
        void Resize(SSize2 size);
        void Reload();
        void Clear();

        void LoadFromWorldSaveFile(SWorldSaveFile worldSaveFile);
        bool InsideTheWorldDimensions(Point position);
    }
}
