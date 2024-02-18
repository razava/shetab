using System.ComponentModel;

namespace Domain.Models.Relational.Common;

public enum MediaType
{
    Image,
    Video,
    Voice,
    Doc,
    Other
}

public enum Gender
{
    [Description("مشخص نشده")]
    NotDefined,
    [Description("زن")]
    Female,
    [Description("مرد")]
    Male
}

public enum Education
{
    [Description("تعریف نشده")]
    NotDefined,
    [Description("ابتدایی")]
    Ebtedai,
    [Description("دیپلم")]
    Diplom,
    [Description("فوق دیپلم")]
    FogheDiplom,
    [Description("لیسانس")]
    Lisans,
    [Description("فوق لیسانس")]
    FogheLisans,
    [Description("دکترا")]
    Doktora
}

public enum UserFlags
{
    New,
    Old,
    OldPassSent
}

public enum ComplaintState
{
    Created,
    Live,
    Finished,
    Objectioned
}

public enum MessageSubject
{
    Report,
    Idea,
    Announcement
}

public enum MessageSendingType
{
    None,
    Sms,
    Push,
    Both
}

public enum RecepientType
{
    Person,
    Role
}

public enum OrganizationalUnitType
{
    Executive,
    Person,
    Role,
    OrganizationalUnit
}

public enum PollType
{
    SingleChoice,
    MultipleChoice,
    Descriptive
}

public enum PollState
{
    Active,
    Suspended,
    Expired
}

public enum ViolationCheckResult
{
    NoAction,
    Deleted,
    Corrected
}

public enum ReportFlags
{
    New,
    Old
}

public enum ReportState
{
    [Description("در حال رسیدگی")]
    Live,
    [Description("پایان یافته")]
    Finished,
    [Description("بررسی مجدد")]
    Review,
    [Description("تأیید شده")]
    AcceptedByCitizen,
    [Description("در انتظار تأیید")]
    NeedAcceptance
}

public enum Visibility
{
    EveryOne,
    Operators,
    Managers
}

public enum Priority
{
    [Description("کم")]
    Low,
    [Description("عادی")]
    Normal,
    [Description("زیاد")]
    High,
    [Description("فوری")]
    Urgent
}

public enum CategoryType
{
    Report, Idea, Root, All
}

public enum FormElementType
{
    Label,
    EditText,
    Select,
    MultiSelect,
    DateTime,
    PlateNumber,
    MultiLine,
    NationalId
}

public enum ActorType
{
    Person,
    Role,
    Auto
}

public enum TransitionType
{
    Regular,
    Objection
}

public enum ReasonMeaning
{
    Ok,
    Nok,
    Other
}

public enum ReportLogType
{
    Transition,
    MessageToCitizen,
    PublicNote,
    InternalNote,
    Change,
    Feedback,
    Created,
    Approved,
    MoveToStage
}