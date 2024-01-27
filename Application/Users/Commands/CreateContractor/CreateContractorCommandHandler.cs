using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ProcessAggregate;
using MediatR;

namespace Application.Users.Commands.CreateContractor;

internal class CreateContractorCommandHandler(
    IUserRepository userRepository,
    IActorRepository actorRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateContractorCommand, Result<ApplicationUser>>
{
    
    public async Task<Result<ApplicationUser>> Handle(CreateContractorCommand request, CancellationToken cancellationToken)
    {
        //todo
        var successMessage = "کاربر با موفقیت ایجاد شد.";

        var executive = await userRepository.GetSingleAsync(e => e.Id == request.ExecutiveId, true, "Contractors");
        if (executive is null)
            return NotFoundErrors.Executive;

        var isExecutive = await userRepository.IsInRoleAsync(executive, "Executive");
        if (!isExecutive)
            return AccessDeniedErrors.Executive;
        var contractor = await userRepository.GetSingleAsync(u => u.UserName == "c-" + request.PhoneNumber);
        if (contractor is not null)
        {
            var isAlreadyAContractor = await userRepository.IsInRoleAsync(contractor, "Contractor");
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
                Title = request.Title,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var password = "aA@12345";
            await userRepository.RegisterWithRoleAsync(contractor, password, "Contractor");

            /*
            var result = await userRepository.CreateAsync(contractor, password);
            //todo : review this for returning result from repository & exception throwing.
            var result2 = await userRepository.AddToRoleAsync(contractor, "Contractor");

            if (!result2.Succeeded)
            {
                await userRepository.DeleteAsync(contractor);
                throw new CreationFailedException("کاربر");
            }
            */
            actorRepository.Insert(
                new Actor(){
                    Identifier = contractor.Id,
                    Type = ActorType.Person,
                });

            //TODO: Inform contractor
            //await new CommunicationServices(_smsOptions).SendContractorCreatedAsync(model.PhoneNumber, password);
        }

        executive.Contractors.Add(contractor);
        userRepository.Update(executive);
        
        await unitOfWork.SaveAsync();

        return contractor;
    }
}
