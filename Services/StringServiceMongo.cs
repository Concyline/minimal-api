using MinimalApi.Models;
using MongoDB.Driver;

namespace MinimalApi.Services
{
    public class StringServiceMongo : IStringService
    {
        private readonly IMongoCollection<StringItem> _collection;

        public StringServiceMongo(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("MongoDb");
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("minimaldb");
            _collection = database.GetCollection<StringItem>("itens");
        }

        public async Task<List<StringItem>> GetAllAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<StringItem?> GetByIdAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<StringItem> CreateAsync(string valor)
        {
            var novo = new StringItem { Valor = valor };
            await _collection.InsertOneAsync(novo);
            return novo;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var resultado = await _collection.DeleteOneAsync(x => x.Id == id);
            return resultado.DeletedCount > 0;
        }
    }
}
