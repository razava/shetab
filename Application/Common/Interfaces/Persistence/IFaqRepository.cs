using Application.Faqs.Queries.GetFaq;
using Domain.Models.Relational;
using System.Linq.Expressions;

namespace Application.Common.Interfaces.Persistence;

public interface IFaqRepository : IGenericRepository<Faq>
{
    public Task<List<T>> GetFaqs<T>(Expression<Func<Faq, bool>>? filter, Expression<Func<Faq, T>> selector); 
}
