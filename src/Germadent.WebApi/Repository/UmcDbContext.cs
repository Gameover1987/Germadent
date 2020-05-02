using Germadent.WebApi.Configuration;
using Germadent.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace Germadent.WebApi.Repository
{
    public class UmcDbContext : DbContext
    {
        private readonly IServiceConfiguration _configuration;

        public UmcDbContext(IServiceConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<UserEntity> Users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.ConnectionString);
        }
    }
}