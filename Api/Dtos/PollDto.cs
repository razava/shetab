using Domain.Models.Relational.Common;
using Domain.Models.Relational.PollAggregate;

namespace Api.Dtos
{
    public class CreatePollDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public PollType PollType { get; set; }
        public string Question { get; set; }
        public IEnumerable<PollChoice> Choices { get; set; }
        public DateTime? Expiration { get; set; }
        public PollState Status { get; set; }
    }

    public class GetPollDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public PollType PollType { get; set; }
        public string Question { get; set; }
        public IEnumerable<ChoiceDto> Choices { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Expiration { get; set; }
        public PollState Status { get; set; }
        public List<AnswerDto> Answers { get; set; }
        public string AuthorId { get; set; }
        public ApplicationUserDto Author { get; set; }
    }

    public class ChoiceDto
    {
        public int Id { get; set; }
        public string ShortTitle { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        public int PollId { get; set; }
    }

    public class AnswerDto
    {
        public Guid Id { get; set; }
        public int PollId { get; set; }
        public ApplicationUserDto ApplicationUser { get; set; }

        //This is used for descriptive polls
        public string Text { get; set; }
        public IEnumerable<ChoiceDto> Choices { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class PollAnswerDto
    {
        public List<int> SelectedChoices { get; set; } = new List<int>();
        public string Text { get; set; }
    }

    public class PollResultDto
    {
        public List<Tuple<ChoiceDto, bool>> Answers { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class PollSummaryDto
    {
        public List<Tuple<ChoiceDto, int>> Frequencies { get; set; }
    }

    public class AttachmentDto
    {
        public IFormFile Upload { get; set; }
        public string Description { get; set; }
    }

    public class AttachmentResultDto
    {
        public string FileName { get; set; }
        public string Url { get; set; }
        public bool Uploaded { get; set; }
    }
}
