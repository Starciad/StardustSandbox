namespace PixelDust
{
    [PElementRegister]
    internal class Oil : PLiquid
    {
        protected override void OnSettings()
        {
            Name = "Oil";
            Description = string.Empty;
            Color = new(236, 162, 55);
        }
    }
}
