using Lesson3_CNLTWeb.Models;
using Lesson3_CNLTWeb.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Lesson3_CNLTWeb.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // READ - Danh sách sách
        public async Task<IActionResult> Index()
        {
            var books = await _bookRepository.GetAllAsync();
            return View(books);
        }

        // READ - Chi tiết sách
        public async Task<IActionResult> Detail(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // CREATE - Hiển thị form thêm
        public IActionResult Create()
        {
            return View();
        }

        // CREATE - Xử lý thêm sách
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (!ModelState.IsValid)
            {
                return View(book);
            }

            await _bookRepository.AddAsync(book);

            TempData["SuccessMessage"] = "Thêm sách thành công!";
            return RedirectToAction(nameof(Index));
        }

        // UPDATE - Hiển thị form sửa
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // UPDATE - Xử lý sửa sách
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(book);
            }

            if (!await _bookRepository.ExistsAsync(id))
            {
                return NotFound();
            }

            await _bookRepository.UpdateAsync(book);

            TempData["SuccessMessage"] = "Cập nhật sách thành công!";
            return RedirectToAction(nameof(Index));
        }

        // DELETE - Hiển thị xác nhận xóa
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // DELETE - Xử lý xóa sách
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _bookRepository.ExistsAsync(id))
            {
                await _bookRepository.DeleteAsync(id);
                TempData["SuccessMessage"] = "Xóa sách thành công!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
