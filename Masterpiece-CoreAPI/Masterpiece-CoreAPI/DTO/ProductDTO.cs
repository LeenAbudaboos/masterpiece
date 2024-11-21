namespace Masterpiece_CoreAPI.DTO
{
    public class ProductDTO
    {

        public string? ProductName { get; set; }

        public IFormFile? ProductImg { get; set; }

        public string? Descriptions { get; set; }

        public int? Price { get; set; }

        public string? PriceDetels { get; set; }

        public int? Quantity { get; set; }


        public int? CategoryId { get; set; }

    }
}
