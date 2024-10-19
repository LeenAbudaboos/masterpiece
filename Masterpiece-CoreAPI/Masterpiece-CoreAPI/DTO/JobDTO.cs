using Masterpiece_CoreAPI.Models;

namespace Masterpiece_CoreAPI.DTO
{
    public class JobDTO
    {

        public string? JobTitle { get; set; }

        public string? Description { get; set; }


        public string? Location { get; set; }

        public string? Salary { get; set; }

        public int? CategoryId { get; set; }
        public string? PostedBy { get; set; }


        public IFormFile? JobImg { get; set; }



    }
}
