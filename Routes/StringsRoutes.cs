namespace MinimalApi.Routes
{
    using Microsoft.AspNetCore.Routing;
    using MinimalApi.Requests;
    using MinimalApi.Services;

    public static class StringsRoutes
    {
        public static void MapStringsRoutes(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/itens");

            // GET - Todos
            group.MapGet("/", () =>
            {
                var lista = StringService.CarregarLista();
                return Results.Ok(lista);
            });

            // GET - Por índice
            group.MapGet("/{id:int}", (int id) =>
            {
                var lista = StringService.CarregarLista();
                if (id < 0 || id >= lista.Count)
                    return Results.NotFound("Item não encontrado");

                return Results.Ok(lista[id]);
            });

            group.MapPost("/", async (StringRequest req) =>
            {
                if (string.IsNullOrWhiteSpace(req.Valor))
                    return Results.BadRequest("Texto inválido");

                var lista = StringService.CarregarLista();
                lista.Add(req.Valor);
                StringService.SalvarLista(lista);

                return Results.Created($"/itens/{lista.Count - 1}", req.Valor);
            });


            // DELETE - Remover
            group.MapDelete("/{id:int}", (int id) =>
            {
                var lista = StringService.CarregarLista();
                if (id < 0 || id >= lista.Count)
                    return Results.NotFound("Item não encontrado");

                var removido = lista[id];
                lista.RemoveAt(id);
                StringService.SalvarLista(lista);

                return Results.Ok(removido);
            });


        }

     
    }

}
