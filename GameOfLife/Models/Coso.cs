namespace GameOfLife.Models;

public class Coso
{
    public  Guid Codigo { get; set; } = Guid.NewGuid(); // ID que los identifica
    public required string Nombre1 { get; set; } = string.Empty;
    public required string Nombre2 { get; set; } = string.Empty;
    public required string Apellido1 { get; set; } = string.Empty;
    public required string Apellido2 { get; set; } = string.Empty;
    public int Edad { get; set; } = 1; // >1  Machos 70, Hembras 85
    public bool Trabaja { get; set; } 
    public double Salario { get; set; } // Si trabaja si no 0.
    public EstadoAnimo EstadoAnimo { get; set; }
    public Sexo Sexo { get; set; } // Macho o  hembra
    public Point Posicion { get; set; } // Momentaneo depende de como lo haga
    public EstadoCivil EstadoCivil { get; set; }
    public TipoSangre TipoSangre { get; set; } // A+, A-, B+, B-, O+, O-, AB+, AB-
    public Estado Estado { get; set; } = Estado.Vivo;
    
    
    // Atributos de lucha tipo juego RPG
    public int Vida { get; set; }
    public int Ataque { get; set; }
    public int Defensa { get; set; }
    public int Arma { get; set; }
    public int Resistencia { get; set; } // +1 por sobrevivir a cualquier evento
    
    
    // Relaciones
    public Guid? CodigoPareja { get; set; }
    public Guid? CodigoPadre { get; set; }
    public Guid? CodigoMadre { get; set; }

    
    
    // Trim ayuda a limpiar espacios vacios por si algun nombre o apellido terminan siendo una cadena vacia.
    public string NombreCompleto => $"{Nombre1} {Nombre2} {Apellido1} {Apellido2}".Trim();
     
}