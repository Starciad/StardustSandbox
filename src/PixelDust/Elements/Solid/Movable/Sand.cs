namespace PixelDust
{
    [PElementRegister]
    internal sealed class Sand : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Sand";
            Description = string.Empty;
            Color = new(203, 165, 95);
        }
    }
}
