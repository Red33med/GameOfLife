using GameOfLife.Utils;

namespace GameOfLife.Data;

using System.Collections.Generic;
using System.IO;

public static class CsvLoader
{
    public static List<NombreConSexo> CargarNombres(string ruta)
    {
        List<NombreConSexo> nombres = new List<NombreConSexo>();
        // using se usa para asegurar de cerrar el archivo luego de usarlo
        // StreamReader clase para leer datos de un CSV

        try
        {
            using StreamReader reader = new StreamReader(ruta);
            reader.ReadLine(); // Saltar encabezado
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(',');
                if (parts.Length == 2)
                {
                    nombres.Add(new NombreConSexo
                    {
                        Nombre = parts[0].Trim(),
                        Sexo = parts[1].Trim()
                    });
                }
            }
        }
        catch (Exception ex)
        {
            throw new FileNotFoundException($"Error al cargar nombres desde {ruta}: {ex.Message}");
        }

        return nombres;
    }

    public static List<string> CargarApellidos(string ruta)
    {
        List<string> apellidos = new List<string>();
        try
        {
            using StreamReader reader = new StreamReader(ruta);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (!string.IsNullOrWhiteSpace(line))
                    apellidos.Add(line.Trim());
            }
        }
        catch (Exception ex)
        {
            throw new FileNotFoundException($"Error al cargar apellidos desde {ruta}: {ex.Message}");
        }
        return apellidos;
    }
}