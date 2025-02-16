using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
namespace Ecommerce.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _env;
        private readonly PasswordHasher<User> _passwordHasher =new PasswordHasher<User>();
        public AuthController(ApplicationDbContext applicationDbContext, IWebHostEnvironment env)
        {
            _applicationDbContext = applicationDbContext;
           _env = env;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            //image create
            string uniqueFileName = $"{Guid.NewGuid()}_{registerDTO.Image.FileName}";
            string ImagePath = Path.Combine(_env.WebRootPath, "UserImages",uniqueFileName) ;
            FileStream fs = new FileStream(ImagePath, FileMode.Create);
            registerDTO.Image.CopyTo(fs);

            string hashedPassword= _passwordHasher.HashPassword(null,registerDTO.Password);

            User user = new User()
            {
                Name = registerDTO.Name,
                Email = registerDTO.Email,
                Password = hashedPassword,
                PhoneNumber = registerDTO.PhoneNumber,
                Image = uniqueFileName,
                Status = true,


            };
            await _applicationDbContext.Users.AddAsync(user);
            await _applicationDbContext.SaveChangesAsync();
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginDTO loginDTO)
        {
            if (string.IsNullOrWhiteSpace(loginDTO.Email) || string.IsNullOrWhiteSpace(loginDTO.Password))
            {
                ViewBag.ErrorMessage = "Email or Password cannot be empty.";
                return View(loginDTO);
            }

            var user = _applicationDbContext.Users.FirstOrDefault(x => x.Email == loginDTO.Email);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid email or password.";
                return View(loginDTO);
            }

            var passwordCheck = _passwordHasher.VerifyHashedPassword(user, user.Password, loginDTO.Password);
            if (passwordCheck == PasswordVerificationResult.Success && user.Email!=null)
            {

                HttpContext.Session.SetString("Username", user.Email);  
                return RedirectToAction("Dashboard", "Home");

               
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid email or password.";
                return View(loginDTO);
            }
        

        }

    }
}
