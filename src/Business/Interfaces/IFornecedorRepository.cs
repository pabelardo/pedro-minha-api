using Business.Models;

namespace Business.Interfaces;

public interface IFornecedorRepository : IRepository<Fornecedor>
{
    Task<Fornecedor> ObterFornecedorEnderecoAsync(Guid id);
    Task<Fornecedor> ObterFornecedorProdutosEnderecoAsync(Guid id);
}