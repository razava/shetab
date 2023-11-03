using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ProcessAggregate;
using MediatR;

namespace Application.Users.Commands.CreateContractor;

internal class CreateContractorCommandHandler : IRequestHandler<CreateContractorCommand, ApplicationUser>
{
    private readonly IUserRepository _userRepository;
    private readonly IActorRepository _actorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateContractorCommandHandler(
        IUserRepository userRepository,
        IActorRepository actorRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _actorRepository = actorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationUser> Handle(CreateContractorCommand request, CancellationToken cancellationToken)
    {
        var successMessage = "کاربر با موفقیت ایجاد شد.";

        var executive = await _userRepository.GetSingleAsync(e => e.Id == request.ExecutiveId, true, "Contractors");
        if (executive is null)
            throw new Exception("Not found.");

        var isExecutive = await _userRepository.IsInRoleAsync(executive, "Executive");
        if (!isExecutive)
            throw new Exception("Only executives can define and assign contractors.");
        var contractor = await _userRepository.GetSingleAsync(u => u.UserName == "c-" + request.PhoneNumber);
        if (contractor is not null)
        {
            var isAlreadyAContractor = await _userRepository.IsInRoleAsync(contractor, "Contractor");
            if (isAlreadyAContractor)
            {
                successMessage = $"کاربر با شماره {contractor.UserName} به نام {contractor.FirstName + " " + contractor.LastName} پیش از این ثبت نام شده و به لیست پیمانکاران شما افزوده شد.";
            }
        }
        else
        {
            contractor = new ApplicationUser()
            {
                UserName = "c-" + request.PhoneNumber,
                PhoneNumber = request.PhoneNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Organization = request.Organization,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var password = "aA@12345";
            var result = await _userRepository.CreateAsync(contractor, password);
            if (!result.Succeeded)
                throw new Exception("User creation failed.");

            var result2 = await _userRepository.AddToRoleAsync(contractor, "Contractor");

            if (!result2.Succeeded)
            {
                await _userRepository.DeleteAsync(contractor);
                throw new Exception("User creation failed");
            }
            _actorRepository.Insert(
                new Actor(){
                    Identifier = contractor.Id,
                    Type = ActorType.Person,
                });

            //TODO: Inform contractor
            //await new CommunicationServices(_smsOptions).SendContractorCreatedAsync(model.PhoneNumber, password);
        }

        executive.Contractors.Add(contractor);
        _userRepository.Update(executive);
        
        await _unitOfWork.SaveAsync();

        return contractor;
    }
}
