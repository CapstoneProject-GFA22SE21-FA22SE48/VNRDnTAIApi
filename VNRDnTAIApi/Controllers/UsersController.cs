using BusinessObjectLibrary;
using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using DTOsLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VNRDnTAILibrary;

namespace VNRDnTAIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]

    public class UsersController : ControllerBase
    {
        private readonly UserBusinessEntity _entity;

        public UsersController(IUnitOfWork work)
        {
            _entity = new UserBusinessEntity(work);
        }

        // GET: api/Users
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                return StatusCode(200, await _entity.GetUsersAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Users/Members
        [HttpGet("Members")]
        [ProducesResponseType(typeof(IEnumerable<User>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<User>>> GetMembers(string? keywordUsername)
        {
            try
            {
                if (string.IsNullOrEmpty(keywordUsername))
                {
                    return StatusCode(200, await _entity.GetMembersAsync());
                }
                else
                {
                    return StatusCode(200, await _entity.GetMembersByUserNameAsync(keywordUsername));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Users/Members/DateRange/
        [HttpGet("Members/DateRange")]
        [ProducesResponseType(typeof(IEnumerable<User>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<User>>> GetMembers(DateTime startDate, DateTime endDate)
        {
            try
            {
                return StatusCode(200, await _entity.GetMembersByCreatedDateRangeAsync(startDate, endDate));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            try
            {
                return StatusCode(200, await _entity.GetUserAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.UpdateUser(user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Users/Members/Deactivate
        [HttpPut("Members/Deactivate/{id}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeactivateMember(Guid id, User member)
        {
            if (id != member.Id)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.DeactivateMember(member));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Users/Members/ReEnable
        [HttpPut("Members/ReEnable/{id}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ReEnableMember(Guid id, User member)
        {
            if (id != member.Id)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.ReEnableMember(member));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Users
        [HttpPost]
        [ProducesResponseType(typeof(User), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            try
            {
                return StatusCode(201, await _entity.AddUser(user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                await _entity.RemoveUser(id);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //POST api/Users/Login
        [HttpPost("Login")]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginUserDTO loginUserDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(loginUserDTO.Username))
                {
                    throw new ArgumentException("Vui lòng nhập tên đăng nhập!");
                }

                if (string.IsNullOrEmpty(loginUserDTO.Password))
                {
                    throw new ArgumentException("Vui lòng nhập mật khẩu!");
                }
                User user = await _entity
                    .Login(loginUserDTO.Username, loginUserDTO.Password);

                if (user != null)
                {
                    var authClaims = new List<Claim>
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Username", user.Username),
                    new Claim("Role", user.Role.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                    var authSignature = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(VNRDnTAIConfiguration.Secret));

                    //Token generate
                    var token = new JwtSecurityToken(
                        issuer: VNRDnTAIConfiguration.JwtIssuer,
                        audience: VNRDnTAIConfiguration.JwtAudience,
                        //expires: DateTime.Now.AddHours(2),
                        claims: authClaims,
                        signingCredentials:
                            new SigningCredentials(authSignature, SecurityAlgorithms.HmacSha256)
                        );

                    return StatusCode(200, new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                    });
                }
                else
                {
                    throw new ApplicationException("Sai tên đăng nhập hoặc mật khẩu");
                }
            }
            catch (ArgumentException ae)
            {
                return Unauthorized(ae.Message);
            }
            catch (ApplicationException ae)
            {
                return Unauthorized(ae.Message);
            }
            catch
            {
                return Unauthorized("Có lỗi xảy ra. Vui lòng thử lại sau.");
            }
        }
    }
}
