using GameOfLife.Forms;

namespace GameOfLife;

internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Iniciar con el formulario de inicio
            Application.Run(new StartForm());
        }
    }
