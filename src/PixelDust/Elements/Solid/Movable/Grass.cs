namespace PixelDust
{
    [PElementRegister]
    internal sealed class Grass : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Grass";
            Description = string.Empty;
            Color = new Color(70, 115, 2);
        }
    }
}
