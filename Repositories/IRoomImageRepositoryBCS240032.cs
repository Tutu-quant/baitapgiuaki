using Lesson3_CNLTWeb.Models;

namespace Lesson3_CNLTWeb.Repositories
{
    public interface IRoomImageRepositoryBCS240032
    {
        Task<RoomImageBCS240032?> GetByIdAsync(int id);
        Task<List<RoomImageBCS240032>> GetByRoomIdAsync(int roomId);
        Task<RoomImageBCS240032> AddAsync(RoomImageBCS240032 roomImage);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByRoomIdAsync(int roomId);
        Task SetThumbnailAsync(int imageId);
        Task<RoomImageBCS240032?> GetThumbnailByRoomIdAsync(int roomId);
        Task<bool> ExistsAsync(int id);
    }
}
