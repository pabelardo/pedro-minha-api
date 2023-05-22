using Api.BaseController;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.V1.Controllers;

[Authorize]
[ApiVersion("1.0", Deprecated = true)]
[Route("api/v{version:apiVersion}/teste")]
public class TesteController : MainController
{
    public TesteController(INotificador notificador, IUser appUser) : base(notificador, appUser) { }

    [HttpGet]
    public string Valor() => "Sou a V1";
}