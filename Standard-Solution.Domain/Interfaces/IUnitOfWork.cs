using Standard_Solution.Domain.Interfaces.Repositories;

namespace Standard_Solution.Domain.Interfaces;
public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    int Complete();
    Task CompleteAsync();
}
