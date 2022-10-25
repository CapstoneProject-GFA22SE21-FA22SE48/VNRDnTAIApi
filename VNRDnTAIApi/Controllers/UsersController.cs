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
using System.Web;
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

        // GET: api/Users/Scribes
        [HttpGet("Scribes")]
        [ProducesResponseType(typeof(IEnumerable<User>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<User>>> GetScribes(string? keywordUsername)
        {
            try
            {
                if (string.IsNullOrEmpty(keywordUsername))
                {
                    return StatusCode(200, await _entity.GetScribesAsync());
                }
                else
                {
                    return StatusCode(200, await _entity.GetScribesByUserNameAsync(keywordUsername));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Users/Admins
        [HttpGet("Admins")]
        [ProducesResponseType(typeof(IEnumerable<AdminDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<AdminDTO>>> GetAdmins()
        {
            try
            {
                return StatusCode(200, await _entity.GetAdminsAsync());
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

        // GET: api/Users/Scribes/DateRange/
        [HttpGet("Scribes/DateRange")]
        [ProducesResponseType(typeof(IEnumerable<User>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<User>>> GetScribes(DateTime startDate, DateTime endDate)
        {
            try
            {
                return StatusCode(200, await _entity.GetScribesByCreatedDateRangeAsync(startDate, endDate));
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
        // POST: api/Users/Register
        [HttpPost("Register")]
        [ProducesResponseType(typeof(User), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<User>> Register(LoginUserDTO loginUserDTO)
        {
            User user;
            try
            {
                user = await _entity.GetUserAsyncByGmail(loginUserDTO.Email);
                if (user == null)
                {
                    user = await _entity
                        .GetUserAsyncByUsername(loginUserDTO.Username);
                    if (user == null)
                    {
                        user = await _entity
                            .RegisterMember(
                                loginUserDTO.Username,
                                loginUserDTO.Password,
                                loginUserDTO.Email
                            );

                        if (user != null)
                        {
                            return StatusCode(201, user);
                        }
                        else
                        {
                            return StatusCode(400, "Có lỗi xảy ra.");
                        }
                    }
                    return StatusCode(409, "Username đã được đăng ký.");
                }
                return StatusCode(409, "Email đã được đăng ký.");
            }
            catch (ArgumentException ae)
            {
                return StatusCode(400,"Có lỗi xảy ra.\n" + ae.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Có lỗi xảy ra. Vui lòng thử lại sau.\n"+ ex.Message);
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
                //if (string.IsNullOrEmpty(loginUserDTO.Username))
                //{
                //    throw new ArgumentException("Vui lòng nhập tên đăng nhập!");
                //}

                //if (string.IsNullOrEmpty(loginUserDTO.Password))
                //{
                //    throw new ArgumentException("Vui lòng nhập mật khẩu!");
                //}
                User user = await _entity
                    .LoginWeb(loginUserDTO.Username, loginUserDTO.Password);

                if (user != null && user.Username != "" && user.Password != "")
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
                        expires: DateTime.MaxValue,
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

        //POST api/Users/AppLogin
        [HttpPost("AppLogin")]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        public async Task<IActionResult> AppLogin(LoginUserDTO loginUserDTO)
        {
            User user = null;
            try
            {
                if (loginUserDTO.Username != null && loginUserDTO.Password != null 
                    && loginUserDTO.Username.Length > 0 && loginUserDTO.Password.Length > 0)
                {
                    user = await _entity
                    .LoginMobile(loginUserDTO.Username, loginUserDTO.Password);
                }
                // second chances
                else if (loginUserDTO.Email != null && loginUserDTO.Email.Length > 0)
                {
                    user = await _entity.LoginWithGmail(loginUserDTO.Email);
                }

                if (user != null)
                {
                    var authClaims = new List<Claim>
                    {
                        new Claim("Id", user.Id.ToString()),
                        new Claim("Username", String.IsNullOrEmpty(user.Username) ? "" : user.Username),
                        new Claim("Email", String.IsNullOrEmpty(user.Gmail) ? "" : user.Gmail),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    var authSignature = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(VNRDnTAIConfiguration.Secret));

                    //Token generate
                    var token = new JwtSecurityToken(
                        issuer: VNRDnTAIConfiguration.JwtIssuer,
                        audience: VNRDnTAIConfiguration.JwtAudience,
                        //expires: DateTime.Now.AddHours(2),
                        expires: DateTime.MaxValue,
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
                return Unauthorized("Có lỗi xảy ra.\n" + ae.Message);
            }
            catch (ApplicationException ae)
            {
                return Unauthorized("Có lỗi xảy ra.\n" + ae.Message);
            }
            catch
            {
                return Unauthorized("Có lỗi xảy ra. Vui lòng thử lại sau.");
            }
        }

        //PUT api/Users/5/ChangePassword
        [HttpPut("{id}/ChangePassword")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(Guid id, string oldPassword, string newPassword)
        {
            User user = null;
            try
            {
                user = await _entity.GetUserAsync(id);
                if (user == null)
                {
                    return StatusCode(404, "Không tim thấy người dùng.");
                }
                else if (!oldPassword.Equals(user.Password))
                {
                    return StatusCode(403, "Mật khẩu hiện tại không đúng.");
                } 
                else
                {
                    user.Password = newPassword;
                    user = await _entity.UpdateUser(user);
                    if (user != null)
                    {
                        var authClaims = new List<Claim>
                        {
                            new Claim("Id", user.Id.ToString()),
                            new Claim("Username", String.IsNullOrEmpty(user.Username) ? "": user.Username),
                            new Claim("Email", String.IsNullOrEmpty(user.Gmail)? "": user.Gmail),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var authSignature = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(VNRDnTAIConfiguration.Secret)
                            );

                        //Token generate
                        var token = new JwtSecurityToken(
                            issuer: VNRDnTAIConfiguration.JwtIssuer,
                            audience: VNRDnTAIConfiguration.JwtAudience,
                            //expires: DateTime.Now.AddHours(2),
                            expires: DateTime.MaxValue,
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
                        return StatusCode(405, "Cập nhật mật thất bại.");
                    }
                }
            }
            catch (ArgumentException ae)
            {
                return StatusCode(400, ae.Message);
            }
            catch (ApplicationException ae)
            {
                return StatusCode(500,ae.Message);
            }
        }

        //PUT api/Users/ForgotPassword
        [HttpPut("ForgotPassword")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody]LoginUserDTO newInfo)
        {
            User user = null;
            try
            {
                user = await _entity.GetUserAsyncByGmail(newInfo.Email);
                if (user == null)
                {
                    return StatusCode(404, "Không tim thấy người dùng.");
                }
                else
                {
                    user.Password = newInfo.Password;
                    user = await _entity.UpdateUser(user);
                    if (user != null)
                    {
                        var authClaims = new List<Claim>
                        {
                            new Claim("Id", user.Id.ToString()),
                            new Claim("Username", String.IsNullOrEmpty(user.Username) ? "": user.Username),
                            new Claim("Email", String.IsNullOrEmpty(user.Gmail)? "": user.Gmail),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var authSignature = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(VNRDnTAIConfiguration.Secret)
                            );

                        //Token generate
                        var token = new JwtSecurityToken(
                            issuer: VNRDnTAIConfiguration.JwtIssuer,
                            audience: VNRDnTAIConfiguration.JwtAudience,
                            //expires: DateTime.Now.AddHours(2),
                            expires: DateTime.MaxValue,
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
                        return StatusCode(405, "Cập nhật mật khẩu thất bại.");
                    }
                }
            }
            catch (ArgumentException ae)
            {
                return StatusCode(400, ae.Message);
            }
            catch (ApplicationException ae)
            {
                return StatusCode(500, ae.Message);
            }
        }
    }
}
