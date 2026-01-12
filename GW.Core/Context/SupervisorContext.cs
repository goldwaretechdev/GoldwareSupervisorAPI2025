using GW.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GW.Core.Context
{
    public class SupervisorContext : DbContext
    {
        private static readonly Guid SeedUserId = Guid.Parse("d3b3c29a-4e2c-4b25-b6f4-2f8ebc4a1f05");
        private static readonly DateTime SeedDate = DateTime.Parse("2025-12-11T09:30:00Z");
        private static readonly int SeedUserRoleId = 1;

        public SupervisorContext(DbContextOptions<SupervisorContext> options) : base(options) { }


        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<FOTA> FOTA { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<SoftwareVersion> SoftwareVersions { get; set; }
        public DbSet<Device> Devices { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Device>()
                .HasOne(a => a.ProductOwner)
                .WithMany(u => u.OwnerDevices)
                .HasForeignKey(a => a.FkOwnerId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Device>()
                .HasOne(a => a.MainOwner)
                .WithMany(u => u.MainOwnerDevices)
                .HasForeignKey(a => a.FkMainOwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Device>()
                .HasOne(a => a.ESP)
                .WithMany(u => u.ESPVersions)
                .HasForeignKey(a => a.FkESPId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Device>()
                .HasOne(a => a.STM)
                .WithMany(u => u.STMVersions)
                .HasForeignKey(a => a.FkSTMId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Device>()
                .HasOne(a => a.Holtek)
                .WithMany(u => u.HoltekVersions)
                .HasForeignKey(a => a.FkHoltekId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Device>()
                .HasOne(a => a.UserRoles)
                .WithMany(u => u.Devices)
                .HasForeignKey(a => a.FkUserRoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FOTA>()
                .HasOne(a => a.ProductOwner)
                .WithMany(u => u.OwnerFOTAs)
                .HasForeignKey(a => a.FkOwnerId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<FOTA>()
                .HasOne(a => a.MainOwner)
                .WithMany(u => u.MainOwnerFOTAs)
                .HasForeignKey(a => a.FkMainOwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FOTA>()
                .HasOne(a => a.ESP)
                .WithMany(u => u.FOTAESPVersions)
                .HasForeignKey(a => a.FkESPId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FOTA>()
                .HasOne(a => a.STM)
                .WithMany(u => u.FOTASTMVersions)
                .HasForeignKey(a => a.FkSTMId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FOTA>()
                .HasOne(a => a.Holtek)
                .WithMany(u => u.FOTAHoltekVersions)
                .HasForeignKey(a => a.FkHoltekId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FOTA>()
                .HasOne(a => a.UserRoles)
                .WithMany(u => u.FOTAs)
                .HasForeignKey(a => a.FkUserRoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SoftwareVersion>()
                .HasOne(a => a.UserRoles)
                .WithMany(u => u.SoftwareVersions)
                .HasForeignKey(a => a.FkUserRoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Log>()
                .HasOne(a => a.Device)
                .WithMany(u => u.Logs)
                .HasForeignKey(a => a.FkDeviceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Log>()
                .HasOne(a => a.UserRole)
                .WithMany(u => u.Logs)
                .HasForeignKey(a => a.FkUserRoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserRoles>()
                .HasOne(a => a.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(a => a.FkUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>()
                .HasOne(a => a.Company)
                .WithMany(u => u.Users)
                .HasForeignKey(a => a.FkCompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserRoles>()
                .HasOne(a => a.Role)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(a => a.FkRoleId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);

            SeedUser(builder);
        }

        private void SeedUser(ModelBuilder builder)
        {


            builder.Entity<Company>().HasData(
                new Company()
                {
                    Id = 1,
                    Name = "Goldware",
                    ShortName = "GW",
                    Charge = 100_000_000,
                    CreatedOn = SeedDate,
                }, new Company()
                {
                    Id = 2,
                    Name = "ASA Service",
                    ShortName = "ASA",
                    Charge = 3,
                    CreatedOn = SeedDate,
                }
            );

            builder.Entity<User>().HasData(
                new User
                {
                    Id = SeedUserId,
                    Username = "sdh",
                    Password = "$2a$13$IJx6qkqyuUqmuM7NLKZDM.V1SnroBT0ICRHtcaS1AwYkr6z4p1xr6",
                    FName = "s",
                    LName = "hasanabadi",
                    Mobile = "09155909973",
                    FkCompanyId = 1
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
                    Id = SeedUserRoleId,
                    FkRoleId = 1,
                    FkUserId = SeedUserId
                });

            //builder.Entity<SoftwareVersion>().HasData(
            //    new SoftwareVersion()
            //    {
            //        Id = 1,
            //        Version = "ESP01",
            //        Path = "",
            //        MicroType = MicroType.es,
            //        Category = ProductCategory.Lena,
            //        DeviceType = DeviceType.SWITCH,
            //        FkUserRoleId = SeedUserRoleId,
            //        DateTime = SeedDate,
            //    },
            //    new SoftwareVersion()
            //    {
            //        Id = 2,
            //        Version = "ESP02",
            //        Path = "",
            //        MicroType = MicroType.ESP,
            //        Category = ProductCategory.Lena,
            //        DeviceType = DeviceType.SWITCH,
            //        FkUserRoleId = SeedUserRoleId,
            //        DateTime= SeedDate,
            //    },
            //    new SoftwareVersion()
            //    {
            //        Id = 3,
            //        Version = "HT01",
            //        Path = "",
            //        MicroType = MicroType.Holtek,
            //        Category = ProductCategory.Lena,
            //        DeviceType = DeviceType.SWITCH,
            //        FkUserRoleId = SeedUserRoleId,
            //        DateTime = SeedDate,
            //    },
            //    new SoftwareVersion()
            //    {
            //        Id = 4,
            //        Version = "HT02",
            //        Path = "",
            //        MicroType = MicroType.Holtek,
            //        Category = ProductCategory.Lena,
            //        DeviceType = DeviceType.SWITCH,
            //        FkUserRoleId = SeedUserRoleId,
            //        DateTime = SeedDate,
            //    },
            //    new SoftwareVersion()
            //    {
            //        Id = 5,
            //        Version = "STM01",
            //        Path = "",
            //        MicroType = MicroType.STM,
            //        Category = ProductCategory.Lena,
            //        DeviceType = DeviceType.SWITCH,
            //        FkUserRoleId = SeedUserRoleId,
            //        DateTime = SeedDate,
            //    },
            //    new SoftwareVersion()
            //    {
            //        Id = 6,
            //        Version = "STM02",
            //        Path = "",
            //        MicroType = MicroType.STM,
            //        Category = ProductCategory.Lena,
            //        DeviceType = DeviceType.SWITCH,
            //        FkUserRoleId = SeedUserRoleId,
            //        DateTime = SeedDate,
            //    }
            //);
        }
    }


}
