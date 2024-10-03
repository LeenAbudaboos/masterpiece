using Masterpiece_CoreAPI.DTO;
using Masterpiece_CoreAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using static Masterpiece_CoreAPI.Shared.ImageSaver;

namespace Masterpiece_CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        

     
            private readonly MyDbContext _db;

            public UserController(MyDbContext db)
            {
                _db =db;
            }

        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var users = _db.Users.ToList();
            return Ok(users);
        }

            [HttpPost("register")]
            public async Task<IActionResult> Register([FromForm] UserRegisterDTO request)
            {
                // Hash and Salt password before saving
                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                var user = new User
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    Password=request.Password,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    Role = request.Role
                };


                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                 var creatcart = new Cart
                {
                UserId = user.UserId
                 };


                  _db.Carts.Add(creatcart);
                  await _db.SaveChangesAsync();

                 var creatjobapp = new JobApplication
                 {
                UserId = user.UserId
                 };
                _db.JobApplications.Add(creatjobapp);
                 await _db.SaveChangesAsync();

            return Ok("تم انشاء الحساب بنجاح");


            }


            private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
            {
                using (var hmac = new System.Security.Cryptography.HMACSHA512())
                {
                    passwordSalt = hmac.Key;
                    passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                }
            }


        [HttpPost("login")]

        public IActionResult login([FromForm] UserLoginDTO userLoginDTO) {

            // التحقق من أن الحقول غير فارغة
            if (string.IsNullOrEmpty(userLoginDTO.PhoneNumber) || string.IsNullOrEmpty(userLoginDTO.Password))
            {
                return BadRequest("الرجاء ادخال رقم الهاتف وكلمة السر.");
            }

            // التحقق من وجود المستخدم بناءً على رقم الهاتف فقط
            var user = _db.Users.FirstOrDefault(x => x.PhoneNumber == userLoginDTO.PhoneNumber);

            // إذا كان المستخدم غير موجود
            if (user == null)
            {
                return NotFound("المستخدم غير موجود، الرجاء انشاء حساب.");
            }

            // التحقق من صحة كلمة المرور
            if (user.Password != userLoginDTO.Password)
            {
                return BadRequest("كلمة السر غير صحيحة، الرجاء المحاولة مرة أخرى.");
            }

            // التحقق من دور المستخدم
            if (user.Role == "Admin")
            {
                var Admin = new User
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    Password = user.Password,
                    Role = user.Role
                };
                return Ok(Admin);
            }

            // إعادة معلومات المستخدم في حالة كان دوره ليس "Admin"
            var u = new User
            {
                UserId = user.UserId,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Password = user.Password,
                UserImage = user.UserImage,
                Email = user.Email,
                Address = user.Address,
                Role = user.Role
            };
            return Ok(u);

        }












    }


}

