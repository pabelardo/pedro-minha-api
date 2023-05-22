using Api.BaseController;
using Api.Extensions;
using Api.ViewModels;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.V1.Controllers;

[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/fornecedores")]
public class FornecedoresController : MainController
{
    #region Injeções de Dependência
     
    private readonly IFornecedorRepository _fornecedorRepository;
    private readonly IMapper _mapper;
    private readonly IFornecedorService _fornecedorService;
    private readonly IEnderecoRepository _enderecoRepository;

    #endregion

    #region Construtor  

    public FornecedoresController(
        INotificador notificador,
        IUser appUser,
        IFornecedorRepository fornecedorRepository,
        IMapper mapper,
        IFornecedorService fornecedorService,
        IEnderecoRepository enderecoRepository)
        : base(notificador, appUser)
    {
        _fornecedorRepository = fornecedorRepository;
        _mapper = mapper;
        _fornecedorService = fornecedorService;
        _enderecoRepository = enderecoRepository;
    }

    #endregion

    [AllowAnonymous]
    [HttpGet]
    public async Task<IEnumerable<FornecedorViewModel>> ObterTodos() =>
        _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodosAsync());

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FornecedorViewModel>> ObterPorId(Guid id)
    {
        var fornecedor = await ObterFornecedorProdutosEndereco(id);

        return fornecedor ?? (ActionResult<FornecedorViewModel>)NotFound();
    }

    [ClaimsAuthorize("Fornecedor", "Adicionar")]
    [HttpPost]
    public async Task<ActionResult<FornecedorViewModel>> Adicionar(FornecedorViewModel fornecedorViewModel)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        await _fornecedorService.AdicionarAsync(_mapper.Map<Fornecedor>(fornecedorViewModel));

        return CustomResponse(fornecedorViewModel);
    }

    [ClaimsAuthorize("Fornecedor", "Atualizar")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<FornecedorViewModel>> Atualizar(Guid id, FornecedorViewModel fornecedorViewModel)
    {
        if (id != fornecedorViewModel.Id)
        {
            NotificarErro("O id informado não é o mesmo que foi passado na query");
            return CustomResponse(fornecedorViewModel);
        }

        if (!ModelState.IsValid) return CustomResponse(ModelState);

        await _fornecedorService.AtualizarAsync(_mapper.Map<Fornecedor>(fornecedorViewModel));

        return CustomResponse(fornecedorViewModel);
    }

    [ClaimsAuthorize("Fornecedor", "Excluir")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<FornecedorViewModel>> Excluir(Guid id)
    {
        var fornecedorViewModel = await ObterFornecedorEndereco(id);

        if (fornecedorViewModel == null) return NotFound();

        await _fornecedorService.RemoverAsync(id);

        return CustomResponse(fornecedorViewModel);
    }

    [HttpGet("endereco/{id:guid}")]
    public async Task<EnderecoViewModel> ObterEnderecoPorId(Guid id) =>
        _mapper.Map<EnderecoViewModel>(await _enderecoRepository.ObterPorIdAsync(id));

    [HttpPut("endereco/{id:guid}")]
    public async Task<IActionResult> AtualizarEndereco(Guid id, EnderecoViewModel enderecoViewModel)
    {
        if (id != enderecoViewModel.Id)
        {
            NotificarErro("O id informado não é o mesmo que foi passado na query");
            return CustomResponse(enderecoViewModel);
        }

        if (!ModelState.IsValid) return CustomResponse(ModelState);

        await _fornecedorService.AtualizarEnderecoAsync(_mapper.Map<Endereco>(enderecoViewModel));

        return CustomResponse(enderecoViewModel);
    }

    #region Utils

    private async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id) =>
        _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEnderecoAsync(id));

    private async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id) =>
        _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEnderecoAsync(id));

    #endregion
}