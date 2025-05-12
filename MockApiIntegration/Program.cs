using MockApiIntegration.Middleware;
using MockApiIntegration.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure HTTP client with base URL from configuration
builder.Services.AddHttpClient<IProductService, ProductService>(client =>
{
    var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];
    client.BaseAddress = new Uri(baseUrl ?? "https://api.restful-api.dev/objects");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Add global exception handling middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

app.Run();
