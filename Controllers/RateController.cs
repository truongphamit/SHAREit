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
    public class RateController : Controller
    {
        private readonly IRateRepository _rateRepository;
        private readonly IUserRepository _userRepository;

        private readonly IBookRepository _bookRepository;

        public RateController(IRateRepository _rateRepository, IUserRepository _userRepository, IBookRepository _bookRepository)
        {
            this._userRepository = _userRepository;
            this._bookRepository = _bookRepository;
            this._rateRepository = _rateRepository;
        }

        /// <summary>
        /// Thêm đánh giá sách
        /// </summary>
        [HttpGet("add")]
        public async Task<IActionResult> add(int book_id, int star, string review)
        {
            try {
                Book book = await _bookRepository.Get(book_id);
                if (book == null) return NotFound(new Respone(404, "Not Found", null));

                var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                 User user = await _userRepository.FindByUsername(username);
                 if (user == null) {
                    return NotFound(new Respone(404, "Not Found", null));
                 }

                if (await _rateRepository.isRated(book_id, user.user_id)) {
                    return BadRequest(new Respone(400, "Rated", null));
                }

                Rate rate = new Rate
                {
                    book_id = book.book_id,
                    star = star,
                    review = review,
                    user_id = user.user_id
                };

                await _rateRepository.Add(rate);

                return Ok(new Respone(200, "ok", null));
            } catch (Exception e) {
                return BadRequest(new Respone(400, "Failed", null));
            }
        }

        /// <summary>
        /// Sửa đánh giá sách
        /// </summary>
        [HttpGet("edit")]
        public async Task<IActionResult> edit(int rate_id, int star, string review)
        {
            try {
                Rate rate = await _rateRepository.Get(rate_id);
                if (rate == null) return NotFound(new Respone(404, "Not Found", null));

                var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                 User user = await _userRepository.FindByUsername(username);
                 if (user == null) {
                    return NotFound(new Respone(404, "Not Found", null));
                 }

                 if (rate.user_id != user.user_id) return BadRequest(new Respone(400, "Failed", null));

                rate.star = star;
                rate.review = review != null ? review : rate.review;

                 await _rateRepository.Update(rate_id, rate);

                return Ok(new Respone(200, "ok", null));
            } catch (Exception) {
                return BadRequest(new Respone(400, "Failed", null));
            }
        }

        /// <summary>
        /// Lấy đánh giá sách
        /// </summary>
        [HttpGet("get")]
        public async Task<IActionResult> get(int rate_id)
        {
            try {
                Rate rate = await _rateRepository.Get(rate_id);
                if (rate == null) return NotFound(new Respone(404, "Not Found", null));

                return Ok(new Respone(200, "ok", rate));
            } catch (Exception e) {
                return BadRequest(new Respone(400, "Failed", null));
            }
        }

        /// <summary>
        /// Lấy toàn bộ đánh giá của sách
        /// </summary>
        [HttpGet("list")]
        public async Task<IActionResult> findByBookcase(int book_id)
        {
            try {
                return Ok(new Respone(200, "ok", await _rateRepository.FindByBook(book_id)));
            } catch (Exception e) {
                return BadRequest(new Respone(400, "Failed", null));
            }
        }  
    }
}