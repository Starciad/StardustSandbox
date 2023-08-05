namespace PixelDust
{
    [PElementRegister]
    internal sealed class Stone : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Stone";
            Description = string.Empty;
            Color = new(90, 90, 90);
        }
    }
}
