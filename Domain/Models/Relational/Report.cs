using System.ComponentModel.DataAnnotations.Schema;
using Domain.Messages;
using Domain.Primitives;

namespace Domain.Models.Relational;

public class Report : Entity
{
    //Constructors
    private Report(Guid id): base(id) { }
    private Report(
        Guid id,
        string citizenId,
        string phoneNumber,
        Category category,
        string comments,
        Address address,
        ICollection<Guid> attachments,
        Visibility visibility = Visibility.EveryOne,
        Priority priority = Priority.Normal,
        bool isIdentityVisible = true) : base(id)
    {
        var now = DateTime.UtcNow;

        CitizenId = citizenId;
        CategoryId = category.Id;
        Comments = comments;
        Medias = attachments;
        IsIdentityVisible = isIdentityVisible;
        Address = address;
        Sent = now;
        //TODO: Update report state to contain more info and use ToString to get description instead of LastStatus been stored in database.
        ReportState = ReportState.NeedAcceptance;
        LastStatus = "ثبت درخواست در سامانه";
        LastStatusDateTime = now;
        Deadline = now.AddHours(category.Duration);
        ResponseDeadline = category.ResponseDuration == null ? null : now.AddHours(category.ResponseDuration.Value);
        Visibility = visibility;
        TrackingNumber = generateTrackingNumber(phoneNumber);
        Priority = priority;
    }
    public int ShahrbinInstanceId { get; private set; }
    public ShahrbinInstance ShahrbinInstance { get; private set; } = null!;
    public DateTime Sent { get; private set; }
    public DateTime? Finished { get; private set; }
    public DateTime? Responsed { get; private set; }
    public DateTime Deadline { get; private set; }
    public DateTime? ResponseDeadline { get; private set; }
    public DateTime LastStatusDateTime { get; private set; }
    public double? Duration { get; private set; }
    public double? ResponseDuration { get; private set; }


    public string CitizenId { get; private set; } = string.Empty;
    [ForeignKey("CitizenId")]
    public ApplicationUser Citizen { get; private set; } = null!;
    public string? RegistrantId { get; private set; }
    [ForeignKey("RegistrantId")]
    public ApplicationUser? Registrant { get; private set; }

    public int CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;
    public Address Address { get; private set; } = null!;
    public string TrackingNumber { get; private set; } = null!;
    //public int? PriorityId { get; private set; }
    public Priority Priority { get; private set; }
    //public int VisibilityId { get; private set; }
    public Visibility Visibility { get; private set; }
    public ICollection<Guid> Medias { get; private set; } = new List<Guid>();
    public string Comments { get; private set; } = string.Empty;
    public ReportState ReportState { get; private set; }
    public string LastStatus { get; private set; } = string.Empty;

    //Process infos
    public int ProcessId { get; private set; }
    public Process Process { get; private set; } = null!;
    public int? CurrentStageId { get; private set; }
    public ProcessStage? CurrentStage { get; private set; }
    public int? LastTransitionId { get; private set; }
    public ProcessTransition? LastTransition { get; private set; }
    public int? LastReasonId { get; private set; }
    public ProcessReason? LastReason { get; private set; }
    public string CurrentActorsStr { get; private set; } = string.Empty;
    public ICollection<Actor> CurrentActors { get; private set; } = new List<Actor>();
    public ICollection<TransitionLog> TransitionLogs { get; private set; } = new List<TransitionLog>();

    public ICollection<Message> Messages { get; private set; } = new List<Message>();

    public string? ExecutiveId { get; private set; }
    [ForeignKey("ExecutiveId")]
    public ApplicationUser? Executive { get; private set; }
    public string? ContractorId { get; private set; }
    [ForeignKey("ContractorId")]
    public ApplicationUser? Contractor { get; private set; }
    public string? InspectorId { get; private set; }
    [ForeignKey("InspectorId")]
    public ApplicationUser? Inspector { get; private set; }

    [InverseProperty("ReportsLiked")]
    public ICollection<ApplicationUser> LikedBy { get; private set; } = new List<ApplicationUser>();
    public int Likes { get; private set; }

    public ICollection<Comment> FeedbackComments { get; private set; } = new List<Comment>();
    public int CommentsCount { get; private set; }

