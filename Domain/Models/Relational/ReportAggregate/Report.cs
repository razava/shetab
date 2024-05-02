using System.ComponentModel.DataAnnotations.Schema;
using Amazon.Auth.AccessControlPolicy;
using Domain.Exceptions;
using Domain.Messages;
using Domain.Models.ComplaintAggregate.Events;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ProcessAggregate;
using Domain.Models.Relational.ReportAggregate;
using Domain.Primitives;

namespace Domain.Models.Relational;

public class Report : Entity
{
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

    public int CategoryId { get; private set; }
    public Category Category { get; set; } = null!;
    public Address Address { get; private set; } = null!;
    public string TrackingNumber { get; private set; } = null!;
    public Priority Priority { get; private set; }
    public Visibility Visibility { get; private set; }
    public ICollection<Media> Medias { get; private set; } = new List<Media>();
    public string Comments { get; private set; } = string.Empty;
    public ReportState ReportState { get; private set; }
    public string LastStatus { get; private set; } = string.Empty;

    //Process infos
    public int ProcessId { get; private set; }
    public Process Process { get; set; } = null!;
    public int? CurrentStageId { get; private set; }
    public ProcessStage? CurrentStage { get; private set; }
    public int? LastTransitionId { get; private set; }
    public ProcessTransition? LastTransition { get; private set; }
    public int? LastReasonId { get; private set; }
    public ProcessReason? LastReason { get; private set; }
    public int? CurrentActorId { get; set; }
    public Actor? CurrentActor { get; set; }
    public ICollection<TransitionLog> TransitionLogs { get; private set; } = new List<TransitionLog>();
    public ReportOperationType? LastOperation { get; set; }


    //People
    public string CitizenId { get; private set; } = string.Empty;
    [ForeignKey("CitizenId")]
    public ApplicationUser Citizen { get; private set; } = null!;
    public string? RegistrantId { get; private set; }
    [ForeignKey("RegistrantId")]
    public ApplicationUser? Registrant { get; private set; }
    public string? ExecutiveId { get; private set; }
    [ForeignKey("ExecutiveId")]
    public ApplicationUser? Executive { get; private set; }
    public string? ContractorId { get; private set; }
    [ForeignKey("ContractorId")]
    public ApplicationUser? Contractor { get; private set; }
    public string? InspectorId { get; private set; }
    [ForeignKey("InspectorId")]
    public ApplicationUser? Inspector { get; private set; }

    //Social
    [InverseProperty("ReportsLiked")]
    public ICollection<ApplicationUser> LikedBy { get; private set; } = new List<ApplicationUser>();
    public int Likes { get; private set; }
    public ICollection<Comment> FeedbackComments { get; private set; } = new List<Comment>();
    public int CommentsCount { get; private set; }
    public ICollection<Violation> Violations { get; private set; } = new List<Violation>();
    public ICollection<Message> Messages { get; private set; } = new List<Message>();
    public Guid? FeedbackId { get; private set; }
    public Feedback? Feedback { get; private set; }
    public int? Rating { get; private set; }
    public Satisfaction? Satisfaction { get; set; }
    public int ViolationCount { get; private set; }
    public bool IsViolationChecked { get; private set; }


    //Status
    public ReportFlags Flags { get; private set; }
    public bool IsIdentityVisible { get; private set; }
    public bool IsObjectioned { get; private set; }
    public bool IsFeedbacked { get; private set; }
    public bool IsDeleted { get; private set; }


