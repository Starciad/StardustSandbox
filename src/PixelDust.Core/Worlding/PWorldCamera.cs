using PixelDust.Core.Camera;

namespace PixelDust.Core.Worlding
{
    public static class PWorldCamera
    {
        public static POrthographicCamera Camera { get; private set; } = new();
    }
}
