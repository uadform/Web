using System.Data;
using Dapper;
using ItemStore.WebApi.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ItemStore.WebApi.Model.Entities;
using ItemStore.WebApi.Contexts;
using System.Collections.Generic;

namespace ItemStore.WebApi.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly IDbConnection _connection;
        public ItemRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public Item? Get(int id)
        {
            var parameters = new { id };
            var record = _connection.QueryFirstOrDefault<Item>("SELECT * FROM itemstore WHERE Id = @id", parameters);
            return record;
        }
        public List<Item> Get()
        {
            var list = _connection.Query<Item>("SELECT id, name, price FROM itemstore");
            return (List<Item>)list;
        }
        public void Create(Item item)
        {
            string sql = $"INSERT INTO itemstore (Name, Price) VALUES (@name, @price)";
            var queryArguments = new
            {
                name = item.Name,
                price = item.Price,
            };
            _connection.Execute(sql, queryArguments);
        }

        public void EditItem(Item item)
        {

            string sql = $"UPDATE itemstore SET name = @name, price = @price WHERE Id = @id";
            var queryArguments = new
            {
                id = item.Id,
                name = item.Name,
                price = item.Price,
            };
            _connection.Execute(sql, queryArguments);
        }

        public void Delete(Item item)
        {
           _connection.Execute("DELETE FROM itemstore WHERE Id = @id", new { item.Id });
        }
        public decimal Buy(int id)
        {
            return _connection.Execute("SELECT * FROM itemstore WHERE Id = @id");
        }

    }
}