    #region Constructors
    private Report(Guid id) : base(id) { }
    private Report(
        Guid id,
        string citizenId,
        string phoneNumber,
        Category category,
        string comments,
        Address address,
        List<Media> attachments,
        Visibility visibility = Visibility.EveryOne,
        Priority priority = Priority.Normal,
        bool isIdentityVisible = true) : base(id)
    {
        var now = DateTime.UtcNow;

        ShahrbinInstanceId = category.ShahrbinInstanceId;
        CitizenId = citizenId;
        CategoryId = category.Id;
        ProcessId = category.ProcessId ?? throw new CategoryHaveNoAssignedProcess();
        Process = category.Process ?? throw new NotLoadedProcessException();
        Comments = comments;
        Medias = attachments;
        IsIdentityVisible = isIdentityVisible;
        Address = address;
        Sent = now;
        //TODO: Update report state to contain more info and use ToString to get description instead of LastStatus been stored in database.
        LastStatusDateTime = now;
        Deadline = now.AddHours(category.Duration);
        ResponseDeadline = category.ResponseDuration == null ? null : now.AddHours(category.ResponseDuration.Value);
        Visibility = visibility;
        TrackingNumber = generateTrackingNumber(phoneNumber);
        Priority = priority;
    }
    #endregion

    #region Factory methods
    public static Report NewByCitizen(
        string citizenId,
        string phoneNumber,
        Category category,
        string comments,
        Address address,
        List<Media> attachments,
        Visibility visibility,
        bool isIdentityVisible = true)
    {
        var report = new Report(
            Guid.NewGuid(),
            citizenId,
            phoneNumber,
            category,
            comments,
            address,
            attachments,
            visibility,
            category.DefaultPriority,
            isIdentityVisible);

        //TODO: Handle the case where need acceptance is false
        report.ReportState = ReportState.NeedAcceptance;
        report.LastStatus = TransitionMessages.NeedAcceptance;
        report.LastOperation = ReportOperationType.Created;

        if (!category.EditingAllowed)
        {
            report.InitProcess();
        }

        var log = TransitionLog.CreateNewReport(report.Id, ActorType.Person, citizenId);

        report.TransitionLogs.Add(log);

        
        var message =
            new Message()
            {
                ShahrbinInstanceId = report.ShahrbinInstanceId,
                Title = "ثبت درخواست" + " - " + report.TrackingNumber,
                Content = $"درخواست شما با کد رهگیری {report.TrackingNumber} در سامانه ثبت شد.",
                DateTime = report.Sent,
                MessageSubject = MessageSubject.Report,
                MessageSendingType = MessageSendingType.Both,
                SubjectId = report.Id,
                Recepient = MessageRecepient.Create(RecepientType.Person, report.CitizenId)
            };
        report.Messages.Add(message);

        report.Raise(new ReportDomainEvent(
            Guid.NewGuid(),
            ReportDomainEventTypes.CreatedByCitizen,
            report));

        return report;
    }

    public static Report NewByOperator(
        string operatorId,
        string citizenId,
        string phoneNumber,
        Category category,
        string comments,
        Address address,
        List<Media> attachments,
        Visibility visibility,
        Priority priority = Priority.Normal,
        bool isIdentityVisible = true)
    {
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

        var log = TransitionLog.CreateNewReport(report.Id, ActorType.Person, operatorId);

        report.LastOperation = ReportOperationType.Created;

        report.TransitionLogs.Add(log);
        
        report.InitProcess();

        var message =
            new Message()
            {
                ShahrbinInstanceId = report.ShahrbinInstanceId,
                Title = "ثبت درخواست" + " - " + report.TrackingNumber,
                Content = $"درخواست شما با کد رهگیری {report.TrackingNumber} در سامانه ثبت شد.",
                DateTime = report.Sent,
                MessageSubject = MessageSubject.Report,
                MessageSendingType = MessageSendingType.Both,
                SubjectId = report.Id,
                Recepient = MessageRecepient.Create(RecepientType.Person, report.CitizenId)
            };
        report.ReportState = ReportState.Live;
        report.LastStatus = "ثبت درخواست در سامانه";
        report.Messages.Add(message);

        report.Raise(new ReportDomainEvent(
            Guid.NewGuid(),
            ReportDomainEventTypes.CreatedByOperator,
            report));
        return report;
    }
    #endregion

    #region PublicMethods
    public void InitProcess()
    {
        var now = DateTime.UtcNow;
        var firstStage = Process.Stages.Where(p => p.Name == "Citizen").First();
        CurrentStageId = firstStage.Id;
        LastStatus = firstStage.Status;
        LastStatusDateTime = now;
        autoTransition();
    }

