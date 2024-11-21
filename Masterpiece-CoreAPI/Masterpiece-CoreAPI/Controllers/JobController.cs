using Masterpiece_CoreAPI.DTO;
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
         

           

            data.JobTitle = jobDto.JobTitle;
            data.Description = jobDto.Description;
            data.Location = jobDto.Location;
            data.Salary = jobDto.Salary;
            data.CategoryId = jobDto.CategoryId;
            data.PostedBy = jobDto.PostedBy;

            _db.Jobs.Update(data);
           _db.SaveChangesAsync();

            return Ok(data);
        }



        //[HttpDelete("Deletejob/{id}")]
        //public IActionResult DeleteJob(int id)
        //{
        //    try
        //    {
        //        var job =  _db.Jobs.Find(id); // ابحث عن الوظيفة باستخدام المعرف
        //        if (job == null) // إذا لم يتم العثور على الوظيفة
        //        {
        //            return NotFound("الوظيفة غير موجودة");
        //        }

        //        _db.Jobs.Remove(job); // حذف الوظيفة
        //         _db.SaveChanges(); // حفظ التغييرات
        //        return Ok("تم حذف الوظيفة بنجاح");
        //    }
        //    catch
        //    {
        //        return StatusCode(500, "حدث خطأ أثناء الحذف.");
        //    }
        //}

        [HttpDelete("Deletejob/{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            try
            {
                // احذف الطلبات المرتبطة بهذه الوظيفة
                var jobApplications = _db.JobApplications.Where(ja => ja.JobId == id).ToList();
                if (jobApplications.Any())
                {
                    _db.JobApplications.RemoveRange(jobApplications);
                }

                // ابحث عن الوظيفة
                var job = await _db.Jobs.FindAsync(id);
                if (job == null)
                {
                    return NotFound("الوظيفة غير موجودة");
                }

                // احذف الوظيفة
                _db.Jobs.Remove(job);
                await _db.SaveChangesAsync();

                return Ok("تم حذف الوظيفة بنجاح");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"حدث خطأ أثناء الحذف: {ex.Message}");
            }
        }




        [HttpGet("GetJobApplicationsWithUsers")]
        public IActionResult GetJobApplicationsWithUsers()
        {
            var jobsWithApplications = _db.Jobs
                .Include(j => j.JobApplications) // تضمين التقديمات على الوظائف
                .ThenInclude(ja => ja.User) // تضمين بيانات المستخدمين الذين قدموا الطلبات
                .Select(j => new
                {
                    JobId = j.JobId,
                    JobTitle = j.JobTitle,
                    JobDescription = j.Description,
                    Applicants = j.JobApplications.Select(ja => new
                    {
                        UserName = ja.User.UserName,
                        Address = ja.User.Address,
                        Profession = ja.User.Profession
                    }).ToList()
                })
                .ToList();

            return Ok(jobsWithApplications);
        }


        [HttpGet("GetApplicantsByJobId/{jobId}")]
        public async Task<IActionResult> GetApplicantsByJobId(int jobId)
        {
            var applicants = await _db.JobApplications
                .Where(a => a.JobId == jobId) // استعلام عن الأشخاص المتقدمين للوظيفة
                .Include(a => a.User) // تضمين بيانات المستخدم المرتبطة
                .Select(a => new
                {
                    a.UserId,
                    UserName = a.User.UserName,
                    PhoneNumber=a.User.PhoneNumber,
                    Address = a.User.Address,
                    Profession = a.User.Profession,
                    ProfessionDescription=a.User.ProfessionDescription,
                    YearsOfExperience=a.User.YearsOfExperience,
                    HasWarranty=a.User.HasWarranty,

                })
                .ToListAsync();

            if (applicants == null || !applicants.Any())
            {
                return NotFound(new { message = "لا يوجد متقدمون لهذه الوظيفة" });
            }

            return Ok(applicants);
        }

    }
}
   