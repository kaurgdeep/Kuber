using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using KuberAPI.Dto;
using KuberAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace KuberAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : KuberBaseController
    {
        private IConfiguration Configuration;
        private KuberContext Context;
        public UsersController(IConfiguration configuration, KuberContext context)
        {
            Configuration = configuration;
            Context = context;
        }

        // POST api/values
        [HttpPost]
        [Route("register")]
        public ActionResult Register([FromBody] RegisterUserDto userDto)
        {
            try
            {
                // todo: Hash the password before saving
                var user = new User
                {
                    EmailAddress = userDto.EmailAddress.ToLower(),
                    PasswordHash = userDto.Password,
                    UserType = userDto.UserType,
                    Created = DateTime.UtcNow
                };
                Context.Users.Add(user);
                Context.SaveChanges();

                // Todo: Authenticate and return the JWT
                return Ok(new { user.UserId });
            }
            catch (Exception ex)
            {
                // TODO: Log the exception details 
                var messages = new List<string>();
                while (ex != null)
                {
                    messages.Add(ex.Message);
                    ex = ex.InnerException;
                }

                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return new JsonResult(new { Error = string.Join("\r\n", messages) });
                // Note: In production, don't return any more details - return just the status
                return StatusCode(StatusCodes.Status500InternalServerError, string.Join("\r\n", messages));
            }
        }

        [HttpPost]
        [Route("log-in")]
        [AllowAnonymous]
        public ActionResult Login([FromBody] UserDto userDto)
        {
            // todo: hash the input password and check with passwordhash from database
            var user = Context.Users.Where(u => u.EmailAddress == userDto.EmailAddress.ToLower() && u.PasswordHash == userDto.Password).FirstOrDefault();
            if (user == null)
            {
                return BadRequest("Could not verify username and password");
            }

            var claims = new[] {
                new Claim(Constants.EmailAddress, user.EmailAddress),
                new Claim(Constants.UserId, user.UserId.ToString()),
                new Claim(Constants.UserType, user.UserType.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration[Constants.JWTSecurityKey]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "rideshare.com",
                audience: "rideshare.com",
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        // Note: This is just an endpoint to test if authentication succeeded. 
        // We may not may not use it in our Web application
        [HttpGet]
        [Route("me")]
        [Authorize]
        public ActionResult Me()
        {
            return Ok(new
            {
                Data = new
                {
                    // note: we are using inferred type names - a new C# feature
                    // so instead of new { UserId = base.UserId, EmailAddres = base.EmailAddress, UserType = base.UserType }
                    base.UserId,
                    base.EmailAddress,
                    base.UserType
                }
            });
        }
    }
}