using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SHAREit.Models;
using SHAREit.Repositories;

namespace SHAREit.Controllers
{

    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private readonly IBookRepository _book;

        public BookController(IBookRepository book)
        {
            _book = book;
        }     

        /// <summary>
        /// Lấy toàn bộ thông tin sách
        /// </summary>
        [Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> getAll()
        {
            //var book = new Book {sku = "2426566471616", title = "Ngày Xưa Có Một Con Bò...", sub_description = "sub_description",
              //  description = "description", author = "Camilo Cruz", company = "Nhà Xuất Bản Trẻ", image = ""};
           // await _book.Add(book);
           try {
                var books = await _book.getAll();
                return Ok(new Respone(200, "ok", books));
           } catch (Exception e) {
               return BadRequest(new Respone(400, "failed", null));
           }
        }

        /// <summary>
        /// Thêm sách vào kho
        /// </summary>
        [Authorize(Roles = "admin")]
        [HttpPost("add")]
        public async Task<IActionResult> add([FromBody] Book book)
        {
            try {
                await _book.Add(book);
                return Ok(new Respone(200, "ok", null));
            } catch (Exception e) {
                return BadRequest(new Respone(400, "", null));
            }
        }

        /// <summary>
        /// Chỉnh sửa thông tin sách
        /// </summary>
        [Authorize(Roles = "admin")]
        [HttpPut("update")]
        public async Task<IActionResult> update(int book_id, string sku, string title, string sub_description, string description, string author, string company, string image) {
            try {
                var book = await _book.Get(book_id);
                if (book == null) return NotFound(new Respone(404, "Not Found", null));
                book.sku = book.sku != null ? sku : book.sku;
                book.title = book.title != null ? title : book.title;
                book.sub_description = book.sub_description != null ? sub_description : book.sub_description;
                book.description = book.description != null ? description : book.description;
                book.author = book.author != null ? author : book.author;
                book.company = book.company != null ? company : book.company;
                book.image = book.image != null ? image : book.image;
                await _book.Update(book_id, book);
                return Ok(new Respone(200, "ok", null));
            } catch(Exception e) {
                return BadRequest(new Respone(400, "Failed", null));
            } 
        }

        /// <summary>
        /// Xóa sách
        /// </summary>
        [Authorize(Roles = "admin")]
        [HttpDelete("delete")]
        public async Task<IActionResult> delete(int book_id) {
            try {
                var book = await _book.Get(book_id);
                if (book == null) NotFound(new Respone(404, "Not Found", null));
                await _book.Delete(book_id);
                return Ok(new Respone(200, "ok", null));
            } catch (Exception e) {
                return BadRequest(new Respone(400, "", null));
            }
        }

        /// <summary>
        /// Lấy thông tin sách
        /// </summary>
        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> get(int book_id) {
            try {
                var book = await _book.Get(book_id);
                if (book == null) return NotFound(new Respone(404, "Not Found", null));
                return Ok(new Respone(200, "ok", book));
            } catch (Exception e) {
                return BadRequest(new Respone(400, "", null));
            }
        }

        /// <summary>
        /// Lấy thông tin sách theo SKU
        /// </summary>
        [Authorize]
        [HttpGet("findbysku")]
        public async Task<IActionResult> findBySKU(string sku) {
            try {
                var book = await _book.FindBySKU(sku);
                if (book == null) return NotFound(new Respone(404, "Not Found", null));
                return Ok(new Respone(200, "ok", book));
            } catch (Exception e) {
                return BadRequest(new Respone(400, "", null));
            }
        }

        /// <summary>
        /// Lấy toàn bộ sách của user
        /// </summary>
        [Authorize]
        [HttpGet("finduserbybook")]
        public async Task<IActionResult> findUserByBookID(int book_id) {
            try {
                
                return Ok(new Respone(200, "ok", await _book.FindUser(book_id)));
            } catch (Exception e) {
                return BadRequest(new Respone(400, "", null));
            }
        }

        /// <summary>
        /// Tìm user có sách với SKU
        /// </summary>
        [Authorize]
        [HttpGet("finduserbysku")]
        public async Task<IActionResult> findUserByBookSKU(string sku) {
            try {
                
                return Ok(new Respone(200, "ok", await _book.FindUser(sku)));
            } catch (Exception e) {
                return BadRequest(new Respone(400, "", null));
            }
        }

        /// <summary>
        /// Tìm kiếm sách
        /// </summary>
        [Authorize]
        [HttpGet("search")]
        public async Task<IActionResult> search(string title, int limit, int page) {
            try {
                List<Book> books = await _book.Search(title, limit, page);
                return Ok(new Respone(200, "ok", books));
            } catch (Exception e) {
                return BadRequest(new Respone(400, "", null));
            }
        }
    }
}