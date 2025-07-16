using System;
using System.Collections.Generic;
using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface IMemberRepository
{
    void Update(Member user);
    Task<bool> SaveAllAsync();
    Task<IReadOnlyList<Member>> GetMembersAsync();
    Task<Member?> GetMemberByIdAsync(string id);
    Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(string memberId);
    Task<Member?> GetMemberForUpdate(string id);

}
