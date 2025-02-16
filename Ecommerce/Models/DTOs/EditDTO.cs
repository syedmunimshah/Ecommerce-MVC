namespace Ecommerce.Models.DTOs
{
    public class EditDTO
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public IFormFile? UpdateImage { get; set; }
        public string? ExistImage { get; set; }
        public bool Status { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
