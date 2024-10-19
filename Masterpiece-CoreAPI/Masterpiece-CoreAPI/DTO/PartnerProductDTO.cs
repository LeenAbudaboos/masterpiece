namespace Masterpiece_CoreAPI.DTO
{
    public class PartnerProductDTO
    {

        public string ProductName { get; set; } = null!;

        public string? ProductDescription { get; set; }

        public IFormFile? ProductImage { get; set; }

        public string? PartnerCompanyName { get; set; }

        public string? PartnerName { get; set; }

        public string? PartnerEmail { get; set; }

        public string? PartnerPhone { get; set; }


        public int? ProductPrice { get; set; }
    }
}
