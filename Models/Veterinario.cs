using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Models;

public class Veterinario
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Nombres { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Apellidos { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Especialidad { get; set; }

    public ICollection<Atencion> Atenciones { get; set; } = new List<Atencion>();
}


