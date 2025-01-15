using CartService.Data;
using CartService.GraphQL;
using CartService.GraphQL.Mutations;
using CartService.GraphQL.Queries;
using CartService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddDbContext<CartDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddGraphQLServer().AddQueryType<CartQuery>()
    .AddMutationType<CartMutation>().AddType<CartType>()
    .AddProjections().AddFiltering().AddSorting();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpClient("ProductService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7018/api/");
});
builder.Services.AddSingleton<ProductUpdateListener>();
var app = builder.Build();

// Start the ProductUpdateListener
var listner = app.Services.GetRequiredService<ProductUpdateListener>();
await Task.Run (() => listner.StartListeningAsync());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGraphQL();

app.Run();
