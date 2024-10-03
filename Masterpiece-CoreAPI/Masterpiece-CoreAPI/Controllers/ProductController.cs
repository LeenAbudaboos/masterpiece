using Azure.Core;
using Masterpiece_CoreAPI.DTO;
using Masterpiece_CoreAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Masterpiece_CoreAPI.Shared.ImageSaver;

namespace Masterpiece_CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly MyDbContext db;

        public ProductController(MyDbContext _db)
        {
            db = _db;

        }

        [HttpGet("getallproduct")]
        public IActionResult getallproduct()
        {
            var product = db.Products.ToList();

            return Ok(product);



        }
        //[HttpGet("GetCategsById{id:min(5)}")] //بناء على شرط معين
        [HttpGet("GetproductByID")]
        public IActionResult Get(int id)
        {

            var Product = db.Products.FirstOrDefault(c => c.ProductId == id);

            return Ok(Product);
        }


        [HttpGet("GetproductBycategoryID/{categoryId}")]
        public IActionResult Getcategoryid(int categoryId)
        {

            var Product = db.Products.Where(c => c.CategoryId == categoryId).ToList();

            return Ok(Product);
        }

        [HttpGet("GetProductByname")]
        public IActionResult Getname(string name)
        {
            var product = db.Products.FirstOrDefault(c => c.ProductName == name);
            return Ok(product);
        }

        [HttpDelete("Deleteprodecte")]
        public IActionResult Delete(int id)
        {

            var product = db.Products.FirstOrDefault(c => c.ProductId == id);
            db.Products.Remove(product);
            db.SaveChanges();
            return Ok(product);
        }

        //[HttpGet("GetproductByID{id}")]
        //public IActionResult GetproductByID(int id)
        //{
        //    //لانو في ريلشن بين التيبل وعلى شان ما يعطيني null
        //    var Product = db.Products.Where(c => c.ProductId == id).Select(c => new
        //    {
        //        c.ProductId,
        //        c.ProductName,
        //        c.ProductImg,
        //        c.Descriptions,
        //        c.Price,
        //        Category = new
        //        {
        //            c.Category.CategoryId,
        //            c.Category.CategoryName,
        //            c.Category.Description

        //        }
        //    }).ToList();

        //    return Ok(Product);
        //}





        [HttpPost("AddNewProdect")]
        public IActionResult AddProduct([FromForm] ProductDTO prodect)
        {
            
              
                var data = new Product
            {
                ProductName = prodect.ProductName,
                ProductImg = prodect.ProductImg.FileName,
                Descriptions = prodect.Descriptions,
                Price = prodect.Price,
                Quantity = prodect.Quantity,
                CategoryId = prodect.CategoryId
            };
            var uploadImageFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads"); //عللى شان ننشأ ملف ونحمل عليخ الصور
            if (!Directory.Exists(uploadImageFolder))// اتحقق هل ملف موجود اذا لا 
            {
                Directory.CreateDirectory(uploadImageFolder);//انشأ هذا الملف
            }
            var imageFile = Path.Combine(uploadImageFolder, prodect.ProductImg.FileName);//صير ارفعلي عليه البيانات
            using (var stream = new FileStream(imageFile, FileMode.Create))


                db.Products.Add(data);
            db.SaveChanges();
            return Ok(data);


        }



        [HttpPut("Update{id}")]
        public IActionResult updateProduct(int id, [FromForm] ProductDTO prodect)
        {



            var data = db.Products.FirstOrDefault(p => p.ProductId == id);
            data.ProductName = prodect.ProductName;
            data.ProductImg = prodect.ProductImg.FileName;
            data.Descriptions = prodect.Descriptions;
            data.Price = prodect.Price;
            data.Quantity = prodect.Quantity;
            data.CategoryId = prodect.CategoryId;

            var uploadImageFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads"); //عللى شان ننشأ ملف ونحمل عليخ الصور
            if (!Directory.Exists(uploadImageFolder))// اتحقق هل ملف موجود اذا لا 
            {
                Directory.CreateDirectory(uploadImageFolder);//انشأ هذا الملف
            }
            var imageFile = Path.Combine(uploadImageFolder, prodect.ProductImg.FileName);//صير ارفعلي عليه البيانات
            using (var stream = new FileStream(imageFile, FileMode.Create))

            db.Products.Update(data);
            db.SaveChanges();
            return Ok();
        }
    }
}
