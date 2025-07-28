using GameOfLife.Models;
using GameOfLife.Utils;

namespace GameOfLife.Forms;

public partial class MainForm : Form
{
    private List<Coso> _cosos = new List<Coso>();
     
    public MainForm()
    {
        InitializeComponent();
    } 

    // public void MostrarCosos()
    // {
    //     var datosMostrar = _cosos.Select(c => new
    //     {
    //         c.Codigo,
    //         c.NombreCompleto,
    //         c.Edad,
    //         c.Trabaja,
    //         c.Salario,
    //         c.EstadoAnimo,
    //         c.Sexo,
    //         c.EstadoCivil,
    //         c.TipoSangre,
    //         c.Estado,
    //         c.Vida,
    //         c.Ataque,
    //         c.Defensa,
    //         c.Arma,
    //         c.Resistencia,
    //         PosX = c.Posicion.X,
    //         PosY = c.Posicion.Y,
    //     }).ToList();
    //     
    //     dataGridViewCosos.DataSource = datosMostrar;
    //     
    //     dataGridViewCosos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
    //     dataGridViewCosos.ReadOnly = true;
    //     dataGridViewCosos.AllowUserToAddRows = false;
    //
    // }

    private void buttonGenerarGrupo_Click(object sender, EventArgs e)
    {
        _cosos = CosoGenerator.GenerarGrupoInicial(100);

        panelMundo.Invalidate();
        // MostrarCosos();
    }

    private void panelMundo_Paint(object sender, PaintEventArgs e)
    {
        var graphics = e.Graphics;
        foreach (var c in _cosos)
        {
            if (c.Estado == Estado.Muerto) continue;

            Color color;
            switch (c.EstadoAnimo)
            {
                case EstadoAnimo.Feliz: color = Color.Green; break;
                case EstadoAnimo.Triste: color = Color.Blue; break;
                case EstadoAnimo.Enojado: color = Color.Red; break;
                case EstadoAnimo.Deprimido: color = Color.Gray; break;
                default: color = Color.Orange; break;
            }
            
            var brush = new SolidBrush(color);
            graphics.FillEllipse(brush, c.Posicion.X, c.Posicion.Y, 10, 10);  
 
        }
    }

}