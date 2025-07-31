using MinimalApi.Requests;
using MinimalApi.Services;

namespace MinimalApi.Routes
{
    public static class FileRoutes
    {
        public static void MapFileRoutes(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/arquivos");

            // GET /arquivos - lista todos
            group.MapGet("/", async (IFileService service) =>
            {
                var arquivos = await service.ListarTodosAsync();
                return Results.Ok(arquivos);
            });

            // GET /arquivos/{id} - metadados do arquivo
            group.MapGet("/{id}", async (string id, IFileService service) =>
            {
                var arquivo = await service.BuscarPorIdAsync(id);
                return arquivo == null ? Results.NotFound() : Results.Ok(arquivo);
            });

            // GET /arquivos/download/{id} - baixar o arquivo
            group.MapGet("/download/{id}", async (string id, IFileService service) =>
            {
                var arquivo = await service.BuscarPorIdAsync(id);
                if (arquivo == null)
                    return Results.NotFound();

                var bytes = await service.ObterArquivoBytesAsync(id);
                return Results.File(bytes!, arquivo.ContentType, arquivo.Nome);
            });

            // POST /arquivos/upload - via multipart/form-data
            group.MapPost("/upload", async (HttpRequest request, IFileService service) =>
            {
                var form = await request.ReadFormAsync();
                var file = form.Files.FirstOrDefault();

                if (file == null || file.Length == 0)
                    return Results.BadRequest("Arquivo inválido");

                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                var id = await service.UploadAsync(file.FileName, file.ContentType, ms.ToArray());

                return Results.Ok(new { id });
            });

            // POST /arquivos/base64 - upload com base64 direto (JSON)
            group.MapPost("/base64", async (Base64UploadRequest req, IFileService service) =>
            {
                if (string.IsNullOrEmpty(req.Base64))
                    return Results.BadRequest("Base64 inválido");

                var bytes = Convert.FromBase64String(req.Base64);
                var id = await service.UploadAsync(req.Nome, req.ContentType ?? "application/octet-stream", bytes);

                return Results.Ok(new { id });
            });

            // PUT /arquivos/{id} - atualizar nome e contentType
            group.MapPut("/{id}", async (string id, UpdateFileRequest req, IFileService service) =>
            {
                var ok = await service.AtualizarAsync(id, req.NovoNome, req.NovoContentType);
                return ok ? Results.Ok() : Results.NotFound();
            });

            // DELETE /arquivos/{id}
            group.MapDelete("/{id}", async (string id, IFileService service) =>
            {
                var ok = await service.RemoverAsync(id);
                return ok ? Results.Ok() : Results.NotFound();
            });
        }
    }
}
