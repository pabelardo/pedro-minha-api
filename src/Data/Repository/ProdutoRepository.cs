using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(MeuDbContext context) : base(context) { }

    public Task<Produto> ObterProdutoFornecedorAsync(Guid id) =>
        Db.Produtos
            .AsNoTracking()
            .Include(f => f.Fornecedor)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Produto>> ObterProdutosFornecedoresAsync() =>
        await Db
            .Produtos
            .AsNoTracking()
            .Include(f => f.Fornecedor)
            .OrderBy(p => p.Nome)
            .ToListAsync();

    public Task<IEnumerable<Produto>> ObterProdutosPorFornecedorAsync(Guid fornecedorId) =>
        BuscarAsync(p => p.FornecedorId == fornecedorId);
}