using GameOfLife.Data;
using GameOfLife.Models;

namespace GameOfLife.Utils;

public class CosoGenerator
{ 
    private static readonly Random Rnd = new Random();
    private static readonly string[] TipoSangre = ["A+", "A-", "B+", "B-", "O+", "O-", "AB+", "AB-"];
    
    
    public static List<Coso> GenerarGrupoInicial(int cantidad)
    {
        List<NombreConSexo> nombresConSexo = CsvLoader.CargarNombres("Data/nombres.csv");
        List<string> apellidos = CsvLoader.CargarApellidos("Data/apellidos.csv");

        List<Coso> cosos = new List<Coso>();

        for (int i = 0; i < cantidad; i++)
        {
            NombreConSexo nombre1 = nombresConSexo[Rnd.Next(nombresConSexo.Count)]; 
            NombreConSexo nombre2 = nombresConSexo[Rnd.Next(nombresConSexo.Count)];
            
            string apellido1 = apellidos[Rnd.Next(apellidos.Count)];
            string apellido2 = apellidos[Rnd.Next(apellidos.Count)];
            
            bool trabaja = Rnd.NextDouble() < 0.6; 
            double salario = trabaja ? Rnd.Next(500, 3000) : 0; 
            
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
                EstadoAnimo = (EstadoAnimo)Rnd.Next(Enum.GetValues(typeof(EstadoAnimo)).Length),
                EstadoCivil = (EstadoCivil)Rnd.Next(Enum.GetValues(typeof(EstadoCivil)).Length),
                
            };
            
            
            
        
        }

        return cosos;
    }
    
    
}