using Masterpiece_CoreAPI.DTO;
using Masterpiece_CoreAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Masterpiece_CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly MyDbContext _db;

        public CartItemController(MyDbContext db)
        {
            _db = db;
        }

        [HttpGet("{cartId}")]
        public IActionResult GetCartItemsByCartId(int cartId)
        {

            var cartItems = _db.CartItems
                                    .Where(c => c.CartId == cartId)
                                    .ToList();

            if (cartItems == null || cartItems.Count == 0)
            {
                return NotFound("No items found for this cart.");
            }

            return Ok(cartItems);
        }




        [HttpPost("addToCart")]
        public IActionResult AddItemToCart(int userId, [FromForm] CartItemDTO cartItemDto)
        {
            if (cartItemDto == null || cartItemDto.ProductId == null)
            {
                return BadRequest("البيانات غير صحيحة. تأكد من إدخال جميع المعلومات.");
            }

         
            var existingCart = _db.CartsProducts.FirstOrDefault(c => c.UserId == userId);

         
            if (existingCart == null)
            {
                existingCart = new CartsProduct
                {
                    UserId = userId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _db.CartsProducts.Add(existingCart);
                _db.SaveChanges();  
            }

            
            var cartItem = new CartItem
            {
                CartId = existingCart.CartId,  
                ProductId = cartItemDto.ProductId,
               
            };

            _db.CartItems.Add(cartItem);
            _db.SaveChanges();

            return Ok("تمت إضافة المنتج إلى السلة بنجاح.");
        }


        [HttpGet("GetCartItems{userId}")]
        public IActionResult GetCartDetails(int userId)
        {
            
            var cart = _db.CartsProducts.FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound("لم يتم العثور على سلة لهذا المستخدم.");
            }

            
            var cartItems = _db.CartItems
                .Where(ci => ci.CartId == cart.CartId)
                .Select(ci => new
                {
                    ci.CartItemId,
                    
                    ProductDetails = _db.Products
                        .Where(p => p.ProductId == ci.ProductId)
                        .Select(p => new
                        {
                            p.ProductId,
                            p.ProductName,
                            p.Descriptions,
                            p.Price,
                            p.ProductImg
                        }).FirstOrDefault()  
                }).ToList();

            if (cartItems.Count == 0)
            {
                return NotFound("السلة فارغة.");
            }

            return Ok(cartItems);
        }


         
        [HttpDelete("RemoveItem{cartItemId}")]
        public IActionResult RemoveItemFromCart(int cartItemId)
        {
           
            var cartItem = _db.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemId);

            if (cartItem == null)
            {
                return NotFound("لم يتم العثور على العنصر في السلة.");
            }

           
            _db.CartItems.Remove(cartItem);
            _db.SaveChanges();  

            return Ok("تمت إزالة المنتج من السلة بنجاح.");
        }



        //[HttpPost]
        //public IActionResult AddItemToCart(int userId, [FromForm] CartItemDTO cartItemDto)
        //{
        //    if (cartItemDto == null || cartItemDto.ProductId == null)
        //    {
        //        return BadRequest("سجل الدخول حتى تتمكن من التقديم على الوظيفة");
        //    }

        //    // تحقق مما إذا كان CartId موجودًا
        //    var cart = _db.CartsProducts.FirstOrDefault(c => c.CartId == cartItemDto.CartId);



        //    // الآن أضف العنصر إلى السلة
        //    var cartItem = new CartItem
        //    {
        //        CartId = cart.CartId,
        //        ProductId = cartItemDto.ProductId
        //    };

        //    _db.CartItems.Add(cartItem);
        //    _db.SaveChanges();

        //    return Ok("تمت اضافة المنتج بنجاح");
        //} 


    }
}