    public ICollection<Violation> Violations { get; private set; } = new List<Violation>();
    public ReportFlags Flags { get; private set; }
    public bool IsIdentityVisible { get; private set; }
    public bool IsObjectioned { get; private set; }
    public bool IsFeedbacked { get; private set; }
    public int? Rating { get; private set; }


    
    //Factory methods
    public static Report NewByCitizen(
        string citizenId,
        string phoneNumber,
        Category category,
        string comments,
        Address address,
        ICollection<Guid> attachments,
        Visibility visibility,
        Priority priority = Priority.Normal,
        bool isIdentityVisible = true)
    {
        var now = DateTime.UtcNow;

        var report = new Report(
            Guid.NewGuid(),
            citizenId,
            phoneNumber,
            category,
            comments,
            address,
            attachments,
            visibility,
            priority,
            isIdentityVisible);


        if (!category.EditingAllowed)
        {
            report.InitProcess();
        }

        var log = new TransitionLog
        {
            DateTime = now,
            ReportId = report.Id,
            Message = ReportMessages.Created
        };

        report.TransitionLogs.Add(log);

        return report;
    }

    public static Report NewByOperator(
        string operatorId,
        string citizenId,
        string phoneNumber,
        Category category,
        string comments,
        Address address,
        ICollection<Guid> attachments,
        Visibility visibility,
        Priority priority = Priority.Normal,
        bool isIdentityVisible = true)
    {
        var now = DateTime.UtcNow;

        var report = new Report(
            Guid.NewGuid(),
            citizenId,
            phoneNumber,
            category,
            comments,
            address,
            attachments,
            visibility,
            priority,
            isIdentityVisible);

        report.RegistrantId = operatorId;

        report.InitProcess();

        var log = new TransitionLog
        {
            ActorIdentifier = operatorId,
            ActorType = ActorType.Person,
            DateTime = now,
            ReportId = report.Id,
            Message = ReportMessages.Created
        };

        report.TransitionLogs.Add(log);

        return report;
    }

    public void InitProcess()
    {
        var now = DateTime.UtcNow;
        var firstStage = Process.Stages.OrderBy(p => p.Id).First();
        CurrentStageId = firstStage.Id;
        LastStatus = firstStage.Status;
        LastStatusDateTime = now;
    }

    public void Accept(
        string operatorId,
        Category? category,
        string? comments,
        Address? address,
        ICollection<Guid>? attachments,
        Visibility? visibility)
    {
        updateReport(category, comments, address, attachments, visibility);
        ReportState = ReportState.Live;
        LastStatus = "تأیید درخواست در سامانه";
        LastStatusDateTime = DateTime.UtcNow;
        Visibility = Visibility.Operators;
        Priority = Priority.Normal;
        InitProcess();
        var log = new TransitionLog()
        {
            ReportId = Id,
            ActorIdentifier = operatorId,
            Message = ""
        };
        TransitionLogs.Add(log);
    }

    public void Update(
        string operatorId,
        Category? category,
        string? comments,
        Address? address,
        ICollection<Guid>? attachments,
        Visibility? visibility)
    {
        updateReport(category, comments, address, attachments, visibility);
        var log = new TransitionLog()
        {
            ReportId = Id,
            ActorIdentifier = operatorId,
            //Message should reflect changes
            Message = ""
        };
        TransitionLogs.Add(log);
    }

    public Message MessageToCitizen(
        string actorIdentifier,
        ActorType actorType,
        List<Guid> attachments,
        string message,
        string comment,
        bool isPublic = true)
    {
        var now = DateTime.UtcNow;

        Responsed = now;
        ResponseDuration = (now - Sent).TotalSeconds;

        var log = new TransitionLog()
        {
            ActorIdentifier = actorIdentifier,
            ActorType = actorType,
            Attachments = attachments,
            Comment = comment,
            DateTime = now,
            IsPublic = isPublic,
            Message = message,
        };
        TransitionLogs.Add(log);


        var resultMessage = new Message()
        {
            ShahrbinInstanceId = ShahrbinInstanceId,
            Title = "پاسخ به شهروند" + " - " + TrackingNumber,
            Content = message,
            DateTime = now,
            MessageType = MessageType.Report,
            SubjectId = Id,
            Recepients = new List<MessageRecepient>()
                {
                        new MessageRecepient() { Type = RecepientType.Person, ToId = CitizenId }
                }
        };

        return resultMessage;
    }

