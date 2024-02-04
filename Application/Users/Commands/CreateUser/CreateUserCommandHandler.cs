using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.IdentityAggregate;
using MediatR;

namespace Application.Users.Commands.CreateUser;

internal class CreateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<CreateUserCommand, Result<ApplicationUser>>
{

    public async Task<Result<ApplicationUser>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        ApplicationUser user = new()
        {
            ShahrbinInstanceId = request.InstanceId,
            UserName = request.Username,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Title = request.Title,
            PhoneNumberConfirmed = true
        };
        await userRepository.CreateAsync(user, request.Password);
        
        return user;
    }
}
