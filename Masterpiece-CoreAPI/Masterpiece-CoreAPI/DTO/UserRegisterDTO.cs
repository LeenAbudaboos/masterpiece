namespace Masterpiece_CoreAPI.DTO
{
    public class UserRegisterDTO
    {
        public string? UserName { get; set; }

        public string? Password { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string? Description { get; set; }

        public string? Profession { get; set; }

        public string? ProfessionDescription { get; set; }

        public int? YearsOfExperience { get; set; }

        public bool? HasWarranty { get; set; }
    }
}
