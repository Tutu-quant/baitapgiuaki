using Lesson3_CNLTWeb.Data;
using Lesson3_CNLTWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace Lesson3_CNLTWeb.Repositories
{
    public class RoomRepositoryBCS240032 : IRoomRepositoryBCS240032
    {
        private readonly RoomManagementDbContextBCS240032 _context;

        public RoomRepositoryBCS240032(RoomManagementDbContextBCS240032 context)
        {
            _context = context;
        }

        public async Task<RoomBCS240032?> GetByIdAsync(int id)
        {
            return await _context.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.RoomImages)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<RoomBCS240032>> GetAllAsync()
        {
            return await _context.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.RoomImages)
                .OrderBy(r => r.Name)
                .ToListAsync();
        }

        public async Task<RoomBCS240032> AddAsync(RoomBCS240032 room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<RoomBCS240032> UpdateAsync(RoomBCS240032 room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return false;

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Rooms.AnyAsync(r => r.Id == id);
        }

        public async Task<(List<RoomBCS240032> Items, int Total)> SearchAndFilterAsync(
            string? searchName = null,
            int? roomTypeId = null,
            bool? isAvailable = null,
            decimal? maxPrice = null,
            string? sortBy = "default",
            int page = 1,
            int pageSize = 10)
        {
            // Build query
            var query = _context.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.RoomImages)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(searchName))
            {
                searchName = searchName.ToLower();
                query = query.Where(r => r.Name.ToLower().Contains(searchName));
            }

            if (roomTypeId.HasValue && roomTypeId > 0)
            {
                query = query.Where(r => r.RoomTypeId == roomTypeId);
            }

            if (isAvailable.HasValue)
            {
                query = query.Where(r => r.IsAvailable == isAvailable);
            }

            if (maxPrice.HasValue && maxPrice > 0)
            {
                query = query.Where(r => r.Price <= maxPrice);
            }

            // Get total count before pagination
            int total = await query.CountAsync();

            // Apply sorting
            query = sortBy?.ToLower() switch
            {
                "price_asc" => query.OrderBy(r => r.Price),
                "price_desc" => query.OrderByDescending(r => r.Price),
                "area_desc" => query.OrderByDescending(r => r.Area),
                _ => query.OrderBy(r => r.Name) // default
            };

            // Apply pagination
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }
    }
}
