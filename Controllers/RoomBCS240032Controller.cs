using Lesson3_CNLTWeb.Models;
using Lesson3_CNLTWeb.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lesson3_CNLTWeb.Controllers
{
    public class RoomBCS240032Controller : Controller
    {
        private readonly IRoomRepositoryBCS240032 _roomRepository;
        private readonly IRoomTypeRepositoryBCS240032 _roomTypeRepository;
        private readonly IRoomImageRepositoryBCS240032 _roomImageRepository;

        public RoomBCS240032Controller(
            IRoomRepositoryBCS240032 roomRepository,
            IRoomTypeRepositoryBCS240032 roomTypeRepository,
            IRoomImageRepositoryBCS240032 roomImageRepository)
        {
            _roomRepository = roomRepository;
            _roomTypeRepository = roomTypeRepository;
            _roomImageRepository = roomImageRepository;
        }

        // GET: RoomBCS240032
        public async Task<IActionResult> Index(
            string? searchName,
            int? roomTypeId,
            bool? isAvailable,
            decimal? maxPrice,
            string? sortBy,
            int page = 1)
        {
            const int pageSize = 10;

            try
            {
                var (rooms, total) = await _roomRepository.SearchAndFilterAsync(
                    searchName: searchName,
                    roomTypeId: roomTypeId,
                    isAvailable: isAvailable,
                    maxPrice: maxPrice,
                    sortBy: sortBy,
                    page: page,
                    pageSize: pageSize);

                // Pass search parameters to view to keep them after search
                ViewBag.SearchName = searchName;
                ViewBag.RoomTypeId = roomTypeId;
                ViewBag.IsAvailable = isAvailable;
                ViewBag.MaxPrice = maxPrice;
                ViewBag.SortBy = sortBy;
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling((double)total / pageSize);
                ViewBag.TotalItems = total;

                // Populate RoomType dropdown
                ViewBag.RoomTypes = new SelectList(
                    await _roomTypeRepository.GetAllAsync(),
                    "Id", "Name", roomTypeId);

                return View(rooms);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tải danh sách phòng: {ex.Message}";
                ViewBag.SearchName = searchName;
                ViewBag.RoomTypeId = roomTypeId;
                ViewBag.IsAvailable = isAvailable;
                ViewBag.MaxPrice = maxPrice;
                ViewBag.SortBy = sortBy;
                ViewBag.CurrentPage = 1;
                ViewBag.TotalPages = 0;
                ViewBag.TotalItems = 0;
                try
                {
                    ViewBag.RoomTypes = new SelectList(
                        await _roomTypeRepository.GetAllAsync(), "Id", "Name", roomTypeId);
                }
                catch
                {
                    ViewBag.RoomTypes = new SelectList(
                        new List<RoomTypeBCS240032>(), "Id", "Name", roomTypeId);
                }
                return View(new List<RoomBCS240032>());
            }
        }

        // GET: RoomBCS240032/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
            {
                TempData["ErrorMessage"] = "ID phòng không hợp lệ";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var room = await _roomRepository.GetByIdAsync(id.Value);
                if (room == null)
                {
                    TempData["ErrorMessage"] = $"Không tìm thấy phòng với ID: {id}";
                    return RedirectToAction(nameof(Index));
                }

                if (!room.RoomImages.Any())
                {
                    TempData["WarningMessage"] = "Phòng này chưa có ảnh";
                }

                return View(room);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tải chi tiết phòng: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: RoomBCS240032/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var roomTypes = await _roomTypeRepository.GetAllAsync();
                ViewBag.RoomTypes = new SelectList(roomTypes, "Id", "Name");
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tải dữ liệu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: RoomBCS240032/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomBCS240032 room)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var roomTypes = await _roomTypeRepository.GetAllAsync();
                    ViewBag.RoomTypes = new SelectList(roomTypes, "Id", "Name", room.RoomTypeId);
                    return View(room);
                }

                // Check if RoomType exists
                if (!await _roomTypeRepository.ExistsAsync(room.RoomTypeId))
                {
                    TempData["ErrorMessage"] = $"Loại phòng ID {room.RoomTypeId} không tồn tại";
                    var roomTypes = await _roomTypeRepository.GetAllAsync();
                    ViewBag.RoomTypes = new SelectList(roomTypes, "Id", "Name", room.RoomTypeId);
                    return View(room);
                }

                await _roomRepository.AddAsync(room);

                TempData["SuccessMessage"] = "Thêm phòng thành công";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "Tên phòng đã tồn tại trong loại phòng này";
                var roomTypes = await _roomTypeRepository.GetAllAsync();
                ViewBag.RoomTypes = new SelectList(roomTypes, "Id", "Name", room.RoomTypeId);
                return View(room);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi thêm phòng: {ex.Message}";
                var roomTypes = await _roomTypeRepository.GetAllAsync();
                ViewBag.RoomTypes = new SelectList(roomTypes, "Id", "Name", room.RoomTypeId);
                return View(room);
            }
        }

        // GET: RoomBCS240032/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                TempData["ErrorMessage"] = "ID phòng không hợp lệ";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var room = await _roomRepository.GetByIdAsync(id.Value);
                if (room == null)
                {
                    TempData["ErrorMessage"] = $"Không tìm thấy phòng với ID: {id}";
                    return RedirectToAction(nameof(Index));
                }

                var roomTypes = await _roomTypeRepository.GetAllAsync();
                ViewBag.RoomTypes = new SelectList(roomTypes, "Id", "Name", room.RoomTypeId);

                return View(room);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tải dữ liệu phòng: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: RoomBCS240032/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RoomBCS240032 room)
        {
            if (id != room.Id)
            {
                TempData["ErrorMessage"] = "ID phòng không khớp";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    var roomTypes = await _roomTypeRepository.GetAllAsync();
                    ViewBag.RoomTypes = new SelectList(roomTypes, "Id", "Name", room.RoomTypeId);
                    return View(room);
                }

                // Check if RoomType exists
                if (!await _roomTypeRepository.ExistsAsync(room.RoomTypeId))
                {
                    TempData["ErrorMessage"] = $"Loại phòng ID {room.RoomTypeId} không tồn tại";
                    var roomTypes = await _roomTypeRepository.GetAllAsync();
                    ViewBag.RoomTypes = new SelectList(roomTypes, "Id", "Name", room.RoomTypeId);
                    return View(room);
                }

                var existingRoom = await _roomRepository.GetByIdAsync(id);
                if (existingRoom == null)
                {
                    TempData["ErrorMessage"] = $"Không tìm thấy phòng với ID: {id}";
                    return RedirectToAction(nameof(Index));
                }

                existingRoom.Name = room.Name;
                existingRoom.RoomTypeId = room.RoomTypeId;
                existingRoom.Price = room.Price;
                existingRoom.Area = room.Area;
                existingRoom.Description = room.Description;
                existingRoom.IsAvailable = room.IsAvailable;

                await _roomRepository.UpdateAsync(existingRoom);
                TempData["SuccessMessage"] = "Cập nhật phòng thành công";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "Tên phòng đã tồn tại trong loại phòng này";
                var roomTypes = await _roomTypeRepository.GetAllAsync();
                ViewBag.RoomTypes = new SelectList(roomTypes, "Id", "Name", room.RoomTypeId);
                return View(room);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi cập nhật phòng: {ex.Message}";
                var roomTypes = await _roomTypeRepository.GetAllAsync();
                ViewBag.RoomTypes = new SelectList(roomTypes, "Id", "Name", room.RoomTypeId);
                return View(room);
            }
        }

        // GET: RoomBCS240032/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue)
            {
                TempData["ErrorMessage"] = "ID phòng không hợp lệ";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var room = await _roomRepository.GetByIdAsync(id.Value);
                if (room == null)
                {
                    TempData["ErrorMessage"] = $"Không tìm thấy phòng với ID: {id}";
                    return RedirectToAction(nameof(Index));
                }

                return View(room);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tải dữ liệu phòng: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: RoomBCS240032/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var success = await _roomRepository.DeleteAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Xóa phòng thành công";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Không tìm thấy phòng với ID: {id}";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi xóa phòng: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
