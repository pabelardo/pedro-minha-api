using Business.Models;

namespace Business.Interfaces;

public interface IProdutoService : IDisposable
{
    Task AdicionarAsync(Produto produto);
    Task AtualizarAsync(Produto produto);
    Task RemoverAsync(Guid id);
}