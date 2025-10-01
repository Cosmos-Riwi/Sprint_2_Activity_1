using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint_2_Activity_1.Models
{
    public class Pet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Species { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Breed { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; }

        [StringLength(10)]
        public string Gender { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Color { get; set; }

        [StringLength(500)]
        public string? Observations { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        // Foreign key for the client
        [Required]
        public int ClientId { get; set; }

        // Relationship: A pet belongs to a client
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; } = null!;

        // Relationship: A pet can have many appointments
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        // Relationship 1:1 with MedicalHistory
        public virtual MedicalHistory? History { get; set; }

        // Relationship N:N with Veterinarians (through appointments)
        public virtual ICollection<Veterinarian> Veterinarians { get; set; } = new List<Veterinarian>();

        // Overloaded methods to apply OOP
        public Pet() { }

        public Pet(string name, string species, string breed, DateTime birthDate, string gender, int clientId)
        {
            Name = name;
            Species = species;
            Breed = breed;
            BirthDate = birthDate;
            Gender = gender;
            ClientId = clientId;
            RegistrationDate = DateTime.Now;
        }

        public Pet(string name, string species, string breed, DateTime birthDate, string gender, string color, int clientId)
        {
            Name = name;
            Species = species;
            Breed = breed;
            BirthDate = birthDate;
            Gender = gender;
            Color = color;
            ClientId = clientId;
            RegistrationDate = DateTime.Now;
        }

        public Pet(string name, string species, string breed, DateTime birthDate, string gender, string color, string observations, int clientId)
        {
            Name = name;
            Species = species;
            Breed = breed;
            BirthDate = birthDate;
            Gender = gender;
            Color = color;
            Observations = observations;
            ClientId = clientId;
            RegistrationDate = DateTime.Now;
        }

        public int CalculateAge()
        {
            var today = DateTime.Today;
            var age = today.Year - BirthDate.Year;
            if (BirthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        public override string ToString()
        {
            return $"{Name} - {Species} ({Breed}) - {CalculateAge()} years - Owner: {Client?.FirstName} {Client?.LastName}";
        }
    }
}
