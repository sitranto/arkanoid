namespace Arkanoid
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitializeFormData();
        }

        public void InitializeFormData()
        {
            FormBorderStyle = FormBorderStyle.None;
            Size = Screen.PrimaryScreen!.Bounds.Size;
            BackgroundImage = Properties.Resources.arkanoid_bg;
            DoubleBuffered = true;

            KeyDown += (_, _) => { Close(); };
        }
    }
}
