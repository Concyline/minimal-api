using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



const string jsonFile = "dados.json";

// Carrega ou cria lista inicial
List<string> CarregarLista()
{
    if (!File.Exists(jsonFile))
        return new List<string>();

    var json = File.ReadAllText(jsonFile);
    return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
}

void SalvarLista(List<string> lista)
{
    var json = JsonSerializer.Serialize(lista, new JsonSerializerOptions { WriteIndented = true });
    File.WriteAllText(jsonFile, json);
}


// GET - Todos
app.MapGet("/itens", () =>
{
    var lista = CarregarLista();
    return Results.Ok(lista);
});

// GET - Por índice
app.MapGet("/itens/{id:int}", (int id) =>
{
    var lista = CarregarLista();
    if (id < 0 || id >= lista.Count)
        return Results.NotFound("Item não encontrado");

    return Results.Ok(lista[id]);
});



app.MapPost("/itens", async (ItemRequest req) =>
{
    if (string.IsNullOrWhiteSpace(req.Valor))
        return Results.BadRequest("Texto inválido");

    var lista = CarregarLista();
    lista.Add(req.Valor);
    SalvarLista(lista);

    return Results.Created($"/itens/{lista.Count - 1}", req.Valor);
});


// DELETE - Remover
app.MapDelete("/itens/{id:int}", (int id) =>
{
    var lista = CarregarLista();
    if (id < 0 || id >= lista.Count)
        return Results.NotFound("Item não encontrado");

    var removido = lista[id];
    lista.RemoveAt(id);
    SalvarLista(lista);

    return Results.Ok(removido);
});


app.Urls.Add("http://+:80");

app.Run();
public class ItemRequest
{
    public string Valor { get; set; } = string.Empty;
}


