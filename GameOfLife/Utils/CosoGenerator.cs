using GameOfLife.Data;
using GameOfLife.Models;

namespace GameOfLife.Utils;

public class CosoGenerator
{
    private static readonly Random Rnd = new Random();
     
    public static List<Coso> GenerarGrupoInicial(int cantidad)
    {
        List<NombreConSexo> nombresConSexo = CsvLoader.CargarNombres("Data/nombres.csv");
        List<string> apellidos = CsvLoader.CargarApellidos("Data/apellidos.csv");
        List<Coso> cosos = new List<Coso>();
        
        for (int i = 0; i < cantidad; i++)
        {
            // Seleccionar nombres basados en el mismo sexo
            var sexo = (Sexo)Rnd.Next(2); // 0 = Macho, 1 = Hembra
            var nombresSexoEspecifico = nombresConSexo.Where(n => 
                ConverterUtility.ConvertirSexo(n.Sexo!) == sexo).ToList();
            
            if (!nombresSexoEspecifico.Any())
                throw new InvalidOperationException($"No hay nombres disponibles para el sexo {sexo}");
            
            var nombre1 = nombresSexoEspecifico[Rnd.Next(nombresSexoEspecifico.Count)]; 
            var nombre2 = nombresSexoEspecifico[Rnd.Next(nombresSexoEspecifico.Count)];
            
            string apellido1 = apellidos[Rnd.Next(apellidos.Count)];
            string apellido2 = apellidos[Rnd.Next(apellidos.Count)];
            
            int edad = Rnd.Next(18, 41);
            bool trabaja = Rnd.NextDouble() < 0.6; 
            double salario = trabaja ? Rnd.Next(500, 3001) : 0;
            
            // Logica de Estado de Animo
            EstadoAnimo estadoAnimo;
            if (edad >= 18 && !trabaja) 
            {
                 estadoAnimo = EstadoAnimo.Deprimido;
            }
            else
            {
                // Excluir Deprimido de las opciones aleatorias ya que tiene lógica específica
                var estadosDisponibles = Enum.GetValues(typeof(EstadoAnimo))
                    .Cast<EstadoAnimo>()
                    .Where(e => e != EstadoAnimo.Deprimido)
                    .ToArray();
                estadoAnimo = estadosDisponibles[Rnd.Next(estadosDisponibles.Length)];
            }
             
            // Generar posición única
            Point posicion = GenerarPosicionUnica(cosos);
            
            var coso = new Coso
            {
                Nombre1 = nombre1.Nombre ?? throw new InvalidOperationException("El nombre1 no puede ser nulo"),
                Nombre2 = nombre2.Nombre ?? throw new InvalidOperationException("El nombre2 no puede ser nulo"),
                Apellido1 = apellido1,
                Apellido2 = apellido2,
                Edad = edad,
                Sexo = sexo,
                Trabaja = trabaja,
                Salario = salario,
                EstadoAnimo = estadoAnimo,
                EstadoCivil = EstadoCivil.Soltero,  
                TipoSangre = (TipoSangre)Rnd.Next(Enum.GetValues(typeof(TipoSangre)).Length),
                Posicion = posicion,
                Vida = Rnd.Next(50, 76),
                Ataque = Rnd.Next(30, 76),
                Defensa = Rnd.Next(20, 51),
                Arma = Rnd.Next(5, 11),
                Resistencia = 0,
                FechaNacimiento = DateTime.Now.AddYears(-edad)
            }; 
            
            cosos.Add(coso); 
        }
        
        return cosos;
    }
    
    private static Point GenerarPosicionUnica(List<Coso> cososExistentes, int intentosMaximos = 100)
    {
        Point posicion;
        int intentos = 0;
        
        do
        {
            posicion = new Point(
                Rnd.Next(40, 860),
                Rnd.Next(40, 560)
            );
            intentos++;
            
            if (intentos >= intentosMaximos)
            {
                break;
            }
            
        } while (cososExistentes.Any(c => 
            Math.Abs(c.Posicion.X - posicion.X) < 15 &&
            Math.Abs(c.Posicion.Y - posicion.Y) < 15));
        
        return posicion;
    }
    
    public static Coso GenerarHijo(Coso padre, Coso madre)
    {
        if (padre.Sexo == madre.Sexo)
            throw new ArgumentException("Padre y madre deben ser de sexos diferentes");
            
        // Asegurar que padre es macho y madre es hembra
        if (padre.Sexo == Sexo.Hembra)
            (padre, madre) = (madre, padre);
        
        var sexoHijo = (Sexo)Rnd.Next(2);
        var nombresDisponibles = CsvLoader.CargarNombres("Data/nombres.csv")
            .Where(n => ConverterUtility.ConvertirSexo(n.Sexo!) == sexoHijo)
            .ToList();
        
        if (!nombresDisponibles.Any())
            throw new InvalidOperationException($"No hay nombres disponibles para el sexo {sexoHijo}");
        
        var nombre1 = nombresDisponibles[Rnd.Next(nombresDisponibles.Count)];
        var nombre2 = nombresDisponibles[Rnd.Next(nombresDisponibles.Count)];
        
        var hijo = new Coso
        {
            Nombre1 = nombre1.Nombre!,
            Nombre2 = nombre2.Nombre!,
            Apellido1 = padre.Apellido1, // Primer apellido del padre
            Apellido2 = madre.Apellido1, // Primer apellido de la madre
            Edad = 1, // Los bebés nacen con edad 1
            Sexo = sexoHijo,
            Trabaja = false, // Los bebés no trabajan
            Salario = 0,
            EstadoAnimo = EstadoAnimo.Feliz, // Los bebés están felices
            EstadoCivil = EstadoCivil.Soltero,
            TipoSangre = (TipoSangre)Rnd.Next(Enum.GetValues(typeof(TipoSangre)).Length),
            Posicion = new Point(
                padre.Posicion.X + Rnd.Next(-20, 21),
                padre.Posicion.Y + Rnd.Next(-20, 21)
            ), // Cerca de los padres
            Vida = Rnd.Next(50, 76),
            Ataque = Rnd.Next(30, 76),
            Defensa = Rnd.Next(20, 51),
            Arma = Rnd.Next(5, 11),
            Resistencia = 0,
            CodigoPadre = padre.Codigo,
            CodigoMadre = madre.Codigo,
            FechaNacimiento = DateTime.Now
        };
        
        // Actualizar listas de hijos de los padres
        padre.HijosIds.Add(hijo.Codigo);
        madre.HijosIds.Add(hijo.Codigo);
        
        return hijo;
    }
} 
