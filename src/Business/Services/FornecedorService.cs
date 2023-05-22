using Business.Interfaces;
using Business.Models;
using Business.Validations;

namespace Business.Services;

public class FornecedorService : BaseService, IFornecedorService
{
    private readonly IFornecedorRepository _fornecedorRepository;
    private readonly IEnderecoRepository _enderecoRepository;

    public FornecedorService(
        IFornecedorRepository fornecedorRepository,
        IEnderecoRepository enderecoRepository,
        INotificador notificador)
        : base(notificador)
    {
        _fornecedorRepository = fornecedorRepository;
        _enderecoRepository = enderecoRepository;
    }

    public async Task AdicionarAsync(Fornecedor fornecedor)
    {
        if (!ExecutarValidacao(new FornecedorValidation(), fornecedor)
            || !ExecutarValidacao(new EnderecoValidation(), fornecedor.Endereco)) return;

        if (_fornecedorRepository.BuscarAsync(f => f.Documento == fornecedor.Documento).Result.Any())
        {
            Notificar("Já existe um fornecedor com este documento informado.");
            return;
        }

        await _fornecedorRepository.AdicionarAsync(fornecedor);
    }

    public async Task AtualizarAsync(Fornecedor fornecedor)
    {
        if (!ExecutarValidacao(new FornecedorValidation(), fornecedor)) return;

        if (_fornecedorRepository.BuscarAsync(f => f.Documento == fornecedor.Documento && f.Id != fornecedor.Id).Result.Any())
        {
            Notificar("Já existe um fornecedor com este documento informado.");
            return;
        }

        await _fornecedorRepository.AtualizarAsync(fornecedor);
    }

    public async Task AtualizarEnderecoAsync(Endereco endereco)
    {
        if (!ExecutarValidacao(new EnderecoValidation(), endereco)) return;

        await _enderecoRepository.AtualizarAsync(endereco);
    }

    public async Task RemoverAsync(Guid id)
    {
        if (_fornecedorRepository.ObterFornecedorProdutosEnderecoAsync(id).Result.Produtos.Any())
        {
            Notificar("O fornecedor possui produtos cadastrados!");
            return;
        }

        var endereco = await _enderecoRepository.ObterEnderecoPorFornecedorAsync(id);

        if (endereco != null) await _enderecoRepository.RemoverAsync(endereco.Id);

        await _fornecedorRepository.RemoverAsync(id);
    }

    public void Dispose() => GC.SuppressFinalize(this);
}