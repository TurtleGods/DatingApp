using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users = await userRepository.GetMembersAsync();
        return Ok(users);
    }

    [HttpGet("{username}")] //api/users/1
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        var user = await userRepository.GetMemberAsync(username);

        if (user == null)
            return NotFound();
        return user;
    }
    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {        //entity framework will track the changes to the user entity
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null)
            return BadRequest("Count not find user");

        // Map the updated properties from the DTO to the user entity
        mapper.Map(memberUpdateDto, user);

        if (await userRepository.SaveAllAsync())
        {
            return NoContent(); // 204 No Content
        }

        return BadRequest("Failed to update user");

    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null)
            return BadRequest("Cannot update user");
        var result = await photoService.AddPhotoAsync(file);
        if (result.Error != null)
        {
            return BadRequest(result.Error.Message);
        }

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        user.Photos.Add(photo);
        if (await userRepository.SaveAllAsync())
        {
            return mapper.Map<PhotoDto>(photo);
        }
        return BadRequest("Problem adding photo");
    }
    
}
