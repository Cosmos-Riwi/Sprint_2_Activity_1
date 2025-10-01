using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint_2_Activity_1.Models
{
    public class MedicalHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Allergies { get; set; }

        [StringLength(500)]
        public string? ChronicConditions { get; set; }

        [StringLength(500)]
        public string? Vaccinations { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.Now;

        // Foreign key for the pet (1:1 relationship)
        [Required]
        public int PetId { get; set; }

        // Navigation property: A medical history belongs to one pet
        [ForeignKey("PetId")]
        public virtual Pet Pet { get; set; } = null!;

        // Overloaded methods to apply OOP
        public MedicalHistory() { }

        public MedicalHistory(string description, int petId)
        {
            Description = description;
            PetId = petId;
            LastUpdated = DateTime.Now;
        }

        public MedicalHistory(string description, string allergies, string chronicConditions, int petId)
        {
            Description = description;
            Allergies = allergies;
            ChronicConditions = chronicConditions;
            PetId = petId;
            LastUpdated = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Medical History for {Pet?.Name} - Last Updated: {LastUpdated:dd/MM/yyyy}";
        }
    }
}
