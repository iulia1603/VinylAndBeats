using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VinylAndBeats.DTOs;
using VinylAndBeats.Models;
using VinylAndBeats.Services;

namespace VinylAndBeats.Controllers.Api;

[ApiController]
[Route("api/auth")]
public class AuthApiController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtService _jwtService;

    public AuthApiController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return Unauthorized(new { message = "Email sau parolă incorectă" });

        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!result.Succeeded)
            return Unauthorized(new { message = "Email sau parolă incorectă" });

        var token = await _jwtService.GenerateTokenAsync(user);
        return Ok(new { token, expiresIn = _jwtService.ExpiresInSeconds });
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (await _userManager.FindByEmailAsync(dto.Email) != null)
            return BadRequest(new { message = "Există deja un cont cu acest email" });

        var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email, FullName = dto.FullName };
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });

        await _userManager.AddToRoleAsync(user, "User");
        var token = await _jwtService.GenerateTokenAsync(user);
        return Ok(new { token, expiresIn = _jwtService.ExpiresInSeconds });
    }
}