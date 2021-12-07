using Core.Models.Base;
using Domain.Contracts.Account.Extensions;
using Domain.Contracts.User;
using Domain.Interfaces.Service;
using Domain.Models;
using Infra.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Authorize]    
    [ApiController]
    [Route("v1/accounts")]
    public class AccountsController : ControllerBase
    {

        #region Dependency Injection

        private readonly SignInManager<UserModel> _signInManager;
        private readonly UserManager<UserModel> _userManager;
        private readonly AppSettings _appSettings;
        private IEmailService _emailService;

        public AccountsController(
           SignInManager<UserModel> signInManager,
           UserManager<UserModel> userManager,
           IOptions<AppSettings> appSettings,
           IEmailService emailService
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _emailService = emailService;
        }

        #endregion
                
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model"></param>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(ResponseLoginOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status400BadRequest)]        
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status423Locked)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(UserLoginInput model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.Select(e => e.Errors).FirstOrDefault());

            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                    return NotFound();

                var role = await _userManager.GetRolesAsync(user);

                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, true);

                if (result.IsLockedOut)
                    return StatusCode(StatusCodes.Status423Locked, "Sua conta foi bloquada temporariamente, por favor tente mais tarde.");

                if (result.Succeeded)
                {
                    var response = AccountExtension.ConvertToResponseLoginContract(
                    TokenRoleService.GenerateJwtRoles(user, role.Count > 0 ? role[0] : "", _appSettings),
                    DateTime.Now.AddHours(_appSettings.Expiration),
                    user.Email,
                    user.Name);

                    return Ok(response);
                }

                return BadRequest("Usuário ou Senha inválidos.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Validar token de usuário
        /// </summary>
        [Authorize]
        [HttpPost("token")]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status401Unauthorized)]
        public IActionResult TokenValidate()
        {
            var userNameInToken = User.Identity.Name;
            var userIsAuthenticatedInToken = User.Identity.IsAuthenticated;

            return Ok();
        }

        /// <summary>
        /// Logout
        /// </summary>
        [HttpPost("logout")]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Resetar Senha
        /// </summary>
        /// <param name="model"></param>       
        [AllowAnonymous]
        [HttpPost]
        [Route("reset-password")]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetPassword(UserResetPasswordInput model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.Select(e => e.Errors).FirstOrDefault());

            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                    return NotFound();

                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

                if (!result.Succeeded)
                    return BadRequest("Senha não alterada. Verifique o padrão da senha informada.");

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Esqueceu a senha
        /// </summary>
        /// <param name="model"></param>
        [AllowAnonymous]
        [HttpPost]
        [Route("forgot-password")]
        [ProducesResponseType(typeof(ResponseLoginOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status404NotFound)]        
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ForgotPassword(UserForgotPasswordInput model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.Select(e => e.Errors).FirstOrDefault());

            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                    return NotFound();

                if (await _userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var response = AccountExtension.ConvertToResponseLoginContract(
                    token,
                    DateTime.Now.AddHours(_appSettings.Expiration),
                    user.Email,
                    user.Name);

                    return base.Ok(response);
                }

                return StatusCode(StatusCodes.Status403Forbidden, "Este usuário não possui email confirmado.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}