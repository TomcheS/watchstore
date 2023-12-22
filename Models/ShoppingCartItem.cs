namespace watchstore.Models
{
    public class ShoppingCartItem
    {
        public int ShoppingCartItemId { get; set; }
        public Watches Watch { get; set; }
        public int Amount { get; set; }
        public string ShoppingCartId { get; set; }
    }
}
