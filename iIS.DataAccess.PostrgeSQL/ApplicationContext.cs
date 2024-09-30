using iIS.DataAccess.PostrgeSQL.Configuractions;
using iIS.DataAccess.PostrgeSQL.Entities;
using Microsoft.EntityFrameworkCore;

namespace iIS.DataAccess.PostrgeSQL
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            
        }

        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguraction());

            base.OnModelCreating(modelBuilder);
        }
    }
}