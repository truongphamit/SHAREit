using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SHAREit.Models;
using SHAREit.Repositories;

namespace SHAREit.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class BookcaseController : Controller
    {
        private readonly IBookcaseRepository _bookcaseRepository;
        private readonly IBookRepository _bookRepository;

        private readonly IUserRepository _userRepository;

        private readonly IBorrowRepository _borrowRepository;

        public BookcaseController(IBookcaseRepository repository, IBookRepository _bookRepository, IUserRepository _userRepository, IBorrowRepository _borrowRepository)
        {
            this._bookcaseRepository = repository;
            this._bookRepository = _bookRepository;
            this._userRepository = _userRepository;
            this._borrowRepository = _borrowRepository;
        } 

        [HttpGet("add")]
        public async Task<IActionResult> add(string sku) 
        {
            try {
                Book book = await _bookRepository.FindBySKU(sku);
                if (book == null) return NotFound(new Respone(404, "Book not found", null));

                var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                 User user = await _userRepository.FindByUsername(username);
                 if (user == null) {
                    return NotFound(new Respone(404, "Not Found", null));
                 }

                 Bookcase bookcase = new Bookcase 
                 {
                     book_id = book.book_id,
                     user_id = user.user_id
                 };

                await _bookcaseRepository.Add(bookcase);

                return Ok(new Respone(200, "ok", null));
            } catch (Exception e) {
                return BadRequest(new Respone(400, "Failed", null));
            }
        }

        [HttpGet("delete")]
        public async Task<IActionResult> delete(int bookcase_id)
        {
            try {
                Bookcase bookcase = await _bookcaseRepository.Get(bookcase_id);
                if (bookcase == null ) return NotFound(new Respone(404, "Bookcase not found", null));

                var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                 User user = await _userRepository.FindByUsername(username);
                 if (user == null) {
                    return NotFound(new Respone(404, "NotFound", null));
                 }

                 if (bookcase.user_id == user.user_id) {
                     await _bookcaseRepository.Delete(bookcase_id);
                 }

                return Ok(new Respone(200, "ok", null));
            } catch (Exception e) {
                return BadRequest(new Respone(400, "Failed", null));
            }
        } 

        [HttpGet("get")]
        public async Task<IActionResult> get(int bookcase_id) 
        {
            try {
                Bookcase bookcase = await _bookcaseRepository.Get(bookcase_id);
                if (bookcase == null ) return NotFound(new Respone(404, "Bookcase not found", null));

                var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                 User user = await _userRepository.FindByUsername(username);
                 if (user == null) {
                    return NotFound(new Respone(404, "Not Found", null));
                 }

                 if (bookcase.user_id != user.user_id) {
                     return BadRequest(new Respone(400, "Failed", null));
                 }

                 Book book = await _bookRepository.Get(bookcase.book_id);
                 var dict = new Dictionary<String, object>();
                dict.Add("bookcase_id", bookcase.bookcase_id);
                dict.Add("book", book);

                return Ok(new Respone(200, "ok", dict));
            } catch (Exception e) {
                return BadRequest(new Respone(400, "Failed", null));
            }
        }

        [HttpGet("list")]
        public async Task<IActionResult> getAll()
        {
            try {
                var bookcases = await _bookcaseRepository.getAll();
                var result = new List<BookcaseRespone>();
                foreach (Bookcase bc in bookcases)
                {
                    BookcaseRespone bookcaseRespone = new BookcaseRespone
                    {
                        bookcase_id = bc.bookcase_id,
                        book = await _bookRepository.Get(bc.book_id)
                    };
                    result.Add(bookcaseRespone);
                }
                return Ok(new Respone(200, "ok", result));
            } catch (Exception e) {
                return BadRequest(new Respone(400, "Failed", null));
            }
        }

        [HttpGet("findbookbyusername")]
        public async Task<IActionResult> findBookByUsername(string username)
        {
            try {
                
                return Ok(new Respone(200, "ok", await _bookcaseRepository.FindByUsername(username)));
            } catch (Exception e) {
                return BadRequest(new Respone(400, "Failed", null)); 
            }
        }

        [HttpGet("borrowbook")]
        public async Task<IActionResult> getBorrowBooks() {
            try {
                
                return Ok(new Respone(200, "ok", null));
            } catch (Exception e) {
                return BadRequest(new Respone(400, "Failed", null)); 
            }
        }

        [HttpGet("loanbook")]
        public async Task<IActionResult> getLoanBooks() {
            try {
                var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                 User user = await _userRepository.FindByUsername(username);
                 if (user == null) {
                    return NotFound(new Respone(404, "Not Found", null));
                 }


                return Ok(new Respone(200, "ok",await _borrowRepository.GetLoanBooks(user.user_id)));
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return BadRequest(new Respone(400, "Failed", null)); 
            }
        }
    }
}