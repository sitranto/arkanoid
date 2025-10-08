namespace Arkanoid.Classes
{
    /// <summary>
    /// 
    /// </summary>
    public class Block: GameObject
    {
        public Color Color { get; set; }

        public Block(float x, float y, float width, float height, Color color) 
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Color = color;
        }

        public override void Draw(Graphics g)
        {
            using var brush = new SolidBrush(Color);
            g.FillRectangle(brush, X, Y, Width, Height);
        }
    }
}