    public void Accept(
        string operatorId,
        Category? category,
        string? comments,
        Address? address,
        List<Media>? attachments,
        Visibility? visibility,
        Priority? priority)
    {
        updateReport(category, comments, address, attachments, visibility, priority);
        ReportState = ReportState.Live;
        LastStatus = "تأیید درخواست در سامانه";
        LastStatusDateTime = DateTime.UtcNow;
        LastOperation = ReportOperationType.Approved;

        InitProcess();
        var log = TransitionLog.CreateApproved(Id, operatorId);

        TransitionLogs.Add(log);
    }

    public void Update(
        string operatorId,
        Category? category,
        string? comments,
        Address? address,
        List<Media>? attachments,
        Visibility? visibility, 
        Priority? priority)
    {
        updateReport(category, comments, address, attachments, visibility, priority);
        //TODO: Log modified values
        var comment = "";
        var log = TransitionLog.CreateUpdate(Id, comment ?? "", operatorId);
        LastOperation = ReportOperationType.Change;
        TransitionLogs.Add(log);
    }

    public void UpdateComments(string comments)
    {
        Comments = comments;
    }

    public Message MessageToCitizen(
        string actorIdentifier,
        List<Media> attachments,
        string comment)
    {
        var now = DateTime.UtcNow;

        Responsed = now;
        ResponseDuration = (now - Sent).TotalSeconds;

        var log = TransitionLog.CreateMessageToCitizen(Id, comment, attachments, actorIdentifier, ResponseDuration ?? 0);

        LastOperation = ReportOperationType.MessageToCitizen;
        TransitionLogs.Add(log);


        var resultMessage = new Message()
        {
            ShahrbinInstanceId = ShahrbinInstanceId,
            Title = "پاسخ به شهروند" + " - " + TrackingNumber,
            Content = "پاسخ به شهروند" + " - " + TrackingNumber + "\n" + comment,
            DateTime = now,
            MessageSubject = MessageSubject.Report,
            MessageSendingType = MessageSendingType.Both,
            SubjectId = Id,
            Recepient = MessageRecepient.Create(RecepientType.Person, CitizenId)
        };

        Messages.Add(resultMessage);

        Raise(new ReportDomainEvent(
            Guid.NewGuid(),
            ReportDomainEventTypes.Responsed,
            this));

        return resultMessage;
    }

    public void MakeTransition(
        int transitionId,
        int reasonId,
        List<Media> attachments,
        string comment,
        ActorType actorType,
        string actorIdentifier,
        int toActorId,
        bool isExecutive = false,
        bool isContractor = false)
    {
        makeTransition(
            transitionId,
            reasonId,
            attachments,
            comment,
            actorType,
            actorIdentifier,
            toActorId,
            isExecutive,
            isContractor);

        var now = DateTime.UtcNow;
        var duration = (now - LastStatusDateTime).TotalSeconds;

        var log = TransitionLog.CreateTransition(
            Id,
            transitionId,
            comment,
            attachments,
            TransitionMessages.Refered,
            actorType,
            actorIdentifier,
            reasonId,
            duration,
            true);

        TransitionLogs.Add(log);

        LastOperation = ReportOperationType.Transition;

        
        if (ReportState == ReportState.Finished || ReportState == ReportState.AcceptedByCitizen)
        {
            Finished = now;
            Raise(new ReportDomainEvent(
                Guid.NewGuid(),
                ReportDomainEventTypes.Finished,
                this));

            Random random = new Random();
            Duration = (now - Sent).TotalSeconds;
            LastStatus = "پایان یافته";
            if (Feedback != null)
            {
                Feedback.Creation = now;
                Feedback.LastSent = null;
                Feedback.ReportId = Id;
                Feedback.UserId = CitizenId;
                Feedback.Token = random.Next(10000, 99999).ToString() + random.Next(10000, 99999).ToString();
                Feedback.TryCount = 0;
            }
            else
            {
                Feedback = new Feedback()
                {
                    ShahrbinInstanceId = ShahrbinInstanceId,
                    Creation = now,
                    LastSent = null,
                    ReportId = Id,
                    UserId = CitizenId,
                    Token = random.Next(10000, 99999).ToString() + random.Next(10000, 99999).ToString()
                };
            }

            Raise(new ReportDomainEvent(
                Guid.NewGuid(),
                ReportDomainEventTypes.Refered,
                this));
        }
    }

