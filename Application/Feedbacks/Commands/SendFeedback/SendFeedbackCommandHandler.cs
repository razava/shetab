using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace Application.Feedbacks.Commands.SendFeedback;

internal sealed class SendFeedbackCommandHandler : IRequestHandler<SendFeedbackCommand, bool>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SendFeedbackCommandHandler(IUnitOfWork unitOfWork, IFeedbackRepository feedbackRepository)
    {
        _unitOfWork = unitOfWork;
        _feedbackRepository = feedbackRepository;
    }

    public async Task<bool> Handle(SendFeedbackCommand request, CancellationToken cancellationToken)
    {
        var _random = new Random();
        Feedback? fb = null;
        var fbs = (await _feedbackRepository.GetAsync(p => p.ReportId == request.reportId)).ToList();
        if (fbs.Count > 0)
            fb = fbs[0];

        if (fb != null)
        {
            fb.Creation = DateTime.UtcNow;
            fb.LastSent = null;
            fb.ReportId = request.reportId;
            fb.UserId = request.citizenId;
            fb.Token = _random.Next(10000, 99999).ToString() + _random.Next(10000, 99999).ToString();
            fb.TryCount = 0;

            _feedbackRepository.Update(fb);
        }
        else
        {
            _feedbackRepository.Insert(new Feedback()
            {
                ShahrbinInstanceId = request.instanceId,
                Creation = DateTime.UtcNow,
                LastSent = null,
                ReportId = request.reportId,
                UserId = request.citizenId,
                Token = _random.Next(10000, 99999).ToString() + _random.Next(10000, 99999).ToString()
            });
        }

        await _unitOfWork.SaveAsync();
        return true;
    }
}
