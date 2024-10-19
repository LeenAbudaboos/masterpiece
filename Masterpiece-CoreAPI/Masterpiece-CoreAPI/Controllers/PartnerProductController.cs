using Masterpiece_CoreAPI.DTO;
using Masterpiece_CoreAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Masterpiece_CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnerProductController : ControllerBase
    {

        private readonly MyDbContext _db;

        public PartnerProductController(MyDbContext db)
        {
            _db = db;
        }

        [HttpGet("GetAllPartnerProduct")]
        public IActionResult AllFormJobs()
        {
            var jobs = _db.PartnerProducts.ToList();

            return Ok(jobs);

        }


        [HttpPost("AddPartnerProduct")]
        public IActionResult CreateJob([FromForm] PartnerProductDTO jobDto)
        {
            var uploadImageFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads"); //عللى شان ننشأ ملف ونحمل عليخ الصور
            if (!Directory.Exists(uploadImageFolder))// اتحقق هل ملف موجود اذا لا 
            {
                Directory.CreateDirectory(uploadImageFolder);//انشأ هذا الملف
            }
            var imageFile = Path.Combine(uploadImageFolder, jobDto.ProductImage.FileName);//صير ارفعلي عليه البيانات
            using (var stream = new FileStream(imageFile, FileMode.Create)) //
            {
                jobDto.ProductImage.CopyTo(stream);

            }


            var job = new PartnerProduct
            {
                ProductName=jobDto.ProductName,
                ProductDescription=jobDto.ProductDescription,
                ProductImage=jobDto.ProductImage.FileName,
                PartnerCompanyName=jobDto.PartnerCompanyName,
                PartnerEmail=jobDto.PartnerEmail,
                PartnerName=jobDto.PartnerName,
                PartnerPhone=jobDto.PartnerPhone,   
                ProductPrice=jobDto.ProductPrice,
               
            };



            _db.PartnerProducts.Add(job);
             _db.SaveChanges();


            return Ok(job);
        }

    }
}
