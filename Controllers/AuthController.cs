using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SHAREit.Models;
using SHAREit.Repositories;

namespace SHAREit.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller 
    {
         private string secrectKey = "binaryinc_shareit";
         private readonly IUserRepository _user;

        public AuthController(IUserRepository user)
        {
            _user = user;
        }     

         [HttpPost("signin")]
         public async Task<IActionResult> signin([FromHeader] String Authorization)
         {
             try {
                string auth = Encoding.UTF8.GetString(Convert.FromBase64String(Authorization.Substring(5)));
                string username = auth.Substring(0, auth.IndexOf(":"));
                string password = auth.Substring(auth.IndexOf(":") + 1);

                var user = await _user.FindByUsername(username);
                if (user == null || !user.password.Equals(password)){
                    return Unauthorized();
                }
                return Ok(new Respone(200, "ok", new { token = genToken(user) }));
             } catch (Exception e) {
                 Console.WriteLine(e.Message);
                 return BadRequest(new Respone(400, "Failed", null));
             }
         }

         [HttpPost("signup")]
         public async Task<IActionResult> signup([FromHeader] String Authorization, [FromBody] UserRequestModel userRequestModel) {
             try {
                string auth = Encoding.UTF8.GetString(Convert.FromBase64String(Authorization.Substring(5)));
                string username = auth.Substring(0, auth.IndexOf(":"));
                string password = auth.Substring(auth.IndexOf(":") + 1);

                var user = await _user.FindByUsername(username);
                if (user != null) {
                    return BadRequest(new Respone(400, "Username does exist", null));
                }

                var newUser = new User 
                {
                    username = username,
                    password = password,
                    address = userRequestModel.address,
                    phone = userRequestModel.phone,
                    email = userRequestModel.email,
                    lat = userRequestModel.lat,
                    lon = userRequestModel.lon,
                    role = "user"
                };

                await _user.Add(newUser);

                return Ok(new Respone(200, "ok", new { token = genToken(newUser) }));
             } catch (Exception e) {
                 return BadRequest(new Respone(400, "Failed", null));
             }
         }

        [Authorize]
         [HttpGet("profile")]
         public async Task<IActionResult> getProfile() {
             try {
                 var currentUsername = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                 User user = await _user.FindByUsername(currentUsername);
                 if (user == null) {
                    return NotFound(new Respone(404, "NotFound", null));
                 }
                 var dict = new Dictionary<string, object>();
                 dict.Add("username", user.username);
                 dict.Add("address", user.address);
                 dict.Add("phone", user.phone);
                 dict.Add("email", user.email);
                 dict.Add("lat", user.lat);
                 dict.Add("lon", user.lon);
                 dict.Add("create_time", user.create_time);
                return Ok(new Respone(200, "ok", dict));
             } catch {
                 return BadRequest(new Respone(400, "Failed", null));
             }
         }

        [Authorize]
         [HttpGet("profile/edit")]
         public async Task<IActionResult> editProfile(string address, string phone, string email, float lat, float lon)
         {
             try {
                 var currentUsername = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                 User user = await _user.FindByUsername(currentUsername);
                 if (user == null) {
                    return NotFound(new Respone(404, "NotFound", null));
                 }
                 user.address = address != null ? address : user.address;
                 user.phone = phone != null ? phone : user.phone;
                 user.email = email != null ? email : user.email;
                 user.lat = lat != null ? lat : user.lat;
                 user.lon = lon != null ? lon : user.lon;

                 await _user.Update(user.user_id, user);
                return Ok(new Respone(200, "ok", null));
             } catch {
                 return BadRequest(new Respone(400, "Failed", null));
             }
         }

         private string genToken(User user)
         {

            var key = new SymmetricSecurityKey(Encoding.Default.GetBytes(this.secrectKey));
            var claims = new Claim[]{
                new Claim(JwtRegisteredClaimNames.NameId, user.username),
                new Claim(JwtRegisteredClaimNames.Jti, user.username),

                new Claim(ClaimTypes.Role, user.role),
                new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}")
            };
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            
            return tokenStr;
         }
    }
}