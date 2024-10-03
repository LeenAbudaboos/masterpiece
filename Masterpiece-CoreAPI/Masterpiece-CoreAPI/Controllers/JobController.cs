﻿using Masterpiece_CoreAPI.DTO;
using Masterpiece_CoreAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Masterpiece_CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        
         private readonly MyDbContext _db;

        public JobController(MyDbContext db)
        {
            _db = db;
        }

        [HttpGet("GetAllJob")]
        public IActionResult AllJobs()
        {
            var jobs = _db.Jobs.ToList();

            return Ok(jobs);

        }

        [HttpGet("JobById{id}")]
        public IActionResult GetJobById(int id)
        {
            var job = _db.Jobs.FirstOrDefault(j => j.JobId == id);
            if (job == null)
            {
                return NotFound();
            }
            return Ok(job);
        }


        [HttpPost("AddJob")]
        public async Task<IActionResult> CreateJob([FromForm] JobDTO jobDto)
        {
            var job = new Job
            {
                JobTitle = jobDto.JobTitle,
                Description = jobDto.Description,
                JobImg = jobDto.JobImg.FileName,
                Location = jobDto.Location,
                Salary = jobDto.Salary,
                CategoryId = jobDto.CategoryId,
                PostedBy = jobDto.PostedBy,
            };

            // Ensure the upload folder exists
            var uploadImageFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadImageFolder))
            {
                Directory.CreateDirectory(uploadImageFolder);
            }

            // Save the image file
            var imageFile = Path.Combine(uploadImageFolder, jobDto.JobImg.FileName);
            using (var stream = new FileStream(imageFile, FileMode.Create))
            {
                await jobDto.JobImg.CopyToAsync(stream); // Ensure async file copy
            }

            // Add job to the database
            _db.Jobs.Add(job);
            await _db.SaveChangesAsync(); // Await save changes

            // Return the created job with JobId populated
            return Ok(job);
        }


        [HttpGet("JobByCategory/{categoryId}")]
        public IActionResult GetJobsByCategory(int categoryId)
        {
            var jobs =  _db.Jobs.Where(j => j.CategoryId == categoryId).ToList();

            if (jobs == null )
            {
                return NotFound(new { message = "الوظيفة غير موجودة" });
            }

            return Ok(jobs);
        }


        [HttpPut("UpdateJob/{id}")]
        public IActionResult UpdateJob(int id, [FromForm] JobDTO jobDto)
        {
            // تحقق من وجود الوظيفة التي نريد تحديثها
            var data = _db.Jobs.FirstOrDefault(c=>c.JobId==id);
         

             var uploadImageFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads"); //عللى شان ننشأ ملف ونحمل عليخ الصور
            if (!Directory.Exists(uploadImageFolder))// اتحقق هل ملف موجود اذا لا 
            {
                Directory.CreateDirectory(uploadImageFolder);//انشأ هذا الملف
            }
            var imageFile = Path.Combine(uploadImageFolder, jobDto.JobImg.FileName);//صير ارفعلي عليه البيانات
            using (var stream = new FileStream(imageFile, FileMode.Create)) //
            {
                jobDto.JobImg.CopyTo(stream);

            }

            data.JobTitle = jobDto.JobTitle;
            data.JobImg=jobDto.JobImg.FileName;
            data.Description = jobDto.Description;
            data.Location = jobDto.Location;
            data.Salary = jobDto.Salary;
            data.CategoryId = jobDto.CategoryId;

            _db.Jobs.Update(data);
           _db.SaveChangesAsync();

            return Ok(data);
        }

        [HttpDelete("Deletejob")]
        public IActionResult Delete(int id)
        {

            var jod = _db.Jobs.FirstOrDefault(c => c.JobId == id);
            _db.Jobs.Remove(jod);
            _db.SaveChanges();
            return Ok(jod);
        }

    }
}
   