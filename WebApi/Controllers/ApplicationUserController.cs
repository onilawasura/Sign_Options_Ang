using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Modals;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {

        //because of #* function (in startup.cs) we can use below 2 functions 

        //in order to work with user registration or authentication we have to use below 2 calsses from identity core   
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _singInManager;
        private readonly ApplicationSettings _appSettings;

        public ApplicationUserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<ApplicationSettings> appSettings)
        {
            _userManager = userManager;
            _singInManager = signInManager;
        }

        [HttpPost]
        [Route("Register")]
        //POST : /api/ApplicationUser/Register
        public async Task<Object> PostApplicationUser(ApplcationUserModal model)
        {
            //this code is responsible for user registration only 
            //var applicationUser = new ApplicationUser()
            //{
            //    UserName = model.UserName,
            //    Email = model.Email,
            //    FullName = model.FullName
            //};

            //try
            //{
            //    var result = await _userManager.CreateAsync(applicationUser, model.Password);
            //    return Ok(result);
            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}


            //this code is responsible for user registration with assigned roles roles to users

            model.Role = "Admin";//new line

            var applicationUser = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName
            };

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, model.Password);
                await _userManager.AddToRoleAsync(applicationUser, model.Role);//new line
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        //user authentication method

        [HttpPost]
        [Route("Login")]
        //POST : /api/ApplicationUser/Login
        public async Task<IActionResult> Login(LoginModel model)
        {

            //this code is responsible for login without user  roles
            //var user = await _userManager.FindByNameAsync(model.UserName);
            //if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            //{
            //    var tokenDescriptor = new SecurityTokenDescriptor
            //    {
            //        Subject = new ClaimsIdentity(new Claim[]
            //        {
            //            new Claim("UserID",user.Id.ToString())
            //        }),
            //        Expires = DateTime.UtcNow.AddDays(1),
            //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234567890123456")), SecurityAlgorithms.HmacSha256Signature)
            //    };
            //    var tokenHandler = new JwtSecurityTokenHandler();
            //    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            //    var token = tokenHandler.WriteToken(securityToken);
            //    return Ok(new { token });
            //}
            //else
            //{
            //    return BadRequest(new { message = "Username or password is incorrect." });
            //}


            //this code is responsible for login with user  roles
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {

                //get roles assigned to the user

                var role = await _userManager.GetRolesAsync(user);
                IdentityOptions _options = new IdentityOptions();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID",user.Id.ToString()),
                        new Claim( _options.ClaimsIdentity.RoleClaimType, role.FirstOrDefault())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234567890123456")), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }
            else
            {
                return BadRequest(new { message = "Username or password is incorrect." });
            }
        }
    }
}