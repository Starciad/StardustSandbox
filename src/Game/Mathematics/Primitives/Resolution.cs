namespace StardustSandbox.Mathematics.Primitives
{
    public readonly struct Resolution(int width, int height)
    {
        public readonly int Width => width;
        public readonly int Height => height;

        public override string ToString()
        {
            return string.Concat(this.Width, 'x', this.Height);
        }
    }
}
