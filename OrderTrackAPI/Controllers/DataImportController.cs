using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderTrackAPI.Context;
using OrderTrackAPI.Models;

namespace OrderTrackAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataImportController : ControllerBase
    {

        DatabaseContext databaseContext = new DatabaseContext();

        [HttpGet("AddProduct")]

        public bool AddProduct()
        {
            for (int i = 0; i < 100000; i++)
            {
                Product product = new Product();
                product.ProductName = "Ürün " + i;
                Random random = new Random();
                product.ProductPrice = random.Next(100, 750);
                product.ProductQuantity = random.Next(0, 10);
                product.OnSellable = Convert.ToBoolean(random.Next(0, 1));
                databaseContext.Products.Add(product);


            }
            databaseContext.SaveChanges();
            return true;
        }

        [HttpGet("AddOrder")]
        public bool AddOrder()
        {
            for (int i = 1; i < 1000000; i++)
            {
                Models.Order order = new Models.Order();
                order.CustomerId = i;
                order.ShipAdress = "Adress" + i;
                databaseContext.Orders.Add(order);
                if (i % 100000 == 0)
                {
                    databaseContext.SaveChanges();
                }
            }
            databaseContext.SaveChanges();

            for (int i = 1; i < 3000000; i++)
            {
                
                Random random = new Random();
                for (int k = 0; k <= random.Next(2, 4); k++)
                {
                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.OrderId = i;
                    orderDetail.OrderQuantity = random.Next(1, 5);
                    orderDetail.Price = random.Next(100, 500);
                    orderDetail.ProductId = random.Next(1, 17);
                    databaseContext.OrderDetails.Add(orderDetail);
                }
                if (i % 100000 == 0)
                {
                    databaseContext.SaveChanges();
                }
            }
            databaseContext.SaveChanges();
            return true;
        }

    }
}
