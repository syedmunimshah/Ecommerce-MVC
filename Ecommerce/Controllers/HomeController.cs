using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Ecommerce.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models.DTOs;
namespace Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _env;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext, IWebHostEnvironment env)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
            _env = env;
        }

        public async Task<IActionResult> Dashboard()
        {

            if (HttpContext.Session.GetString("Username") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("Username").ToString();
                var user = await _applicationDbContext.Users.ToListAsync();
                return View(user);
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }

        }

        [HttpGet]

        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user != null)
            {
                EditDTO editDTO = new EditDTO()
                {Id=id,
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    PhoneNumber = user.PhoneNumber,
                    ExistImage = user.Image,
                    Status = user.Status,
                    UpdateDate = DateTime.Now,


                };
                return View(editDTO);
            }
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditDTO editDTO)
        {
            if (editDTO == null)
            {
                return BadRequest("Invalid data!"); 
            }

            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Id == editDTO.Id);

            if (user == null)
            {
                return NotFound();
            }

            // ✅ Check karein ke image update ho rahi hai ya nahi
            if (editDTO.UpdateImage != null)
            {
                string uniqueFileName = $"{Guid.NewGuid()}_{editDTO.UpdateImage.FileName}";
                string imagePath = Path.Combine(_env.WebRootPath, "UserImages", uniqueFileName);

               

                using (var fs = new FileStream(imagePath, FileMode.Create))
                {
                    await editDTO.UpdateImage.CopyToAsync(fs);
                }

                if (!string.IsNullOrEmpty(user.Image))
                {
                    string oldImagePath = Path.Combine(_env.WebRootPath, "UserImages", user.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                user.Image = uniqueFileName;
            }

           
              
              
                
             
            

            user.Name = editDTO.Name;
            user.Email = editDTO.Email;
            user.PhoneNumber = editDTO.PhoneNumber;
            user.Password = _passwordHasher.HashPassword(user, editDTO.Password);

            user.UpdateDate = DateTime.Now;

            _applicationDbContext.Users.Update(user);
            await _applicationDbContext.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }


        public async Task<IActionResult> Delete(Guid id) 
        {
            var del = await _applicationDbContext.Users.FirstOrDefaultAsync(x=>x.Id==id);
            if (del == null)

            {
                return NotFound(); 
            }
            _applicationDbContext.Users.Remove(del);
           await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction("Dashboard");
        }


    }
}
