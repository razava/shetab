namespace Api.Dtos
{

    public class GetCommentForOperatorDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsVerified { get; set; }
        public string UserId { get; set; }
        public ApplicationUserDto User { get; set; }
        public Guid ReportId { get; set; }
        public GetReportDto Report { get; set; }
        public bool IsSeen { get; set; }
        public Guid? ReplyId { get; set; }
        public GetCommentForOperatorDto Reply { get; set; }
    }
    public class CreateCommentDto
    {
        public Guid ReportId { get; set; }
        public string Comment { get; set; }
        public bool? IsSeen { get; set; }
        public bool? IsVerified { get; set; }

    }
}
