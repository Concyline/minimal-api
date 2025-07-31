using MinimalApi.Models;
using MongoDB.Driver;
using System.Text.Json;

namespace MinimalApi.Services
{
    public class FileService : IFileService
    {
        private readonly IMongoCollection<ArquivoBase64> _collection;

        public FileService(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("MongoDb");
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("minimaldb");
            _collection = database.GetCollection<ArquivoBase64>("arquivos");
        }

        public async Task<List<ArquivoBase64>> ListarTodosAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<ArquivoBase64?> BuscarPorIdAsync(string id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<string> UploadAsync(string nome, string contentType, byte[] dados)
        {
            var arquivo = new ArquivoBase64
            {
                Nome = nome,
                ContentType = contentType,
                Base64 = Convert.ToBase64String(dados),
                TamanhoBytes = dados.Length,
                CriadoEm = DateTime.Now
            };

            await _collection.InsertOneAsync(arquivo);
            return arquivo.Id!;
        }

        public async Task<bool> RemoverAsync(string id)
        {
            var resultado = await _collection.DeleteOneAsync(x => x.Id == id);
            return resultado.DeletedCount > 0;
        }

        public async Task<bool> AtualizarAsync(string id, string novoNome, string? novoContentType = null)
        {
            var update = Builders<ArquivoBase64>.Update
                .Set(x => x.Nome, novoNome)
                .Set(x => x.ContentType, novoContentType ?? "application/octet-stream");

            var resultado = await _collection.UpdateOneAsync(x => x.Id == id, update);
            return resultado.ModifiedCount > 0;
        }

        public async Task<byte[]?> ObterArquivoBytesAsync(string id)
        {
            var arquivo = await BuscarPorIdAsync(id);
            if (arquivo == null || string.IsNullOrEmpty(arquivo.Base64))
                return null;

            return Convert.FromBase64String(arquivo.Base64);
        }
    }
}
