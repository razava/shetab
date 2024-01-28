﻿using Domain.Models.Relational.Common;
using Infrastructure.Storage;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Uploads.Commands.CreateUpload;

public sealed record AddUploadCommand(
    string UserId, IFormFile File, AttachmentType AttachmentType) : IRequest<Result<Upload>>;

