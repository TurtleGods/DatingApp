using System;
using System.Data.Common;
using API.Interfaces;

namespace API.Data;

public class UnitOfWork(AppDbContext context) : INuitOfWork
{
    private IMemberRepository? _memberRepository;
    private IMessageRepository? _messageRepository;
    private ILikesRepository? _likesRepository;
    public IMemberRepository MemberRepository => _memberRepository
        ??= new MemberRepository(context);

    public IMessageRepository MessageRepository => _messageRepository
        ??= new MessageRepository(context);

    public ILikesRepository LikesRepository => _likesRepository
        ??= new LikesRepository(context);

    public async  Task<bool> Complete()
    {
        try
        {
            return await context.SaveChangesAsync() > 0;
        }
        catch (DbException ex)
        {
            throw new Exception("An erro occured while saving change", ex);
        }
    }

    public bool HasChanges()
    {
        return context.ChangeTracker.HasChanges();
    }
}
