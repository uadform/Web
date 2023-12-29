namespace ItemStore.WebApi.Model.Entities
{
    public class Shop
    {
        public int Id { get; set; }
        public string ?Name { get; set; }
        public string ?Address { get; set; }
        public List<Item> ?Items { get; set; }
    }
}
