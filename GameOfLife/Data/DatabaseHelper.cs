using System.Data;
using Microsoft.Data.Sqlite;
using GameOfLife.Models;
namespace GameOfLife.Data;


public static class DatabaseHelper
{
    private static readonly string DbPath = InitializeDatabasePath();
    private static readonly string ConnectionString = $"Data Source={DbPath};Mode=ReadWriteCreate;Cache=Shared;";

    private static string InitializeDatabasePath()
    {
        var env = Environment.GetEnvironmentVariable("GAME_DB_PATH");
        if (!string.IsNullOrWhiteSpace(env))
        {
            env = Environment.ExpandEnvironmentVariables(env);
            return Path.GetFullPath(env);
        }

        var defaultPath = Path.Combine(AppContext.BaseDirectory, "Data", "juego.db");
        var dir = Path.GetDirectoryName(defaultPath)!;
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        return Path.GetFullPath(defaultPath);
    }

    static DatabaseHelper()
    {
        CrearTabla();
    }

    public static void CrearTabla()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            var createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText = """

                                                             CREATE TABLE IF NOT EXISTS Cosos (
                                                                 Codigo TEXT PRIMARY KEY,
                                                                 Nombre1 TEXT NOT NULL,
                                                                 Nombre2 TEXT NOT NULL,
                                                                 Apellido1 TEXT NOT NULL,
                                                                 Apellido2 TEXT NOT NULL,
                                                                 Edad INTEGER NOT NULL,
                                                                 Trabaja INTEGER NOT NULL, -- 1 para true, 0 para false
                                                                 Salario REAL NOT NULL,
                                                                 EstadoAnimo INTEGER NOT NULL, -- Almacenamos el valor del enum
                                                                 Sexo INTEGER NOT NULL,
                                                                 PosicionX INTEGER NOT NULL,
                                                                 PosicionY INTEGER NOT NULL,
                                                                 EstadoCivil INTEGER NOT NULL,
                                                                 TipoSangre INTEGER NOT NULL,
                                                                 Estado INTEGER NOT NULL,
                                                                 Vida INTEGER NOT NULL,
                                                                 Ataque INTEGER NOT NULL,
                                                                 Defensa INTEGER NOT NULL,
                                                                 Arma INTEGER NOT NULL,
                                                                 Resistencia INTEGER NOT NULL,
                                                                 CodigoPareja TEXT, -- Puede ser NULL
                                                                 CodigoPadre TEXT,  -- Puede ser NULL
                                                                 CodigoMadre TEXT,  -- Puede ser NULL
                                                                 HaResucitado INTEGER DEFAULT 0, -- Nueva columna
                                                                 FechaNacimiento TEXT NOT NULL, -- Nueva columna
                                                                 FechaMuerte TEXT, -- Nueva columna, puede ser NULL
                                                                 HijosIds TEXT -- Nueva columna para almacenar IDs de hijos como JSON
                                                             )
                                         """;
            createTableCmd.ExecuteNonQuery();

