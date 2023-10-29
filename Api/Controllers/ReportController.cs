using Api.Abstractions;
using Api.Dtos;
using Api.Services.Authentication;
using Application.Common.Interfaces.Persistence;
using Application.Reports.Commands.AcceptByOperator;
using Application.Reports.Commands.CreateReportByCitizen;
using Application.Reports.Commands.CreateReportByOperator;
using Application.Reports.Commands.UpdateByOperator;
using Application.Reports.Common;
using Application.Reports.Queries.GetPossibleTransitions;
using Application.Reports.Queries.GetReports;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class ReportController : ApiController
{
    public ReportController(ISender sender) : base(sender)
    {
    }

    [Authorize(Roles = "Citizen")]
    [HttpPost]
    public async Task<ActionResult<CreateReportDto>> CreateReport(int instanceId, [FromForm] CreateReportDto model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var username = User.FindFirstValue(ClaimTypes.Name);
        if(userId==null || username==null)
        {
            return Unauthorized();
        }

        var phoneNumber = username;
        var addressInfo = new AddressInfo(
            model.Address.RegionId!.Value,
            model.Address.Street,
            model.Address.Valley,
            model.Address.Detail,
            model.Address.Number,
            model.Address.PostalCode,
            model.Address.Latitude!.Value,
            model.Address.Longitude!.Value);

        var command = new CreateReportByCitizenCommand(
            instanceId,
            userId,
            phoneNumber,
            model.CategoryId,
            model.Comments,
            addressInfo,
            model.Attachments,
            model.IsIdentityVisible);
        var report = await Sender.Send(command);

        //TODO: Fix this
        return CreatedAtAction(null, null);
    }


    [Authorize(Roles = "Operator")]
    [HttpPost("RegisterByOperator")]
    public async Task<ActionResult<Guid>> CreateReportByOperator([FromForm] OperatorCreateReportDto model)
    {
        var instanceIdStr = User.FindFirstValue(AppClaimTypes.InstanceId);
        if (instanceIdStr == null)
        {
            return BadRequest();
        }
        var instanceId = int.Parse(instanceIdStr);
        if (instanceId <= 0)
        {
            return BadRequest();
        }
        var operatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (operatorId == null)
        {
            return Unauthorized();
        }

        var addressInfo = new AddressInfo(
            model.Address.RegionId!.Value,
            model.Address.Street,
            model.Address.Valley,
            model.Address.Detail,
            model.Address.Number,
            model.Address.PostalCode,
            model.Address.Latitude!.Value,
            model.Address.Longitude!.Value);

        var command = new CreateReportByOperatorCommand(
            instanceId,
            operatorId,
            model.PhoneNumber,
            model.FirstName,
            model.LastName,
            model.CategoryId,
            model.Comments,
            addressInfo,
            model.Attachments,
            model.IsIdentityVisible,
            model.Visibility == Visibility.EveryOne);


        var report = await Sender.Send(command);

        return Ok(report.Id);
    }

    [Authorize(Roles = "Operator")]
    [HttpPut]
    public async Task<ActionResult<Guid>> UpdateReportByOperator([FromForm] UpdateReportDto model)
    {
        var instanceIdStr = User.FindFirstValue(AppClaimTypes.InstanceId);
        if (instanceIdStr == null)
        {
            return BadRequest();
        }
        var instanceId = int.Parse(instanceIdStr);
        if (instanceId <= 0)
        {
            return BadRequest();
        }
        var operatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (operatorId == null)
        {
            return Unauthorized();
        }

        AddressInfo? addressInfo = null;
        if (model.Address != null)
        {
            addressInfo = new AddressInfo(
                model.Address.RegionId!.Value,
                model.Address.Street,
                model.Address.Valley,
                model.Address.Detail,
                model.Address.Number,
                model.Address.PostalCode,
                model.Address.Latitude!.Value,
                model.Address.Longitude!.Value);

        }
        //TODO: Visibility should be considerd
        var command = new UpdateByOperatorCommand(
            model.Id,
            operatorId,
            model.CategoryId,
            model.Comments,
            addressInfo,
            model.Attachments,
            model.Visibility == Visibility.EveryOne);
        await Sender.Send(command);

        return NoContent();
    }

    [Authorize(Roles = "Operator")]
    [HttpPut("Accept")]
    public async Task<ActionResult<Guid>> AcceptReportByOperator([FromForm] UpdateReportDto model)
    {
        var instanceIdStr = User.FindFirstValue(AppClaimTypes.InstanceId);
        if (instanceIdStr == null)
        {
            return BadRequest();
        }
        var instanceId = int.Parse(instanceIdStr);
        if (instanceId <= 0)
        {
            return BadRequest();
        }
        var operatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (operatorId == null)
        {
            return Unauthorized();
        }

        AddressInfo? addressInfo = null;
        if (model.Address != null)
        {
            addressInfo = new AddressInfo(
                model.Address.RegionId!.Value,
                model.Address.Street,
                model.Address.Valley,
                model.Address.Detail,
                model.Address.Number,
                model.Address.PostalCode,
                model.Address.Latitude!.Value,
                model.Address.Longitude!.Value);

        }
        //TODO: Visibility should be considerd
        var command = new AcceptByOperatorCommand(
            model.Id,
            operatorId,
            model.CategoryId,
            model.Comments,
            addressInfo,
            model.Attachments,
            model.Visibility == Visibility.EveryOne);
        await Sender.Send(command);

        return NoContent();
    }

    [Authorize(Roles = "Operator")]
    [HttpGet("PossibleTransitions/{id:Guid}")]
    public async Task<ActionResult<List<Application.Reports.Queries.GetPossibleTransitions.PossibleTransitionDto>>> GetPossibleTransitions(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized();

        //TODO: Get this from token
        var instanceId = 1;

        var command = new GetPossibleTransitionsQuery(id, userId, instanceId);
        var result = await Sender.Send(command);

        return result;
    }

    //TODO: Define access policy
    [Authorize]
    [HttpPost("MakeTransitions/{id:Guid}")]
    public async Task<ActionResult> MakeTransition(Guid id, MakeTransitionDto model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized();

        MakeTransitionCommand command = new(
            id,
            model.TransitionId,
            model.ReasonId,
            model.Attachments,
            model.Comment,
            userId,
            model.ActorIds,
            User.IsInRole("Executive"),
            User.IsInRole("Contractor"));
        var result = await Sender.Send(command);

        return Ok();
    }

    [Authorize]
    [HttpGet("Reports")]
    public async Task<ActionResult<List<Report>>> GetReports([FromQuery] PagingInfo pagingInfo, int instanceId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized();

        var query = new GetReportsQuery(pagingInfo, userId, instanceId);
        var result = await Sender.Send(query);
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.Meta));
        return Ok(result.ToList());
    }

    public record MakeTransitionDto(
    int TransitionId,
    int ReasonId,
    List<Guid> Attachments,
    string Comment,
    List<int> ActorIds)
    {
        public List<Guid> Attachments { get; init; } = Attachments ?? new List<Guid>();
        public string Comment { get; init; } = Comment ?? "";
    }
        /*
        [Authorize(Roles = "Citizen")]
        [HttpGet("Like")]
        public async Task<ActionResult<int>> Like(Guid reportId, bool isLiked)
        {
            var user = await _userManager.GetUserAsync(User);
            var report = await _context.Reports
                .Where(p => p.Id == reportId)
                .Include(p => p.LikedBy)
                .SingleOrDefaultAsync();

            if (report == null)
            {
                return NotFound();
            }

            if (isLiked)
            {
                if (report.LikedBy.Any(p => p.Id == user.Id))
                {
                    //Already liked!
                }
                else
                {
                    report.LikedBy.Add(user);
                    report.Likes = report.LikedBy.Count;
                }
            }
            else
            {
                if (report.LikedBy.Any(p => p.Id == user.Id))
                {
                    report.LikedBy.Remove(user);
                    report.Likes = report.LikedBy.Count;
                }
                else
                {
                    //Not liked, do nothing
                }
            }

            _context.Entry(report).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return report.Likes;
        }

        [Authorize(Roles = "Citizen")]
        [HttpPost("Comment/{reportId}")]
        public async Task<IActionResult> PostCommentByCitizen(int instanceId, Guid reportId, CreateCommentDto comment)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var model = new Comment()
            {
                ShahrbinInstanceId = instanceId,
                DateTime = DateTime.Now,
                IsSeen = false,
                IsVerified = true,
                ReplyId = null,
                Text = comment.Comment,
                UserId = userId,
                ReportId = reportId
            };

            var report = await _context.Reports
                .Where(p => p.Id == reportId)
                .Include(p => p.FeedbackComments)
                .SingleOrDefaultAsync();

            if (report == null)
            {
                return NotFound();
            }

            report.FeedbackComments
                .Add(model);
            //Consider verified and not verified comments. If verification is needed then CommentsCount should be updated in Tasks controller not here.
            report.CommentsCount = report.FeedbackComments.Count;
            await _context.SaveChangesAsync();

            return Ok();
        }
        */
        /*





        /// <summary>
        /// This microservice returns all the reports that current citizen has made.
        /// </summary>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Citizen")]
        [HttpGet]
        public async Task<ActionResult<List<CitizenGetReportDto>>> GetCitizensReports([FromQuery] PagingInfo pagingInfo)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var query = _context.Reports
                .AsNoTracking()
                //.Include(p => p.CurrentActors)
                .Where(p => p.CitizenId == userId)
                .Include(p => p.Citizen)
                .ThenInclude(p => p.Avatar)
                .Include(p => p.Category)
                .Include(p => p.Address)
                .Include(p => p.Medias)
                .Include(p => p.TransitionLogs)
                .ThenInclude(p => p.Reason)
                .Include(p => p.LikedBy.Where(q => q.Id == userId))
                .Include(p => p.Messages)
                .OrderByDescending(p => p.Sent);

            if (pagingInfo.CategoryType != CategoryType.All)
            {
                query = (IOrderedQueryable<Report>)query.Where(p => p.Category.CategoryType == pagingInfo.CategoryType);
            }

            if (pagingInfo.Query != null && pagingInfo.Query.Length >= 3)
            {
                query.Where(p => p.TrackingNumber.Contains(pagingInfo.Query));
            }

            var reports = await PagedList<Report>.ToPagedList(
                query,
                pagingInfo.PageNumber,
                pagingInfo.PageSize);

            var metadata = new
            {
                reports.TotalCount,
                reports.PageSize,
                reports.CurrentPage,
                reports.TotalPages,
                reports.HasNext,
                reports.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return _mapper.Map<List<CitizenGetReportDto>>(reports);
        }

        /// <summary>
        /// This microservice returns all the reports that current citizen has made.
        /// </summary>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Citizen")]
        [HttpGet("All")]
        public async Task<ActionResult<List<CitizenGetReportDto>>> GetAllReports([FromQuery] PagingInfo pagingInfo)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = _context.Reports
                .AsNoTracking()
                .Where(p => p.Visibility == Visibility.EveryOne)
                .Include(p => p.Citizen)
                .ThenInclude(p => p.Avatar)
                .Include(p => p.Registrant)
                .ThenInclude(p => p.Avatar)
                //.Include(p => p.CurrentActors)
                .Include(p => p.Category)
                .Include(p => p.Address)
                .Include(p => p.Medias)
                .Include(p => p.LikedBy.Where(q => q.Id == userId))
                .Include(p => p.TransitionLogs)
                .ThenInclude(p => p.Reason)
                .OrderByDescending(p => p.Sent);

            if (pagingInfo.CategoryType != CategoryType.All)
            {
                query = (IOrderedQueryable<Report>)query.Where(p => p.Category.CategoryType == pagingInfo.CategoryType);
            }

            if (pagingInfo.Query != null && pagingInfo.Query.Length >= 3)
            {
                query = (IOrderedQueryable<Report>)query.Where(p => p.TrackingNumber.Contains(pagingInfo.Query));
            }

            if (pagingInfo.CategoryIds.Count() > 0)
            {
                query = (IOrderedQueryable<Report>)query.Where(p => pagingInfo.CategoryIds.Contains(p.CategoryId));
            }

            if (pagingInfo.SentFromDate != null)
            {
                query = (IOrderedQueryable<Report>)query.Where(p => p.Sent >= pagingInfo.SentFromDate);
            }

            if (pagingInfo.SentToDate != null)
            {
                query = (IOrderedQueryable<Report>)query.Where(p => p.Sent < pagingInfo.SentToDate);
            }

            var reports = await PagedList<Report>.ToPagedList(
                query,
                pagingInfo.PageNumber,
                pagingInfo.PageSize);

            var metadata = new
            {
                reports.TotalCount,
                reports.PageSize,
                reports.CurrentPage,
                reports.TotalPages,
                reports.HasNext,
                reports.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return _mapper.Map<List<CitizenGetReportDto>>(reports);
        }



        [Authorize(Roles = "Citizen")]
        [HttpGet("Comment/{reportId}")]
        public async Task<ActionResult<List<GetCommentForCitizenDto>>> GetComments(Guid reportId, [FromQuery] PagingInfo pagingInfo)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = _context.Comment
                .AsNoTracking()
                .Where(p => p.ReportId == reportId && p.IsVerified)
                .Include(p => p.Reply)
                .Include(p => p.User)
                .ThenInclude(p => p.Avatar);

            var comments = await PagedList<Comment>.ToPagedList(
                query,
                pagingInfo.PageNumber,
                pagingInfo.PageSize);

            var metadata = new
            {
                comments.TotalCount,
                comments.PageSize,
                comments.CurrentPage,
                comments.TotalPages,
                comments.HasNext,
                comments.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var result = _mapper.Map<List<GetCommentForCitizenDto>>(comments);
            result.ForEach(p => p.CanDelete = p.User.Id == userId);
            result.ForEach(p => p.User.Id = "");    //For security
            return result;
        }

        [Authorize(Roles = "Citizen")]
        [HttpDelete("Comment/{commentId}")]
        public async Task<ActionResult<List<GetCommentForCitizenDto>>> DeleteComment(Guid commentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var comment = await _context.Comment
                .Where(p => p.Id == commentId)
                .Include(p => p.Reply)
                .Include(p => p.Report)
                .ThenInclude(p => p.FeedbackComments)
                .SingleOrDefaultAsync();

            if (comment == null)
            {
                return NotFound();
            }

            if (comment.UserId != userId)
            {
                return Unauthorized();
            }
            comment.Report.CommentsCount = comment.Report.FeedbackComments.Count - 1;
            _context.Entry(comment.Report).State = EntityState.Modified;

            if (comment.Reply != null)
            {
                _context.Entry(comment.Reply).State = EntityState.Deleted;
            }
            _context.Entry(comment).State = EntityState.Deleted;

            await _context.SaveChangesAsync();


            return Ok();
        }

        [Authorize(Roles = "Citizen")]
        [HttpGet("Nearest")]
        public async Task<ActionResult<List<CitizenGetReportDto>>> GetNearestReports([FromQuery] PagingInfo pagingInfo)
        {
            if (pagingInfo.Latitude == null || pagingInfo.Longitude == null)
            {
                return BadRequest();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var reportLocations = await _context.Reports
                .AsNoTracking()
                .Where(p => p.Visibility == Visibility.EveryOne)
                .Where(p => p.Address.Latitude != null && p.Address.Longitude != null)
                .Include(p => p.Address)
                .Select(p => new ReportLocation() { ReportId = p.Id, Latitude = p.Address.Latitude.Value, Longitude = p.Address.Longitude.Value })
                .ToListAsync();

            reportLocations.ForEach(p => p.Distance =
                Utilities.Utilities.CalculateDistance(
                    p.Latitude, p.Longitude,
                    pagingInfo.Latitude.Value, pagingInfo.Longitude.Value));

            var query = reportLocations.OrderBy(p => p.Distance).AsQueryable();
            var result = PagedList<ReportLocation>.ToPagedListSync(
                query,
                pagingInfo.PageNumber,
                pagingInfo.PageSize);

            var metadata = new
            {
                result.TotalCount,
                result.PageSize,
                result.CurrentPage,
                result.TotalPages,
                result.HasNext,
                result.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var reportIds = result.Select(p => p.ReportId).ToList();

            var reports = await _context.Reports
                .AsNoTracking()
                .Where(p => reportIds.Contains(p.Id))
                .Include(p => p.Citizen)
                .ThenInclude(p => p.Avatar)
                .Include(p => p.Registrant)
                .ThenInclude(p => p.Avatar)
                //.Include(p => p.CurrentActors)
                .Include(p => p.Category)
                .Include(p => p.Address)
                .Include(p => p.Medias)
                .Include(p => p.LikedBy.Where(q => q.Id == userId))
                .Include(p => p.TransitionLogs)
                .ThenInclude(p => p.Reason)
                .ToListAsync();

            reports = reports.OrderBy(p => reportIds.IndexOf(p.Id)).ToList();

            return _mapper.Map<List<CitizenGetReportDto>>(reports);
        }

        [Authorize(Roles = "Citizen")]
        [HttpPost("Objection/{reportId}")]
        public async Task<IActionResult> PostObjectionByCitizen(int instanceId, Guid reportId, [FromForm] ObjectionDto objectionDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var report = await _context.Reports
                .Where(p => p.Id == reportId)
                //.Include(p => p.CurrentActors)
                .Include(p => p.Address)
                .Include(p => p.Category)
                .Include(p => p.Citizen)
                .Include(p => p.Medias)
                .SingleOrDefaultAsync();

            if (report == null)
            {
                return NotFound();
            }

            if (report.CitizenId != userId)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { Status = "Error", Message = "امکان ثبت درخواست بررسی مجدد برای این گزارش وجود ندارد." });
            }

            if (report.ReportState != ReportState.Finished)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { Status = "Error", Message = "امکان ثبت درخواست بررسی مجدد در این مرحله وجود ندارد." });
            }

            if (report.Category.ObjectionAllowed == false)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { Status = "Error", Message = "امکان درخواست بررسی مجدد وجود ندارد." });
            }


            var processManager = new ProcessManager(instanceId, _settings, _context, report, _mapper, _messaging, _hub, _userManager, _hostEnvironment, _imageQualities);


            var model = _mapper.Map<ObjectionModel>(objectionDto);

            if (objectionDto.Attachments != null && objectionDto.Attachments.Count > 0)
            {
                model.Medias = await Utilities.Utilities.WriteFile(objectionDto.Attachments, _hostEnvironment.WebRootPath, Utilities.Utilities.AttachmentType.Report, _imageQualities);
                if (objectionDto.Attachments.Count != model.Medias.Count)
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new { Status = "Error", Message = "Attachment failed!" });
                }
            }


            await processManager.MakeObjection(model);
            return Ok();
        }


        [Authorize(Roles = "Operator")]
        [HttpPut("UpdateComments/{id}")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] UpdateReportCommentsDto comments)
        {
            var instanceId = int.Parse(User.FindFirstValue(AppClaimTypes.InstanceId));
            if (instanceId <= 0)
            {
                return BadRequest();
            }

            var report = await _context.Reports
                .Where(p => p.Id == id)
                .SingleOrDefaultAsync();
            if (report == null || report.ShahrbinInstanceId != instanceId)
                return NotFound();

            report.Comments = comments.Comments;
            _context.Entry(report).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "PowerUser")]
        [HttpGet("DeleteReport/{trackingNumbers}")]
        public async Task<IActionResult> DeleteReport(string trackingNumbers)
        {
            int total = 0, failed = 0;
            var trs = trackingNumbers.Split(' ');
            foreach (var trackingNumber in trs)
            {
                if (string.IsNullOrEmpty(trackingNumber))
                {
                    continue;
                }
                try
                {
                    var report = await _context.Reports
                    .Where(p => p.TrackingNumber == trackingNumber)
                    .Include(p => p.Medias)
                    .Include(p => p.TransitionLogs)
                    .ThenInclude(p => p.Attachments)
                    .SingleOrDefaultAsync();

                    foreach (var transitionLog in report.TransitionLogs)
                    {
                        foreach (var media in transitionLog.Attachments)
                        {
                            _context.Entry(media).State = EntityState.Deleted;
                        }
                    }
                    await _context.SaveChangesAsync();

                    foreach (var transitionLog in report.TransitionLogs)
                    {
                        _context.Entry(transitionLog).State = EntityState.Deleted;
                    }
                    await _context.SaveChangesAsync();

                    foreach (var media in report.Medias)
                    {
                        _context.Entry(media).State = EntityState.Deleted;
                    }
                    await _context.SaveChangesAsync();

                    var comments = await _context.Comment.Where(p => p.ReportId == report.Id).ToListAsync();
                    comments.ForEach(p => _context.Entry(p).State = EntityState.Deleted);
                    await _context.SaveChangesAsync();

                    _context.Entry(report).State = EntityState.Deleted;

                    await _context.SaveChangesAsync();
                }
                catch
                {
                    failed++;
                }
                total++;
            }


            return Ok($"success: {total - failed}, failed:{failed}");
        }
        */
    }
