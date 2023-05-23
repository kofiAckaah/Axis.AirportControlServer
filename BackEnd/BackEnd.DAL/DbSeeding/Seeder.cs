using BackEnd.DAL.DbContexts;
using BackEnd.DataDomain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;

namespace BackEnd.DAL.DbSeeding
{
    public class Seeder : ISeeder
    {
        #region Constants
        
        private readonly Guid runawayId = new("E7893A3D-4A21-443C-8388-3863FD987949");

        private readonly List<Guid> largeSpotsIds = new()
        {
            new("0DF97D1F-F581-4FFB-9E53-49592419CAF8"),
            new("DD342103-2ECB-46BF-BA76-0EBB138918B7"),
            new("a18be9c0-aa65-4af8-bd17-00bd9344e575"),
            new("fbb1e820-8c55-4959-bd1c-1e4b9565b02e"),
            new("419C1707-6AA6-4FCC-AA21-7C16D075F5A0")
        };

        private readonly List<Guid> smallSpotsIds = new()
        {
            new("E9FD3C89-AB1E-4995-9A69-366BB22645A4"), new("a6a10abd-4368-43bc-a6ff-6adfab726c3d"),
            new("004a253d-0a74-4da0-8d27-ecf6696762e3"), new("cbc8b2b2-6586-4e0e-a6b9-a67e99822cee"),
            new("e4c373c1-b9cd-4072-80d7-bda75d1aa061"), new("9ba4a7a4-b239-443f-9f75-88eb12e1be6c"),
            new("3e104b6c-72bb-4286-8662-a3c2b7459962"), new("ea662ddf-eee1-4022-b2b7-1c73ae543314"),
            new("13dea30f-569f-4488-8d30-9042e2400a37"), new("C2C9143B-A512-4718-B944-B826F0B41114")
        };

        private readonly Guid adminRoleId = new("13d57046-397d-4e89-b67e-e2ae9ae8d211");
        private readonly Guid adminId = new("74A1A777-95A5-4916-BF24-1B05DD6F5454");

        #endregion

        private readonly ControlServerDbContext dbContext;

        public Seeder(ControlServerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Seed()
        {
            dbContext.Database.Migrate();
            SeedUser();
            SeedProjectEntities();
        }

        private void SeedUser()
        {
            AddRole(new ApplicationRole()
            {
                Id = adminRoleId,
                Name = "admin",
                NormalizedName = "admin".ToUpper(),
            });

            AddUser(new ApplicationUser
            {
                Id = adminId,
                Email = "admin@mail.com",
                UserName = "admin@mail.com",
                NormalizedUserName = "admin@mail.com".ToUpper(),
                EmailConfirmed = true,
                NormalizedEmail = "admin@mail.com".ToUpper(),
                PhoneNumberConfirmed = true,
                CreatedOn = DateTime.UtcNow,
                SecurityStamp = string.Empty,
            });

            AddUserRole(new IdentityUserRole<Guid>
            {
                RoleId = adminRoleId,
                UserId = adminId
            });
        }

        private void SeedProjectEntities()
        {
            AddRunaway(new Runaway()
            {
                IsFree = true,
                Id = runawayId
            });

            var smallSpots = new List<ParkingSpot>();
            var largeSpots = new List<ParkingSpot>();
            for (var i = 0; i < 10; i++)
            {
                smallSpots.Add(new ParkingSpot
                {
                    Id = smallSpotsIds[i],
                    IsEmpty = true,
                    IsSmall = true
                });
                if (largeSpots.Count < 5)
                    largeSpots.Add(new ParkingSpot
                    {
                        Id = largeSpotsIds[i],
                        IsEmpty = true,
                        IsSmall = false
                    });
            }
            
            foreach (var parkingSpot in smallSpots)
            {
                AddParkingSpots(parkingSpot);
            }
            foreach (var parkingSpot in largeSpots)
            {
                AddParkingSpots(parkingSpot);
            }
        }

        private void AddRole(ApplicationRole role)
        {
            if (!dbContext.Roles.Any(c => c.Id == role.Id))
            {
                dbContext.Roles.Add(role);
            }
        }

        private void AddUser(ApplicationUser user)
        {
            var passHash = new PasswordHasher<ApplicationUser>();
            if (!dbContext.Users.Any(c => c.Id == user.Id))
            {
                user.PasswordHash = passHash.HashPassword(user, user.PasswordHash ?? "1234abc");
                dbContext.Users.Add(user);
            }
        }

        private void AddUserRole(IdentityUserRole<Guid> userRole)
        {
            if (!dbContext.UserRoles.Any(c => c.RoleId == userRole.RoleId && c.UserId == userRole.UserId))
            {
                dbContext.UserRoles.Add(userRole);
            }
        }

        private async void AddRunaway(Runaway runaway)
        {
            if (!await dbContext.Runaways.AnyAsync(r => r.Id == runaway.Id))
                await dbContext.Runaways.AddAsync(runaway);
        }

        private void AddParkingSpots(ParkingSpot spot)
        {
            var exists = dbContext.ParkingSpots.ToList().Any(r => r.Id == spot.Id);
            Task.Run(async () =>
            {
                if (!exists)
                    await dbContext.ParkingSpots.AddAsync(spot);
            }).GetAwaiter().GetResult();
        }
    }
}
