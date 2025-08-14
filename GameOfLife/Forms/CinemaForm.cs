using GameOfLife.Models;

namespace GameOfLife.Forms;
public partial class CinemaForm : Form
{
    private List<Coso> _todasLasFamilias;
    private List<List<Coso>> _familiasEnCine;
    private const int FILAS = 8;
    private const int ASIENTOS_POR_FILA = 12;
    private const int TAMAÑO_ASIENTO = 40;
    private const int MARGEN_X = 50;
    private const int MARGEN_Y = 80;
    private bool[,] _asientosOcupados;
    
    public CinemaForm(List<Coso> poblacion)
    {
        InitializeComponent();
        _todasLasFamilias = poblacion;
        _asientosOcupados = new bool[FILAS, ASIENTOS_POR_FILA];
        
        // Configurar el formulario
        this.Text = "Cine - Familias en Función";
        this.Size = new Size(ASIENTOS_POR_FILA * TAMAÑO_ASIENTO + MARGEN_X * 2 + 100, 
                           FILAS * TAMAÑO_ASIENTO + MARGEN_Y * 2 + 100);
        this.StartPosition = FormStartPosition.CenterParent;
        this.BackColor = Color.Black;
        
        GenerarFamiliasEnCine();
        AsignarAsientos();
    }
    
    private void GenerarFamiliasEnCine()
    {
        _familiasEnCine = new List<List<Coso>>();
        
        // Buscar todas las parejas casadas vivas
        var parejasConHijos = _todasLasFamilias
            .Where(c => c.Estado == Estado.Vivo && 
                       c.EstadoCivil == EstadoCivil.Casado && 
                       c.CodigoPareja != null)
            .GroupBy(c => c.CodigoPareja < c.Codigo ? $"{c.CodigoPareja}-{c.Codigo}" : $"{c.Codigo}-{c.CodigoPareja}")
            .Select(g => g.ToList())
            .Where(pareja => pareja.Count == 2)
            .ToList();
        
        foreach (var pareja in parejasConHijos)
        {
            // 60% de probabilidad de que la familia vaya al cine
            if (new Random().NextDouble() < 0.6)
            {
                var familia = new List<Coso>();
                
                // Agregar padres
                var padre = pareja.First(p => p.Sexo == Sexo.Macho);
                var madre = pareja.First(p => p.Sexo == Sexo.Hembra);
                
                familia.Add(padre);
                familia.Add(madre);
                
                // Agregar hijos vivos
                var hijos = _todasLasFamilias
                    .Where(h => h.Estado == Estado.Vivo && 
                               (h.CodigoPadre == padre.Codigo || h.CodigoMadre == madre.Codigo))
                    .OrderBy(h => h.Edad) // Ordenar por edad para sentarlos ordenadamente
                    .ToList();
                
                familia.AddRange(hijos);
                
                // Solo agregar familias de 2 o más miembros
                if (familia.Count >= 2)
                {
                    _familiasEnCine.Add(familia);
                }
            }
        }
        
        // También incluir cosos solteros que van solos (pocos)
        var solterosSolos = _todasLasFamilias
            .Where(c => c.Estado == Estado.Vivo && 
                       c.EstadoCivil == EstadoCivil.Soltero && 
                       c.EsMayorDeEdad &&
                       new Random().NextDouble() < 0.2) // 20% de solteros van solos
            .Take(5) // Máximo 5 solteros
            .Select(s => new List<Coso> { s })
            .ToList();
            
        _familiasEnCine.AddRange(solterosSolos);
    }
    
    private void AsignarAsientos()
    {
        var random = new Random();
        
        foreach (var familia in _familiasEnCine)
        {
            int tamañoFamilia = familia.Count;
            bool asientosAsignados = false;
            int intentos = 0;
            
            // Intentar asignar asientos contiguos para la familia
            while (!asientosAsignados && intentos < 50)
            {
                int fila = random.Next(FILAS);
                int asientoInicial = random.Next(ASIENTOS_POR_FILA - tamañoFamilia + 1);
                
                // Verificar si hay suficientes asientos contiguos libres
                bool disponible = true;
                for (int i = 0; i < tamañoFamilia; i++)
                {
                    if (_asientosOcupados[fila, asientoInicial + i])
                    {
                        disponible = false;
                        break;
                    }
                }
                
                if (disponible)
                {
                    // Asignar posiciones a la familia
                    for (int i = 0; i < tamañoFamilia; i++)
                    {
                        _asientosOcupados[fila, asientoInicial + i] = true;
                        
                        // Asignar posición visual al coso
                        familia[i].Posicion = new Point(
                            MARGEN_X + (asientoInicial + i) * TAMAÑO_ASIENTO + TAMAÑO_ASIENTO / 2,
                            MARGEN_Y + fila * TAMAÑO_ASIENTO + TAMAÑO_ASIENTO / 2
                        );
                    }
                    asientosAsignados = true;
                }
                intentos++;
            }
        }
    }
    
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        
        Graphics g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        
        // Dibujar pantalla del cine
        using (var pantallaBrush = new SolidBrush(Color.LightGray))
        using (var pantallaFont = new Font("Arial", 16, FontStyle.Bold))
        using (var textBrush = new SolidBrush(Color.Black))
        {
            int pantallaWidth = ASIENTOS_POR_FILA * TAMAÑO_ASIENTO - 40;
            int pantallaX = MARGEN_X + 20;
            int pantallaY = 20;
            
            g.FillRectangle(pantallaBrush, pantallaX, pantallaY, pantallaWidth, 30);
            
            string texto = "PANTALLA";
            var textoSize = g.MeasureString(texto, pantallaFont);
            g.DrawString(texto, pantallaFont, textBrush, 
                pantallaX + (pantallaWidth - textoSize.Width) / 2, 
                pantallaY + (30 - textoSize.Height) / 2);
        }
        
