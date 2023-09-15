using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Api.Services.Sms;
using Api.Services.MemoryCaching;
using Domain.Data;
using Domain.Models.Relational;
using Api.Hubs;
using AutoMapper;
using Api.Services.Business.ProcessManagement;
using Message = Domain.Models.Relational.Message;
using Mapster;
using Domain.Messages;

namespace Api.Services.Business;

public class ProcessManager
{
    private readonly IStaticSettings _settings;
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<NewEventHub, INewEventClient> _hub;
    private readonly Random _random;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProcessManager(
        IStaticSettings settings,
        ApplicationDbContext dbContext,
        IHubContext<NewEventHub, INewEventClient> hub,
        UserManager<ApplicationUser> userManager)
    {
        _settings = settings;
        _context = dbContext;
        _hub = hub;
        _random = new Random();
        _userManager = userManager;
    }

    public async Task<Report> CreateReportAsync(int instanceId, string citizenId, string phoneNumber, ReportCreateModel reportInfo)
    {
        var category = getCategory(instanceId, reportInfo.CategoryId);
        var address = reportInfo.Address.Adapt<Address>();
        var report = Report.NewByCitizen(
            citizenId,
            phoneNumber,
            category,
            reportInfo.Comments,
            address,
            reportInfo.Attachments,
            Visibility.EveryOne,
            Priority.Normal,
            reportInfo.IsIdentityVisible);


        _context.Reports.Add(report);

        await CommunicationServices.AddNotification(
            new Message()
            {
                ShahrbinInstanceId = report.ShahrbinInstanceId,
                Title = "ثبت درخواست" + " - " + report.TrackingNumber,
                Content = ReportMessages.Created,
                DateTime = report.Sent,
                MessageType = MessageType.Report,
                SubjectId = report.Id,
                Recepients = new List<MessageRecepient>()
                {
                    new MessageRecepient() { Type = RecepientType.Person, ToId = report.CitizenId }
                }
            },
            _context);
        await _context.SaveChangesAsync();

        //TODO: Inform related users not all
        await _hub.Clients.All.Update();

        return report;
    }

    public async Task<Report> CreateReportByOperatorAsync(int instanceId, string operatorId, string phoneNumber, string firstName, string lastName, ReportCreateModel reportInfo)
    {
        var user = await GetOrCreateCitizen(phoneNumber, firstName, lastName);

        var category = getCategory(instanceId, reportInfo.CategoryId);
        var address = reportInfo.Address.Adapt<Address>();
        var report = Report.NewByOperator(
            operatorId,
            user.Id,
            user.PhoneNumber!,
            category,
            reportInfo.Comments,
            address,
            reportInfo.Attachments,
            Visibility.EveryOne,
            Priority.Normal,
            reportInfo.IsIdentityVisible);

        _context.Reports.Add(report);
        await _context.SaveChangesAsync();

        await CommunicationServices.AddNotification(
            new Message()
            {
                ShahrbinInstanceId = report.ShahrbinInstanceId,
                Title = "ثبت درخواست" + " - " + report.TrackingNumber,
                Content = ReportMessages.Created,
                DateTime = report.Sent,
                MessageType = MessageType.Report,
                SubjectId = report.Id,
                Recepients = new List<MessageRecepient>()
                {
                    new MessageRecepient() { Type = RecepientType.Person, ToId = report.CitizenId }
                }
            },
            _context);
        await _context.SaveChangesAsync();

        //TODO: Inform related users not all
        await _hub.Clients.All.Update();

        return report;
    }

    private async Task<ApplicationUser> GetOrCreateCitizen(string phoneNumber, string firstName, string lastName)
    {
        var user = await _userManager.FindByNameAsync(phoneNumber);
        if (user == null)
        {
            user = new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = phoneNumber,
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed = false,
                FirstName = firstName,
                LastName = lastName
            };
            //TODO: Generate password randomly
            var password = "aA@12345";
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception("User creation failed.", null);

            var result2 = await _userManager.AddToRoleAsync(user, "Citizen");

            if (!result2.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                throw new Exception("Role assignment failed.", null);
            }

            //TODO: Send the user its credentials
        }