    public void MakeObjection(
        List<Media> attachments,
        string comment)
    {
        if (ReportState != ReportState.Finished)
        {
            throw new Exception("Processing the request is not completed yet.");
        }

        var objectionStage = Process.Stages.Where(s => s.Name == "Inspector").FirstOrDefault();
        if (objectionStage is null) 
        {
            throw new Exception("No objection stage found.");
        }
        var transition = Process.Transitions
            .Where(t => t.FromId == CurrentStageId && t.ToId == objectionStage.Id)
            .FirstOrDefault();
        if (transition is null)
        {
            throw new Exception("No objection transition found.");
        }

        makeTransition(
            transition.Id,
            transition.ReasonList.First().Id,
            attachments,
            comment,
            ActorType.Person,
            CitizenId,
            objectionStage.Actors.First().Id);

        var now = DateTime.UtcNow;
        var duration = (now - LastStatusDateTime).TotalSeconds;

        var log = TransitionLog.CreateObjection(
            Id,
            comment,
            attachments,
            TransitionMessages.Refered,
            CitizenId);

        TransitionLogs.Add(log);

        LastOperation = ReportOperationType.Objection;

        Raise(new ReportDomainEvent(
                Guid.NewGuid(),
                ReportDomainEventTypes.Objectioned,
                this));
    }

    public void MakeObjectionByInspector(
        List<Media> attachments,
        string comment,
        string userId)
    {
        var objectionStage = Process.Stages.Where(s => s.Name == "Inspector").FirstOrDefault();
        if (objectionStage is null)
        {
            throw new Exception("No objection stage found.");
        }

        CurrentStageId = objectionStage.Id;
        CurrentActorId = objectionStage.Actors.FirstOrDefault()?.Id ??
            throw new Exception("No actor defined as inspector");
        IsObjectioned = true;
        ReportState = ReportState.Review;
        LastStatus = "بررسی توسط واحد بازرسی";
        LastTransitionId = null;

        var log = TransitionLog.CreateObjection(
            Id,
            comment,
            attachments,
            "انتقال به واحد بازرسی جهت بررسی",
            userId);
        TransitionLogs.Add(log);

        LastOperation = ReportOperationType.Objection;

        Raise(new ReportDomainEvent(
                Guid.NewGuid(),
                ReportDomainEventTypes.Objectioned,
                this));
    }

    public void MoveToStage(
        bool isAccepted,
        int stageId,
        int toActorId,
        string comment,
        List<Media> attachments,
        string inspectorId,
        Visibility? visibility)
    {
        moveToStage(isAccepted, stageId, toActorId, comment, attachments, inspectorId, visibility);
        LastOperation = ReportOperationType.MoveToStage;
    }

    public List<ProcessTransition> GetPossibleTransitions()
    {
        var possibleTransitions = Process.Transitions
            .Where(p => p.FromId == CurrentStageId)
            .OrderBy(p => p.Order)
            .ToList();

        return possibleTransitions;
    }
    #endregion

    public void UpdateFeedback(int rating)
    {
        IsFeedbacked = true;
        Rating = rating;

        TransitionLogs.Add(TransitionLog.CreateFeedback(
            Id,
            CitizenId,
            $"ثبت بازخورد شهروند با امتیاز {rating}"));
        LastOperation = ReportOperationType.Feedback;
    }

    public void Delete()
    {
        IsDeleted = true;
    }

    public void ViolationChecked()
    {
        IsViolationChecked = true;
    }

