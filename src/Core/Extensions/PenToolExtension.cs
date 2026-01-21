using StardustSandbox.Core.Enums.Inputs.Game;

namespace StardustSandbox.Core.Extensions
{
    internal static class PenToolExtension
    {
        internal static PenTool Next(this PenTool penTool)
        {
            return penTool switch
            {
                PenTool.Visualization => PenTool.Pencil,
                PenTool.Pencil => PenTool.Eraser,
                PenTool.Eraser => PenTool.Fill,
                PenTool.Fill => PenTool.Replace,
                PenTool.Replace => PenTool.Visualization,
                _ => penTool
            };
        }

        internal static PenTool Previous(this PenTool penTool)
        {
            return penTool switch
            {
                PenTool.Visualization => PenTool.Replace,
                PenTool.Pencil => PenTool.Visualization,
                PenTool.Eraser => PenTool.Pencil,
                PenTool.Fill => PenTool.Eraser,
                PenTool.Replace => PenTool.Fill,
                _ => penTool
            };
        }
    }
}
