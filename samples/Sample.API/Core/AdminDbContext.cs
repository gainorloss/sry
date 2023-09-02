using Microsoft.EntityFrameworkCore;

namespace Sample.API.Core
{
    public class AdminDbContext : DbContext
    {
        public AdminDbContext(DbContextOptions options) : base(options)
        {
        }

       
    }
}
