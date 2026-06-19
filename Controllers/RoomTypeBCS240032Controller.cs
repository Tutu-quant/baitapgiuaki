using Lesson3_CNLTWeb.Models;
using Lesson3_CNLTWeb.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lesson3_CNLTWeb.Controllers
{
    public class RoomTypeBCS240032Controller : Controller
    {
        private readonly IRoomTypeRepositoryBCS240032 _roomTypeRepository;

        public RoomTypeBCS240032Controller(IRoomTypeRepositoryBCS240032 roomTypeRepository)
        {
            _roomTypeRepository = roomTypeRepository;
        }

        // GET: RoomTypeBCS240032
        public async Task<IActionResult> Index()
        {
            try
            {
                var roomTypes = await _roomTypeRepository.GetAllAsync();
                return View(roomTypes);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tải danh sách loại phòng: {ex.Message}";
                return View(new List<RoomTypeBCS240032>());
            }
        }

        // GET: RoomTypeBCS240032/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RoomTypeBCS240032/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomTypeBCS240032 roomType)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(roomType);
                }

                await _roomTypeRepository.AddAsync(roomType);
                TempData["SuccessMessage"] = "Thêm loại phòng thành công";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi thêm loại phòng: {ex.Message}";
                return View(roomType);
            }
        }

        // GET: RoomTypeBCS240032/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                TempData["ErrorMessage"] = "ID loại phòng không hợp lệ";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var roomType = await _roomTypeRepository.GetByIdAsync(id.Value);
                if (roomType == null)
                {
                    TempData["ErrorMessage"] = $"Không tìm thấy loại phòng với ID: {id}";
                    return RedirectToAction(nameof(Index));
                }

                return View(roomType);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tải dữ liệu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: RoomTypeBCS240032/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RoomTypeBCS240032 roomType)
        {
            if (id != roomType.Id)
            {
                TempData["ErrorMessage"] = "ID loại phòng không khớp";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(roomType);
                }

                await _roomTypeRepository.UpdateAsync(roomType);
                TempData["SuccessMessage"] = "Cập nhật loại phòng thành công";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi cập nhật: {ex.Message}";
                return View(roomType);
            }
        }

        // GET: RoomTypeBCS240032/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue)
            {
                TempData["ErrorMessage"] = "ID loại phòng không hợp lệ";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var roomType = await _roomTypeRepository.GetByIdAsync(id.Value);
                if (roomType == null)
                {
                    TempData["ErrorMessage"] = $"Không tìm thấy loại phòng với ID: {id}";
                    return RedirectToAction(nameof(Index));
                }

                // Check if this room type has rooms
                if (await _roomTypeRepository.HasRoomsAsync(id.Value))
                {
                    TempData["ErrorMessage"] = "Không thể xóa loại phòng này vì vẫn còn phòng sử dụng";
                    return RedirectToAction(nameof(Index));
                }

                return View(roomType);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tải dữ liệu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: RoomTypeBCS240032/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // Check if this room type has rooms
                if (await _roomTypeRepository.HasRoomsAsync(id))
                {
                    TempData["ErrorMessage"] = "Không thể xóa loại phòng này vì vẫn còn phòng sử dụng";
                    return RedirectToAction(nameof(Index));
                }

                var success = await _roomTypeRepository.DeleteAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Xóa loại phòng thành công";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Không tìm thấy loại phòng với ID: {id}";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi xóa: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
