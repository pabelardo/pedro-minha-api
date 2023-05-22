using Business.Models;

namespace Business.Interfaces;

public interface IFornecedorService : IDisposable
{
    Task AdicionarAsync(Fornecedor fornecedor);
    Task AtualizarAsync(Fornecedor fornecedor);
    Task RemoverAsync(Guid id);
    Task AtualizarEnderecoAsync(Endereco endereco);
}