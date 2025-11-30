using System;

namespace StardustSandbox.Enums.UISystem
{
    [Flags]
    internal enum LabelBorderDirection : byte
    {
        None = 0,
        North = 1 << 0,
        NorthEast = 1 << 1,
        East = 1 << 2,
        SouthEast = 1 << 3,
        South = 1 << 4,
        SouthWest = 1 << 5,
        West = 1 << 6,
        NorthWest = 1 << 7,
        All = North | NorthEast | East | SouthEast | South | SouthWest | West | NorthWest
    }
}
