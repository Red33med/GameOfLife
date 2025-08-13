using System.Text;
using System.Text.Json;
using GameOfLife.Models;
using GameOfLife.Utils;

namespace GameOfLife.Services;
public class CosoServices
    {
        private static readonly Random Rnd = new Random();
        
        // Método auxiliar para matar un coso (usado en varios procesos)
        private static void MatarCoso(Coso coso, List<Coso> poblacion)
        {
            coso.Estado = Estado.Muerto;
            coso.FechaMuerte = DateTime.Now;
            
            // Si tenía pareja, la pareja pasa a soltero y cambia estado de ánimo a triste
            if (coso.CodigoPareja.HasValue)
            {
                var pareja = poblacion.FirstOrDefault(c => c.Codigo == coso.CodigoPareja);
                if (pareja != null)
                {
                    pareja.EstadoCivil = EstadoCivil.Soltero;
                    pareja.CodigoPareja = null;
                    pareja.EstadoAnimo = EstadoAnimo.Triste;
                }
            }
            
            coso.CodigoPareja = null;
        }
        
        // Proceso 2: Generar Parejas
        public static int GenerarParejas(List<Coso> poblacion)
        {
            int parejasFormadas = 0;
            var solteros = poblacion.Where(c => c.Estado == Estado.Vivo && c.EstadoCivil == EstadoCivil.Soltero).ToList();
            
            foreach (var coso in solteros.ToList()) // ToList() para evitar modificar mientras iteramos
            {
                if (coso.EstadoCivil != EstadoCivil.Soltero) continue; // Puede haberse emparejado ya
                
                var candidatos = solteros.Where(c => c.Codigo != coso.Codigo && 
                                                     c.EstadoCivil == EstadoCivil.Soltero &&
                                                     coso.PuedeEmparejarseCon(c)).ToList();
                
                if (candidatos.Any())
                {
                    var pareja = candidatos[Rnd.Next(candidatos.Count)];
                    coso.Emparejar(pareja);
                    parejasFormadas++;
                }
            }
            
            return parejasFormadas;
        }
        
        // Proceso 3: Generar Hijos
        public static (int hijosNacidos, List<Coso> nuevosHijos) GenerarHijos(List<Coso> poblacion)
        {
            var nuevosHijos = new List<Coso>();
            var parejas = poblacion.Where(c => c.Estado == Estado.Vivo && 
                                              c.EstadoCivil == EstadoCivil.Casado &&
                                              c.Sexo == Sexo.Macho) // Solo tomamos machos para evitar duplicados
                                        .ToList();
            
            foreach (var padre in parejas)
            {
                var madre = poblacion.FirstOrDefault(c => c.Codigo == padre.CodigoPareja);
                if (madre == null || madre.Estado != Estado.Vivo) continue;
                
                // Verificar compatibilidad de factor Rh
                if (padre.TieneTipoSangreCompatible(madre))
                {
                    var hijo = CosoGenerator.GenerarHijo(padre, madre);
                    nuevosHijos.Add(hijo);
                }
            }
            
            return (nuevosHijos.Count, nuevosHijos);
        }
        
        // Proceso 4: Enfermedad Covid
        public static int SimularCovid(List<Coso> poblacion, double porcentajeInfeccion)
        {
            int muertes = 0;
            var vivos = poblacion.Where(c => c.Estado == Estado.Vivo).ToList();
            int infectados = (int)(vivos.Count * porcentajeInfeccion / 100);
            
            // Seleccionar infectados al azar
            var cososMezclados = vivos.OrderBy(x => Rnd.Next()).Take(infectados).ToList();
            
            foreach (var coso in cososMezclados)
            {
                bool muere = false;
                
                // Población de riesgo: >40 años, macho, soltero
                if (coso.EsPoblacionRiesgo)
                {
                    muere = Rnd.NextDouble() < 0.2; // 20% de mortalidad
                }
                else
                {
                    muere = Rnd.NextDouble() < 0.05; // 5% para población general
                }
                
                if (muere)
                {
                    MatarCoso(coso, poblacion);
                    muertes++;
                }
                else
                {
                    // Sobrevive: +1 resistencia
                    coso.Resistencia++;
                }
            }
            
            return muertes;
        }
        
        // Proceso 5: Destrucción Global
        public static int SimularDesastre(List<Coso> poblacion, double porcentajeMortalidad, string tipoDesastre)
        {
            int muertes = 0;
            var vivos = poblacion.Where(c => c.Estado == Estado.Vivo).ToList();
            int victimas = (int)(vivos.Count * porcentajeMortalidad / 100);
            
            // Seleccionar víctimas al azar
            var cososAfectados = vivos.OrderBy(x => Rnd.Next()).Take(victimas).ToList();
            
            foreach (var coso in cososAfectados)
            {
                MatarCoso(coso, poblacion);
                muertes++;
            }
            
            // Los supervivientes ganan resistencia
            var supervivientes = vivos.Except(cososAfectados);
            foreach (var superviviente in supervivientes)
            {
                superviviente.Resistencia++;
            }
            
            return muertes;
        }
        
        // Proceso 6: Lucha del más fuerte
        public static (string resultado, bool hayMuerto) SimularLucha(List<Coso> poblacion)
        {
            var vivos = poblacion.Where(c => c.Estado == Estado.Vivo).ToList();
            if (vivos.Count < 2)
                return ("No hay suficientes cosos vivos para luchar", false);
                
            var luchador1 = vivos[Rnd.Next(vivos.Count)];
            var luchador2 = vivos[Rnd.Next(vivos.Count)];
            
            // Asegurar que sean diferentes
            while (luchador2.Codigo == luchador1.Codigo)
            {
                luchador2 = vivos[Rnd.Next(vivos.Count)];
            }
            
            return EjecutarLucha(luchador1, luchador2, poblacion);
        }
        
        private static (string resultado, bool hayMuerto) EjecutarLucha(Coso luchador1, Coso luchador2, List<Coso> poblacion)
        {
            var resultado = new StringBuilder();
            resultado.AppendLine($"🥊 LUCHA: {luchador1.NombreCompleto} vs {luchador2.NombreCompleto}");
            resultado.AppendLine($"L1: Vida={luchador1.Vida + luchador1.DefensaTotal}, Ataque={luchador1.AtaqueTotal}");
            resultado.AppendLine($"L2: Vida={luchador2.Vida + luchador2.DefensaTotal}, Ataque={luchador2.AtaqueTotal}");
            
            int vidaL1 = luchador1.Vida + luchador1.DefensaTotal;
            int vidaL2 = luchador2.Vida + luchador2.DefensaTotal;
            
            int turno = 1;
            while (vidaL1 > 0 && vidaL2 > 0)
            {
                if (turno % 2 == 1) // Turno de luchador1
                {
                    int daño = luchador1.AtaqueTotal;
                    vidaL2 -= daño;
                    resultado.AppendLine($"Turno {turno}: {luchador1.NombreCompleto} ataca por {daño}. Vida L2: {Math.Max(0, vidaL2)}");
                }
                else // Turno de luchador2
                {
                    int daño = luchador2.AtaqueTotal;
                    vidaL1 -= daño;
                    resultado.AppendLine($"Turno {turno}: {luchador2.NombreCompleto} ataca por {daño}. Vida L1: {Math.Max(0, vidaL1)}");
                }
                turno++;
                
                // Prevenir bucles infinitos
                if (turno > 100)
                {
                    resultado.AppendLine("¡Lucha muy larga! Empate por agotamiento.");
                    return (resultado.ToString(), false);
                }
            }
            
            if (vidaL1 <= 0)
            {
                resultado.AppendLine($"🏆 GANADOR: {luchador2.NombreCompleto}");
                resultado.AppendLine($"💀 {luchador1.NombreCompleto} ha muerto en combate");
                luchador2.Resistencia++; // El ganador gana resistencia
                MatarCoso(luchador1, poblacion);
                return (resultado.ToString(), true);
            }
            else
            {
                resultado.AppendLine($"🏆 GANADOR: {luchador1.NombreCompleto}");
                resultado.AppendLine($"💀 {luchador2.NombreCompleto} ha muerto en combate");
                luchador1.Resistencia++; // El ganador gana resistencia
                MatarCoso(luchador2, poblacion);
                return (resultado.ToString(), true);
            }
        }
        
        // Proceso 7: Avance de tiempo
        public static int AvanzarTiempo(List<Coso> poblacion)
        {
            int muertes = 0;
            var vivos = poblacion.Where(c => c.Estado == Estado.Vivo).ToList();
            
            foreach (var coso in vivos)
            {
                coso.Edad++;
                
                // Verificar muerte por edad
                if (coso.Edad >= coso.EdadMaxima)
                {
                    MatarCoso(coso, poblacion);
                    muertes++;
                }
                else
                {
                    // Actualizar estado de ánimo basado en nuevas condiciones
                    coso.ActualizarEstadoAnimo(poblacion);
                }
            }
            
            return muertes;
        }
        
        // Proceso 8: Resucitar
        public static int Resucitar(List<Coso> poblacion, double porcentajeResurreccion)
        {
            var muertos = poblacion.Where(c => c.Estado == Estado.Muerto && !c.HaResucitado).ToList();
            int cantidadResucitar = (int)(muertos.Count * porcentajeResurreccion / 100);
            
            var resucitados = muertos.OrderBy(x => Rnd.Next()).Take(cantidadResucitar).ToList();
            
            foreach (var muerto in resucitados)
            {
                // Verificar si tenía pareja y si esa pareja está ahora emparejada
                if (muerto.CodigoPareja.HasValue)
                {
                    var exPareja = poblacion.FirstOrDefault(c => c.Codigo == muerto.CodigoPareja);
                    if (exPareja != null && exPareja.EstadoCivil == EstadoCivil.Casado && exPareja.CodigoPareja != muerto.Codigo)
                    {
                        // La ex pareja tiene nueva pareja - posesión del alma
                        var nuevaPareja = poblacion.FirstOrDefault(c => c.Codigo == exPareja.CodigoPareja);
                        if (nuevaPareja != null)
                        {
                            // El alma del resucitado posee el cuerpo de la nueva pareja
                            PoseerCuerpo(muerto, nuevaPareja, poblacion);
                            continue; // No agregar el muerto original, ya fue poseído
                        }
                    }
                }
                
                // Resurrección normal
                muerto.Estado = Estado.Vivo;
                muerto.HaResucitado = true;
                muerto.FechaMuerte = null;
                muerto.EstadoAnimo = EstadoAnimo.Neutro; // Los resucitados empiezan neutrales
                
                // Restaurar algo de vida
                muerto.Vida = Rnd.Next(30, 51); // Los resucitados tienen menos vida
            }
            
            return cantidadResucitar;
        }
        
        // Método para la posesión de cuerpos (Proceso 8)
        private static void PoseerCuerpo(Coso alma, Coso cuerpo, List<Coso> poblacion)
        {
            // El alma sobrescribe todos los datos del cuerpo, excepto las relaciones actuales
            var parejaActual = cuerpo.CodigoPareja;
            var posicionActual = cuerpo.Posicion;
            
            // Transferir datos del alma al cuerpo
            var codigoOriginalCuerpo = cuerpo.Codigo;
            
            cuerpo.Codigo = alma.Codigo; // Mantener el ID del alma
            cuerpo.Nombre1 = alma.Nombre1;
            cuerpo.Nombre2 = alma.Nombre2;
            cuerpo.Apellido1 = alma.Apellido1;
            cuerpo.Apellido2 = alma.Apellido2;
            cuerpo.Sexo = alma.Sexo;
            cuerpo.TipoSangre = alma.TipoSangre;
            cuerpo.CodigoPadre = alma.CodigoPadre;
            cuerpo.CodigoMadre = alma.CodigoMadre;
            cuerpo.HijosIds = alma.HijosIds;
            cuerpo.FechaNacimiento = alma.FechaNacimiento;
            
            // Mantener relación actual y posición
            cuerpo.CodigoPareja = parejaActual;
            cuerpo.Posicion = posicionActual;
            cuerpo.EstadoCivil = EstadoCivil.Casado;
            
            // El alma poseída mantiene su personalidad pero con vida reducida
            cuerpo.Estado = Estado.Vivo;
            cuerpo.HaResucitado = true;
            cuerpo.FechaMuerte = null;
            cuerpo.EstadoAnimo = EstadoAnimo.Enojado; // Los poseídos están enojados
            cuerpo.Vida = Rnd.Next(20, 40); // Vida muy reducida por la posesión
            
            // Remover el alma original de la población (ya no existe como entidad separada)
            poblacion.Remove(alma);
            
            // Actualizar la pareja actual para que apunte al alma poseída
            if (parejaActual.HasValue)
            {
                var pareja = poblacion.FirstOrDefault(c => c.Codigo == parejaActual);
                if (pareja != null)
                {
                    pareja.CodigoPareja = alma.Codigo; // Apunta al alma, no al cuerpo original
                }
            }
        }
        
        // Proceso 9: Ir al cine
        public static (int familiasEnCine, List<Familia> familias) IrAlCine(List<Coso> poblacion)
        {
            var familias = new List<Familia>();
            var vivos = poblacion.Where(c => c.Estado == Estado.Vivo).ToList();
            
            // Identificar familias (parejas con o sin hijos)
            var padres = vivos.Where(c => c.EstadoCivil == EstadoCivil.Casado && c.Sexo == Sexo.Macho).ToList();
            
            foreach (var padre in padres)
            {
                var madre = vivos.FirstOrDefault(c => c.Codigo == padre.CodigoPareja);
                if (madre == null) continue;
                
                var hijos = vivos.Where(c => c.CodigoPadre == padre.Codigo && c.CodigoMadre == madre.Codigo).ToList();
                
                // Crear familia
                var familia = new Familia
                {
                    Padre = padre,
                    Madre = madre,
                    Hijos = hijos,
                    AsientosAsignados = new List<Point>()
                };
                
                familias.Add(familia);
            }
            
            // Determinar cuántas familias van al cine (aleatorio entre 30-70%)
            int familiasQueVan = (int)(familias.Count * (0.3 + Rnd.NextDouble() * 0.4));
            var familiasSeleccionadas = familias.OrderBy(x => Rnd.Next()).Take(familiasQueVan).ToList();
            
            // Asignar asientos
            AsignarAsientosCine(familiasSeleccionadas);
            
            return (familiasQueVan, familiasSeleccionadas);
        }
        
        private static void AsignarAsientosCine(List<Familia> familias)
        {
            // Cine de 10 filas x 15 asientos
            bool[,] asientosOcupados = new bool[10, 15];
            
            foreach (var familia in familias)
            {
                int tamañoFamilia = familia.TamañoFamilia;
                bool asientosAsignados = false;
                int intentos = 0;
                
                while (!asientosAsignados && intentos < 100)
                {
                    int fila = Rnd.Next(10);
                    int asientoInicial = Rnd.Next(Math.Max(0, 15 - tamañoFamilia));
                    
                    // Verificar si hay asientos consecutivos disponibles
                    bool disponible = true;
                    for (int i = 0; i < tamañoFamilia; i++)
                    {
                        if (asientosOcupados[fila, asientoInicial + i])
                        {
                            disponible = false;
                            break;
                        }
                    }
                    
                    if (disponible)
                    {
                        // Asignar asientos
                        for (int i = 0; i < tamañoFamilia; i++)
                        {
                            asientosOcupados[fila, asientoInicial + i] = true;
                            familia.AsientosAsignados.Add(new Point(asientoInicial + i, fila));
                        }
                        asientosAsignados = true;
                    }
                    
                    intentos++;
                }
            }
        }
        
        // Proceso 10: Identificación y Reporte
        public static string GenerarReporteCoso(Coso coso, List<Coso> poblacion)
        {
            var reporte = new StringBuilder();
            
            reporte.AppendLine("=== REPORTE INDIVIDUAL ===");
            reporte.AppendLine($"Código: {coso.Codigo}");
            reporte.AppendLine($"Nombre Completo: {coso.NombreCompleto}");
            reporte.AppendLine($"Edad: {coso.Edad} años");
            reporte.AppendLine($"Sexo: {coso.Sexo}");
            reporte.AppendLine($"Estado: {coso.Estado}");
            reporte.AppendLine($"Estado Civil: {coso.EstadoCivil}");
            reporte.AppendLine($"Estado de Ánimo: {coso.EstadoAnimo}");
            reporte.AppendLine($"Trabaja: {(coso.Trabaja ? "Sí" : "No")}");
            reporte.AppendLine($"Salario: ${coso.Salario:F2}");
            reporte.AppendLine($"Tipo de Sangre: {ConverterUtility.ToString(coso.TipoSangre)}");
            reporte.AppendLine($"Posición: ({coso.Posicion.X}, {coso.Posicion.Y})");
            
            // Estadísticas de combate
            reporte.AppendLine($"\n--- ESTADÍSTICAS DE COMBATE ---");
            reporte.AppendLine($"Vida: {coso.Vida}");
            reporte.AppendLine($"Ataque: {coso.Ataque} + Arma: {coso.Arma} = Total: {coso.AtaqueTotal}");
            reporte.AppendLine($"Defensa: {coso.Defensa} + Resistencia: {coso.Resistencia} = Total: {coso.DefensaTotal}");
            
            // Información familiar
            reporte.AppendLine($"\n--- INFORMACIÓN FAMILIAR ---");
            
            if (coso.CodigoPadre.HasValue || coso.CodigoMadre.HasValue)
            {
                var padre = poblacion.FirstOrDefault(c => c.Codigo == coso.CodigoPadre);
                var madre = poblacion.FirstOrDefault(c => c.Codigo == coso.CodigoMadre);
                
                reporte.AppendLine($"Padre: {(padre != null ? $"{padre.NombreCompleto} ({padre.Codigo})" : "Desconocido")}");
                reporte.AppendLine($"Madre: {(madre != null ? $"{madre.NombreCompleto} ({madre.Codigo})" : "Desconocida")}");
            }
            else
            {
                reporte.AppendLine("Padre: N/A (Generación inicial)");
                reporte.AppendLine("Madre: N/A (Generación inicial)");
            }
            
            if (coso.CodigoPareja.HasValue)
            {
                var pareja = poblacion.FirstOrDefault(c => c.Codigo == coso.CodigoPareja);
                reporte.AppendLine($"Pareja: {(pareja != null ? $"{pareja.NombreCompleto} ({pareja.Codigo})" : "No encontrada")}");
            }
            
            var hijos = poblacion.Where(c => c.CodigoPadre == coso.Codigo || c.CodigoMadre == coso.Codigo).ToList();
            reporte.AppendLine($"Número de hijos: {hijos.Count}");
            
            if (hijos.Any())
            {
                reporte.AppendLine("Hijos:");
                foreach (var hijo in hijos)
                {
                    reporte.AppendLine($"  - {hijo.NombreCompleto} ({hijo.Codigo}) - {hijo.Estado}");
                }
            }
            
            // Información adicional
            reporte.AppendLine($"\n--- INFORMACIÓN ADICIONAL ---");
            reporte.AppendLine($"Ha resucitado: {(coso.HaResucitado ? "Sí" : "No")}");
            reporte.AppendLine($"Fecha de nacimiento: {coso.FechaNacimiento:yyyy-MM-dd}");
            
            if (coso.FechaMuerte.HasValue)
            {
                reporte.AppendLine($"Fecha de muerte: {coso.FechaMuerte:yyyy-MM-dd}");
            }
            
            return reporte.ToString();
        }
        
        // Métodos para generar reportes JSON filtrados
        public static string GenerarReporteJSON(List<Coso> poblacion, TipoReporte tipo)
        {
            List<Coso> cososFiltrados = tipo switch
            {
                TipoReporte.TodosMuertos => poblacion.Where(c => c.Estado == Estado.Muerto).ToList(),
                TipoReporte.TodosVivos => poblacion.Where(c => c.Estado == Estado.Vivo).ToList(),
                TipoReporte.TodosConHijos => poblacion.Where(c => c.HijosIds.Any()).ToList(),
                TipoReporte.TodosResucitados => poblacion.Where(c => c.HaResucitado).ToList(),
                _ => poblacion
            };
            
            return JsonSerializer.Serialize(cososFiltrados, new JsonSerializerOptions 
            { 
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
