namespace PixelDust
{
    [PElementRegister]
    internal sealed class Wall : PImmovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Wall";
            Description = string.Empty;
            Color = Color.Gray;
        }
    }
}
