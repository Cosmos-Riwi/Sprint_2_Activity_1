using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint_2_Activity_1.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [StringLength(1000)]
        public string Diagnosis { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Treatment { get; set; }

        [StringLength(500)]
        public string? Observations { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Cost { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        // Foreign keys
        [Required]
        public int PetId { get; set; }

        [Required]
        public int VeterinarianId { get; set; }

        // Relationships
        [ForeignKey("PetId")]
        public virtual Pet Pet { get; set; } = null!;

        [ForeignKey("VeterinarianId")]
        public virtual Veterinarian Veterinarian { get; set; } = null!;

        // Overloaded methods to apply OOP
        public Appointment() { }

        public Appointment(DateTime appointmentDate, string diagnosis, int petId, int veterinarianId)
        {
            AppointmentDate = appointmentDate;
            Diagnosis = diagnosis;
            PetId = petId;
            VeterinarianId = veterinarianId;
            RegistrationDate = DateTime.Now;
        }

        public Appointment(DateTime appointmentDate, string diagnosis, string treatment, int petId, int veterinarianId)
        {
            AppointmentDate = appointmentDate;
            Diagnosis = diagnosis;
            Treatment = treatment;
            PetId = petId;
            VeterinarianId = veterinarianId;
            RegistrationDate = DateTime.Now;
        }

        public Appointment(DateTime appointmentDate, string diagnosis, string treatment, string observations, decimal cost, int petId, int veterinarianId)
        {
            AppointmentDate = appointmentDate;
            Diagnosis = diagnosis;
            Treatment = treatment;
            Observations = observations;
            Cost = cost;
            PetId = petId;
            VeterinarianId = veterinarianId;
            RegistrationDate = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Appointment on {AppointmentDate:dd/MM/yyyy} - {Pet?.Name} - Dr. {Veterinarian?.FirstName} {Veterinarian?.LastName} - ${Cost:F2}";
        }
    }
}
