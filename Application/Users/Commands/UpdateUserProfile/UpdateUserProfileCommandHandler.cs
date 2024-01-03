using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.IdentityAggregate;
using MediatR;

namespace Application.Users.Commands.UpdateUserProfile;

internal class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, ApplicationUser>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateUserProfileCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationUser> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetSingleAsync(u => u.Id == request.UserId);
        if (user is null)
            throw new NotFoundException("کاربر");
        user.FirstName = request.FirstName ?? user.FirstName;
        user.LastName = request.LastName ?? user.LastName;
        user.Title = request.Title ?? user.Title;
        user.Organization = request.Organization ?? user.Organization;
        user.NationalId = request.NationalId ?? user.NationalId;
        user.Gender = request.Gender ?? user.Gender;
        user.Education = request.Education ?? user.Education;
        user.BirthDate = request.BirthDate ?? user.BirthDate;
        user.PhoneNumber2 = request.PhoneNumber2 ?? user.PhoneNumber2;

        _userRepository.Update(user);
        await _unitOfWork.SaveAsync();

        return user;
    }
}
