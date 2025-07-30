using MinimalApi.Requests;
using MinimalApi.Services;

namespace MinimalApi.Routes
{
    public static class StringsRoutesMongo
    {

        public static void MapStringsRoutesMongo(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/itensmongo");

            group.MapGet("/", async (IStringService service) =>
            {
                return Results.Ok(await service.GetAllAsync());
            });

            group.MapGet("/{id}", async (string id, IStringService service) =>
            {
                var item = await service.GetByIdAsync(id);
                return item == null ? Results.NotFound() : Results.Ok(item);
            });

            group.MapPost("/", async (StringRequest req, IStringService service) =>
            {
                if (string.IsNullOrWhiteSpace(req.Valor))
                    return Results.BadRequest("Texto inválido");

                var novo = await service.CreateAsync(req.Valor);
                return Results.Created($"/itens/{novo.Id}", novo);
            });

            group.MapDelete("/{id}", async (string id, IStringService service) =>
            {
                var ok = await service.DeleteAsync(id);
                return ok ? Results.Ok() : Results.NotFound();
            });
        }
    }
}
