using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
{
    public EnderecoRepository(MeuDbContext context)
        : base(context) { }

    public Task<Endereco> ObterEnderecoPorFornecedorAsync(Guid fornecedorId) =>
        Db.Enderecos
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.FornecedorId == fornecedorId);
}