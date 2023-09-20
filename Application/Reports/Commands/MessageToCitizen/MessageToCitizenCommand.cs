using Application.Reports.Common;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Commands.MessageToCitizen;

public sealed record MessageToCitizenCommand(
    Guid reportId,
    string ActorIdentifier,
    ActorType ActorType,
    List<Guid> Attachments,
    string Comment,
    bool IsPublic,
    string Message) : IRequest<Report>; //TODO: Fix the return typd

