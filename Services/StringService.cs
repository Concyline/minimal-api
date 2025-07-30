using System.Text.Json;

namespace MinimalApi.Services
{
    public class StringService
    {


        const string jsonFile = "dados.json";

        // Carrega ou cria lista inicial
        public static List<string> CarregarLista()
        {
            if (!File.Exists(jsonFile))
                return new List<string>();

            var json = File.ReadAllText(jsonFile);
            return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
        }

        public static void SalvarLista(List<string> lista)
        {
            var json = JsonSerializer.Serialize(lista, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(jsonFile, json);
        }
    }
}
