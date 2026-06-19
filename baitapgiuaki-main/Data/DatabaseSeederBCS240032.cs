using Lesson3_CNLTWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace Lesson3_CNLTWeb.Data
{
    public static class DatabaseSeederBCS240032
    {
        private static readonly Dictionary<string, string[]> DemoImageUrlsByRoomName = new()
        {
            ["A101"] =
            [
                "https://cdn3.ivivu.com/2014/01/SUPER-DELUXE2.jpg",
                "https://cdn3.ivivu.com/2014/01/Khach-san-Midtown-Hue-4-Sao-Lobby.jpg"
            ],
            ["A202"] =
            [
                "https://cdn3.ivivu.com/2014/01/20762698_images1477905_6.jpg"
            ],
            ["S301"] =
            [
                "https://cdn3.ivivu.com/2014/01/be-boi-horison-hoi-nghi-khach-hang.jpg"
            ],
            ["G102"] =
            [
                "https://cdn3.ivivu.com/2014/01/ngoai-canh-khach-san-ha-an.jpg"
            ],
            ["S402"] =
            [
                "https://cdn3.ivivu.com/2014/01/diemhencafe1.jpg"
            ]
        };

        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            await SeedRoomDataAsync(scope.ServiceProvider.GetRequiredService<RoomManagementDbContextBCS240032>());
            await SeedBookDataAsync(scope.ServiceProvider.GetRequiredService<BookDbContext>());
        }

        private static async Task SeedRoomDataAsync(RoomManagementDbContextBCS240032 context)
        {
            await context.Database.EnsureCreatedAsync();

            if (await context.Rooms.AnyAsync())
            {
                await EnsureIvivuDemoImagesAsync(context);
                return;
            }

            var standard = await EnsureRoomTypeAsync(
                context,
                "Phòng tiêu chuẩn",
                "Phòng khép kín, phù hợp sinh viên hoặc người đi làm.");

            var studio = await EnsureRoomTypeAsync(
                context,
                "Studio",
                "Phòng rộng hơn, có khu bếp và không gian sinh hoạt riêng.");

            var shared = await EnsureRoomTypeAsync(
                context,
                "Phòng gác lửng",
                "Phòng có gác, tối ưu diện tích sử dụng.");

            var rooms = new List<RoomBCS240032>
            {
                new()
                {
                    Name = "A101",
                    RoomTypeId = standard.Id,
                    Price = 2500000,
                    Area = 20,
                    IsAvailable = true,
                    Description = "Gần cổng chính, có cửa sổ và nhà vệ sinh riêng."
                },
                new()
                {
                    Name = "A202",
                    RoomTypeId = standard.Id,
                    Price = 2800000,
                    Area = 22,
                    IsAvailable = true,
                    Description = "Tầng 2 thoáng mát, phù hợp một đến hai người."
                },
                new()
                {
                    Name = "S301",
                    RoomTypeId = studio.Id,
                    Price = 4200000,
                    Area = 32,
                    IsAvailable = false,
                    Description = "Studio có bếp nhỏ, ban công và nhiều ánh sáng."
                },
                new()
                {
                    Name = "G102",
                    RoomTypeId = shared.Id,
                    Price = 3300000,
                    Area = 27,
                    IsAvailable = true,
                    Description = "Phòng gác lửng, có chỗ để xe riêng."
                },
                new()
                {
                    Name = "S402",
                    RoomTypeId = studio.Id,
                    Price = 4600000,
                    Area = 35,
                    IsAvailable = true,
                    Description = "Studio góc, diện tích rộng, có khu phơi đồ."
                }
            };

            context.Rooms.AddRange(rooms);
            await context.SaveChangesAsync();

            await EnsureIvivuDemoImagesAsync(context);
        }

        private static async Task EnsureIvivuDemoImagesAsync(RoomManagementDbContextBCS240032 context)
        {
            var rooms = await context.Rooms
                .Include(room => room.RoomImages)
                .ToListAsync();

            foreach (var room in rooms)
            {
                if (!DemoImageUrlsByRoomName.TryGetValue(room.Name, out var imageUrls))
                {
                    continue;
                }

                var oldDemoImages = room.RoomImages
                    .Where(image => image.ImageUrl.Contains("images.unsplash.com"))
                    .ToList();

                if (oldDemoImages.Count > 0)
                {
                    context.RoomImages.RemoveRange(oldDemoImages);
                    await context.SaveChangesAsync();
                }

                var currentImages = await context.RoomImages
                    .Where(image => image.RoomId == room.Id)
                    .ToListAsync();

                var currentUrls = currentImages
                    .Select(image => image.ImageUrl)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                var hasThumbnail = currentImages.Any(image => image.IsThumbnail);

                foreach (var imageUrl in imageUrls)
                {
                    if (currentUrls.Contains(imageUrl))
                    {
                        continue;
                    }

                    context.RoomImages.Add(new RoomImageBCS240032
                    {
                        RoomId = room.Id,
                        ImageUrl = imageUrl,
                        IsThumbnail = !hasThumbnail
                    });

                    hasThumbnail = true;
                }
            }

            await context.SaveChangesAsync();
        }

        private static async Task<RoomTypeBCS240032> EnsureRoomTypeAsync(
            RoomManagementDbContextBCS240032 context,
            string name,
            string description)
        {
            var existing = await context.RoomTypes.FirstOrDefaultAsync(roomType => roomType.Name == name);
            if (existing != null)
            {
                return existing;
            }

            var roomType = new RoomTypeBCS240032
            {
                Name = name,
                Description = description
            };

            context.RoomTypes.Add(roomType);
            await context.SaveChangesAsync();

            return roomType;
        }

        private static async Task SeedBookDataAsync(BookDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (await context.Books.AnyAsync())
            {
                return;
            }

            context.Books.AddRange(
                new Book { Name = "ASP.NET Core MVC", Price = 120000 },
                new Book { Name = "Entity Framework Core", Price = 150000 });

            await context.SaveChangesAsync();
        }
    }
}
