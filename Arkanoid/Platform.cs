namespace Arkanoid
{
    /// <summary>
    /// Класс игрового объекта движущейся платформы.
    /// </summary>
    internal class Platform : GameObject
    {
        public Platform(float x, float y, float width, float height) 
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public override void Draw(Graphics g)
        {
            using var brush = new SolidBrush(Color.Yellow);
            g.FillRectangle(brush, X, Y, Width, Height);
        }
    }
}
