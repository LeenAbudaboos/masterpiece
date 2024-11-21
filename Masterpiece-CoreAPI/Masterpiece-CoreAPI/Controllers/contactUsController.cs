using Masterpiece_CoreAPI.DTO;
using Masterpiece_CoreAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Masterpiece_CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class contactUsController : ControllerBase
    { // الرد على رسالة
        
        private readonly EmailService _emailService;

      

        private readonly MyDbContext _context;

            public contactUsController(MyDbContext context , EmailService emailService)
            {
                _context = context;
            _emailService = emailService;

        }

        [HttpGet("GetAllMessages")]
        public async Task<IActionResult> GetAllMessages()
        {
            try
            {
                var messages = await _context.ContactUs
                    .Select(msg => new
                    {
                        msg.MessageId,
                        msg.Name,
                        msg.Email,
                        msg.Subject,
                        msg.Message,
                        msg.SubmittedAt,
                        msg.Status
                    })
                    .ToListAsync();

                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching messages: {ex.Message}");
            }
        }


        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromForm] ContactUDTO messageDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("الرجاء التأكد من صحة البيانات.");

            // تحويل DTO إلى كائن الجدول
            var message = new ContactU
            {
                Name = messageDto.Name,
                Email = messageDto.Email,
                Subject = messageDto.Subject,
                Message = messageDto.Message,
                SubmittedAt = DateTime.Now // إذا كان الحقل موجودًا في الجدول
            };

            // إضافة الرسالة إلى قاعدة البيانات
            _context.ContactUs.Add(message);
            await _context.SaveChangesAsync();

            return Ok("تم إرسال الرسالة بنجاح.");
        }


        // حذف رسالة
        [HttpDelete("DeleteMessage/{id}")]
            public async Task<IActionResult> DeleteMessage(int id)
            {
                var message = await _context.ContactUs.FindAsync(id);
                if (message == null)
                    return NotFound("الرسالة غير موجودة.");

                _context.ContactUs.Remove(message);
                await _context.SaveChangesAsync();
                return Ok("تم حذف الرسالة بنجاح.");
            }

       

        // الرد على الرسالة
        [HttpPost("ReplyMessage/{id}")]
        public async Task<IActionResult> ReplyMessage([FromForm] string replyMessage, int id)
        {
            var message = await _context.ContactUs.FindAsync(id);
            if (message == null)
                return NotFound("الرسالة غير موجودة.");

            try
            {
                // إرسال البريد الإلكتروني
                await _emailService.SendEmailAsync(
                    toEmail: message.Email,
                    subject: $"رد على رسالتك: {message.Subject}",
                    body: $"<p>مرحبا {message.Name},</p><p>{replyMessage}</p><p>شكرا لك على التواصل معنا!</p>"
                );

                return Ok($"تم إرسال الرد إلى البريد الإلكتروني: {message.Email}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"حدث خطأ أثناء إرسال البريد الإلكتروني: {ex.Message}");
            }
        }
    }

    }

