using Core.Models.Base;
using Domain.Contracts.Account.Extensions;
using Domain.Contracts.User.Extensions;
using Domain.Contracts.User.Input;
using Domain.Interfaces.Repository;
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

namespace Api.Controllers
{
    [Authorize]
    [Route("v1/users")]
    [ApiController]
    public class UsersController : Controller
    {
        #region Dependency Injection

        private readonly SignInManager<UserModel> _signInManager;
        private readonly UserManager<UserModel> _userManager;
        private readonly AppSettings _appSettings;
        private readonly IUserRepository _userRepository;

        public UsersController(
           SignInManager<UserModel> signInManager,
           UserManager<UserModel> userManager,
           IOptions<AppSettings> appSettings,
           IUserRepository userRepository
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
        }

        #endregion

        /// <summary>
        /// Cadastro de Usuários
        /// </summary>
        /// <param name="model"></param>
        /// <response code="201">Criado com sucesso</response>
        /// <response code="400">Payload incorreto</response>    
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterContract model)
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

                return Created("", response);
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
        /// <response code="200">Busca de lista de usuário bem sucedida</response>
        /// <response code="400">Payload incorreto</response>
        //[Authorize(Roles = "Master,Admin")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                return Ok(await _userRepository.GetUsers());
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
        /// <response code="200">Busca de usuário bem sucedida</response>
        /// <response code="400">Payload incorreto</response>
        /// <response code="404">Usuário não encontrado</response>
        //[Authorize(Roles = "Master,Admin")]
        [Authorize]
        [HttpGet("{email}")]
        public async Task<IActionResult> GetUsersByEmail(string email)
        {
            try
            {
                return Ok(await _userRepository.GetUserByEmail(email));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
