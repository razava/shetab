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
    NotDefined,
    Female,
    Male,
    Other
}

public enum Education
{
    NotDefined,
    Ebtedai,
    Diplom,
    FogheDiplom,
    Lisans,
    FogheLisans,
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

public enum MessageType
{
    Report,
    Idea,
    Announcement
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
    Expired,
    Deleted
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
    Live,
    Finished,
    Review,
    Accepted,
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
    VeryLow,
    Low,
    Normal,
    High,
    VeryHigh,
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
    Feedback
}