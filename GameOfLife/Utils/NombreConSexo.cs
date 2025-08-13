using GameOfLife.Models;

namespace GameOfLife.Utils;

public class NombreConSexo
{
    public required string  Nombre { get; set; }
    public required string Sexo { get; set; }
}

public class Familia
{
    public Coso Padre { get; set; } = null!;
    public Coso Madre { get; set; } = null!;
    public List<Coso> Hijos { get; set; } = new List<Coso>();
    public List<System.Drawing.Point> AsientosAsignados { get; set; } = new List<System.Drawing.Point>();
        
    public int TamañoFamilia => 2 + Hijos.Count; // Padre + Madre + Hijos
        
    public List<Coso> TodosLosMiembros
    {
        get
        {
            var miembros = new List<Coso> { Padre, Madre };
            miembros.AddRange(Hijos);
            return miembros;
        }
    }
}