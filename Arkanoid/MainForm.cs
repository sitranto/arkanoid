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

        public MainForm()
        {
            InitializeComponent();
            InitializeFormData();
            InitializeGameData();
        }

        /// <summary>
        /// ћетод инициализации основных свойств формы.
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
        /// ћетод инициализации игровых объектов и прив€зки игровых функций к обработчикам событий формы.
        /// </summary>
        private void InitializeGameData()
        {
            var objectsHeight = screenBounds.Height / 50;  
            var blockWidth = screenBounds.Width / (RowsCount + 1);

            platform = new Platform(200, screenBounds.Height - 50, screenBounds.Width / 10, objectsHeight);
            gameObjects.Add(platform);

            ball = new Ball(platform.X + platform.Width / 2f, platform.Y - 9, 8);
            gameObjects.Add(ball);

            for (int row = 0; row < RowsCount; row++)
            {
                for (int col = 0; col < ColumnsCount; col++)
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
        /// ‘ункци€ прив€зки объекта <see cref="Platform"/> к движению мыши.
        /// </summary>
        private void OnMouseMove(object? sender, MouseEventArgs e)
        {
            float newX = e.X - platform!.Width / 2f;
            newX = Math.Max(0, Math.Min(newX, ClientSize.Width - platform.Width));
            platform.X = newX;
        }

        /// <summary>
        /// ћетод тика таймера. ќбновл€ет состо€ние м€чика на форме, вызывает проверку коллизии м€ча с другими объектами,
        /// а также провер€ет статус игры.
        /// </summary>
        private void OnTick(object? sender, EventArgs e)
        {
            ball!.Update();

            for (int i = gameObjects.Count - 1; i >= 0; i--)
            {
                if (gameObjects[i] is Block block)
                {
                    if (IsColliding(ball, block))
                    {
                        float overlapX = Math.Min(ball.X + ball.Radius - block.X, block.X + block.Width - (ball.X - ball.Radius));
                        float overlapY = Math.Min(ball.Y + ball.Radius - block.Y, block.Y + block.Height - (ball.Y - ball.Radius));

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

                float hitPos = (ball.X - platform!.X) / platform.Width;
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
        /// ћетод, создающий пр€моугольную зону хитбокса м€чика, и провер€ющий столкновение этого хитбокса с другими игровыми объектами
        /// </summary>
        /// <param name="ball">Ёкземпл€р м€чика</param>
        /// <param name="obj">Ёкземпл€р игрового объекта, с которым провер€етс€ столкновение</param>
        /// <returns></returns>
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
        /// ћетод окончани€ игры. ќтображает <see cref="MessageBox"/> с сообщением об окончании и закрывает форму.
        /// </summary>
        /// <param name="isWon">јргумент, определ€ющий исход игры.</param>
        private void GameOver(bool isWon)
        {
            timer!.Stop();
            Cursor.Show();
            MessageBox.Show(isWon ? "¬ы победили!" : "¬ы проиграли!", "»гра окончена");
            Close();
        }

        /// <summary>
        /// ѕерегрузка метода отрисовки, с добавлением цикла отрисовки всех игровых объектов в коллекции.
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
