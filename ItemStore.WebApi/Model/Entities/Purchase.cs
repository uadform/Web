namespace ItemStore.WebApi.Model.Entities
{
    public class Purchase
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Item ?Item { get; set; }
    }
}
