﻿using Business.Models;

namespace Business.Interfaces;

public interface IEnderecoRepository : IRepository<Endereco>
{
    Task<Endereco> ObterEnderecoPorFornecedorAsync(Guid fornecedorId);
}