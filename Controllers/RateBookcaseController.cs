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
    public class RateBookcaseController : Controller
    {
        private readonly IRateBookcaseRepository _rateBookcaseRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBookcaseRepository _bookcaseRepository;

        private readonly IBookRepository _bookRepository;

        public RateBookcaseController(IRateBookcaseRepository _rateBookcaseRepository, IUserRepository _userRepository, IBookcaseRepository _bookcaseRepository, IBookRepository _bookRepository)
        {
            this._userRepository = _userRepository;
            this._bookRepository = _bookRepository;
            this._bookcaseRepository = _bookcaseRepository;
            this._rateBookcaseRepository = _rateBookcaseRepository;
        }

        [HttpGet("add")]
        public async Task<IActionResult> add(int bookcase_id, int star, string review)
        {
            try {
                Bookcase bookcase = await _bookcaseRepository.Get(bookcase_id);
                if (bookcase == null) return NotFound(new Respone(404, "Not Found", null));

                var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                 User user = await _userRepository.FindByUsername(username);
                 if (user == null) {
                    return NotFound(new Respone(404, "Not Found", null));
                 }

                RateBookcase rateBookcase = new RateBookcase
                {
                    bookcase_id = bookcase.bookcase_id,
                    star = star,
                    review = review,
                    user_id = user.user_id
                };

                await _rateBookcaseRepository.Add(rateBookcase);

                return Ok(new Respone(200, "ok", null));
            } catch (Exception e) {
                return BadRequest(new Respone(400, "Failed", null));
            }
        }

        [HttpGet("edit")]
        public async Task<IActionResult> edit(int rate_bookcase_id, int star, string review)
        {
            try {
                RateBookcase rateBookcase = await _rateBookcaseRepository.Get(rate_bookcase_id);
                if (rateBookcase == null) return NotFound(new Respone(404, "Not Found", null));

                var username = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                 User user = await _userRepository.FindByUsername(username);
                 if (user == null) {
                    return NotFound(new Respone(404, "Not Found", null));
                 }

                 if (rateBookcase.user_id != user.user_id) return BadRequest(new Respone(400, "Failed", null));

                rateBookcase.star = star;
                rateBookcase.review = review != null ? review : rateBookcase.review;

                 await _rateBookcaseRepository.Update(rate_bookcase_id, rateBookcase);

                return Ok(new Respone(200, "ok", null));
            } catch (Exception) {
                return BadRequest(new Respone(400, "Failed", null));
            }
        }

        [HttpGet("get")]
        public async Task<IActionResult> get(int rate_bookcase_id)
        {
            try {
                RateBookcase rateBookcase = await _rateBookcaseRepository.Get(rate_bookcase_id);
                if (rateBookcase == null) return NotFound(new Respone(404, "Not Found", null));

                return Ok(new Respone(200, "ok", rateBookcase));
            } catch (Exception e) {
                return BadRequest(new Respone(400, "Failed", null));
            }
        }

        [HttpGet("list")]
        public async Task<IActionResult> findByBookcase(int bookcase_id)
        {
            try {
                return Ok(new Respone(200, "ok", await _rateBookcaseRepository.FindByBookcase(bookcase_id)));
            } catch (Exception e) {
                return BadRequest(new Respone(400, "Failed", null));
            }
        }
    }
}