﻿namespace Ecommerce.Models.DTOs
{
    public class RegisterDTO
    {
        
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }
        public IFormFile Image { get; set; }
       
      

    }
}
