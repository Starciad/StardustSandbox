using StardustSandbox.Core.Enums.Inputs.Game;

namespace StardustSandbox.Core.Extensions
{
    internal static class PenShapeExtension
    {
        internal static PenShape Next(this PenShape shape)
        {
            return shape switch
            {
                PenShape.Circle => PenShape.Square,
                PenShape.Square => PenShape.Triangle,
                PenShape.Triangle => PenShape.Circle,
                _ => shape,
            };
        }
    }
}
