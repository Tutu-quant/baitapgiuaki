using Lesson3_CNLTWeb.Models;

namespace Lesson3_CNLTWeb.Repositories
{
    public interface IRoomRepositoryBCS240032
    {
        // Basic CRUD
        Task<RoomBCS240032?> GetByIdAsync(int id);
        Task<List<RoomBCS240032>> GetAllAsync();
        Task<RoomBCS240032> AddAsync(RoomBCS240032 room);
        Task<RoomBCS240032> UpdateAsync(RoomBCS240032 room);
        Task<bool> DeleteAsync(int id);

        // Advanced search and filter
        Task<(List<RoomBCS240032> Items, int Total)> SearchAndFilterAsync(
            string? searchName = null,
            int? roomTypeId = null,
            bool? isAvailable = null,
            decimal? maxPrice = null,
            string? sortBy = "default",
            int page = 1,
            int pageSize = 10);

        // Check if room exists
        Task<bool> ExistsAsync(int id);
    }
}
