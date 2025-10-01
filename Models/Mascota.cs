using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Veterinaria.Models;

public enum Especie
{
    Perro = 1,
    Gato = 2,
    Ave = 3,
    Roedor = 4,
    Reptil = 5,
    Otro = 99
}

public class Mascota
{
    public int Id { get; set; }

    [Required, MaxLength(80)]
    public string Nombre { get; set; } = string.Empty;

    public Especie Especie { get; set; }

    [MaxLength(80)]
    public string? Raza { get; set; }

    [Column(TypeName = "date")]
    public DateTime? FechaNacimiento { get; set; }

    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }

    public ICollection<Atencion> Atenciones { get; set; } = new List<Atencion>();
}


