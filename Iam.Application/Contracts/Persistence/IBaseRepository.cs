using Iam.Domain.Common;

namespace Iam.Application.Contracts.Persistence;

public interface IBaseRepository<T> : IGenericRepository<T> where T : BaseEntity
{

}
