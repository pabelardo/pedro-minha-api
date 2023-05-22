using System.Security.Claims;
using Business.Interfaces;

namespace Api.Extensions;

public class AspNetUser : IUser
{
    private readonly IHttpContextAccessor _accessor;

    public AspNetUser(IHttpContextAccessor accessor) => _accessor = accessor;

    public string Name => _accessor.HttpContext.User.Identity.Name;

    public Guid GetUserId() => IsAuthenticated() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty; //Caso o guid seja vazio, validar aonde for chamado

    public string GetUserEmail() => IsAuthenticated() ? _accessor.HttpContext.User.GetUserEmail() : "";

    public bool IsAuthenticated() => _accessor.HttpContext.User.Identity.IsAuthenticated;

    public bool IsInRole(string role) => _accessor.HttpContext.User.IsInRole(role);

    public IEnumerable<Claim> GetClaimsIdentity() => _accessor.HttpContext.User.Claims;
}