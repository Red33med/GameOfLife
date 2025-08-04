using GameOfLife.Models;
using GameOfLife.Utils;
using System.IO;
using System.Text.Json;

namespace GameOfLife.Forms;

public partial class MainForm : Form
{
    private List<Coso> _cosos = new List<Coso>();
    private Coso? _ultimoCosoMostrado;
    private bool _grupoGenerado;
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        WriteIndented = true
    };
     
    public MainForm()
    {
        InitializeComponent();
    } 
    
    // Boton que genera los cosos y los dibuja
    private void buttonGenerarGrupo_Click(object sender, EventArgs e)
    {
        if (_grupoGenerado) return;
                           
        _cosos = CosoGenerator.GenerarGrupoInicial(100);
        _grupoGenerado = true;
        panelMundo.Invalidate();
        
    }

    // Logica dibujar los cosos
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
                case EstadoAnimo.Deprimido: color = Color.Black; break;
                case EstadoAnimo.Neutro: color = Color.Gray; break;
                default: color = Color.Orange; break;
            }
            
            var brush = new SolidBrush(color);
            graphics.FillEllipse(brush, c.Posicion.X, c.Posicion.Y, 10, 10);  
 
        }
    }
    

    private void panelMundo_MouseMove(object sender, MouseEventArgs e)
    {
       var cosoCercano = _cosos.FirstOrDefault(c => c.Estado != Estado.Muerto &&
                                                    Math.Abs(c.Posicion.X - e.X) < 10 &&
                                                     Math.Abs(c.Posicion.Y - e.Y) < 10);

       // Solo actualiza el ToolTip si el coso ha cambiado
       // Solo continúa si el coso ha cambiado
       if (cosoCercano == _ultimoCosoMostrado)
           return;

        // Si no hay coso cerca, limpiar el ToolTip
       if (cosoCercano == null)
       {
           toolTipCoso.SetToolTip(panelMundo, "");
           _ultimoCosoMostrado = null;
           return;
       }

       // Mostrar información del nuevo coso
       string info = $"Código: {cosoCercano.Codigo}\n" +
                     $"Nombre: {cosoCercano.NombreCompleto}\n" +
                     $"Edad: {cosoCercano.Edad}\n" +
                     $"Sexo: {cosoCercano.Sexo}\n" +
                     $"Estado Civil: {cosoCercano.EstadoCivil}\n" +
                     $"Trabaja: {cosoCercano.Trabaja}\n" +
                     $"Sueldo: {cosoCercano.Salario:C}\n" +
                     $"Estado Animo: {cosoCercano.EstadoAnimo}\n" +
                     $"Tipo Sangre: {cosoCercano.TipoSangre}\n" +
                     $"Vida: {cosoCercano.Vida}\n" +
                     $"Ataque: {cosoCercano.Ataque}\n" +
                     $"Defensa: {cosoCercano.Defensa}\n" +
                     $"Resistencia: {cosoCercano.Resistencia}";

       toolTipCoso.SetToolTip(panelMundo, info);
       _ultimoCosoMostrado = cosoCercano;
    }


    private void panelMundo_MouseClick(object sender, MouseEventArgs e)
    {
        var cosoClicked = _cosos.FirstOrDefault(c => c.Estado != Estado.Muerto &&
                                                     Math.Abs(c.Posicion.X - e.X) < 10 &&
                                                     Math.Abs(c.Posicion.Y - e.Y) < 10);
         
        if (cosoClicked == null) return;
        try
        {            
            var jsonString = JsonSerializer.Serialize(cosoClicked, JsonOptions);
            
            var nombreArchivo = $"coso-{cosoClicked.Codigo}_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            
            var rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "Data", nombreArchivo);
            File.WriteAllText(rutaArchivo, jsonString);
            
            // Opcional: Mostrar un mensaje al usuario
            MessageBox.Show($"Información del Coso {cosoClicked.Codigo} exportada a:\n{rutaArchivo}", 
                "Exportación Exitosa!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
        }
        catch (Exception ex)
        {
            // Manejar errores de serialización o escritura de archivo
            MessageBox.Show($"Error al exportar el coso: {ex.Message}", 
                "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        
    }
}