using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Veterinaria.Models;

public class Atencion
{
    public int Id { get; set; }

    [Column(TypeName = "datetime(6)")]
    public DateTime Fecha { get; set; } = DateTime.Now;

    [Required, MaxLength(500)]
    public string Diagnostico { get; set; } = string.Empty;

    public int MascotaId { get; set; }
    public Mascota? Mascota { get; set; }

    public int VeterinarioId { get; set; }
    public Veterinario? Veterinario { get; set; }
}


