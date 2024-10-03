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

        [HttpDelete("DeleteCatrgorie{id}")]
        public IActionResult DeleteCategorie(int id) {
            var category = _db.Categories.FirstOrDefault(x => x.CategoryId == id);
            _db.Categories.Remove(category);
            _db.SaveChanges();
            return Ok(category);

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

            [HttpPut("UpdateCategorie{id}")]

            public IActionResult UpdateCategorie(int id, [FromForm] CategorieDTO categories) {

                var data = _db.Categories.FirstOrDefault(c => c.CategoryId == id);

                var uploadImageFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads"); //عللى شان ننشأ ملف ونحمل عليخ الصور
                if (!Directory.Exists(uploadImageFolder))// اتحقق هل ملف موجود اذا لا 
                {
                    Directory.CreateDirectory(uploadImageFolder);//انشأ هذا الملف
                }
                var imageFile = Path.Combine(uploadImageFolder, categories.CategoryImg.FileName);//صير ارفعلي عليه البيانات
                using (var stream = new FileStream(imageFile, FileMode.Create)) //
                {
                    categories.CategoryImg.CopyToAsync(stream);
                }

                data.CategoryName = categories.CategoryName;
                data.Description = categories.Description;
                data.CategoryImg = categories.CategoryImg.FileName;


                _db.Categories.Update(data);
                _db.SaveChanges();
                return Ok();
            }





        

    } }