        // Dibujar asientos vacíos
        using (var asientoVacioBrush = new SolidBrush(Color.White))
        {
            for (int fila = 0; fila < FILAS; fila++)
            {
                for (int asiento = 0; asiento < ASIENTOS_POR_FILA; asiento++)
                {
                    if (!_asientosOcupados[fila, asiento])
                    {
                        int x = MARGEN_X + asiento * TAMAÑO_ASIENTO;
                        int y = MARGEN_Y + fila * TAMAÑO_ASIENTO;
                        
                        g.FillRectangle(asientoVacioBrush, x + 2, y + 2, TAMAÑO_ASIENTO - 4, TAMAÑO_ASIENTO - 4);
                    }
                }
            }
        }
        
        // Dibujar familias en sus asientos
        var familiaColors = GenerarColoresFamilias();
        int familiaIndex = 0;
        
        foreach (var familia in _familiasEnCine)
        {
            var colorFamilia = familiaColors[familiaIndex % familiaColors.Count];
            
            for (int i = 0; i < familia.Count; i++)
            {
                var coso = familia[i];
                var pos = coso.Posicion;
                
                // Dibujar asiento ocupado
                using (var asientoBrush = new SolidBrush(Color.Maroon))
                {
                    g.FillRectangle(asientoBrush, 
                        pos.X - TAMAÑO_ASIENTO / 2 + 2, 
                        pos.Y - TAMAÑO_ASIENTO / 2 + 2, 
                        TAMAÑO_ASIENTO - 4, 
                        TAMAÑO_ASIENTO - 4);
                }
                
                // Dibujar coso
                using (var cosoBrush = new SolidBrush(colorFamilia))
                {
                    int tamaño = coso.EsMayorDeEdad ? 16 : 12;
                    g.FillEllipse(cosoBrush, 
                        pos.X - tamaño / 2, 
                        pos.Y - tamaño / 2, 
                        tamaño, tamaño);
                }
                
                // Indicador de género
                using (var generoFont = new Font("Arial", 8, FontStyle.Bold))
                using (var generoBrush = new SolidBrush(Color.White))
                {
                    string genero = coso.Sexo == Sexo.Macho ? "♂" : "♀";
                    var generoSize = g.MeasureString(genero, generoFont);
                    g.DrawString(genero, generoFont, generoBrush,
                        pos.X - generoSize.Width / 2,
                        pos.Y - generoSize.Height / 2);
                }
            }
            familiaIndex++;
        }
        
        // Dibujar información
        DibujarInformacion(g);
        
        // Dibujar leyenda
        DibujarLeyenda(g);
    }
    
    private void DibujarInformacion(Graphics g)
    {
        using (var font = new Font("Arial", 10, FontStyle.Bold))
        using (var brush = new SolidBrush(Color.White))
        {
            int totalFamilias = _familiasEnCine.Count;
            int totalPersonas = _familiasEnCine.Sum(f => f.Count);
            int asientosOcupados = totalPersonas;
            int asientosLibres = (FILAS * ASIENTOS_POR_FILA) - asientosOcupados;
            
            string info = $"Familias en el cine: {totalFamilias} | " +
                         $"Personas: {totalPersonas} | " +
                         $"Asientos ocupados: {asientosOcupados} | " +
                         $"Asientos libres: {asientosLibres}";
            
            g.DrawString(info, font, brush, 10, this.Height - 40);
        }
    }
    
    private void DibujarLeyenda(Graphics g)
    {
        using (var font = new Font("Arial", 9))
        using (var brush = new SolidBrush(Color.White))
        {
            int x = this.Width - 150;
            int y = 80;
            
            g.DrawString("LEYENDA:", font, brush, x, y);
            
            // Asiento vacío
            using (var vaciBrush = new SolidBrush(Color.White))
            {
                g.FillRectangle(vaciBrush, x, y + 20, 15, 15);
                g.DrawString("Asiento vacío", font, brush, x + 20, y + 22);
            }
            
            // Asiento ocupado
            using (var ocupBrush = new SolidBrush(Color.Maroon))
            {
                g.FillRectangle(ocupBrush, x, y + 40, 15, 15);
                g.DrawString("Asiento ocupado", font, brush, x + 20, y + 42);
            }
            
            // Símbolos de género
            g.DrawString("♂ Hombre", font, brush, x, y + 60);
            g.DrawString("♀ Mujer", font, brush, x, y + 75);
            g.DrawString("Tamaño = Edad", font, brush, x, y + 90);
        }
    }
    
    private List<Color> GenerarColoresFamilias()
    {
        return new List<Color>
        {
            Color.FromArgb(255, 100, 150, 255), // Azul claro
            Color.FromArgb(255, 255, 150, 100), // Naranja claro
            Color.FromArgb(255, 150, 255, 150), // Verde claro
            Color.FromArgb(255, 255, 255, 100), // Amarillo claro
            Color.FromArgb(255, 255, 150, 255), // Magenta claro
            Color.FromArgb(255, 150, 255, 255), // Cian claro
            Color.FromArgb(255, 255, 200, 150), // Melocotón
            Color.FromArgb(255, 200, 150, 255), // Lavanda
            Color.FromArgb(255, 150, 200, 255), // Azul cielo
            Color.FromArgb(255, 200, 255, 150)  // Verde lima
        };
    }
    
}
