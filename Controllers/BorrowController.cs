using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SHAREit.Models;
using SHAREit.Repositories;

namespace SHAREit.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class BorrowController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IBookcaseRepository _bookcaseRepository;   
        private readonly IBorrowRepository _borrowRepository;

        public BorrowController(IUserRepository _userRepository, IBookcaseRepository _bookcaseRepository, IBorrowRepository _borrowRepository) 
        {
            this._userRepository = _userRepository;
            this._bookcaseRepository = _bookcaseRepository;
            this._borrowRepository = _borrowRepository;
        }

        /// <summary>
        /// Mượn sách
        /// </summary>
        [HttpGet("borrow")]
        public async Task<IActionResult> borrow(String code)
        {
            try {
                code = Encoding.UTF8.GetString(Convert.FromBase64String(code));
                int bookcase_id = Convert.ToInt32(code.Substring(6));

                Borrow borrowed = await _borrowRepository.FindByBookcase(bookcase_id);
                if (borrowed != null && borrowed.return_date == null) return BadRequest(new Respone(400, "Borrowed", null));

                Bookcase bookcase = await _bookcaseRepository.Get(bookcase_id);
                if (bookcase == null) return NotFound(new Respone(404, "Not Found", null));

                var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                 User user = await _userRepository.FindByUsername(username);
                 if (user == null) {
                    return NotFound(new Respone(404, "Not Found", null));
                 }

                if (bookcase.user_id == user.user_id) return BadRequest(new Respone(400, "Failed", null));                 

                Borrow borrow = new Borrow{
                      user_id_borrow = user.user_id,
                      bookcase_id = bookcase_id
                };

                await _borrowRepository.Add(borrow);

                return Ok(new Respone(200, "ok", null));
            } catch (Exception e) {
                return BadRequest(new Respone(400, "Failed", null));
            }
        }

        /// <summary>
        /// Trả sách
        /// </summary>
        [HttpGet("return")]
        public async Task<IActionResult> returnBook(string code)
        {
            try {
                code = Encoding.UTF8.GetString(Convert.FromBase64String(code));
                int borrow_id = Convert.ToInt32(code.Substring(6));

                Borrow borrow = await _borrowRepository.Get(borrow_id);

                var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                 User user = await _userRepository.FindByUsername(username);
                 if (user == null) {
                    return NotFound(new Respone(404, "Not Found", null));
                 }

                if (user.user_id != borrow.user_id_borrow) return BadRequest(new Respone(400, "Failed", null));

                borrow.return_date = DateTime.Now;

                await _borrowRepository.Update(borrow_id, borrow);

                return Ok(new Respone(200, "ok", null));
            } catch (Exception e) {
                return BadRequest(new Respone(400, "Failed", null));
            }
        }

        /// <summary>
        /// Lấy mã cho mượn sách
        /// </summary>
        [HttpGet("borrowcode")]
        public async Task<IActionResult> getBorrowCode(int bookcase_id)
        {
            try {
                Bookcase bookcase = await _bookcaseRepository.Get(bookcase_id);
                if (bookcase == null) return NotFound(new Respone(404, "Not Found", null));

                var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                 User user = await _userRepository.FindByUsername(username);
                 if (user == null) {
                    return NotFound(new Respone(404, "Not Found", null));
                 }

                 if (bookcase.user_id != user.user_id) return BadRequest(new Respone(400, "Failed", null));

                var base64EncodedBytes = System.Text.Encoding.UTF8.GetBytes("Borrow " + bookcase_id);
                var resultCode = System.Convert.ToBase64String(base64EncodedBytes);

                return Ok(new Respone(200, "ok", new {
                    code = resultCode
                }));
            } catch (Exception e) {
                return BadRequest(new Respone(400, "Failed", null));
            }
        }

        /// <summary>
        /// Lấy mã trả sách
        /// </summary>
        [HttpGet("returncode")]
        public async Task<IActionResult> getReturnCode(int bookcase_id)
        {
            try {
                var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                 User user = await _userRepository.FindByUsername(username);
                 if (user == null) {
                    return NotFound(new Respone(404, "Not Found", null));
                 }

                Bookcase bookcase = await _bookcaseRepository.Get(bookcase_id);
                if (bookcase.user_id != user.user_id) return BadRequest(new Respone(400, "Failed", null));

                Borrow borrow = await _borrowRepository.FindByBookcase(bookcase_id);
                if (borrow == null || borrow.return_date != null) return BadRequest(new Respone(400, "Chưa mượn", null));

                var resultCode = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Return " + borrow.borrow_id));
                return Ok(new Respone(200, "ok", new {
                    code = resultCode
                }));
            } catch (Exception e) {
                return BadRequest(new Respone(400, "Failed", null));
            }
        }
    }
}