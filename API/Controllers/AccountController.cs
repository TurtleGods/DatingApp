using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(AppDbContext context, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")]// account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        // if (await UserExists(registerDto.DisplayName))
        //     return BadRequest("Username is taken");
        // return Ok();
        if (await EmailExist(registerDto.Email)) return BadRequest("Email taken");
        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            DisplayName = registerDto.DisplayName.ToLower(),
            Email = registerDto.Email,
            UserName = registerDto.Email,
            Member = new Member
            {
                DisplayName = registerDto.DisplayName,
                Gender = registerDto.Gender,
                City = registerDto.City,
                Country = registerDto.Country,
                DateOfBirth = registerDto.DateOfBirth
            }
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user.ToDto(tokenService);
    }
    private async Task<bool> EmailExist(string email)
    {
        return await context.Users.AnyAsync(x => x.Email!.ToLower() == email.ToLower());
    }
    [HttpPost("login")]// account/login
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await context.Users.SingleOrDefaultAsync(x => x.Email == loginDto.Email);
        if (user == null) return Unauthorized("Invalid email");

        return user.ToDto(tokenService);
    }
}
