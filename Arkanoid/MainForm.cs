using Arkanoid.Classes;

namespace Arkanoid
{
    public partial class MainForm : Form
    {
        private List<GameObject> gameObjects = [];
        private Ball? ball;
        private Platform? platform;
        private Rectangle screenBounds = Screen.PrimaryScreen!.Bounds;
        private System.Windows.Forms.Timer? timer;
        private const int RowsCount = 5;
        private const int ColumnsCount = 3;
        private const int ObjectsHeightCoefficient = 50;
        private const int PlatformWidthCoefficient = 10;
        private const int PlatformStartX = 200;

        public MainForm()
        {
            InitializeComponent();
            InitializeFormData();
            InitializeGameData();
        }

        /// <summary>
        /// Метод инициализации основных свойств формы.
        /// </summary>
        private void InitializeFormData()
        {
            FormBorderStyle = FormBorderStyle.None;
            Size = screenBounds.Size;
            DoubleBuffered = true;
            Cursor.Hide();

            KeyDown += (_, _) => { Close(); };
        }

        /// <summary>
        /// Метод инициализации игровых объектов и привязки игровых функций к обработчикам событий формы.
        /// </summary>
        private void InitializeGameData()
        {
            var objectsHeight = screenBounds.Height / ObjectsHeightCoefficient;  
            var blockWidth = screenBounds.Width / (RowsCount + 1);
            var platformWidth = screenBounds.Width / PlatformWidthCoefficient;
            var platformStartY = screenBounds.Height - ObjectsHeightCoefficient;

            platform = new Platform(PlatformStartX, platformStartY, platformWidth, objectsHeight);
            gameObjects.Add(platform);

            ball = new Ball(platform.X + platform.Width / 2f, platform.Y - 9, 8);
            gameObjects.Add(ball);

            for (var row = 0; row < RowsCount; row++)
            {
                for (var col = 0; col < ColumnsCount; col++)
                {
                    var block = new Block(row * (blockWidth + blockWidth / RowsCount) + blockWidth / 10, col * (objectsHeight + 10) + 5, blockWidth, objectsHeight, Color.Orange);
                    gameObjects.Add(block);
                }
            }

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 16;
            timer.Tick += OnTick;
            timer.Start();

            MouseMove += OnMouseMove;
        }

        /// <summary>
        /// Функция привязки объекта <see cref="Platform"/> к движению мыши.
        /// </summary>
        private void OnMouseMove(object? sender, MouseEventArgs e)
        {
            var newX = e.X - platform!.Width / 2f;
            newX = Math.Max(0, Math.Min(newX, ClientSize.Width - platform.Width));
            platform.X = newX;
        }

        /// <summary>
        /// Метод тика таймера. Обновляет состояние мячика на форме, вызывает проверку коллизии мяча с другими объектами,
        /// а также проверяет статус игры.
        /// </summary>
        private void OnTick(object? sender, EventArgs e)
        {
            ball!.Update();

            for (var i = gameObjects.Count - 1; i >= 0; i--)
            {
                if (gameObjects[i] is Block block)
                {
                    if (IsColliding(ball, block))
                    {
                        var overlapX = Math.Min(ball.X + ball.Radius - block.X, block.X + block.Width - (ball.X - ball.Radius));
                        var overlapY = Math.Min(ball.Y + ball.Radius - block.Y, block.Y + block.Height - (ball.Y - ball.Radius));

                        if (overlapX < overlapY)
                        {
                            ball.Dx = -ball.Dx;
                        }
                        else
                        {
                            ball.Dy = -ball.Dy;
                        }

                        gameObjects.RemoveAt(i);
                    }
                }
            }

            if (IsColliding(ball, platform!))
            {
                ball.Dy = -Math.Abs(ball.Dy);

                var hitPos = (ball.X - platform!.X) / platform.Width;
                ball.Dx = (hitPos - 0.5f) * 8f;
            }

            if (ball.X - ball.Radius <= 0 || ball.X + ball.Radius >= ClientSize.Width)
            {
                ball.Dx = -ball.Dx;
            }

            if (ball.Y - ball.Radius <= 0)
            {
                ball.Dy = -ball.Dy;
            }

            if (ball.Y > ClientSize.Height)
            {
                GameOver(false);
            }

            if (gameObjects.Count == 2)
            {
                GameOver(true);
            }

            Invalidate();
        }

        /// <summary>
        /// Метод, создающий прямоугольную зону хитбокса мячика, и проверяющий столкновение этого хитбокса с другими игровыми объектами
        /// </summary>
        /// <param name="ball">Экземпляр мячика</param>
        /// <param name="obj">Экземпляр игрового объекта, с которым проверяется столкновение</param>
        /// <returns>Истинное значение в случае столкновения, ложное в обратном случае.</returns>
        private bool IsColliding(Ball ball, GameObject obj)
        {
            var ballRect = new RectangleF(
                ball.X - ball.Radius,
                ball.Y - ball.Radius,
                ball.Radius * 2,
                ball.Radius * 2
            );

            return ballRect.IntersectsWith(obj.Bounds);
        }

        /// <summary>
        /// Метод окончания игры. Отображает <see cref="MessageBox"/> с сообщением об окончании и закрывает форму.
        /// </summary>
        /// <param name="isWon">Аргумент, определяющий исход игры.</param>
        private void GameOver(bool isWon)
        {
            timer!.Stop();
            Cursor.Show();
            MessageBox.Show(isWon ? "Вы победили!" : "Вы проиграли!", "Игра окончена");
            Close();
        }

        /// <summary>
        /// Перегрузка метода отрисовки, с добавлением цикла отрисовки всех игровых объектов в коллекции.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            foreach (var gameObject in gameObjects)
            {
                gameObject.Draw(e.Graphics);
            }
        }
    }
}
