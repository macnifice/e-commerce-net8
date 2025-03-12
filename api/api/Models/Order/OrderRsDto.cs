namespace api.Models.Order
{
    public class OrderRsDto
    {
        public List<int> OrderIds { get; set; }
        public decimal TotalAmount { get; set; }
        public string Message { get; set; }
    }
} 