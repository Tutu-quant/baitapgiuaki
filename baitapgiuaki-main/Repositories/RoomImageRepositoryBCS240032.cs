using Lesson3_CNLTWeb.Data;
using Lesson3_CNLTWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace Lesson3_CNLTWeb.Repositories
{
    public class RoomImageRepositoryBCS240032 : IRoomImageRepositoryBCS240032
    {
        private readonly RoomManagementDbContextBCS240032 _context;

        public RoomImageRepositoryBCS240032(RoomManagementDbContextBCS240032 context)
        {
            _context = context;
        }

        public async Task<RoomImageBCS240032?> GetByIdAsync(int id)
        {
            return await _context.RoomImages.FirstOrDefaultAsync(ri => ri.Id == id);
        }

        public async Task<List<RoomImageBCS240032>> GetByRoomIdAsync(int roomId)
        {
            return await _context.RoomImages
                .Where(ri => ri.RoomId == roomId)
                .OrderByDescending(ri => ri.IsThumbnail)
                .ThenBy(ri => ri.Id)
                .ToListAsync();
        }

        public async Task<RoomImageBCS240032> AddAsync(RoomImageBCS240032 roomImage)
        {
            if (roomImage.IsThumbnail)
            {
                var oldThumbnails = await _context.RoomImages
                    .Where(ri => ri.RoomId == roomImage.RoomId && ri.IsThumbnail)
                    .ToListAsync();

                foreach (var oldThumbnail in oldThumbnails)
                {
                    oldThumbnail.IsThumbnail = false;
                }

                await _context.SaveChangesAsync();
            }

            _context.RoomImages.Add(roomImage);
            await _context.SaveChangesAsync();
            return roomImage;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var image = await _context.RoomImages.FindAsync(id);
            if (image == null) return false;

            var roomId = image.RoomId;
            var wasThumbnail = image.IsThumbnail;

            _context.RoomImages.Remove(image);
            await _context.SaveChangesAsync();

            if (wasThumbnail)
            {
                var replacement = await _context.RoomImages
                    .Where(ri => ri.RoomId == roomId)
                    .OrderBy(ri => ri.Id)
                    .FirstOrDefaultAsync();

                if (replacement != null)
                {
                    replacement.IsThumbnail = true;
                    await _context.SaveChangesAsync();
                }
            }

            return true;
        }

        public async Task<bool> DeleteByRoomIdAsync(int roomId)
        {
            var images = await _context.RoomImages
                .Where(ri => ri.RoomId == roomId)
                .ToListAsync();

            if (images.Count == 0) return false;

            _context.RoomImages.RemoveRange(images);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<RoomImageBCS240032?> GetThumbnailByRoomIdAsync(int roomId)
        {
            return await _context.RoomImages
                .FirstOrDefaultAsync(ri => ri.RoomId == roomId && ri.IsThumbnail);
        }

        public async Task SetThumbnailAsync(int imageId)
        {
            var image = await _context.RoomImages.FindAsync(imageId);
            if (image == null) return;

            var roomImages = await _context.RoomImages
                .Where(ri => ri.RoomId == image.RoomId)
                .ToListAsync();

            foreach (var roomImage in roomImages)
            {
                roomImage.IsThumbnail = false;
            }

            await _context.SaveChangesAsync();

            image.IsThumbnail = true;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.RoomImages.AnyAsync(ri => ri.Id == id);
        }
    }
}
