using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
{
    public FornecedorRepository(MeuDbContext context) : base(context) { }

    public Task<Fornecedor> ObterFornecedorEnderecoAsync(Guid id) =>
        Db.Fornecedores
            .AsNoTracking()
            .Include(c => c.Endereco)
            .FirstOrDefaultAsync(c => c.Id == id);

    public Task<Fornecedor> ObterFornecedorProdutosEnderecoAsync(Guid id) =>
        Db.Fornecedores
            .AsNoTracking()
            .Include(c => c.Produtos)
            .Include(c => c.Endereco)
            .FirstOrDefaultAsync(c => c.Id == id);
}