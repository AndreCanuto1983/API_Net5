using Core.Exceptions;
using Core.Models.Base;
using Domain.Contracts.Account.Extensions;
using Domain.Contracts.User.Extensions;
using Domain.Contracts.User.Input;
using Domain.Models;
using Infra.Configuration;
using Infra.Interfaces;
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
    [Route("api/v1/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        #region Dependency Injection

        private readonly SignInManager<UserModel> _signInManager;
        private readonly UserManager<UserModel> _userManager;
        private readonly AppSettings _appSettings;
        private IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        public AccountController(
           SignInManager<UserModel> signInManager,
           UserManager<UserModel> userManager,
           IOptions<AppSettings> appSettings,
           IEmailService emailService,
           IUserRepository userRepository
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _emailService = emailService;
            _userRepository = userRepository;
        }

        #endregion

        /// <summary>
        /// Cadastro de Usuários
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterContract model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.Select(e => e.Errors).FirstOrDefault());

            try
            {
                var user = model.UserRegister2Back();

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                    return BadRequest("A senha deve ter de 6 a 20 caracteres e conter letra maiúscula, minúscula, caracter especial e número.");

                var createdUser = await _userManager.FindByEmailAsync(model.Email);

                await _signInManager.SignInAsync(user, false);

                var response = AccountExtension.ResponseLogin2Front(
                    TokenRoleService.GenerateJwtRoles(createdUser, "", _appSettings),
                    DateTime.Now.AddHours(_appSettings.Expiration),
                    user.Email,
                    user.Name);

                return Ok(response);
            }
            catch (CustomErrorException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserLoginContract model)
        {
            //if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.Select(e => e.Errors).FirstOrDefault());

            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                    return NotFound("Usuário não encontrado.");

                var role = await _userManager.GetRolesAsync(user);

                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, true);

                if (result.IsLockedOut)
                    return StatusCode(StatusCodes.Status423Locked, "Sua conta foi bloquada temporariamente, por favor tente mais tarde.");

                if (result.Succeeded)
                {
                    var response = AccountExtension.ResponseLogin2Front(
                    TokenRoleService.GenerateJwtRoles(user, role.Count > 0 ? role[0] : "", _appSettings),
                    DateTime.Now.AddHours(_appSettings.Expiration),
                    user.Email,
                    user.Name);

                    return Ok(response);
                }

                return BadRequest("Usuário ou Senha inválidos.");
            }
            catch (CustomErrorException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Validar token de usuário
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("token")]
        public ActionResult TokenValidate()
        {
            var userNameInToken = User.Identity.Name;
            var userIsAuthenticatedInToken = User.Identity.IsAuthenticated;

            return Ok();
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Ok();
            }
            catch (CustomErrorException ex)
            {
                return BadRequest(ex.Message);
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
        /// <returns></returns>                        
        //[Authorize(Roles = "Master,Admin")]
        [AllowAnonymous]
        [HttpPost]
        [Route("resetpassword")]
        public async Task<ActionResult> ResetPassword([FromBody] UserResetPasswordContract model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.Select(e => e.Errors).FirstOrDefault());

            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                    return NotFound("Usuário não encontrado.");

                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

                if (!result.Succeeded)
                    return BadRequest("Senha não alterada. Verifique o padrão da senha informada.");

                return Ok("Senha alterada com sucesso.");
            }
            catch (CustomErrorException ex)
            {
                return BadRequest(ex.Message);
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
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("forgotpassword")]
        public async Task<ActionResult> ForgotPassword([FromBody] UserForgotPasswordContract model)
        {
            //if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.Select(e => e.Errors).FirstOrDefault());

            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                    return NotFound("Usuário não encontrado.");

                if (await _userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    //método original para resetar password
                    //var passwordResetLink = Url.Action("resetpassword", "account", new { email = model.Email, token = token }, Request.Scheme);

                    //envia email
                    //var emailResponse = await _emailService.SendEmailAsync(model.Email, "Recuperar Senha");

                    //if (!response)
                    //    return base.Ok("E-mail não enviado. Por favor tente mais tarde.");

                    var response = AccountExtension.ResponseLogin2Front(
                    token,
                    DateTime.Now.AddHours(_appSettings.Expiration),
                    user.Email,
                    user.Name);

                    return base.Ok(response);
                }

                return BadRequest("Este usuário não possui email confirmado.");
            }
            catch (CustomErrorException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Buscar usuários
        /// </summary>
        /// <returns></returns>
        /// 
        [Authorize]
        [HttpGet("user")]
        public async Task<ActionResult> GetUsers()
        {
            try
            {
                return Ok(await _userRepository.GetUsers());
            }
            catch (CustomErrorException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Buscar usuário por email
        /// </summary>
        /// <returns></returns>
        /// 
        [Authorize]
        [HttpGet("user/{email}")]
        public async Task<ActionResult> GetUsersByEmail(string email)
        {
            try
            {
                return Ok(await _userRepository.GetUserByEmail(email));
            }
            catch (CustomErrorException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}