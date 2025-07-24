using GameOfLife.Models;

namespace GameOfLife.Utils;

public class ConverterUtility
{
    public static Sexo ConvertirSexo(string valor)
    {
        
        return valor.Trim().ToLower() switch
        {
            "macho" => Sexo.Macho,
            "hembra" => Sexo.Hembra,
            _ => throw new ArgumentException($"Valor de sexo inválido: {valor}")
        };
    }
     
    public static TipoSangre Parse(string valor)
    {
        return valor.Trim().ToUpper() switch
        {
            "A+" => TipoSangre.APos,
            "A-" => TipoSangre.ANeg,
            "B+" => TipoSangre.BPos,
            "B-" => TipoSangre.BNeg,
            "O+" => TipoSangre.OPos,
            "O-" => TipoSangre.ONeg,
            "AB+" => TipoSangre.ABPos,
            "AB-" => TipoSangre.ABNeg,
            _ => throw new ArgumentException($"Tipo de sangre inválido: {valor}")
        };
    }

    public static string ToString(TipoSangre tipo)
    {
        return tipo switch
        {
            TipoSangre.APos => "A+",
            TipoSangre.ANeg => "A-",
            TipoSangre.BPos => "B+",
            TipoSangre.BNeg => "B-",
            TipoSangre.OPos => "O+",
            TipoSangre.ONeg => "O-",
            TipoSangre.ABPos => "AB+",
            TipoSangre.ABNeg => "AB-",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    
}