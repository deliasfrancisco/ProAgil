using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProAgil.Domain.Identity;
using ProAgil.WebAPI.Dtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProAgil.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IConfiguration _config;
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IMapper _mapper;

		public UserController(IConfiguration config, UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
		{
			_config = config;
			_userManager = userManager;
			_signInManager = signInManager;
			_mapper = mapper;
		}

		[HttpGet("GetUser")]
		public async Task<IActionResult> GetUser()
		{
			return Ok(new UserDto());
		}

		[HttpPost("Register")]
		[AllowAnonymous]
		public async Task<IActionResult> Register(UserDto userDto)
		{
			try
			{
				var user = _mapper.Map<User>(userDto);
				var result = await _userManager.CreateAsync(user, userDto.Password);

				var userToReturn = _mapper.Map<UserDto>(user);

				if (result.Succeeded)
				{
					return Created("GetUser", userToReturn);
				}

				return BadRequest(result.Errors);
			}
			catch (Exception ex)
			{
				return this.StatusCode(StatusCodes.Status500InternalServerError, ex + " Consulta falhou falhou");
			}
		}

		[HttpPost("Login")]
		[AllowAnonymous]
		public async Task<IActionResult> Login(UserLoginDto userLogin)
		{
			try
			{
				// Vai no banco de dados e verifica se o usuário tem algum nome
				var user = await _userManager.FindByNameAsync(userLogin.UserName);
				// Se tiver é feito a verificação da senha e não trava caso a senha esteja errada
				var result = await _signInManager.CheckPasswordSignInAsync(user, userLogin.Password, false);

				if (result.Succeeded)
				{
					// Se deu sucesso atribui o usuário encontrado na variavel
					var appUser = await _userManager.Users
						.FirstOrDefaultAsync(x => x.NormalizedUserName == userLogin.UserName.ToUpper());

					// E é feito o mapeamento para retornar uma dto
					var userToReturn = _mapper.Map<UserLoginDto>(appUser);

					return Ok(new { 
						// Gera o token baseado no usuário encontrado no _userManager
						token = GenerateJWToken(appUser).Result,
						user = userToReturn
					});
				}
				// Caso de algo errado retorna não autorizado
				return Unauthorized();
			}
			catch (Exception ex)
			{
				return this.StatusCode(StatusCodes.Status500InternalServerError, ex + " Consulta falhou falhou");
			}
		}

		private async Task<string> GenerateJWToken(User user)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.UserName)
			};

			var roles = await _userManager.GetRolesAsync(user);

			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(1),
				SigningCredentials  =creds
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}
	}
}
