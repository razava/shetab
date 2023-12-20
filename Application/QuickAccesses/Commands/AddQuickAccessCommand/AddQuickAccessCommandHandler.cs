using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Infrastructure.Storage;
using MediatR;

namespace Application.QuickAccesses.Commands.AddQuickAccessCommand;

internal sealed class AddQuickAccessCommandHandler : IRequestHandler<AddQuickAccessCommand, QuickAccess>
{
    private readonly IQuickAccessRepository _quickAccessRepository;
    private readonly IStorageService _storageService;
    private readonly IUnitOfWork _unitOfWork;

    public AddQuickAccessCommandHandler(
        IQuickAccessRepository quickAccessRepository,
        IUnitOfWork unitOfWork,
        IStorageService storageService)
    {
        _quickAccessRepository = quickAccessRepository;
        _unitOfWork = unitOfWork;
        _storageService = storageService;
    }

    public async Task<QuickAccess> Handle(AddQuickAccessCommand request, CancellationToken cancellationToken)
    {
        var media = await _storageService.WriteFileAsync(request.Image, AttachmentType.News);
        if (media is null)
            throw new SaveImageFailedException();

        var quickAccess = new QuickAccess()
        {
            ShahrbinInstanceId = request.InstanceId,
            CategoryId = request.CategoryId,
            Title = request.Title,
            Media = media,
            Order = request.Order,
            IsDeleted = request.IsDeleted,
        };

        _quickAccessRepository.Insert(quickAccess);
        await _unitOfWork.SaveAsync();

        return quickAccess;
    }
}
