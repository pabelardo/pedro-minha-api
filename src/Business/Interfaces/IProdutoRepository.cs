using Business.Models;

namespace Business.Interfaces;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<IEnumerable<Produto>> ObterProdutosPorFornecedorAsync(Guid fornecedorId);
    Task<IEnumerable<Produto>> ObterProdutosFornecedoresAsync();
    Task<Produto> ObterProdutoFornecedorAsync(Guid id);
}