using Core.Models.Base;
using Domain.Contracts.Account.Extensions;
using Domain.Contracts.User.Extensions;
using Domain.Contracts.User;
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
        /// Cadastrar Usuário
        /// </summary>
        /// <param name="model"></param>  
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseLoginOutput), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(UserRegisterInput model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.Select(e => e.Errors).FirstOrDefault());

            try
            {
                var user = model.ConverterToUserModel();

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                    return BadRequest("A senha deve ter de 6 a 20 caracteres e conter letra maiúscula, minúscula, caracter especial e número.");

                var createdUser = await _userManager.FindByEmailAsync(model.Email);

                await _signInManager.SignInAsync(user, false);

                var response = AccountExtension.ConvertToResponseLoginContract(
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
        /// Atualizar Usuário
        /// </summary>
        /// <param name="model"></param>
        [Authorize]
        [HttpPut]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(UserUpdateInput model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.Select(e => e.Errors).FirstOrDefault());

            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                    return NotFound();

                var result = await _userManager.UpdateAsync(UserExtension.ConverterToUserModelForUpdate(ref user, ref model));

                if (!result.Succeeded)
                    return BadRequest();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Atualizar Senha do Usuário
        /// </summary>
        /// <param name="model"></param>
        [Authorize]
        [HttpPatch]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PartUpdate(PartUpdateUser model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.Select(e => e.Errors).FirstOrDefault());

            if (!model.IsValid())
                return BadRequest("A nova senha não pode ser igual a atual");

            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                    return NotFound();

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (!result.Succeeded)
                    return BadRequest();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Deletar Usuário
        /// </summary>
        //[Authorize(Roles = "Master,Admin")]
        [Authorize]
        [HttpDelete("{email}")]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest();

            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                    return NotFound();

                await _userManager.DeleteAsync(user);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Buscar Usuário Por Email
        /// </summary>
        //[Authorize(Roles = "Master,Admin")]
        [Authorize]
        [HttpGet("{email}")]
        [ProducesResponseType(typeof(UserOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Buscar Todos Usuários
        /// </summary>
        //[Authorize(Roles = "Master,Admin")]
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(UserOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _userRepository.GetUsers();

                if (!response.Any())
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
