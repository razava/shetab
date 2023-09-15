using Domain.Data;

namespace Api.Services.Dal;

public class RequestHandlingDal
{
    internal ApplicationDbContext context;

    public RequestHandlingDal(ApplicationDbContext context)
    {
        this.context = context;
    }
}