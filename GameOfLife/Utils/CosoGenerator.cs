using GameOfLife.Data;
using GameOfLife.Models;

namespace GameOfLife.Utils;

public class CosoGenerator
{ 
    private static readonly Random Rnd = new Random();
    // private static readonly string[] TipoSangre = ["A+", "A-", "B+", "B-", "O+", "O-", "AB+", "AB-"];
     
    public static List<Coso> GenerarGrupoInicial(int cantidad)
    {
        List<NombreConSexo> nombresConSexo = CsvLoader.CargarNombres("Data/nombres.csv");
        List<string> apellidos = CsvLoader.CargarApellidos("Data/apellidos.csv");

        // Mi lista principal de Cosos
        List<Coso> cosos = new List<Coso>();

        for (int i = 0; i < cantidad; i++)
        {
            NombreConSexo nombre1 = nombresConSexo[Rnd.Next(nombresConSexo.Count)]; 
            NombreConSexo nombre2 = nombresConSexo[Rnd.Next(nombresConSexo.Count)];
            
            string apellido1 = apellidos[Rnd.Next(apellidos.Count)];
            string apellido2 = apellidos[Rnd.Next(apellidos.Count)];
            
            bool trabaja = Rnd.NextDouble() < 0.6; 
            double salario = trabaja ? Rnd.Next(500, 3000) : 0; 
            int edad = Rnd.Next(18, 41);
            
            // Logica de Estado de Animo si el coso >= 18 Y no trabaja esta deprimido
            EstadoAnimo estadoAnimo;
            if (edad >= 18 && !trabaja) {
                 estadoAnimo = EstadoAnimo.Deprimido;
            }
            else
            {
                estadoAnimo = (EstadoAnimo)Rnd.Next(Enum.GetValues(typeof(EstadoAnimo)).Length);
            }
             
            // Logica de la posicion 
            Point posicion;
            do
            {
                posicion = new Point(
                    Rnd.Next(40, 860),
                    Rnd.Next(40, 560)
                );
            } while (cosos.Any(c => Math.Abs(c.Posicion.X - posicion.X) < 15 &&
                                    Math.Abs(c.Posicion.Y - posicion.Y) < 15));
            // Esto evita que dos cosos estén demasiado cerca (menos de 15 px)
            
            Coso coso = new Coso
            {
                Nombre1 = nombre1.Nombre,
                Nombre2 = nombre2.Nombre,
                Apellido1 = apellido1,
                Apellido2 = apellido2,
                Edad = Rnd.Next(18, 41),
                Sexo = ConverterUtility.ConvertirSexo(nombre1.Sexo),
                Trabaja = trabaja,
                Salario = salario,
                EstadoAnimo = estadoAnimo,
                EstadoCivil = EstadoCivil.Soltero,  
                TipoSangre = (TipoSangre)Rnd.Next(Enum.GetValues(typeof(TipoSangre)).Length),
                Posicion = posicion,
                Vida = Rnd.Next(50, 76),
                Ataque = Rnd.Next(30, 75),
                Defensa = Rnd.Next(20, 51),
                Arma = Rnd.Next(5, 10),
                Resistencia = 0 
            }; 
            cosos.Add(coso); 
            
        }
        return cosos;
        
    }
    
    
}