namespace Api.Services.MemoryCaching;

public class StaticData<T>
{
    public T Data { get; set; } = default!;
    public DateTimeOffset LastModified { get; set; }
}
