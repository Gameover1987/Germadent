using Germadent.WebApi.Configuration;
using Germadent.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace Germadent.WebApi.DataAccess
{
    public class UmcDbContext : DbContext
    {
        private readonly IServiceConfiguration _configuration;

        public UmcDbContext(IServiceConfiguration configuration)
        {
            _configuration = configuration;

            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<RoleInUserEntity> RoleInUsers { get; set; }

        public DbSet<RoleEntity> Roles { get; set; }

        public DbSet<RightInRoleEntity> RightInRoles { get; set; }

        public DbSet<RightEntity> Rights { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.ConnectionString);
        }
    }
}