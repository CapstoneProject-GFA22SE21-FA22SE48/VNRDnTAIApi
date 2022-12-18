using BusinessObjectLibrary;
using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using DTOsLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

        // GET: api/Users/Members
        [Authorize]
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
        [Authorize]
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

        // GET: api/Users/Scribes/Detail/5
        [Authorize]
        [HttpGet("Scribes/Detail/{month}/{year}/{scribeId}")]
        [ProducesResponseType(typeof(ScribeDTO), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ScribeDTO>> GetScribeDetail(int month, int year, Guid scribeId)
        {
            try
            {
                return StatusCode(200, await _entity.GetScribeDetail(month, year, scribeId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Users/Scribes/ApprovalRate/5
        [Authorize]
        [HttpGet("Scribes/ApprovalRate/{scribeId}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<User>> GetApprovalRateInformation(Guid scribeId)
        {
            try
            {
                return StatusCode(200, await _entity.ScribeGetApprovalRateInformation(scribeId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Users/Admins
        [Authorize]
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

        // GET: api/Users/5
        [Authorize]
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

        // PUT: api/Users/Members/Deactivate/5
        [Authorize]
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

        // PUT: api/Users/Scribes/Deactivate/3
        [Authorize]
        [HttpPut("Scribes/Deactivate/{id}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeactivateScribe(Guid id, User scribe)
        {
            if (id != scribe.Id)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.DeactivateScribe(scribe));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Users/Scribes/ReEnable
        [Authorize]
        [HttpPut("Scribes/ReEnable/{id}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ReEnableScribe(Guid id, User scribe)
        {
            if (id != scribe.Id)
            {
                return BadRequest();
            }

            try
            {
                return StatusCode(200, await _entity.ReEnableScribe(scribe));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Users/Scribes/Promote
        [Authorize]
        [HttpPost("Scribes/Promote")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PromoteScribe([FromBody] ScribePromotionDTO scribePromotionDTO)
        {
            try
            {
                return StatusCode(200, await _entity.PromoteScribe(scribePromotionDTO));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Users/Members/ReEnable
        [Authorize]
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
        [Authorize]
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

        // POST: api/Users/Register
        [AllowAnonymous]
        [HttpPost("Register")]
        [ProducesResponseType(typeof(User), 201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<User>> Register(LoginUserDTO loginUserDTO)
        {
            User user = null;
            try
            {
                if (loginUserDTO.Email != null)
                {
                    user = await _entity.GetUserAsyncByGmail(loginUserDTO.Email.Trim());

                    if (user == null)
                    {
                        user = await _entity
                            .GetUserAsyncByUsername(loginUserDTO.Username.Trim());
                        if (user == null)
                        {
                            user = await _entity
                                .RegisterMember(
                                    loginUserDTO.Username.Trim(),
                                    loginUserDTO.Password.Trim(),
                                    loginUserDTO.Email.Trim()
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
                        else
                        {
                            return StatusCode(409, "Tên đăng nhập này đã được đăng ký.");
                        }
                    }
                    else
                    {
                        return StatusCode(409, "Email này đã được đăng ký.");
                    }
                }
                else
                {
                    user = await _entity
                            .GetUserAsyncByUsername(loginUserDTO.Username.Trim());
                    if (user == null)
                    {
                        user = await _entity
                            .RegisterMember(
                                loginUserDTO.Username,
                                loginUserDTO.Password,
                                null
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
                    else
                    {
                        return StatusCode(409, "Tên đăng nhập này đã được đăng ký.");
                    }
                }
            }
            catch (ArgumentException ae)
            {
                return StatusCode(400, "Có lỗi xảy ra.\n" + ae.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Có lỗi xảy ra. Vui lòng thử lại sau.\n" + ex.Message);
            }
        }

        //POST api/Users/WebLogin
        [AllowAnonymous]
        [HttpPost("WebLogin")]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
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

        //POST api/Users/MobileLogin
        [AllowAnonymous]
        [HttpPost("MobileLogin")]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> AppLogin(LoginUserDTO loginUserDTO)
        {
            User? user = null;
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
                        new Claim("Role", user.Role.ToString()),
                        new Claim("Avatar", String.IsNullOrEmpty(user.Avatar) ? "" : user.Avatar),
                        new Claim("DisplayName", String.IsNullOrEmpty(user.DisplayName) ? "" : user.DisplayName),
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
                    throw new ApplicationException("Sai thông tin đăng nhập hoặc tài khoản đã ngưng hoạt động");
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
        [Authorize]
        [HttpPut("{id}/ChangePassword")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> ChangePassword(Guid id, string oldPassword, string newPassword)
        {
            User? user = null;
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
                            new Claim("Username", String.IsNullOrEmpty(user.Username) ? "" : user.Username),
                            new Claim("Email", String.IsNullOrEmpty(user.Gmail) ? "" : user.Gmail),
                            new Claim("Role", user.Role.ToString()),
                            new Claim("Avatar", String.IsNullOrEmpty(user.Avatar) ? "" : user.Avatar),
                            new Claim("DisplayName", String.IsNullOrEmpty(user.DisplayName) ? "" : user.DisplayName),
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

        //PUT api/Users/5/UpdateProfile
        [Authorize]
        [HttpPut("{id}/UpdateProfile")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateProfile(Guid id, ProfileDTO profileDTO)
        {
            if (profileDTO is null)
            {
                throw new ArgumentNullException(nameof(profileDTO));
            }

            User? user = null;
            try
            {
                user = await _entity.GetUserAsync(id);
                if (user == null)
                {
                    return StatusCode(404, "Không tim thấy người dùng.");
                }
                else
                {
                    if (profileDTO.email != null)
                    {
                        User exist = await _entity.GetUserAsyncByGmail(profileDTO.email);
                        if (exist != null)
                        {
                            return StatusCode(409, "Email này đã được đăng ký.");
                        }
                        else
                        {
                            user.Gmail = profileDTO.email.Trim();
                        }

                    }
                    if (profileDTO.avatar != null) user.Avatar = profileDTO.avatar.Trim();
                    if (profileDTO.displayName != null) user.DisplayName = profileDTO.displayName.Trim();
                    user = await _entity.UpdateUser(user);
                    if (user != null)
                    {
                        var authClaims = new List<Claim>
                        {
                            new Claim("Id", user.Id.ToString()),
                            new Claim("Username", String.IsNullOrEmpty(user.Username) ? "" : user.Username),
                            new Claim("Email", String.IsNullOrEmpty(user.Gmail) ? "" : user.Gmail),
                            new Claim("Role", user.Role.ToString()),
                            new Claim("Avatar", String.IsNullOrEmpty(user.Avatar) ? "" : user.Avatar),
                            new Claim("DisplayName", String.IsNullOrEmpty(user.DisplayName) ? "" : user.DisplayName),
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
                        return StatusCode(405, "Cập nhật thông tin thất bại.");
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

        //POST api/Users/GetUserByEmail
        [HttpPost("GetUserByEmail")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserByEmail([FromBody] string email)
        {
            User user;
            try
            {
                user = await _entity.GetUserAsyncByGmail(email.Trim());

                if (user != null)
                {
                    return StatusCode(200, true);
                }
                else
                {
                    return StatusCode(404, "Email không tồn tại");
                }
            }
            catch (ArgumentException ae)
            {
                return BadRequest("Có lỗi xảy ra.\n" + ae.Message);
            }
            catch
            {
                return StatusCode(500, "Có lỗi xảy ra.\nVui lòng thử lại sau.");
            }
        }

        //PUT api/Users/ForgotPassword
        [HttpPut("ForgotPassword")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] LoginUserDTO newInfo)
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
                            new Claim("Username", String.IsNullOrEmpty(user.Username) ? "" : user.Username),
                            new Claim("Email", String.IsNullOrEmpty(user.Gmail) ? "" : user.Gmail),
                            new Claim("Role", user.Role.ToString()),
                            new Claim("Avatar", String.IsNullOrEmpty(user.Avatar) ? "" : user.Avatar),
                            new Claim("DisplayName", String.IsNullOrEmpty(user.DisplayName) ? "" : user.DisplayName),
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

        // PUT: api/Users/SelfDeactivate/5
        [Authorize]
        [HttpPut("SelfDeactivate/{id}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> selfDeactivate(Guid id)
        {
            try
            {
                User? member = (await _entity.GetMembersAsync()).Where(m => m.Id == id).FirstOrDefault();
                if (member != null)
                    return StatusCode(200, await _entity.DeactivateMember(member));
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
