
namespace Arkanoid.Classes
{
    public class Ball: GameObject
    {
        public float Radius { get; set; }

        public Ball(float x, float y, float radius) 
        {
            X = x;
            Y = y;
            Width = radius * 2;
            Height = radius * 2;
        }

        public override void Draw(Graphics g)
        {
            using var brush = new SolidBrush(Color.Fuchsia);
            g.FillEllipse(brush, X - Radius, Y - Radius, Width, Height);
        }
    }
}
