using Masterpiece_CoreAPI.DTO;
using Masterpiece_CoreAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Masterpiece_CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnerJobController : ControllerBase
    {

        private readonly MyDbContext _db;

        public PartnerJobController(MyDbContext db)
        {
            _db = db;
        }

        [HttpGet("GetAllPartnerJob")]
        public IActionResult AllFormJobs()
        {
            var jobs = _db.PartnerJobs.ToList();

            return Ok(jobs);

        }

      
        [HttpPost("AddPartnerJob")]
        public IActionResult CreateJob([FromForm] PartnerJobDTO jobDto)
        {
            var job = new PartnerJob
            {
                JobTitle = jobDto.JobTitle,
                JobDescription = jobDto.JobDescription,
                Salary = jobDto.Salary,
                JobLocation = jobDto.JobLocation,
                PartnerName = jobDto.PartnerName,
                PartnerEmail = jobDto.PartnerEmail,
                PartnerPhone = jobDto.PartnerPhone,
             
            };



            _db.PartnerJobs.Add(job);
            _db.SaveChanges(); 

        
            return Ok(job);
        }

    }
}
