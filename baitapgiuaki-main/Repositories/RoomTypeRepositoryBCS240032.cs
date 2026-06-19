using Lesson3_CNLTWeb.Data;
using Lesson3_CNLTWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace Lesson3_CNLTWeb.Repositories
{
    public class RoomTypeRepositoryBCS240032 : IRoomTypeRepositoryBCS240032
    {
        private readonly RoomManagementDbContextBCS240032 _context;

        public RoomTypeRepositoryBCS240032(RoomManagementDbContextBCS240032 context)
        {
            _context = context;
        }

        public async Task<RoomTypeBCS240032?> GetByIdAsync(int id)
        {
            return await _context.RoomTypes
                .Include(rt => rt.Rooms)
                .FirstOrDefaultAsync(rt => rt.Id == id);
        }

        public async Task<List<RoomTypeBCS240032>> GetAllAsync()
        {
            return await _context.RoomTypes
                .Include(rt => rt.Rooms)
                .OrderBy(rt => rt.Name)
                .ToListAsync();
        }

        public async Task<RoomTypeBCS240032> AddAsync(RoomTypeBCS240032 roomType)
        {
            _context.RoomTypes.Add(roomType);
            await _context.SaveChangesAsync();
            return roomType;
        }

        public async Task<RoomTypeBCS240032> UpdateAsync(RoomTypeBCS240032 roomType)
        {
            _context.RoomTypes.Update(roomType);
            await _context.SaveChangesAsync();
            return roomType;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var roomType = await _context.RoomTypes.FindAsync(id);
            if (roomType == null) return false;

            _context.RoomTypes.Remove(roomType);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasRoomsAsync(int id)
        {
            return await _context.Rooms.AnyAsync(r => r.RoomTypeId == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.RoomTypes.AnyAsync(rt => rt.Id == id);
        }
    }
}