            // Verificar y agregar nuevas columnas si no existen (para compatibilidad con DB existentes)
        }
    }


    public static void GuardarPoblacion(List<Coso> cosos)
    {
        ArgumentNullException.ThrowIfNull(cosos);
        if (cosos.Count == 0)
            return; // No hay nada que guardar

        try
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                using var transaction = connection.BeginTransaction();

                // Limpiamos la tabla antes de insertar
                using (var deleteCmd = connection.CreateCommand())
                {
                    deleteCmd.CommandText = "DELETE FROM Cosos";
                    deleteCmd.ExecuteNonQuery();
                }

                foreach (var coso in cosos)
                {
                    var insertCmd = connection.CreateCommand();
                    insertCmd.CommandText = """

                                                                        INSERT INTO Cosos (
                                                                            Codigo, Nombre1, Nombre2, Apellido1, Apellido2, Edad, Trabaja, Salario,
                                                                            EstadoAnimo, Sexo, PosicionX, PosicionY, EstadoCivil, TipoSangre, Estado,
                                                                            Vida, Ataque, Defensa, Arma, Resistencia, CodigoPareja, CodigoPadre, CodigoMadre,
                                                                            HaResucitado, FechaNacimiento, FechaMuerte, HijosIds
                                                                        ) VALUES (
                                                                            $codigo, $nombre1, $nombre2, $apellido1, $apellido2, $edad, $trabaja, $salario,
                                                                            $estadoAnimo, $sexo, $posicionX, $posicionY, $estadoCivil, $tipoSangre, $estado,
                                                                            $vida, $ataque, $defensa, $arma, $resistencia, $codigoPareja, $codigoPadre, $codigoMadre,
                                                                            $haResucitado, $fechaNacimiento, $fechaMuerte, $hijosIds
                                                                        )
                                            """;

                    // Evita inyeccion SQL
                    insertCmd.Parameters.AddWithValue("$codigo", coso.Codigo.ToString());
                    insertCmd.Parameters.AddWithValue("$nombre1", coso.Nombre1 ?? "");
                    insertCmd.Parameters.AddWithValue("$nombre2", coso.Nombre2 ?? "");
                    insertCmd.Parameters.AddWithValue("$apellido1", coso.Apellido1 ?? "");
                    insertCmd.Parameters.AddWithValue("$apellido2", coso.Apellido2 ?? "");
                    insertCmd.Parameters.AddWithValue("$edad", coso.Edad);
                    insertCmd.Parameters.AddWithValue("$trabaja", coso.Trabaja ? 1 : 0);
                    insertCmd.Parameters.AddWithValue("$salario", coso.Salario);
                    insertCmd.Parameters.AddWithValue("$estadoAnimo", (int)coso.EstadoAnimo);
                    insertCmd.Parameters.AddWithValue("$sexo", (int)coso.Sexo);
                    insertCmd.Parameters.AddWithValue("$posicionX", coso.Posicion.X);
                    insertCmd.Parameters.AddWithValue("$posicionY", coso.Posicion.Y);
                    insertCmd.Parameters.AddWithValue("$estadoCivil", (int)coso.EstadoCivil);
                    insertCmd.Parameters.AddWithValue("$tipoSangre", (int)coso.TipoSangre);
                    insertCmd.Parameters.AddWithValue("$estado", (int)coso.Estado);
                    insertCmd.Parameters.AddWithValue("$vida", coso.Vida);
                    insertCmd.Parameters.AddWithValue("$ataque", coso.Ataque);
                    insertCmd.Parameters.AddWithValue("$defensa", coso.Defensa);
                    insertCmd.Parameters.AddWithValue("$arma", coso.Arma);
                    insertCmd.Parameters.AddWithValue("$resistencia", coso.Resistencia);

                    // Manejar valores que pueden ser null
                    insertCmd.Parameters.AddWithValue("$codigoPareja",
                        coso.CodigoPareja?.ToString() ?? (object)DBNull.Value);
                    insertCmd.Parameters.AddWithValue("$codigoPadre",
                        coso.CodigoPadre?.ToString() ?? (object)DBNull.Value);
                    insertCmd.Parameters.AddWithValue("$codigoMadre",
                        coso.CodigoMadre?.ToString() ?? (object)DBNull.Value);

                    // Nuevos campos
                    insertCmd.Parameters.AddWithValue("$haResucitado", coso.HaResucitado ? 1 : 0);
                    insertCmd.Parameters.AddWithValue("$fechaNacimiento",
                        coso.FechaNacimiento.ToString("yyyy-MM-dd HH:mm:ss"));
                    insertCmd.Parameters.AddWithValue("$fechaMuerte",
                        coso.FechaMuerte?.ToString("yyyy-MM-dd HH:mm:ss") ?? (object)DBNull.Value);

                    // Serializar lista de hijos como JSON
                    string hijosJson = coso.HijosIds.Count > 0
                        ? string.Join(",", coso.HijosIds.Select(id => id.ToString()))
                        : "";
                    insertCmd.Parameters.AddWithValue("$hijosIds", hijosJson);

                    insertCmd.ExecuteNonQuery();
                }

                transaction.Commit();
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"Error SQLite: {ex.Message}");
            throw new DataException($"Error al guardar población: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error inesperado: {ex.Message}", ex);
        }
    }

    public static List<Coso> CargarPoblacion()
    {
        var cosos = new List<Coso>();
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            var cargarCmd = connection.CreateCommand();
            cargarCmd.CommandText = "SELECT * FROM Cosos";

            using var reader = cargarCmd.ExecuteReader();
            while (reader.Read())
            {
                // Parsear fecha de nacimiento
                DateTime fechaNacimiento = DateTime.Now;
                if (!reader.IsDBNull("FechaNacimiento"))
                {
                    DateTime.TryParse(reader.GetString("FechaNacimiento"), out fechaNacimiento);
                }

                // Parsear fecha de muerte
                DateTime? fechaMuerte = null;
                if (!reader.IsDBNull("FechaMuerte"))
                {
                    if (DateTime.TryParse(reader.GetString("FechaMuerte"), out DateTime fechaMuerteTemp))
                        fechaMuerte = fechaMuerteTemp;
                }

                // Parsear lista de hijos
                var hijosIds = new List<Guid>();
                if (!reader.IsDBNull("HijosIds"))
                {
                    string hijosStr = reader.GetString("HijosIds");
                    if (!string.IsNullOrWhiteSpace(hijosStr))
                    {
                        var hijosArray = hijosStr.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        foreach (var hijoStr in hijosArray)
                        {
                            if (Guid.TryParse(hijoStr, out Guid hijoId))
                                hijosIds.Add(hijoId);
                        }
                    }
                }

                Coso coso = new Coso
                {
                    Codigo = Guid.Parse(reader.GetString("Codigo")),
                    Nombre1 = reader.IsDBNull("Nombre1") ? string.Empty : reader.GetString("Nombre1"),
                    Nombre2 = reader.IsDBNull("Nombre2") ? string.Empty : reader.GetString("Nombre2"),
                    Apellido1 = reader.IsDBNull("Apellido1") ? string.Empty : reader.GetString("Apellido1"),
                    Apellido2 = reader.IsDBNull("Apellido2") ? string.Empty : reader.GetString("Apellido2"),

                    Edad = reader.GetInt32("Edad"),
                    Trabaja = reader.GetBoolean("Trabaja"), // Sqlite maneja 1/0 como boolean
                    Salario = reader.GetDouble("Salario"),

                    EstadoAnimo = (EstadoAnimo)reader.GetInt32("EstadoAnimo"),
                    Sexo = (Sexo)reader.GetInt32("Sexo"),
                    Posicion = new Point(reader.GetInt32("PosicionX"), reader.GetInt32("PosicionY")),

                    EstadoCivil = (EstadoCivil)reader.GetInt32("EstadoCivil"),
                    TipoSangre = (TipoSangre)reader.GetInt32("TipoSangre"),
                    Estado = (Estado)reader.GetInt32("Estado"),

                    Vida = reader.GetInt32("Vida"),
                    Ataque = reader.GetInt32("Ataque"),
                    Defensa = reader.GetInt32("Defensa"),
                    Arma = reader.GetInt32("Arma"),
                    Resistencia = reader.GetInt32("Resistencia"),

                    CodigoPareja = reader.IsDBNull("CodigoPareja")
                        ? (Guid?)null
                        : Guid.Parse(reader.GetString("CodigoPareja")),
                    CodigoPadre = reader.IsDBNull("CodigoPadre")
                        ? (Guid?)null
                        : Guid.Parse(reader.GetString("CodigoPadre")),
                    CodigoMadre = reader.IsDBNull("CodigoMadre")
                        ? (Guid?)null
                        : Guid.Parse(reader.GetString("CodigoMadre")),

                    // Nuevos campos
                    HaResucitado = !reader.IsDBNull("HaResucitado") && reader.GetBoolean("HaResucitado"),
                    FechaNacimiento = fechaNacimiento,
                    FechaMuerte = fechaMuerte,
                    HijosIds = hijosIds
                };
                cosos.Add(coso);
            }

            return cosos;
        }
    }
}


