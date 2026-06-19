using Lesson3_CNLTWeb.Models;
using Lesson3_CNLTWeb.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Lesson3_CNLTWeb.Controllers
{
    public class RoomImageBCS240032Controller : Controller
    {
        private readonly IRoomRepositoryBCS240032 _roomRepository;
        private readonly IRoomImageRepositoryBCS240032 _roomImageRepository;

        public RoomImageBCS240032Controller(
            IRoomRepositoryBCS240032 roomRepository,
            IRoomImageRepositoryBCS240032 roomImageRepository)
        {
            _roomRepository = roomRepository;
            _roomImageRepository = roomImageRepository;
        }

        // POST: RoomImageBCS240032/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int roomId, string imageUrl)
        {
            try
            {
                // Check if room exists
                var room = await _roomRepository.GetByIdAsync(roomId);
                if (room == null)
                {
                    TempData["ErrorMessage"] = $"Không tìm thấy phòng với ID: {roomId}";
                    return RedirectToAction("Details", "RoomBCS240032", new { id = roomId });
                }

                if (string.IsNullOrWhiteSpace(imageUrl))
                {
                    TempData["ErrorMessage"] = "Đường dẫn ảnh không được để trống";
                    return RedirectToAction("Details", "RoomBCS240032", new { id = roomId });
                }

                // If this is the first image, set it as thumbnail
                var images = await _roomImageRepository.GetByRoomIdAsync(roomId);
                bool isFirstImage = images.Count == 0;

                var roomImage = new RoomImageBCS240032
                {
                    RoomId = roomId,
                    ImageUrl = imageUrl,
                    IsThumbnail = isFirstImage
                };

                await _roomImageRepository.AddAsync(roomImage);
                TempData["SuccessMessage"] = "Thêm ảnh thành công";

                return RedirectToAction("Details", "RoomBCS240032", new { id = roomId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi thêm ảnh: {ex.Message}";
                return RedirectToAction("Details", "RoomBCS240032", new { id = roomId });
            }
        }

        // POST: RoomImageBCS240032/SetThumbnail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetThumbnail(int imageId, int roomId)
        {
            try
            {
                var image = await _roomImageRepository.GetByIdAsync(imageId);
                if (image == null)
                {
                    TempData["ErrorMessage"] = $"Không tìm thấy ảnh với ID: {imageId}";
                    return RedirectToAction("Details", "RoomBCS240032", new { id = roomId });
                }

                if (image.RoomId != roomId)
                {
                    TempData["ErrorMessage"] = "Ảnh không thuộc phòng đang chọn";
                    return RedirectToAction("Details", "RoomBCS240032", new { id = roomId });
                }

                await _roomImageRepository.SetThumbnailAsync(imageId);
                TempData["SuccessMessage"] = "Đặt ảnh đại diện thành công";

                return RedirectToAction("Details", "RoomBCS240032", new { id = roomId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi đặt ảnh đại diện: {ex.Message}";
                return RedirectToAction("Details", "RoomBCS240032", new { id = roomId });
            }
        }

        // POST: RoomImageBCS240032/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int imageId, int roomId)
        {
            try
            {
                var image = await _roomImageRepository.GetByIdAsync(imageId);
                if (image == null)
                {
                    TempData["ErrorMessage"] = $"Không tìm thấy ảnh với ID: {imageId}";
                    return RedirectToAction("Details", "RoomBCS240032", new { id = roomId });
                }

                if (image.RoomId != roomId)
                {
                    TempData["ErrorMessage"] = "Ảnh không thuộc phòng đang chọn";
                    return RedirectToAction("Details", "RoomBCS240032", new { id = roomId });
                }

                var success = await _roomImageRepository.DeleteAsync(imageId);
                if (success)
                {
                    TempData["SuccessMessage"] = "Xóa ảnh thành công";
                }
                else
                {
                    TempData["ErrorMessage"] = "Xóa ảnh thất bại";
                }

                return RedirectToAction("Details", "RoomBCS240032", new { id = roomId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi xóa ảnh: {ex.Message}";
                return RedirectToAction("Details", "RoomBCS240032", new { id = roomId });
            }
        }
    }
}
