namespace PixelDust
{
    [PElementRegister]
    internal class Steam : PGas
    {
        protected override void OnSettings()
        {
            Name = "Steam";
            Description = string.Empty;
            Color = new(196, 228, 243);
        }
    }
}
