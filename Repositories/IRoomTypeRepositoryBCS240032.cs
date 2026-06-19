using Lesson3_CNLTWeb.Models;

namespace Lesson3_CNLTWeb.Repositories
{
    public interface IRoomTypeRepositoryBCS240032
    {
        Task<RoomTypeBCS240032?> GetByIdAsync(int id);
        Task<List<RoomTypeBCS240032>> GetAllAsync();
        Task<RoomTypeBCS240032> AddAsync(RoomTypeBCS240032 roomType);
        Task<RoomTypeBCS240032> UpdateAsync(RoomTypeBCS240032 roomType);
        Task<bool> DeleteAsync(int id);
        Task<bool> HasRoomsAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
