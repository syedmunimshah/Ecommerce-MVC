using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Models.Entities
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) 
        {
            
        }
        public  DbSet<User> Users { get; set; }
    }
}
