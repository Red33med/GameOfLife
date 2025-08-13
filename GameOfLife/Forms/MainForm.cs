using GameOfLife.Models;
using GameOfLife.Utils;
using System.Text.Json;

namespace GameOfLife.Forms;
public partial class MainForm : Form
{
    private List<Coso> _cosos = new List<Coso>();
    private Coso? _ultimoCosoMostrado;
    private bool _grupoGenerado;
    private Random _random = new Random();
    
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        WriteIndented = true
    };
     
    public MainForm()
    {
        InitializeComponent();
        EnsureDataDirectoryExists();
    } 
    
    private static void EnsureDataDirectoryExists()
    {
        var dataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }
    }
    
    // Botón: Generar Grupo Inicial
    private void buttonGenerarGrupo_Click(object sender, EventArgs e)
    {
        if (_grupoGenerado) return;
                           
        _cosos = CosoGenerator.GenerarGrupoInicial(100);
        _grupoGenerado = true;
        panelMundo.Invalidate();
    }

    // Botón: Generar Parejas
    private void buttonGenerarParejas_Click(object sender, EventArgs e)
    {
        if (!_grupoGenerado)
        {
            MessageBox.Show("Primero debes generar el grupo inicial!", "Advertencia", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var cososDisponibles = _cosos.Where(c => c.Estado != Estado.Muerto && 
                                                c.EstadoCivil == EstadoCivil.Soltero).ToList();
        
        int parejasFormadas = 0;
        var cososProcesados = new HashSet<Guid>();

        foreach (var coso in cososDisponibles)
        {
            if (cososProcesados.Contains(coso.Codigo)) continue;

            // Buscar pareja compatible
            var posiblesParejas = cososDisponibles
                .Where(p => p.Codigo != coso.Codigo && 
                           !cososProcesados.Contains(p.Codigo) &&
                           Math.Abs(p.Edad - coso.Edad) <= 10) // Diferencia máxima de edad
                .ToList();

            if (posiblesParejas.Count == 0) continue;
            var pareja = posiblesParejas[_random.Next(posiblesParejas.Count)];
                
            // Formar la pareja
            coso.EstadoCivil = EstadoCivil.Casado;
            pareja.EstadoCivil = EstadoCivil.Casado;
                
            // Mejorar estado de ánimo
            if (coso.EstadoAnimo is EstadoAnimo.Triste or EstadoAnimo.Deprimido)
                coso.EstadoAnimo = EstadoAnimo.Feliz;
            if (pareja.EstadoAnimo is EstadoAnimo.Triste or EstadoAnimo.Deprimido)
                pareja.EstadoAnimo = EstadoAnimo.Feliz;

            cososProcesados.Add(coso.Codigo);
            cososProcesados.Add(pareja.Codigo);
            parejasFormadas++;
        }

        panelMundo.Invalidate();
        MessageBox.Show($"Se formaron {parejasFormadas} parejas!", "Parejas Formadas", 
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // Botón: COVID-19 (Pandemia)
    private void buttonCovid_Click(object sender, EventArgs e)
    {
        if (!_grupoGenerado)
        {
            MessageBox.Show("Primero debes generar el grupo inicial!", "Advertencia", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        int infectados = 0;
        int muertos = 0;

        foreach (var coso in _cosos.Where(c => c.Estado != Estado.Muerto))
        {
            // 30% probabilidad de infección
            if (_random.NextDouble() < 0.3)
            {
                infectados++;
                
                // Los mayores de 60 tienen más riesgo de muerte
                double probabilidadMuerte = coso.Edad > 60 ? 0.15 : 0.05;
                
                if (_random.NextDouble() < probabilidadMuerte)
                {
                    coso.Estado = Estado.Muerto;
                    muertos++;
                }
                else
                {
                    // Sobrevive pero con efectos
                    coso.EstadoAnimo = _random.NextDouble() < 0.5 ? EstadoAnimo.Triste : EstadoAnimo.Deprimido;
                    coso.Vida = Math.Max(1, coso.Vida - 20); // Reduce vida
                }
            }
        }

        panelMundo.Invalidate();
        MessageBox.Show($"Pandemia COVID-19:\n- Infectados: {infectados}\n- Fallecidos: {muertos}", 
            "Pandemia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    // Botón: Ir al Cine
    private void buttonCine_Click(object sender, EventArgs e)
    {
        if (!_grupoGenerado)
        {
            MessageBox.Show("Primero debes generar el grupo inicial!", "Advertencia", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var cososVivos = _cosos.Where(c => c.Estado != Estado.Muerto).ToList();
        
        // 40% de los cosos van al cine
        var cososEnCine = cososVivos.Where(c => _random.NextDouble() < 0.4).ToList();

        foreach (var coso in cososEnCine)
        {
            // El cine mejora el estado de ánimo
            switch (coso.EstadoAnimo)
            {
                case EstadoAnimo.Deprimido:
                    coso.EstadoAnimo = EstadoAnimo.Triste;
                    break;
                case EstadoAnimo.Triste:
                    coso.EstadoAnimo = EstadoAnimo.Neutro;
                    break;
                case EstadoAnimo.Neutro:
                    coso.EstadoAnimo = EstadoAnimo.Feliz;
                    break;
                case EstadoAnimo.Enojado:
                    coso.EstadoAnimo = _random.NextDouble() < 0.7 ? EstadoAnimo.Neutro : EstadoAnimo.Feliz;
                    break;
                // Los felices siguen felices
            }
        }

        panelMundo.Invalidate();
        MessageBox.Show($"{cososEnCine.Count} cosos fueron al cine y mejoraron su estado de ánimo!", 
            "Día de Cine", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // Botón: Guerra
    private void buttonGuerra_Click(object sender, EventArgs e)
    {
        if (!_grupoGenerado)
        {
            MessageBox.Show("Primero debes generar el grupo inicial!", "Advertencia", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var cososVivos = _cosos.Where(c => c.Estado != Estado.Muerto && c.Edad >= 18 && c.Edad <= 50).ToList();
        
        // 20% van guerra
        var cososEnGuerra = cososVivos.Where(c => _random.NextDouble() < 0.2).ToList();
        
        int muertos = 0;
        int heridos = 0;

        foreach (var coso in cososEnGuerra)
        {
            // Probabilidad de muerte en guerra
            if (_random.NextDouble() < 0.25) // 25% de muerte
            {
                coso.Estado = Estado.Muerto;
                muertos++;
            }
            else if (_random.NextDouble() < 0.4) // 40% de heridos
            {
                coso.Vida = Math.Max(1, coso.Vida - _random.Next(30, 60));
                coso.EstadoAnimo = EstadoAnimo.Triste;
                heridos++;
            }
            else
            {
                // Sobrevivientes sin heridas graves
                coso.EstadoAnimo = _random.NextDouble() < 0.6 ? EstadoAnimo.Triste : EstadoAnimo.Enojado;
            }
        }

        panelMundo.Invalidate();
        MessageBox.Show($"Guerra:\n- Participantes: {cososEnGuerra.Count}\n- Muertos: {muertos}\n- Heridos: {heridos}", 
            "Conflicto Bélico", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    // Botón: Reset/Reiniciar
    private void buttonReset_Click(object sender, EventArgs e)
    {
        _cosos.Clear();
        _grupoGenerado = false;
        _ultimoCosoMostrado = null;
        toolTipCoso.SetToolTip(panelMundo, "");
        panelMundo.Invalidate();
        
        MessageBox.Show("Mundo reiniciado!", "Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // Lógica para dibujar los cosos
    private void panelMundo_Paint(object sender, PaintEventArgs e)
    {
        var graphics = e.Graphics;
        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        
        foreach (var coso in _cosos)
        {
            if (coso.Estado == Estado.Muerto) continue;

            Color color = GetColorByMood(coso.EstadoAnimo);
            
            using (var brush = new SolidBrush(color))
            {
                graphics.FillEllipse(brush, coso.Posicion.X, coso.Posicion.Y, 10, 10);
                
                // Indicador visual para casados (anillo)
                if (coso.EstadoCivil == EstadoCivil.Casado)
                {
                    using (var pen = new Pen(Color.Gold, 2))
                    {
                        graphics.DrawEllipse(pen, coso.Posicion.X - 1, coso.Posicion.Y - 1, 12, 12);
                    }
                }
            }
        }
        
        // Mostrar estadísticas en la esquina
        DrawStatistics(graphics);
    }
    
    private void DrawStatistics(Graphics graphics)
    {
        if (!_grupoGenerado) return;

        var vivos = _cosos.Count(c => c.Estado != Estado.Muerto);
        var muertos = _cosos.Count(c => c.Estado == Estado.Muerto);
        var casados = _cosos.Count(c => c.EstadoCivil == EstadoCivil.Casado && c.Estado != Estado.Muerto);
        var solteros = _cosos.Count(c => c.EstadoCivil == EstadoCivil.Soltero && c.Estado != Estado.Muerto);

        string stats = $"Vivos: {vivos} | Muertos: {muertos} | Casados: {casados} | Solteros: {solteros}";
        
        using (var font = new Font("Arial", 10))
        using (var brush = new SolidBrush(Color.Black))
        {
            graphics.DrawString(stats, font, brush, 10, 10);
        }
    }
    
    private static Color GetColorByMood(EstadoAnimo estadoAnimo)
    {
        return estadoAnimo switch
        {
            EstadoAnimo.Feliz => Color.Green,
            EstadoAnimo.Triste => Color.Blue,
            EstadoAnimo.Enojado => Color.Red,
            EstadoAnimo.Deprimido => Color.Black,
            EstadoAnimo.Neutro => Color.Gray,
            _ => Color.Orange
        };
    }

    private void panelMundo_MouseMove(object sender, MouseEventArgs e)
    {
        var cosoCercano = FindCosoAtPosition(e.X, e.Y);

        if (cosoCercano == _ultimoCosoMostrado)
            return;

        if (cosoCercano == null)
        {
            toolTipCoso.SetToolTip(panelMundo, "");
            _ultimoCosoMostrado = null;
            return;
        }

        string info = FormatCosoInfo(cosoCercano);
        toolTipCoso.SetToolTip(panelMundo, info);
        _ultimoCosoMostrado = cosoCercano;
    }

    private void panelMundo_MouseClick(object sender, MouseEventArgs e)
    {
        var cosoClicked = FindCosoAtPosition(e.X, e.Y);
         
        if (cosoClicked == null) return;
        
        ExportCosoToJson(cosoClicked);
    }
    
    // Métodos helper
    private Coso? FindCosoAtPosition(int x, int y)
    {
        return _cosos.FirstOrDefault(c => c.Estado != Estado.Muerto &&
                                         Math.Abs(c.Posicion.X - x) < 10 &&
                                         Math.Abs(c.Posicion.Y - y) < 10);
    }
    
    private static string FormatCosoInfo(Coso coso)
    {
        return $"Código: {coso.Codigo}\n" +
               $"Nombre: {coso.NombreCompleto}\n" +
               $"Edad: {coso.Edad}\n" +
               $"Sexo: {coso.Sexo}\n" +
               $"Estado Civil: {coso.EstadoCivil}\n" +
               $"Trabaja: {coso.Trabaja}\n" +
               $"Sueldo: {coso.Salario:C}\n" +
               $"Estado Ánimo: {coso.EstadoAnimo}\n" +
               $"Tipo Sangre: {coso.TipoSangre}\n" +
               $"Vida: {coso.Vida}\n" +
               $"Ataque: {coso.Ataque}\n" +
               $"Defensa: {coso.Defensa}\n" +
               $"Resistencia: {coso.Resistencia}";
    }
    
    private static void ExportCosoToJson(Coso coso)
    {
        try
        {            
            var jsonString = JsonSerializer.Serialize(coso, JsonOptions);
            
            var nombreArchivo = $"coso-{coso.Codigo}_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            var rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "Data", nombreArchivo);
            
            File.WriteAllText(rutaArchivo, jsonString);
            
            MessageBox.Show($"Información del Coso {coso.Codigo} exportada a:\n{rutaArchivo}", 
                "Exportación Exitosa!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al exportar el coso: {ex.Message}", 
                "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
