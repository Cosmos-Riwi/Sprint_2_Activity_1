using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint_2_Activity_1.Models
{
    public class Client
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
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        // Relationship: A client can have many pets
        public virtual ICollection<Pet> Pets { get; set; } = new List<Pet>();

        // Overloaded methods to apply OOP
        public Client() { }

        public Client(string firstName, string lastName, string phone, string email, string address)
        {
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Email = email;
            Address = address;
            RegistrationDate = DateTime.Now;
        }

        public Client(string firstName, string lastName, string phone, string email, string address, DateTime registrationDate)
        {
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Email = email;
            Address = address;
            RegistrationDate = registrationDate;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName} - Tel: {Phone} - Email: {Email}";
        }
    }
}
