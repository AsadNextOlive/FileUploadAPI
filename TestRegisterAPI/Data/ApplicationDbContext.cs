using Microsoft.EntityFrameworkCore;
using TestRegisterAPI.Model;
using TestRegisterAPI.ViewModel;

namespace TestRegisterAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Register> Register { get; set; }
        public DbSet<UploadContent> UploadContent { get; set; }
        public DbSet<CustomerErrorResponseViewModel> CustomerErrorResponseViewModel { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options) 
        { 

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerErrorResponseViewModel>().HasNoKey();
        }
    }
}
