using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace Talabat.Repository
{
    public class Basketepository : IBasketRepository
    {
        private readonly IDatabase _database;

        public Basketepository(IConnectionMultiplexer Redis)
        {
            _database= Redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string BasketId)
        {
            return await _database.KeyDeleteAsync(BasketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
        {
            var basket = await _database.StringGetAsync(BasketId);
            if (basket.IsNull)
                return null;
            else
                return JsonSerializer.Deserialize<CustomerBasket>(basket);
          //  return basket.IsNull ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var jsonBasket = JsonSerializer.Serialize(basket);
          var CreatedOrUpdated=  await _database.StringSetAsync(basket.Id, jsonBasket, TimeSpan.FromDays(1));
            if (!CreatedOrUpdated)
                return null;
            return await GetBasketAsync(basket.Id);
        }
    }
}
