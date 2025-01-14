using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.Simulation;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.Entities;
using StardustSandbox.Core.Interfaces.System;
using StardustSandbox.Core.IO.Files.World;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.World.General;

namespace StardustSandbox.Core.Interfaces.World
{
    public interface ISWorld : ISResettable, ISElementHandler, ISEntityHandler, ISWorldChunking
    {
        SWorldInfo Infos { get; }
        SWorldTime Time { get; }
        SWorldSimulation Simulation { get; }

        bool IsActive { get; set; }
        bool IsVisible { get; set; }

        void StartNew(SSize2 size);
        void Resize(SSize2 size);
        void Reload();
        void Clear();
        void SetSpeed(SSimulationSpeed speed);

        void LoadFromWorldSaveFile(SWorldSaveFile worldSaveFile);
        bool InsideTheWorldDimensions(Point position);
    }
}
