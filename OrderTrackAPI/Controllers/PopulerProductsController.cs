using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderTrackAPI.Context;
using OrderTrackAPI.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace OrderTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PopulerProductsController : ControllerBase
    {
        DatabaseContext databaseContext = new DatabaseContext();
        ConnectionMultiplexer redisConnection = ConnectionMultiplexer.Connect("localhost");

        [HttpGet]
        public Object GetPopulerProductbySQL()
        {
            var populerProducts = databaseContext.OrderDetails.GroupBy(c => c.ProductId)
    .Select(g => new
    {
        g.Key,
        orderQuantitySum = g.Sum(s => s.OrderQuantity)
    }).OrderByDescending(x => x.orderQuantitySum).Join(databaseContext.Products,
                orderDetail => orderDetail.Key,
                products => products.Id,
                (orderDetail, products) =>
                new
                {
                    Name = products.ProductName,
                    ID = products.Id,
                    Price = products.ProductPrice,
                    OnSellable = products.OnSellable

                }
            ).Where(x => x.OnSellable == true).Take(5);
            return populerProducts;

        }


        [HttpGet("GETREDIS")]
        public Object GetPopulerProductbyREDIS()
        {
            IDatabase redisDB = redisConnection.GetDatabase(0);
            var populerProduct = redisDB.StringGet("populer_products");
            

            if (!populerProduct.IsNullOrEmpty )
            {
                return JsonSerializer.Deserialize<Object>(populerProduct);
            }

         
                var populerProducts = databaseContext.OrderDetails.GroupBy(c => c.ProductId)
    .Select(g => new
    {
        g.Key,
        orderQuantitySum = g.Sum(s => s.OrderQuantity)
    }).OrderByDescending(x => x.orderQuantitySum).Join(databaseContext.Products,
                orderDetail => orderDetail.Key,
                products => products.Id,
                (orderDetail, products) =>
                new
                {
                    Name = products.ProductName,
                    ID = products.Id,
                    Price = products.ProductPrice,
                    OnSellable = products.OnSellable

                }
            ).Where(x => x.OnSellable == true).Take(5);

                string serializedProduct = JsonSerializer.Serialize(populerProducts);

                redisDB.StringSet("populer_products", serializedProduct, TimeSpan.FromSeconds(10));
                return populerProducts;
             
        }   
    }
}