    //Private methods
    private void updateReport(
        Category? category,
        string? comments,
        Address? address,
        ICollection<Guid>? attachments,
        Visibility? visibility)
    {
        var now = DateTime.UtcNow;

        if (category is not null)
        {
            CategoryId = Category.Id;
            Category = category;
            Deadline = now.AddHours(category.Duration);
            ResponseDeadline = category.ResponseDuration == null ? null : now.AddHours(category.ResponseDuration.Value);
            InitProcess();
        }

        Comments = comments ?? Comments;
        Medias = attachments ?? Medias;
        Address = address ?? Address;
        Visibility = visibility ?? Visibility;

        return;
    }

    private string generateTrackingNumber(string userName)
    {
        Random random = new Random();
        var result = userName.Substring(userName.Length - 4);
        result += random.Next(111111, 999999).ToString();

        return result;
    }

    private void autoTransition()
    {
        if (ReportState == ReportState.Finished || ReportState == ReportState.Accepted)
            return;

        var currentStage = Process.Stages.FirstOrDefault(p => p.Id == CurrentStageId);
        if (currentStage == null)
            throw new Exception();


        var autoActors = currentStage.Actors.Where(p => p.Type == ActorType.Auto).ToList();
        foreach (var autoActor in autoActors)
        {
            //Revise this
            var bot = autoActor.BotActors
                .Where(p => p.Id == autoActor.Identifier)
                .SingleOrDefault();
            if (bot == null)
                throw new Exception("Bot not found.");

            if (bot.ReasonMeaning == null || LastReason != null && LastReason.ReasonMeaning == bot.ReasonMeaning)
            {
                makeTransition(bot.TransitionId, bot.ReasonId, new List<Guid>(), "", ActorType.Auto, bot.Id, new List<int>());
                break;
            }
        }
    }

    private void makeTransition(
        int transitionId,
        int? reasonId,
        List<Guid> attachments,
        string comment,
        ActorType actorType,
        string actorIdentifier,
        List<int> actorIds,
        bool isExecutive = false,
        bool isContractor = false)
    {
        var now = DateTime.UtcNow;

        var transition = Process.Transitions.SingleOrDefault(p => p.Id == transitionId);
        if (transition == null)
            throw new Exception("Transition cannot be null here.");

        var reason = transition.ReasonList.Where(p => p.Id == reasonId).SingleOrDefault();
        if (reason == null)
        {
            throw new Exception("Reason cannot be null here.");
        }

        //report.Priority = transitionInfo.Priority != null ? transitionInfo.Priority.Value : report.Priority;
        //Visibility = transitionInfo.Visibility ?? report.Visibility;
        CurrentStageId = transition.ToId;
        LastStatus = transition.To.Status;
        ReportState = transition.ReportState;
        LastStatusDateTime = now;
        LastTransitionId = transition.Id;
        LastReasonId = reasonId;
        //Reasin is needed in autoTransition but should not be updated in database
        if (reason != null)
        {
            LastReason = reason;
        }


        //Check if the actor is an executive or contractor, if so, set the executor or contractor of the report
        if (actorType == ActorType.Person)
        {
            if (isExecutive)
            {
                ExecutiveId = actorIdentifier;
            }
            else if (isContractor)
            {
                ContractorId = actorIdentifier;
            }
        }


        if (actorIds.Count == 0)
        {
            actorIds = transition.To.Actors.Select(p => p.Id).ToList();
        }

        CurrentActorsStr = "";
        actorIds.ForEach(p => CurrentActorsStr = $"{CurrentActorsStr}|{p}");
        CurrentActorsStr = CurrentActorsStr + "|";

        var duration = now - LastStatusDateTime;

        var log = new TransitionLog
        {
            Attachments = attachments,
            Comment = comment,
            DateTime = now,
            ReportId = Id,
            TransitionId = transitionId,
            Message = transition.Message,
            ActorType = actorType,
            ActorIdentifier = actorIdentifier,
            ReasonId = reasonId,
            Duration = duration.TotalSeconds,
            IsPublic = transition.IsTransitionLogPublic
        };

        TransitionLogs.Add(log);

        autoTransition();
    }

}

