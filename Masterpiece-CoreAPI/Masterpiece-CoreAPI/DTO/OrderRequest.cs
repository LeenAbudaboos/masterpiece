namespace Masterpiece_CoreAPI.DTO
{
    public class OrderRequest
    {
        public int UserId { get; set; }
        public string TransportMethod { get; set; }
        public string PaymentMethod { get; set; }
        public string DeliveryAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string? LocationLink { get; set; }
        public List<OrderItem> Items { get; set; } = new();
    }

    public class OrderItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

}
