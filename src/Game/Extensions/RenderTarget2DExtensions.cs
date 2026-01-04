using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardustSandbox.Extensions
{
    internal static class RenderTarget2DExtensions
    {
        internal static void FlattenAlpha(this RenderTarget2D renderTarget)
        {
            int width = renderTarget.Width;
            int height = renderTarget.Height;

            Color[] data = new Color[width * height];
            renderTarget.GetData(data);

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new(
                    data[i].R,
                    data[i].G,
                    data[i].B,
                    (byte)255
                );
            }

            renderTarget.SetData(data);
        }
    }
}
