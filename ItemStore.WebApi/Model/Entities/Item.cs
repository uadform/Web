﻿namespace ItemStore.WebApi.Model.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string ?Name { get; set; }
        public decimal Price { get; set; }
        public int? ShopId { get; set; }
    }
}
