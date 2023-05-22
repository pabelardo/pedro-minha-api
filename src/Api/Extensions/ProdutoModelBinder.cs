using System.Text.Json;
using Api.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Extensions;

public class ProdutoModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);

        var serializeOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNameCaseInsensitive = true
        };

        var produtoImagemViewModel = JsonSerializer.Deserialize<ProdutoImagemViewModel>(
            bindingContext.ValueProvider.GetValue("produto").FirstOrDefault() ?? throw new ArgumentException("Produto não pode ser nulo."),
            serializeOptions);

        produtoImagemViewModel.ImagemUpload = bindingContext.ActionContext.HttpContext.Request.Form.Files.ToList().FirstOrDefault();

        bindingContext.Result = ModelBindingResult.Success(produtoImagemViewModel);

        return Task.CompletedTask;
    }
}