        return user;
    }

    public async Task<Report> AcceptAsync(Guid reportId, string operatorId, ReportAcceptModel reportInfo)
    {
        var report = await _context.Reports.Where(p => p.Id == reportId)
            .Include(p => p.Address)
            .Include(p => p.Medias)
            .Include(p => p.Process)
            .Include(p => p.Category)
            .Include(p => p.TransitionLogs)
            .SingleOrDefaultAsync();
        if (report == null)
            throw new Exception("Report not found");

        Category? category = null;
        if (reportInfo.CategoryId is not null)
        {
            category = getCategory(report.ShahrbinInstanceId, reportInfo.CategoryId.Value);
        }
        Address? address = null;
        if (reportInfo.Address is not null)
        {
            address = report.Address.Adapt<Address>();
        }

        report.Accept(operatorId, category, reportInfo.Comments, address, reportInfo.Attachments, null);

        await _context.SaveChangesAsync();

        return report;
    }

    public async Task<Report> UpdateAsync(Guid reportId, string operatorId, ReportUpdateModel reportInfo)
    {
        var report = await _context.Reports.Where(p => p.Id == reportId)
            .Include(p => p.Address)
            .Include(p => p.Medias)
            .Include(p => p.Process)
            .Include(p => p.Category)
            .Include(p => p.TransitionLogs)
            .SingleOrDefaultAsync();
        if (report == null)
            throw new Exception("Report not found");

        Category? category = null;
        if (reportInfo.CategoryId is not null)
        {
            category = getCategory(report.ShahrbinInstanceId, reportInfo.CategoryId.Value);
        }
        Address? address = null;
        if (reportInfo.Address is not null)
        {
            address = report.Address.Adapt<Address>();
        }

        report.Update(operatorId, category, reportInfo.Comments, address, reportInfo.Attachments, null);

        await _context.SaveChangesAsync();

        return report;
    }

    private Category getCategory(int instanceId, int categoryId)
    {
        var category = _settings.InstanceSettings[instanceId].Categories.Data.Where(p => p.Id == categoryId).SingleOrDefault();
        if (category == null)
            throw new Exception("Category not found.");
        return category;
    }

    public async Task SendMessageToCitizen(Guid reportId, ReplyCitizenModel reply)
    {
        var report = await _context.Reports
            .Where(r => r.Id == reportId)
            .Include(p => p.TransitionLogs)
            .SingleOrDefaultAsync();
        if (report == null)
        {
            throw new Exception("Report not found!");
        }

        var message = report.MessageToCitizen(reply.ActorIdentifier, reply.ActorType, reply.Attachments, reply.Message, reply.Comment);
        await _context.SaveChangesAsync();

        await CommunicationServices.AddNotification(message, _context);
    }

    private async Task sendFeedback(int instanceId, Guid reportId, string citizenId, DateTime now)
    {
        var fb = await _context.Feedback.Where(p => p.ReportId == reportId).FirstOrDefaultAsync();
        if (fb != null)
        {
            fb.Creation = now;
            fb.LastSent = null;
            fb.ReportId = reportId;
            fb.UserId = citizenId;
            fb.Token = _random.Next(10000, 99999).ToString() + _random.Next(10000, 99999).ToString();
            fb.TryCount = 0;
        }
        else
        {
            _context.Feedback.Add(new Feedback()
            {
                ShahrbinInstanceId = instanceId,
                Creation = now,
                LastSent = null,
                ReportId = reportId,
                UserId = citizenId,
                Token = _random.Next(10000, 99999).ToString() + _random.Next(10000, 99999).ToString()
            });
        }
    }
}
