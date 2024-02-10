using PixelDust.Game.Camera;

namespace PixelDust.Game.Worlding
{
    public static class PWorldCamera
    {
        public static POrthographicCamera Camera { get; private set; } = new();
    }
}
