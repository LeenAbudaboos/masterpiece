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
    public class CategorieController : ControllerBase
    {


        private readonly MyDbContext _db;

        public CategorieController(MyDbContext db)
        {
            _db = db;
        }


        [HttpGet("GetAllCategorie")]
        public IActionResult GetAllCategorie()
        {
            var category = _db.Categories.ToList();
            return Ok(category);
        }

        [HttpGet("GetCategorieByID{id}")]

        public IActionResult GetCategorie(int id)
        {
            var category = _db.Categories.FirstOrDefault(c => c.CategoryId == id);
            return Ok(category);
        }

        [HttpDelete("DeleteCategorie/{id}")]
        public IActionResult DeleteCategorie(int id)
        {
            // البحث عن الفئة بناءً على id
            var category = _db.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
            {
                return NotFound("Category not found");
            }

            // حذف الفئة
            _db.Categories.Remove(category);
            _db.SaveChanges();

            return Ok("Category deleted successfully");
        }

    

    [HttpPost("AddCategorie")]

        public IActionResult AddCategorie([FromForm] CategorieDTO categories)
        {
            var data = new Categorie
            {

                CategoryName = categories.CategoryName,
                Description = categories.Description,
                CategoryImg = categories.CategoryImg.FileName

            };

            var uploadImageFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads"); //عللى شان ننشأ ملف ونحمل عليخ الصور
            if (!Directory.Exists(uploadImageFolder))// اتحقق هل ملف موجود اذا لا 
            {
                Directory.CreateDirectory(uploadImageFolder);//انشأ هذا الملف
            }  
            var imageFile = Path.Combine(uploadImageFolder, categories.CategoryImg.FileName);//صير ارفعلي عليه البيانات
            using (var stream = new FileStream(imageFile, FileMode.Create)) //
            {
                categories.CategoryImg.CopyTo(stream);

            }
            
            _db.Categories.Add(data);
                _db.SaveChanges();
                return Ok(data);



        }


       

        [HttpPut("UpdateCategorie/{id}")]
        public IActionResult UpdateCategorie(int id, [FromForm] CategorieDTO categories)
        {
            // البحث عن الفئة بناءً على id
            var category = _db.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
            {
                return NotFound("Category not found");
            }

            // تحديث البيانات
            category.CategoryName = categories.CategoryName;
            category.Description = categories.Description;

            if (categories.CategoryImg != null)
            {
                var uploadImageFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadImageFolder))
                {
                    Directory.CreateDirectory(uploadImageFolder);
                }

                var imageFile = Path.Combine(uploadImageFolder, categories.CategoryImg.FileName);
                using (var stream = new FileStream(imageFile, FileMode.Create))
                {
                    categories.CategoryImg.CopyTo(stream);
                }

                category.CategoryImg = categories.CategoryImg.FileName;
            }

            _db.Categories.Update(category);
            _db.SaveChanges();

            return Ok("Category updated successfully");
        }



     

            // API لجلب المنتجات بناءً على الفئة
            [HttpGet("GetProductsByCategory/{categoryId}")]
            public IActionResult GetProductsByCategory(int categoryId)
            {
                // جلب المنتجات التي تنتمي للفئة
                var products = _db.Products
                                  .Where(p => p.CategoryId == categoryId)
                                  .Select(p => new
                                  {
                                      p.ProductId,
                                      p.ProductName,
                                      p.Price,
                                      p.Descriptions,
                                      p.ProductImg,
                                      p.Quantity,
                                      CategoryName = p.Category.CategoryName // إذا كنت بحاجة إلى اسم الفئة
                                  })
                                  .ToList();

                if (products == null || !products.Any())
                {
                    return NotFound(new { message = "لا توجد منتجات لهذه الفئة" });
                }

                return Ok(products);
            }
        



    }
}
