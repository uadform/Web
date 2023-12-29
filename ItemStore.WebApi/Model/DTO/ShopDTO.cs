namespace ItemStore.WebApi.Model.DTO
{
    public class ShopDTO
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Address { get; set; }
        public List<ItemDTO> Items { get; set; }
    }
}
