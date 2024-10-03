using Masterpiece_CoreAPI.DTO;
using Masterpiece_CoreAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Masterpiece_CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationController : ControllerBase
    {
        private readonly MyDbContext _db;

        public JobApplicationController(MyDbContext db)
        {
            _db = db;
        }

        [HttpPost("ApplyForJob")]
        public async Task<IActionResult> ApplyForJob([FromForm] JobApplicationDTO applicationDTO)
        {
            if (applicationDTO == null || applicationDTO.UserId == null || applicationDTO.JobId == null)
            {
                return BadRequest("سجل الدخول حتى تتمكن من التقديم على الوظيفة");
            }

            var jobApplication = new JobApplication
            {
                UserId = applicationDTO.UserId,
                JobId = applicationDTO.JobId,
            };

            _db.JobApplications.Add(jobApplication);
            await _db.SaveChangesAsync();

            return Ok("لقد تم تقديم الطلب بنجاح");
        }


        [HttpGet("getallapply")]

        public IActionResult getallapply() {
        
            var data = _db.JobApplications.ToList();
        return Ok("");
        }



        //[HttpGet("CheckApplication")]
        //public async Task<IActionResult> HasApplied(int userId, int jobId)
        //{
        //    var application = await _db.JobApplications.FirstOrDefaultAsync(a => a.UserId == userId && a.JobId == jobId);

        //    if (application != null)
        //    {
        //        return Ok( true); // المستخدم قد تقدم لهذه الوظيفة من قبل
        //    }

        //    return Ok(false); // المستخدم لم يتقدم لهذه الوظيفة
        //}

        [HttpGet("HasApplied")]
        public async Task<IActionResult> HasApplied(int userId, int jobId)
        {
            var existingApplication = await _db.JobApplications
                .FirstOrDefaultAsync(a => a.UserId == userId && a.JobId == jobId);

            if (existingApplication != null)
            {
                return Ok(true); // المستخدم قد تقدم لهذه الوظيفة بالفعل
            }

            return Ok(false); // المستخدم لم يتقدم لهذه الوظيفة
        }








        [HttpGet("GetUserApplications")]
        public async Task<IActionResult> GetUserApplications(int userId)
        {
            var applications = await _db.JobApplications
                .Where(a => a.UserId == userId)
                .Include(a => a.Job) // Assuming JobApplication has a navigation property for Job
                .Select(a => new
                {
                    a.ApplicationId, // This is the JobApplication ID, used for deleting the application
                    JobTitle = a.Job.JobTitle,
                    Location = a.Job.Location,
                    Description = a.Job.Description,
                    Salary=a.Job.Salary,

                })
                .ToListAsync();

            return Ok(applications);
        }

        [HttpDelete("DeleteApplication/{applicationId}")]
        public async Task<IActionResult> DeleteApplication(int applicationId)
        {
            var application = await _db.JobApplications.FindAsync(applicationId);
            if (application == null)
            {
                return NotFound("Application not found");
            }

            _db.JobApplications.Remove(application);
            await _db.SaveChangesAsync();

            return Ok("Application deleted successfully");
        }




    }
}

