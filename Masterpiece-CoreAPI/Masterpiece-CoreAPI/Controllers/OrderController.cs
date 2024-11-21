using Masterpiece_CoreAPI.DTO;
using Masterpiece_CoreAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Masterpiece_CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly MyDbContext _db;

        public OrderController(MyDbContext db)
        {
            _db = db;
        }

        [HttpPost("PlaceOrder")]
        public IActionResult PlaceOrder([FromForm] OrderRequest request)
        {
            if (request == null || request.Items == null || !request.Items.Any())
            {
                return BadRequest("Invalid order request.");
            }

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    // إنشاء الطلب
                    var order = new Order
                    {
                        UserId = request.UserId,
                        TransportMethod = request.TransportMethod,
                        PaymentMethod = request.PaymentMethod,
                        DeliveryAddress = request.DeliveryAddress,
                        PhoneNumber = request.PhoneNumber,
                        LocationLink = request.LocationLink,
                        TotalPrice = request.Items.Sum(i => i.Price * i.Quantity),
                        OrderDate = DateTime.Now,
                        OrderStatus = "Pending"
                    };

                    _db.Orders.Add(order);
                    _db.SaveChanges();

                    // إضافة تفاصيل الطلب
                    foreach (var item in request.Items)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderId = order.OrderId,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            Price = item.Price
                        };

                        _db.OrderDetails.Add(orderDetail);
                    }

                    _db.SaveChanges();
                    transaction.Commit();

                    return Ok(new { Message = "Order placed successfully.", OrderId = order.OrderId });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
                }
            }
        }


        [HttpGet("GetAllOrders")]
        public IActionResult GetAllOrders()
        {
            try
            {
                var orders = _db.Orders
                    .Select(order => new
                    {
                        order.OrderId,
                        order.OrderDate,
                        order.OrderStatus,
                        order.TotalPrice,
                        order.TransportMethod,
                        order.PaymentMethod,
                        order.DeliveryAddress,
                        order.PhoneNumber,
                        order.LocationLink,
                        UserName = _db.Users
                            .Where(user => user.UserId == order.UserId)
                            .Select(user => user.UserName)
                            .FirstOrDefault()
                    })
                    .ToList();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }




        [HttpGet("GetOrderById/{orderId}")]
        public IActionResult GetOrderById(int orderId)
        {
            try
            {
                var order = _db.Orders
                    .Where(o => o.OrderId == orderId)
                    .Select(order => new
                    {
                        order.OrderId,
                        order.OrderDate,
                        order.OrderStatus,
                        order.TotalPrice,
                        order.TransportMethod,
                        order.PaymentMethod,
                        order.DeliveryAddress,
                        order.PhoneNumber,
                        order.LocationLink,
                        UserName = _db.Users
                            .Where(user => user.UserId == order.UserId)
                            .Select(user => user.UserName)
                            .FirstOrDefault(),
                        Items = _db.OrderDetails
                            .Where(od => od.OrderId == order.OrderId)
                            .Select(od => new
                            {
                                od.ProductId,
                                ProductName = _db.Products
                                    .Where(p => p.ProductId == od.ProductId)
                                    .Select(p => p.ProductName)
                                    .FirstOrDefault(),
                                od.Quantity,
                                od.Price
                            })
                            .ToList()
                    })
                    .FirstOrDefault();

                if (order == null)
                {
                    return NotFound(new { Message = "Order not found." });
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }

        [HttpPut("UpdateOrderStatus/{orderId}")]
        public IActionResult UpdateOrderStatus(int orderId, [FromBody] string newStatus)
        {
            try
            {
                var order = _db.Orders.FirstOrDefault(o => o.OrderId == orderId);
                if (order == null)
                {
                    return NotFound(new { Message = "Order not found." });
                }

                order.OrderStatus = newStatus;
                _db.SaveChanges();

                return Ok(new { Message = "Order status updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }



    }
}
