using Api.BaseController;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.V2.Controllers;

[Authorize]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/teste")]
public class TesteController : MainController
{
    private readonly ILogger<TesteController> _logger;

    public TesteController(
        INotificador notificador, 
        IUser appUser,
        ILogger<TesteController> logger) : base(notificador, appUser)
    {
        _logger = logger;
    }

    [HttpGet]
    public string Valor()
    {
        _logger.LogTrace("Log de Trace");
        _logger.LogDebug("Hello world from dotnet7!");
        _logger.LogInformation("Log de Informação");
        _logger.LogWarning("Log de Aviso");
        _logger.LogError("Log de Erro");
        _logger.LogCritical("Log de Problema Critico");

        return "Sou a V2";
    }
}