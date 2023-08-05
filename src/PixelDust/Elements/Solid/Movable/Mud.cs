namespace PixelDust
{
    [PElementRegister]
    internal sealed class Mud : PMovableSolid
    {
        protected override void OnSettings()
        {
            Name = "Mud";
            Description = string.Empty;
            Color = new Color(51, 36, 25);
        }
    }
}
