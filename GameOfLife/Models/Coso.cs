namespace GameOfLife.Models;

public class Coso
{
    public  Guid Codigo { get; set; } = Guid.NewGuid(); // ID que los identifica
    public string Nombre1 { get; set; } = string.Empty;
    public string Nombre2 { get; set; } = string.Empty;
    public string Apellido1 { get; set; } = string.Empty;
    public string Apellido2 { get; set; } = string.Empty;
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
     
   // Campos adicionales
    public bool HaResucitado { get; set; } = false;
    public DateTime FechaNacimiento { get; set; } = DateTime.Now;
    public DateTime? FechaMuerte { get; set; }
    public List<Guid> HijosIds { get; set; } = new List<Guid>();
     
    
    
    public int EdadMaxima => Sexo == Sexo.Macho ? 70 : 85;
    
    public bool EsMayorDeEdad => Edad >= 18;
    
    public int AtaqueTotal => Ataque + Arma;
    
    public int DefensaTotal => Defensa + Resistencia;
    
    public bool TieneTipoSangreCompatible(Coso otroCoso)
    {
        // Para tener hijos, ambos deben tener el mismo factor Rh
        bool esPositivo = TipoSangre.ToString().EndsWith("Pos");
        bool otroEsPositivo = otroCoso.TipoSangre.ToString().EndsWith("Pos");
        return esPositivo == otroEsPositivo;
    }
    
    public bool EsPoblacionRiesgo => Edad > 40 && Sexo == Sexo.Macho && EstadoCivil == EstadoCivil.Soltero;
    
    public Color GetColorPorEstadoAnimo()
    {
        return EstadoAnimo switch
        {
            EstadoAnimo.Feliz => Color.Yellow,
            EstadoAnimo.Triste => Color.Blue,
            EstadoAnimo.Enojado => Color.Red,
            EstadoAnimo.Deprimido => Color.Purple,
            EstadoAnimo.Neutro => Color.Gray,
            _ => Color.Black
        };
    }
    
    public void ActualizarEstadoAnimo(List<Coso> poblacion)
    {
        // Si está emparejado -> Feliz
        if (EstadoCivil == EstadoCivil.Casado)
        {
            EstadoAnimo = EstadoAnimo.Feliz;
        }
        // Si es mayor de edad y no trabaja -> Deprimido
        else if (EsMayorDeEdad && !Trabaja)
        {
            EstadoAnimo = EstadoAnimo.Deprimido;
        }
        // Si perdió a su pareja -> Triste
        else if (CodigoPareja.HasValue && 
                 poblacion.FirstOrDefault(c => c.Codigo == CodigoPareja)?.Estado == Estado.Muerto)
        {
            EstadoAnimo = EstadoAnimo.Triste;
            EstadoCivil = EstadoCivil.Soltero;
            CodigoPareja = null;
        }
    }
    
    public bool PuedeEmparejarseCon(Coso otroCoso)
    {
        if (otroCoso == null || Estado != Estado.Vivo || otroCoso.Estado != Estado.Vivo)
            return false;
            
        if (EstadoCivil != EstadoCivil.Soltero || otroCoso.EstadoCivil != EstadoCivil.Soltero)
            return false;
            
        if (Sexo == otroCoso.Sexo)
            return false;
            
        // Determinar quién es macho y quién es hembra
        var macho = Sexo == Sexo.Macho ? this : otroCoso;
        var hembra = Sexo == Sexo.Hembra ? this : otroCoso;
        
        // El macho debe ser al menos 5 años mayor
        if (macho.Edad - hembra.Edad < 5)
            return false;
            
        // El macho debe trabajar
        if (!macho.Trabaja)
            return false;

        // Si la hembra trabaja, el salario del macho debe ser mayor o igual
        return !hembra.Trabaja || !(macho.Salario < hembra.Salario);
    }
    
    public void Emparejar(Coso pareja)
    {
        EstadoCivil = EstadoCivil.Casado;
        CodigoPareja = pareja.Codigo;
        EstadoAnimo = EstadoAnimo.Feliz;
        
        pareja.EstadoCivil = EstadoCivil.Casado;
        pareja.CodigoPareja = Codigo;
        pareja.EstadoAnimo = EstadoAnimo.Feliz;
    } 
    
    
}