using GameOfLife.Models;
using GameOfLife.Utils;
using System.Text.Json;
using GameOfLife.Data;

namespace GameOfLife.Forms;
public partial class MainForm : Form
{
    private List<Coso> _cosos = new List<Coso>();
    private Coso? _ultimoCosoMostrado;
    private bool _grupoGenerado;
    private Random _random = new Random();
    private bool _poblacionCargadaDeDB = false;
    
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        WriteIndented = true
    };
     
    public MainForm()
    {
        InitializeComponent();
        EnsureDataDirectoryExists();
        CargarPoblacionInicial();

        FormClosing += MainForm_FormClosing;
    } 
    
    private void EnsureDataDirectoryExists()
    {
        var dataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }
    }
     
    // Cargar población al iniciar
    private void CargarPoblacionInicial()
    {
        try
        {
            // Intentar cargar población existente de la base de datos
            var poblacionDB = DatabaseHelper.CargarPoblacion();
            
            if (poblacionDB.Count > 0)
            {
                _cosos = poblacionDB;
                _poblacionCargadaDeDB = true;
                _grupoGenerado = true;
                MessageBox.Show($"Población cargada desde base de datos: {poblacionDB.Count} individuos", 
                    "Carga exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                panelMundo.Invalidate();
            }
            else
            {
                _poblacionCargadaDeDB = false;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al cargar población: {ex.Message}\nInicia creando un nuevo grupo.", 
                "Error de carga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            _poblacionCargadaDeDB = false;
        }
    }
    
    // Guardar antes de cerrar
    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (_grupoGenerado && _cosos.Count > 0)
        {
            try
            {
                DatabaseHelper.GuardarPoblacion(_cosos);
                MessageBox.Show($"Población guardada exitosamente: {_cosos.Count} individuos", 
                    "Guardado exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                var result = MessageBox.Show($"Error al guardar población: {ex.Message}\n\n¿Desea cerrar sin guardar?", 
                    "Error de guardado", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                
                if (result == DialogResult.No)
                {
                    e.Cancel = true; // Cancelar el cierre
                }
            }
        }
    }
     
    // Botón: Generar Grupo Inicial
    private void buttonGenerarGrupo_Click(object sender, EventArgs e)
    {
        if (_grupoGenerado) 
        {
            var result = MessageBox.Show("Ya hay un grupo generado. ¿Desea crear uno nuevo?", 
                                       "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No) return;
        }
                           
        _cosos = CosoGenerator.GenerarGrupoInicial(100);
        _grupoGenerado = true;
        _poblacionCargadaDeDB = false;
        panelMundo.Invalidate();
    }
    
    // Exportar JSON con filtros
    private void buttonExportarJSON_Click(object sender, EventArgs e)
    {
        if (!_grupoGenerado)
        {
            MessageBox.Show("Primero debes generar o cargar un grupo!", "Advertencia", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var exportData = new
            {
                // Filtros solicitados
                CososMuertos = _cosos.Where(c => c.Estado == Estado.Muerto).Select(c => new
                {
                    c.Codigo,
                    c.NombreCompleto,
                    c.Edad,
                    c.Sexo,
                    c.FechaNacimiento,
                    c.FechaMuerte,
                    c.Posicion,
                    TieneHijos = c.HijosIds.Count > 0,
                    CantidadHijos = c.HijosIds.Count
                }).ToList(),

                CososVivos = _cosos.Where(c => c.Estado == Estado.Vivo).Select(c => new
                {
                    c.Codigo,
                    c.NombreCompleto,
                    c.Edad,
                    c.Sexo,
                    c.EstadoCivil,
                    c.EstadoAnimo,
                    c.Posicion,
                    c.Vida,
                    c.Trabaja,
                    c.Salario,
                    TieneHijos = c.HijosIds.Count > 0,
                    CantidadHijos = c.HijosIds.Count,
                    c.HaResucitado
                }).ToList(),

                CososConHijos = _cosos.Where(c => c.HijosIds.Count > 0).Select(c => new
                {
                    c.Codigo,
                    c.NombreCompleto,
                    c.Edad,
                    c.Sexo,
                    c.Estado,
                    CantidadHijos = c.HijosIds.Count,
                    HijosIds = c.HijosIds,
                    c.Posicion,
                    c.EstadoCivil
                }).ToList(),

                CososResucitados = _cosos.Where(c => c.HaResucitado).Select(c => new
                {
                    c.Codigo,
                    c.NombreCompleto,
                    c.Edad,
                    c.Sexo,
                    c.Estado,
                    c.FechaNacimiento,
                    c.FechaMuerte,
                    c.Posicion,
                    c.EstadoCivil,
                    c.EstadoAnimo
                }).ToList(),

                // Estadísticas generales
                Estadisticas = new
                {
                    TotalPoblacion = _cosos.Count,
                    TotalVivos = _cosos.Count(c => c.Estado == Estado.Vivo),
                    TotalMuertos = _cosos.Count(c => c.Estado == Estado.Muerto),
                    TotalCasados = _cosos.Count(c => c.EstadoCivil == EstadoCivil.Casado && c.Estado == Estado.Vivo),
                    TotalSolteros = _cosos.Count(c => c.EstadoCivil == EstadoCivil.Soltero && c.Estado == Estado.Vivo),
                    TotalConHijos = _cosos.Count(c => c.HijosIds.Count > 0),
                    TotalResucitados = _cosos.Count(c => c.HaResucitado),
                    TotalAdultos = _cosos.Count(c => c.EsMayorDeEdad && c.Estado == Estado.Vivo),
                    TotalMenores = _cosos.Count(c => !c.EsMayorDeEdad && c.Estado == Estado.Vivo),
                    FechaExportacion = DateTime.Now
                }
            };

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            saveDialog.FileName = $"poblacion_filtrada_{DateTime.Now:yyyyMMdd_HHmmss}.json";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                string json = JsonSerializer.Serialize(exportData, JsonOptions);
                File.WriteAllText(saveDialog.FileName, json);
                
                MessageBox.Show($"Exportación exitosa a:\n{saveDialog.FileName}\n\n" +
                              $"📊 RESUMEN DE FILTROS:\n" +
                              $"• Cosos muertos: {exportData.CososMuertos.Count}\n" +
                              $"• Cosos vivos: {exportData.CososVivos.Count}\n" +
                              $"• Cosos con hijos: {exportData.CososConHijos.Count}\n" +
                              $"• Cosos resucitados: {exportData.CososResucitados.Count}\n" +
                              $"• Total población: {exportData.Estadisticas.TotalPoblacion}",
                              "✅ Exportación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al exportar: {ex.Message}", "Error", 
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    // Guardar manualmente
    private void buttonGuardarDB_Click(object sender, EventArgs e)
    {
        if (!_grupoGenerado)
        {
            MessageBox.Show("No hay población para guardar!", "Advertencia", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            DatabaseHelper.GuardarPoblacion(_cosos);
            MessageBox.Show($"Población guardada exitosamente en base de datos:\n{_cosos.Count} individuos", 
                           "Guardado exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al guardar en base de datos: {ex.Message}", "Error", 
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // NUEVO BOTÓN: Cargar desde DB
    private void buttonCargarDB_Click(object sender, EventArgs e)
    {
        try
        {
            var poblacionDB = DatabaseHelper.CargarPoblacion();
            
            if (poblacionDB.Count > 0)
            {
                if (_grupoGenerado)
                {
                    var result = MessageBox.Show($"Ya tienes una población activa.\n¿Desea reemplazarla con la población guardada ({poblacionDB.Count} individuos)?", 
                                               "Confirmar carga", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No) return;
                }

                _cosos = poblacionDB;
                _grupoGenerado = true;
                _poblacionCargadaDeDB = true;
                panelMundo.Invalidate();
                
                MessageBox.Show($"Población cargada exitosamente desde base de datos:\n{poblacionDB.Count} individuos", 
                               "Carga exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No hay población guardada en la base de datos.", "Sin datos", 
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al cargar desde base de datos: {ex.Message}", "Error", 
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
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

        var hembras = _cosos.Where(c => c.Estado != Estado.Muerto && 
                                       c.EstadoCivil == EstadoCivil.Soltero && 
                                       c.Sexo == Sexo.Hembra).ToList();
        
        int parejasFormadas = 0;

        foreach (var hembra in hembras)
        {
            // Buscar macho compatible usando el método de tu clase
            var machosCompatibles = _cosos
                .Where(m => hembra.PuedeEmparejarseCon(m))
                .ToList();

            if (machosCompatibles.Any())
            {
                var macho = machosCompatibles[_random.Next(machosCompatibles.Count)];
                
                // Usar el método de tu clase para emparejar
                hembra.Emparejar(macho);
                parejasFormadas++;
            }
        }

        // Actualizar estados de ánimo de toda la población
        foreach (var coso in _cosos)
        {
            coso.ActualizarEstadoAnimo(_cosos);
        }

        panelMundo.Invalidate();
        MessageBox.Show($"Se formaron {parejasFormadas} parejas!", "Parejas Formadas", 
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // Botón: Generar Hijos
    private void buttonGenerarHijos_Click(object sender, EventArgs e)
    {
        if (!_grupoGenerado)
        {
            MessageBox.Show("Primero debes generar el grupo inicial!", "Advertencia", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var parejasConHijos = new List<(Coso macho, Coso hembra)>();
        int hijosNacidos = 0;

        // Buscar todas las parejas casadas
        var cososCasados = _cosos.Where(c => c.Estado != Estado.Muerto && 
                                            c.EstadoCivil == EstadoCivil.Casado).ToList();

        var parejasFormadas = new HashSet<string>();

        foreach (var coso in cososCasados)
        {
            if (coso.CodigoPareja == null) continue;

            var pareja = _cosos.FirstOrDefault(p => p.Codigo == coso.CodigoPareja);
            if (pareja == null || pareja.Estado == Estado.Muerto) continue;

            // Evitar duplicados (procesar cada pareja solo una vez)
            string parId = $"{coso.Codigo}-{pareja.Codigo}";
            if (parejasFormadas.Contains(parId) || parejasFormadas.Contains($"{pareja.Codigo}-{coso.Codigo}")) 
                continue;
            parejasFormadas.Add(parId);

            // Determinar macho y hembra
            var macho = coso.Sexo == Sexo.Macho ? coso : pareja;
            var hembra = coso.Sexo == Sexo.Hembra ? coso : pareja;

            // Verificar compatibilidad de factor RH usando tu método
            if (macho.TieneTipoSangreCompatible(hembra))
            {
                // Crear el hijo
                var hijo = GenerarHijo(macho, hembra);
                _cosos.Add(hijo);

                // Actualizar referencias familiares
                macho.HijosIds.Add(hijo.Codigo);
                hembra.HijosIds.Add(hijo.Codigo);

                parejasConHijos.Add((macho, hembra));
                hijosNacidos++;
            }
        }

        panelMundo.Invalidate();
        MessageBox.Show($"¡Nacieron {hijosNacidos} hijos!\n{parejasConHijos.Count} parejas tuvieron descendencia.", 
            "Nuevos Nacimientos", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private Coso GenerarHijo(Coso padre, Coso madre)
    {
        var hijo = new Coso
        {
            Codigo = Guid.NewGuid(),
            Edad = 0, // Los hijos nacen con 0 años
            Sexo = _random.NextDouble() < 0.5 ? Sexo.Macho : Sexo.Hembra,
            EstadoCivil = EstadoCivil.Soltero,
            Trabaja = false, // Los bebés no trabajan
            Salario = 0,
            EstadoAnimo = EstadoAnimo.Feliz, // Los bebés son felices
            Estado = Estado.Vivo,
            FechaNacimiento = DateTime.Now,
            
            // Genética: tipo de sangre aleatorio de los padres
            TipoSangre = _random.NextDouble() < 0.5 ? padre.TipoSangre : madre.TipoSangre,
            
            // Atributos de combate para bebés (bajos)
            Vida = _random.Next(20, 30),
            Ataque = _random.Next(5, 15),
            Defensa = _random.Next(5, 15),
            Arma = _random.Next(1, 5),
            Resistencia = 0,
            
            // Referencias familiares
            CodigoPadre = padre.Codigo,
            CodigoMadre = madre.Codigo,
            
            // Posición cerca de los padres
            Posicion = new Point(
                Math.Max(0, Math.Min(panelMundo.Width - 10, padre.Posicion.X + _random.Next(-20, 21))),
                Math.Max(0, Math.Min(panelMundo.Height - 10, padre.Posicion.Y + _random.Next(-20, 21)))
            )
        };

        // Lógica de nombres según el sexo del hijo
        if (hijo.Sexo == Sexo.Macho)
        {
            // Toma nombres del padre
            hijo.Nombre1 = padre.Nombre1;
            hijo.Nombre2 = padre.Nombre2;
        }
        else
        {
            // Toma nombres de la madre
            hijo.Nombre1 = madre.Nombre1;
            hijo.Nombre2 = madre.Nombre2;
        }

        // Apellidos: Primer apellido del padre, segundo apellido de la madre
        hijo.Apellido1 = padre.Apellido1;
        hijo.Apellido2 = madre.Apellido1; // Primer apellido de la madre

        return hijo;
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

        // Pedir porcentaje al usuario
        string input = Microsoft.VisualBasic.Interaction.InputBox(
            "Ingrese el porcentaje de población que se contagiará de COVID (0-100):", 
            "Porcentaje de Contagio", "30");
        
        if (!double.TryParse(input, out double porcentaje) || porcentaje < 0 || porcentaje > 100)
        {
            MessageBox.Show("Porcentaje inválido. Debe ser un número entre 0 y 100.", "Error");
            return;
        }

        int infectados = 0;
        int muertos = 0;

        foreach (var coso in _cosos.Where(c => c.Estado != Estado.Muerto))
        {
            // Verificar si se infecta según el porcentaje
            if (_random.NextDouble() * 100 < porcentaje)
            {
                infectados++;
                
                // Verificar si es población de riesgo usando tu método
                double probabilidadMuerte = coso.EsPoblacionRiesgo ? 0.20 : 0.05;
                
                if (_random.NextDouble() < probabilidadMuerte)
                {
                    coso.Estado = Estado.Muerto;
                    coso.FechaMuerte = DateTime.Now;
                    muertos++;
                    
                    // Si tenía pareja, la pareja queda viuda
                    if (coso.CodigoPareja.HasValue)
                    {
                        var pareja = _cosos.FirstOrDefault(p => p.Codigo == coso.CodigoPareja);
                        if (pareja != null)
                        {
                            pareja.EstadoCivil = EstadoCivil.Soltero;
                            pareja.CodigoPareja = null;
                            pareja.EstadoAnimo = EstadoAnimo.Triste;
                        }
                    }
                }
                else
                {
                    // Sobrevive pero con efectos y gana resistencia
                    coso.EstadoAnimo = _random.NextDouble() < 0.5 ? EstadoAnimo.Triste : EstadoAnimo.Deprimido;
                    coso.Vida = Math.Max(1, coso.Vida - 20);
                    coso.Resistencia += 1; // Incrementar resistencia
                }
            }
        }

        panelMundo.Invalidate();
        MessageBox.Show($"Pandemia COVID-19:\n- Infectados: {infectados}\n- Fallecidos: {muertos}\n- Sobrevivientes ganaron +1 Resistencia", 
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

        // Abrir el formulario del cine
        using (var cinemaForm = new CinemaForm(_cosos))
        {
            cinemaForm.ShowDialog(this);
        }

        // Opcional: Mejorar el estado de ánimo de quienes fueron al cine
        var cososVivos = _cosos.Where(c => c.Estado != Estado.Muerto).ToList();
        var cososQueAsistieron = 0;

        foreach (var coso in cososVivos)
        {
            // Solo algunos cosos mejoran su ánimo (los que "fueron" al cine)
            if (_random.NextDouble() < 0.4) // 40% fueron al cine
            {
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
                cososQueAsistieron++;
            }
        }

        panelMundo.Invalidate();
        MessageBox.Show($"Función terminada. {cososQueAsistieron} cosos disfrutaron del cine!", 
            "Fin de Función", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        var cososVivos = _cosos.Where(c => c.Estado != Estado.Muerto && c.EsMayorDeEdad).ToList();
        
        // 20% van a la guerra
        var cososEnGuerra = cososVivos.Where(c => _random.NextDouble() < 0.2).ToList();
        
        int muertos = 0;
        int heridos = 0;

        foreach (var coso in cososEnGuerra)
        {
            // Probabilidad de muerte en guerra
            if (_random.NextDouble() < 0.25) // 25% de muerte
            {
                coso.Estado = Estado.Muerto;
                coso.FechaMuerte = DateTime.Now;
                muertos++;
                
                // Si tenía pareja, la pareja queda viuda
                if (coso.CodigoPareja.HasValue)
                {
                    var pareja = _cosos.FirstOrDefault(p => p.Codigo == coso.CodigoPareja);
                    if (pareja != null)
                    {
                        pareja.EstadoCivil = EstadoCivil.Soltero;
                        pareja.CodigoPareja = null;
                        pareja.EstadoAnimo = EstadoAnimo.Triste;
                    }
                }
            }
            else if (_random.NextDouble() < 0.4) // 40% de heridos
            {
                coso.Vida = Math.Max(1, coso.Vida - _random.Next(30, 60));
                coso.EstadoAnimo = EstadoAnimo.Triste;
                coso.Resistencia += 1; // Gana resistencia por sobrevivir
                heridos++;
            }
            else
            {
                // Sobrevivientes sin heridas graves pero ganan resistencia
                coso.EstadoAnimo = _random.NextDouble() < 0.6 ? EstadoAnimo.Triste : EstadoAnimo.Enojado;
                coso.Resistencia += 1;
            }
        }

        panelMundo.Invalidate();
        MessageBox.Show($"Guerra:\n- Participantes: {cososEnGuerra.Count}\n- Muertos: {muertos}\n- Heridos: {heridos}\n- Sobrevivientes ganaron +1 Resistencia", 
            "Conflicto Bélico", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    // Botón: Avance de Tiempo
    private void buttonAvanzarTiempo_Click(object sender, EventArgs e)
    {
        if (!_grupoGenerado)
        {
            MessageBox.Show("Primero debes generar el grupo inicial!", "Advertencia", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        int muertosPorEdad = 0;
        var cososVivos = _cosos.Where(c => c.Estado != Estado.Muerto).ToList();

        foreach (var coso in cososVivos)
        {
            // Incrementar edad
            coso.Edad++;

            // Verificar muerte por edad
            if (coso.Edad >= coso.EdadMaxima)
            {
                coso.Estado = Estado.Muerto;
                coso.FechaMuerte = DateTime.Now;
                muertosPorEdad++;

                // Si tenía pareja, la pareja queda viuda
                if (coso.CodigoPareja.HasValue)
                {
                    var pareja = _cosos.FirstOrDefault(p => p.Codigo == coso.CodigoPareja);
                    if (pareja != null)
                    {
                        pareja.EstadoCivil = EstadoCivil.Soltero;
                        pareja.CodigoPareja = null;
                        pareja.EstadoAnimo = EstadoAnimo.Triste;
                    }
                }
            }
            else
            {
                // Los que cumplen 18 pueden empezar a trabajar
                if (coso.Edad == 18 && !coso.Trabaja && _random.NextDouble() < 0.7)
                {
                    coso.Trabaja = true;
                    coso.Salario = _random.Next(800, 3000);
                }

                // Actualizar estado de ánimo
                coso.ActualizarEstadoAnimo(_cosos);
            }
        }

        panelMundo.Invalidate();
        MessageBox.Show($"¡Un año ha pasado!\n- Fallecidos por edad: {muertosPorEdad}\n- Los jóvenes de 18 pueden trabajar ahora.", 
            "Avance del Tiempo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // Botón: Destrucción Global
    private void buttonDestruccionGlobal_Click(object sender, EventArgs e)
    {
        if (!_grupoGenerado)
        {
            MessageBox.Show("Primero debes generar el grupo inicial!", "Advertencia", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Preguntar tipo de catástrofe
        var resultado = MessageBox.Show("¿Qué catástrofe quieres desatar?\n\nSí = Meteorito\nNo = Terremoto\nCancelar = Cancelar", 
            "Seleccionar Catástrofe", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

        if (resultado == DialogResult.Cancel) return;

        string tipoCatastrofe = resultado == DialogResult.Yes ? "Meteorito" : "Terremoto";

        // Pedir porcentaje de mortalidad
        string input = Microsoft.VisualBasic.Interaction.InputBox(
            $"Ingrese el porcentaje de mortalidad del {tipoCatastrofe} (0-100):", 
            "Porcentaje de Mortalidad", "25");
        
        if (!double.TryParse(input, out double porcentajeMortalidad) || porcentajeMortalidad < 0 || porcentajeMortalidad > 100)
        {
            MessageBox.Show("Porcentaje inválido. Debe ser un número entre 0 y 100.", "Error");
            return;
        }

        int muertos = 0;
        int sobrevivientes = 0;
        var cososVivos = _cosos.Where(c => c.Estado != Estado.Muerto).ToList();

        foreach (var coso in cososVivos)
        {
            if (_random.NextDouble() * 100 < porcentajeMortalidad)
            {
                coso.Estado = Estado.Muerto;
                coso.FechaMuerte = DateTime.Now;
                muertos++;

                // Si tenía pareja, la pareja queda viuda
                if (coso.CodigoPareja.HasValue)
                {
                    var pareja = _cosos.FirstOrDefault(p => p.Codigo == coso.CodigoPareja);
                    if (pareja != null)
                    {
                        pareja.EstadoCivil = EstadoCivil.Soltero;
                        pareja.CodigoPareja = null;
                        pareja.EstadoAnimo = EstadoAnimo.Triste;
                    }
                }
            }
            else
            {
                // Sobrevive y gana resistencia
                coso.Resistencia += 1;
                sobrevivientes++;

                // Puede quedar traumatizado
                if (_random.NextDouble() < 0.3)
                {
                    coso.EstadoAnimo = _random.NextDouble() < 0.5 ? EstadoAnimo.Triste : EstadoAnimo.Deprimido;
                }
            }
        }

        panelMundo.Invalidate();
        MessageBox.Show($"¡{tipoCatastrofe} devastador!\n- Fallecidos: {muertos}\n- Sobrevivientes: {sobrevivientes}\n- Todos los sobrevivientes ganaron +1 Resistencia", 
            "Catástrofe Global", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    // Botón: Resucitar
    private void buttonResucitar_Click(object sender, EventArgs e)
    {
        if (!_grupoGenerado)
        {
            MessageBox.Show("Primero debes generar el grupo inicial!", "Advertencia", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var muertos = _cosos.Where(c => c.Estado == Estado.Muerto).ToList();
        
        if (!muertos.Any())
        {
            MessageBox.Show("No hay cosos muertos para resucitar.", "Sin Muertos", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        // Pedir porcentaje de resucitados
        string input = Microsoft.VisualBasic.Interaction.InputBox(
            $"Hay {muertos.Count} cosos muertos.\nIngrese el porcentaje que resucitará (0-100):", 
            "Porcentaje de Resurrección", "10");
        
        if (!double.TryParse(input, out double porcentajeResurreccion) || porcentajeResurreccion < 0 || porcentajeResurreccion > 100)
        {
            MessageBox.Show("Porcentaje inválido. Debe ser un número entre 0 y 100.", "Error");
            return;
        }

        int resucitados = 0;
        int posesiones = 0;

        foreach (var muerto in muertos)
        {
            if (_random.NextDouble() * 100 < porcentajeResurreccion)
            {
                // Verificar si su ex-pareja está viva y casada con otra persona
                if (muerto.CodigoPareja.HasValue)
                {
                    var exPareja = _cosos.FirstOrDefault(p => p.Codigo == muerto.CodigoPareja);
                    
                    if (exPareja != null && exPareja.Estado == Estado.Vivo && 
                        exPareja.EstadoCivil == EstadoCivil.Casado && exPareja.CodigoPareja != muerto.Codigo)
                    {
                        // La ex-pareja está casada con otra persona - POSESIÓN
                        var nuevaPareja = _cosos.FirstOrDefault(p => p.Codigo == exPareja.CodigoPareja);
                        
                        if (nuevaPareja != null)
                        {
                            // El alma del resucitado posee el cuerpo de la nueva pareja
                            // Mantener solo el Codigo, Posicion y referencias familiares de la nueva pareja
                            var codigoOriginal = nuevaPareja.Codigo;
                            var posicionOriginal = nuevaPareja.Posicion;
                            var hijosOriginales = nuevaPareja.HijosIds.ToList();
                            var padreOriginal = nuevaPareja.CodigoPadre;
                            var madreOriginal = nuevaPareja.CodigoMadre;

                            // Copiar todos los datos del resucitado
                            nuevaPareja.Nombre1 = muerto.Nombre1;
                            nuevaPareja.Nombre2 = muerto.Nombre2;
                            nuevaPareja.Apellido1 = muerto.Apellido1;
                            nuevaPareja.Apellido2 = muerto.Apellido2;
                            nuevaPareja.Edad = muerto.Edad;
                            nuevaPareja.Sexo = muerto.Sexo;
                            nuevaPareja.Trabaja = muerto.Trabaja;
                            nuevaPareja.Salario = muerto.Salario;
                            nuevaPareja.EstadoAnimo = muerto.EstadoAnimo;
                            nuevaPareja.TipoSangre = muerto.TipoSangre;
                            nuevaPareja.Vida = muerto.Vida;
                            nuevaPareja.Ataque = muerto.Ataque;
                            nuevaPareja.Defensa = muerto.Defensa;
                            nuevaPareja.Arma = muerto.Arma;
                            nuevaPareja.Resistencia = muerto.Resistencia;
                            
                            // Mantener algunas cosas del cuerpo poseído
                            nuevaPareja.Codigo = codigoOriginal;
                            nuevaPareja.Posicion = posicionOriginal;
                            nuevaPareja.CodigoPadre = padreOriginal;
                            nuevaPareja.CodigoMadre = madreOriginal;
                            
                            // Estado especial
                            nuevaPareja.HaResucitado = true;
                            nuevaPareja.Estado = Estado.Vivo;
                            nuevaPareja.FechaMuerte = null;
                            
                            // Mantener la relación matrimonial
                            nuevaPareja.EstadoCivil = EstadoCivil.Casado;
                            nuevaPareja.CodigoPareja = exPareja.Codigo;
                            
                            posesiones++;
                        }
                    }
                }
                
                if (posesiones == 0 || muerto.CodigoPareja == null) // Resurrección normal
                {
                    muerto.Estado = Estado.Vivo;
                    muerto.HaResucitado = true;
                    muerto.FechaMuerte = null;
                    muerto.EstadoAnimo = EstadoAnimo.Neutro;
                    muerto.Vida = Math.Max(muerto.Vida, 20); // Mínimo de vida
                    
                    // Si estaba casado, volver a soltero
                    muerto.EstadoCivil = EstadoCivil.Soltero;
                    muerto.CodigoPareja = null;
                    
                    // Posición aleatoria nueva
                    muerto.Posicion = new Point(
                        _random.Next(10, panelMundo.Width - 20),
                        _random.Next(10, panelMundo.Height - 20)
                    );
                }
                
                resucitados++;
            }
        }

        panelMundo.Invalidate();
        
        string mensaje = $"¡Milagro de resurrección!\n- Resucitados: {resucitados}";
        if (posesiones > 0)
        {
            mensaje += $"\n- Posesiones: {posesiones}\n- Algunos cosos fueron poseídos por almas de ex-parejas";
        }
        
        MessageBox.Show(mensaje, "Resurrección", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // Botón: Reset/Reiniciar
    private void buttonReset_Click(object sender, EventArgs e)
    {
       var result = MessageBox.Show("¿Está seguro de que desea reiniciar?\nSe perderá la población actual (pero se mantendrá la última guardada en BD).", 
                                   "Confirmar reinicio", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        
        if (result == DialogResult.Yes)
        {
            _cosos.Clear();
            _grupoGenerado = false;
            _poblacionCargadaDeDB = false;
            _ultimoCosoMostrado = null;
            toolTipCoso.SetToolTip(panelMundo, "");
            panelMundo.Invalidate();
            
            MessageBox.Show("Mundo reiniciado!", "Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    // Lógica para dibujar los cosos
    private void panelMundo_Paint(object sender, PaintEventArgs e)
    {
        var graphics = e.Graphics;
        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        
        foreach (var coso in _cosos)
        {
            if (coso.Estado == Estado.Muerto) continue;

            Color color = coso.GetColorPorEstadoAnimo();
            
            using (var brush = new SolidBrush(color))
            {
                // Tamaño diferente según la edad
                int tamaño = coso.EsMayorDeEdad ? 10 : 6; // Los menores son más pequeños
                graphics.FillEllipse(brush, coso.Posicion.X, coso.Posicion.Y, tamaño, tamaño);
                
                // Indicador visual para casados (anillo)
                if (coso.EstadoCivil == EstadoCivil.Casado)
                {
                    using (var pen = new Pen(Color.Gold, 2))
                    {
                        graphics.DrawEllipse(pen, coso.Posicion.X - 1, coso.Posicion.Y - 1, tamaño + 2, tamaño + 2);
                    }
                }
                
                // Indicador para resucitados
                if (coso.HaResucitado)
                {
                    using (var pen = new Pen(Color.White, 1))
                    {
                        graphics.DrawRectangle(pen, coso.Posicion.X - 2, coso.Posicion.Y - 2, tamaño + 4, tamaño + 4);
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
        var hijos = _cosos.Count(c => c.Estado != Estado.Muerto && !c.EsMayorDeEdad);
        var adultos = _cosos.Count(c => c.Estado != Estado.Muerto && c.EsMayorDeEdad);
        var resucitados = _cosos.Count(c => c.HaResucitado && c.Estado != Estado.Muerto);

        string stats = $"Vivos: {vivos} | Muertos: {muertos} | Casados: {casados} | Solteros: {solteros}";
        string stats2 = $"Adultos: {adultos} | Hijos: {hijos} | Resucitados: {resucitados}";
        
        using (var font = new Font("Arial", 9))
        using (var brush = new SolidBrush(Color.Black))
        using (var bgBrush = new SolidBrush(Color.FromArgb(200, Color.White)))
        {
            var size1 = graphics.MeasureString(stats, font);
            var size2 = graphics.MeasureString(stats2, font);
            var maxWidth = Math.Max(size1.Width, size2.Width);
            
            graphics.FillRectangle(bgBrush, 5, 5, maxWidth + 10, 35);
            graphics.DrawString(stats, font, brush, 10, 10);
            graphics.DrawString(stats2, font, brush, 10, 25);
        }
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
    
    private string FormatCosoInfo(Coso coso)
    {
        var info = $"Código: {coso.Codigo}\n" +
                   $"Nombre: {coso.NombreCompleto}\n" +
                   $"Edad: {coso.Edad}\n" +
                   $"Sexo: {coso.Sexo}\n" +
                   $"Estado Civil: {coso.EstadoCivil}\n";

        // Información de pareja
        if (coso.CodigoPareja != null)
        {
            var pareja = _cosos.FirstOrDefault(p => p.Codigo == coso.CodigoPareja);
            if (pareja != null)
            {
                info += $"Pareja: {pareja.NombreCompleto}\n";
            }
        }

        // Información de padres
        if (coso.CodigoPadre != null || coso.CodigoMadre != null)
        {
            var padre = coso.CodigoPadre != null ? _cosos.FirstOrDefault(p => p.Codigo == coso.CodigoPadre) : null;
            var madre = coso.CodigoMadre != null ? _cosos.FirstOrDefault(p => p.Codigo == coso.CodigoMadre) : null;
            
            if (padre != null)
                info += $"Padre: {padre.NombreCompleto}\n";
            if (madre != null)
                info += $"Madre: {madre.NombreCompleto}\n";
        }

        // Información de hijos
        if (coso.HijosIds.Any())
        {
            info += $"Hijos: {coso.HijosIds.Count}\n";
        }

        info += $"Trabaja: {coso.Trabaja}\n" +
                $"Sueldo: {coso.Salario:C}\n" +
                $"Estado Ánimo: {coso.EstadoAnimo}\n" +
                $"Tipo Sangre: {coso.TipoSangre}\n" +
                $"Vida: {coso.Vida}\n" +
                $"Ataque: {coso.Ataque} (Total: {coso.AtaqueTotal})\n" +
                $"Defensa: {coso.Defensa} (Total: {coso.DefensaTotal})\n" +
                $"Resistencia: {coso.Resistencia}";

        if (coso.HaResucitado)
            info += "\n¡RESUCITADO!";

        return info;
    }
    
    private void ExportCosoToJson(Coso coso)
    {
        try
        {            
            var jsonString = JsonSerializer.Serialize(coso, JsonOptions);
            
            var nombreArchivo = $"coso-{coso.Codigo}_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            var rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "Data", nombreArchivo);
            
            File.WriteAllText(rutaArchivo, jsonString);
            
            MessageBox.Show($"Información del Coso {coso.NombreCompleto} exportada a:\n{rutaArchivo}", 
                "Exportación Exitosa!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al exportar el coso: {ex.Message}", 
                "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void buttonTransferirCoso_Click(object sender, EventArgs e)
    {
        if (!_grupoGenerado || !_cosos.Any(c => c.Estado == Estado.Vivo))
        {
            MessageBox.Show(@"No tienes cosos vivos para transferir!", @"Sin cosos", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using (var transferForm = new TransferForm(_cosos))
        {
            transferForm.OnCosoReceived += (cosoRecibido) =>
            {
                _cosos.Add(cosoRecibido);
                panelMundo.Invalidate();
            };
        
            transferForm.ShowDialog(this);
        }
    }
    
}
