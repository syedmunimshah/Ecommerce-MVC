namespace Ecommerce.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }
        public DateTime CreateDate { get; set; }= DateTime.Now;
        public DateTime? UpdateDate { get; set; }

    }
}
