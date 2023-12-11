namespace Domain.Models.Relational;

public class Faq : BaseModel
{
    public int Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }

    private Faq()
    {
        
    }

    public static Faq Create(int instanceId, string question, string answer, bool isDeleted)
    {
        var faq = new Faq()
        {
            ShahrbinInstanceId = instanceId,
            Question = question,
            Answer = answer,
            IsDeleted = isDeleted
        };

        return faq;
    }

    public void Update(string? question, string? answer, bool? isDeleted)
    {
        Question = question ?? Question;
        Answer = answer ?? Answer;
        IsDeleted = isDeleted ?? IsDeleted;
    }
}
