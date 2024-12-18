namespace ecommerceAyman.Models
{
    public class UserCart
    {
        public int UserCartID { get; set; }
        public int UserID { get; set; }  // Foreign Key to User
        public int ProductID { get; set; }  // Foreign Key to Product
        public int Quantity { get; set; }
        public DateTime DateAdded { get; set; }

        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
    }

}
