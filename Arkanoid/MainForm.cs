namespace Arkanoid
{
    public partial class MainForm : Form
    {
        private List<GameObject> gameObjects = [];
        private Platform? platform;
        private Rectangle screenBounds = Screen.PrimaryScreen!.Bounds;

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
            BackgroundImage = Properties.Resources.arkanoid_bg;
            DoubleBuffered = true;
            Cursor.Hide();

            KeyDown += (_, _) => { Close(); };
        }

        /// <summary>
        /// Метод инициализации игровых объектов и привязки игровых функций к обработчикам событий формы.
        /// </summary>
        private void InitializeGameData()
        {
            var objectsHeight = screenBounds.Height / 50;  
            var blockWidth = screenBounds.Width / 11;

            platform = new Platform(200, screenBounds.Height - 50, screenBounds.Width / 10, objectsHeight);
            gameObjects.Add(platform);
            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    var block = new Block(col * (blockWidth + blockWidth / 10) + blockWidth / 10, row * (objectsHeight + 10) + 5, blockWidth, objectsHeight, Color.Red);
                    gameObjects.Add(block);
                }
            }

            MouseMove += OnMouseMove;
        }

        /// <summary>
        /// Функция привязки объекта <see cref="Platform"/> к движению мыши.
        /// </summary>
        private void OnMouseMove(object? sender, MouseEventArgs e)
        {
            float newX = e.X - platform!.Width / 2f;
            newX = Math.Max(0, Math.Min(newX, ClientSize.Width - platform.Width));
            platform.X = newX;

            Invalidate();
        }

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
