using System.Reflection;

namespace GameOfLife.Forms;


public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();

            // Ajustar para que el formulario cambie los tamaños proporcionalmente;
            OnResize(EventArgs.Empty);
        }

        private void btnJugar_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.FormClosed += (s, args) => this.Show();
            mainForm.Show();
        }
 
    }
