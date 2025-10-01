using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint_2_Activity_1.Models
{
    public class Veterinarian
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Specialty { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string LicenseNumber { get; set; } = string.Empty;

        public DateTime HireDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        // Relationship: A veterinarian can perform many appointments
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        // Relationship N:N with Pets
        public virtual ICollection<Pet> Pets { get; set; } = new List<Pet>();

        // Overloaded methods to apply OOP
        public Veterinarian() { }

        public Veterinarian(string firstName, string lastName, string phone, string email, string specialty, string licenseNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Email = email;
            Specialty = specialty;
            LicenseNumber = licenseNumber;
            HireDate = DateTime.Now;
            IsActive = true;
        }

        public Veterinarian(string firstName, string lastName, string phone, string email, string specialty, string licenseNumber, DateTime hireDate)
        {
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Email = email;
            Specialty = specialty;
            LicenseNumber = licenseNumber;
            HireDate = hireDate;
            IsActive = true;
        }

        public override string ToString()
        {
            return $"Dr. {FirstName} {LastName} - {Specialty} - Lic: {LicenseNumber}";
        }
    }
}
