namespace ecommerceAyman.Models
{
    public class ReceiptViewModel
    {
        public List<UserCart> CartItems { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