    #region Private methods
    private void updateReport(
        Category? category,
        string? comments,
        Address? address,
        List<Media>? attachments,
        Visibility? visibility,
        Priority? priority)
    {
        var now = DateTime.UtcNow;

        if (category is not null)
        {
            CategoryId = category.Id;
            Category = category;
            Deadline = now.AddHours(category.Duration);
            ResponseDeadline = category.ResponseDuration == null ? null : now.AddHours(category.ResponseDuration.Value);
            InitProcess();
        }

        Comments = comments ?? Comments;
        Address = address ?? Address;
        Visibility = visibility ?? Visibility;
        Priority = priority ?? Priority;
        if (attachments != null)
        {
            Medias = attachments;
        }
            
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
        if (ReportState == ReportState.Finished || ReportState == ReportState.AcceptedByCitizen)
            return;

        var currentStage = Process.Stages.FirstOrDefault(p => p.Id == CurrentStageId);
        if (currentStage == null)
            throw new NullCurrentStageException();


        var autoActors = currentStage.Actors.Where(p => p.Type == ActorType.Auto).ToList();
        foreach (var autoActor in autoActors)
        {
            //Revise this
            var bot = autoActor.BotActor;
            if (bot == null)
                throw new BotNotFoundException();

            if (bot.ReasonMeaning == null 
                || LastReason != null && LastReason.ReasonMeaning == bot.ReasonMeaning)
            {
                makeTransition(
                    bot.TransitionId,
                    bot.ReasonId,
                    new List<Media>(),
                    "",
                    ActorType.Auto,
                    bot.Id,
                    bot.DestinationActorId);
                break;
            }
        }
    }

    private void makeTransition(
        int transitionId,
        int? reasonId,
        List<Media> attachments,
        string comment,
        ActorType actorType,
        string actorIdentifier,
        int toActorId,
        bool isExecutive = false,
        bool isContractor = false)
    {
        var now = DateTime.UtcNow;

        var transition = Process.Transitions.SingleOrDefault(p => p.Id == transitionId);
        if (transition == null)
            throw new ForbidNullTransitionException();

        if (CurrentStageId != transition.FromId)
            throw new InvalidOperationException();

        //TODO: Check this later
        //if(!transition.To.Actors.Any(a => a.Id == toActorId))
        //{
        //    throw new UnauthorizedAccessException();
        //}

        var reason = transition.ReasonList.Where(p => p.Id == reasonId).SingleOrDefault();

        CurrentStageId = transition.ToId;
        LastStatus = transition.To.Status;
        ReportState = transition.ReportState;
        LastStatusDateTime = now;
        LastTransitionId = transition.Id;
        LastReasonId = reasonId;
        LastReason = reason;


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

        //TODO: Check if toActorId is allowed
        CurrentActorId = toActorId;

        autoTransition();
    }


    private void moveToStage(
        bool isAccepted,
        int stageId,
        int toActorId,
        string comment,
        List<Media> attachments,
        string inspectorId,
        Visibility? visibility)
    {
        var now = DateTime.UtcNow;
        var duration = now - LastStatusDateTime;

        //set the inspector id
        InspectorId = inspectorId;

        if (!isAccepted)
        {
            ReportState = ReportState.Finished;
        }
        else
        {
            var stage = Process.Stages.Where(p => p.Id == stageId).SingleOrDefault();
            if (stage is null)
                throw new NullStageException();

            //_report.Priority = transitionInfo.Priority != null ? transitionInfo.Priority.Value : _report.Priority;
            Visibility = visibility ?? Visibility;
            CurrentStageId = stage.Id;
            LastStatus = "ارجاع به " + stage.Name;
            ReportState = ReportState.Live;
            LastStatusDateTime = now;
            LastTransitionId = null;
            LastReasonId = null;
            CurrentActorId = toActorId;
        }

        TransitionLog.CreateMoveToStage(
            Id,
            comment,
            attachments,
            "",
            ActorType.Person,
            inspectorId,
            duration.TotalSeconds,
            true);

        if (isAccepted)
        {
            autoTransition();
        }
    }
    #endregion
}

