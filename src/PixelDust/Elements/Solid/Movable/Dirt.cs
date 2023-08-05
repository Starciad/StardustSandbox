namespace PixelDust
{
    [PElementRegister]
    internal sealed class Dirt : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Dirt";
            Description = string.Empty;
            Color = new Color(79, 58, 43);
        }
    }
}
