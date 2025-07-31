using MinimalApi.Routes;
using MinimalApi.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IStringService, StringServiceMongo>();
builder.Services.AddSingleton<IFileService, FileService>();

// Adicione a string no appsettings.json ou direto:
builder.Configuration["ConnectionStrings:MongoDb"] =
    "mongodb+srv://concyline:TJ8ZEc40JFjXlmA2@cluster0.qhn0zwz.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()     // ou use .WithOrigins("http://localhost:5500") se quiser restringir
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.MapStringsRoutes();
app.MapStringsRoutesMongo();
app.MapFileRoutes();

app.Urls.Add("http://+:80");

app.Run();



