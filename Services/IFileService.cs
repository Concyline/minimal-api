using MinimalApi.Models;

namespace MinimalApi.Services
{
    public interface IFileService
    {
        Task<List<ArquivoBase64>> ListarTodosAsync();
        Task<ArquivoBase64?> BuscarPorIdAsync(string id);
        Task<string> UploadAsync(string nome, string contentType, byte[] dados);
        Task<bool> RemoverAsync(string id);
        Task<bool> AtualizarAsync(string id, string novoNome, string? novoContentType = null);
        Task<byte[]?> ObterArquivoBytesAsync(string id);
    }
}
