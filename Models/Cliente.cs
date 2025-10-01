using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Models;

public class Cliente
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Nombres { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Apellidos { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? Email { get; set; }

    [MaxLength(30)]
    public string? Telefono { get; set; }

    public ICollection<Mascota> Mascotas { get; set; } = new List<Mascota>();
}


