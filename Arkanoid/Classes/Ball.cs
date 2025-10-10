
namespace Arkanoid.Classes
{
    /// <summary>
    /// Класс мячика.
    /// </summary>
    public class Ball: GameObject
    {
        /// <summary>
        /// Радиус мячика в пикселях.
        /// </summary>
        public float Radius { get; set; }

        /// <summary>
        /// Скорость движения мячика по X.
        /// </summary>
        public float Dx { get; set; } = 3f;

        /// <summary>
        /// Скорость движения мячика по Y.
        /// </summary>
        public float Dy { get; set; } = -4f;

        public Ball(float x, float y, float radius) 
        {
            X = x;
            Y = y;
            Radius = radius;
            Width = radius * 2;
            Height = radius * 2;
        }

        public override void Draw(Graphics g)
        {
            using var brush = new SolidBrush(Color.Fuchsia);
            g.FillEllipse(brush, X - Radius, Y - Radius, Width, Height);
        }

        /// <summary>
        /// Метод обновления координат мячика.
        /// </summary>
        public void Update()
        {
            X += Dx;
            Y += Dy;
        }
    }
}
