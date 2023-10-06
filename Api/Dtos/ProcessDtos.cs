using Domain.Models.Relational.Common;

namespace Api.Dtos
{
    public class ProcessDto
    {
        public int Id { get; set; }
        public ICollection<StageDto> Stages { get; set; }
        public ICollection<TransitionDto> Transitions { get; set; }
    }

    public class ProcessGetDto
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public ICollection<StageDto> Stages { get; set; } = new List<StageDto>();
        public ICollection<TransitionDto> Transitions { get; set; } = new List<TransitionDto>();
    }

    public class StageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int Order { get; set; }
        public string Status { get; set; }
        public ICollection<ActorDto> Actors { get; set; }
        public bool CanSendMessageToCitizen { get; set; }
    }

    public class TransitionDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int FromId { get; set; }
        //public StageDto From { get; set; }
        public int ToId { get; set; }
        //public StageDto To { get; set; }
        public ReportState ReportState { get; set; }

        public IEnumerable<ReasonDto> ReasonList { get; set; }
    }

    public class PossibleSourceDto
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleTitle { get; set; }
    }


    public class TypicalProcessCreateDto
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public List<int> ActorIds { get; set; }
        public ShahrbinInstance ShahrbinInstance { get; set; }
    }
}
