using Business.Interfaces;
using Business.Models;
using Business.Validations;

namespace Business.Services;

public class ProdutoService : BaseService, IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;

    public ProdutoService(
        IProdutoRepository produtoRepository,
        INotificador notificador)
        : base(notificador)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task AdicionarAsync(Produto produto)
    {
        if (!ExecutarValidacao(new ProdutoValidation(), produto)) return;

        await _produtoRepository.AdicionarAsync(produto);
    }

    public async Task AtualizarAsync(Produto produto)
    {
        if (!ExecutarValidacao(new ProdutoValidation(), produto)) return;

        await _produtoRepository.AtualizarAsync(produto);
    }

    public async Task RemoverAsync(Guid id) => await _produtoRepository.RemoverAsync(id);

    public void Dispose() => GC.SuppressFinalize(this);
}