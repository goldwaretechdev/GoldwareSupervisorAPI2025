using GW.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Core.Context
{
    public class SupervisorContext:DbContext
    {
        private static readonly Guid SeedUserId = Guid.Parse("d3b3c29a-4e2c-4b25-b6f4-2f8ebc4a1f05");

        public SupervisorContext(DbContextOptions<SupervisorContext> options):base(options) { }


        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Access> Access { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<FOTA> FOTA { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<SoftwareVersion> SoftwareVersions { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<UserAndCompany> UserAndCompany { get; set; }
        public DbSet<Device> Devices { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Access>()
                .HasOne(a=>a.User)
                .WithMany(u=>u.Access)
                .HasForeignKey(a=>a.FkUserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Access>()
                .HasOne(a=>a.Unit)
                .WithMany(u=>u.Access)
                .HasForeignKey(a=>a.FkUnitId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Device>()
                .HasOne(a=>a.ProductOwner)
                .WithMany(u=>u.Devices)
                .HasForeignKey(a=>a.FkOwnerId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Device>()
                .HasOne(a=>a.ESP)
                .WithMany(u=>u.ESPVersions)
                .HasForeignKey(a=>a.FkESPId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Device>()
                .HasOne(a=>a.STM)
                .WithMany(u=>u.STMVersions)
                .HasForeignKey(a=>a.FkSTMId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Device>()
                .HasOne(a=>a.Holtek)
                .WithMany(u=>u.HoltekVersions)
                .HasForeignKey(a=>a.FkHoltekId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<FOTA>()
                .HasOne(a=>a.ProductOwner)
                .WithMany(u=>u.FOTAs)
                .HasForeignKey(a=>a.FkOwnerId)
                .OnDelete(DeleteBehavior.Restrict);
             
            builder.Entity<FOTA>()
                .HasOne(a=>a.ESP)
                .WithMany(u=>u.FOTAESPVersions)
                .HasForeignKey(a=>a.FkESPId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<FOTA>()
                .HasOne(a=>a.STM)
                .WithMany(u=>u.FOTASTMVersions)
                .HasForeignKey(a=>a.FkSTMId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<FOTA>()
                .HasOne(a=>a.Holtek)
                .WithMany(u=>u.FOTAHoltekVersions)
                .HasForeignKey(a=>a.FkHoltekId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Log>()
                .HasOne(a=>a.Device)
                .WithMany(u=>u.Logs)
                .HasForeignKey(a=>a.FkDeviceId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Log>()
                .HasOne(a=>a.UserRole)
                .WithMany(u=>u.Logs)
                .HasForeignKey(a=>a.FkUserRoleId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<UserAndCompany>()
                .HasOne(a=>a.User)
                .WithMany(u=>u.UserAndCompanies)
                .HasForeignKey(a=>a.FkUserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<UserAndCompany>()
                .HasOne(a=>a.Company)
                .WithMany(u=>u.UserAndCompanies)
                .HasForeignKey(a=>a.FkCompanyId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<UserRoles>()
                .HasOne(a=>a.User)
                .WithMany(u=>u.UserRoles)
                .HasForeignKey(a=>a.FkUserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<UserRoles>()
                .HasOne(a=>a.Role)
                .WithMany(u=>u.UserRoles)
                .HasForeignKey(a=>a.FkRoleId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);

            SeedUser(builder);
        }

        private void SeedUser(ModelBuilder builder)
        {
            builder.Entity<User>().HasData(
                new User
                {
                    Id = SeedUserId,
                    Username = "sdh",
                    Password = "$2a$13$IJx6qkqyuUqmuM7NLKZDM.V1SnroBT0ICRHtcaS1AwYkr6z4p1xr6",
                    FName = "s",
                    LName = "hasanabadi",
                    Mobile = "09155909973"
                });

            builder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "Development"
                },
                new Role
                {
                    Id = 2,
                    Name = "Production"
                },
                new Role
                {
                    Id = 3,
                    Name = "Collaborators"
                },
                new Role
                {
                    Id = 4,
                    Name = "Support"
                });
            builder.Entity<UserRoles>().HasData(
                new UserRoles
                {
                    Id = 1,
                    FkRoleId = 1,
                    FkUserId = SeedUserId
                });
        }
    }

   
}
