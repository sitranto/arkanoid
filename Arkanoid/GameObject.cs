namespace Arkanoid
{
    /// <summary>
    /// Абстрактный класс, являющийся общим для всех игровых объектов.
    /// </summary>
    public abstract class GameObject
    {
        public float X {  get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        /// <summary>
        /// Метод отрисовки игрового объекта.
        /// </summary>
        /// <param name="g">Объект типа Graphics, который будет использован для отрисовки.</param>
        public abstract void Draw(Graphics g);
    }
}
